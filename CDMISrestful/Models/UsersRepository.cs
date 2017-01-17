using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using CDMISrestful.CommonLibrary;
using CDMISrestful.DataMethod;
using CDMISrestful.DataModels;
using CDMISrestful.DataViewModels;
using InterSystems.Data.CacheClient;

namespace CDMISrestful.Models
{
    public class UsersRepository : IUsersRepository
    {
        //DataConnection
      
        UsersMethod usersMethod = new UsersMethod();
        CommonMethod commonMethod = new CommonMethod();
        PlanInfoMethod planInfoMethod = new PlanInfoMethod();
        VitalInfoMethod vitalInfoMethod = new VitalInfoMethod();
        ModuleInfoMethod moduleInfoMethod = new ModuleInfoMethod();
        DictMethod dictMethod = new DictMethod();

        /// <summary>
        /// 验证token是否正确 syf 2015-10-13
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool IsTokenValid(string token)
        {
            return SecurityManager.IsTokenValid(token);
        }

        public ForToken LogOn(DataConnection pclsCache, string PwType, string userId, string password, string role)
        {
            int result = usersMethod.CheckPasswordByInput(pclsCache, PwType, userId, password);
            ForToken response = new ForToken();
            response.Status = "";
            response.Token = "";
            if (result == 1)
            {
                //密码验证成功
                string UId = usersMethod.GetIDByInput(pclsCache, PwType, userId); //输入手机号获取用户ID
                if (UId != "")
                {
                    string Class = usersMethod.GetActivatedState(pclsCache, UId, role);
                    if (Class == "0")
                    {
                        int flag = 0;
                        List<string> AllRoleMatch = usersMethod.GetAllRoleMatch(pclsCache, UId);
                        if (AllRoleMatch != null)
                        {
                            for (int i = 0; i < AllRoleMatch.Count; i++)
                            {
                                if (AllRoleMatch[i].ToString() == role)//查询条件
                                {
                                    flag = 1;
                                    break;
                                }
                            }
                        }
                        if (flag == 1)
                        {
                            string ticks = new CommonMethod().GetServerTime(pclsCache);
                            response.Token = SecurityManager.GenerateToken(userId, password, role, ticks);
                            response.Status = "已注册激活且有权限，登陆成功，跳转到主页";
                            return response; //"已注册激活且有权限，登陆成功，跳转到主页"1;
                        }
                        else
                        {
                            response.Status = "已注册激活,但没有权限"; //"已注册激活 但没有权限"2;
                        }
                    }
                    else      //Class == "1" or Class == ""
                    {
                        response.Status = "您的账号对应的角色未激活，需要先激活；界面跳转到游客页面(已注册但未激活)";            //您的账号对应的角色未激活，需要先激活；界面跳转到游客页面（已注册但未激活）3
                    }
                }
                else
                {
                    response.Status = "用户不存在"; //"用户不存在"4;
                }
            }
            else if (result == 0)
            {
                response.Status = "密码错误"; //"密码错误"5;
            }
            else
            {
                response.Status = "用户不存在";   //"用户不存在"4
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PwType"></param>
        /// <param name="userId">手机号/邮箱等</param>
        /// <param name="UserName">用户姓名</param>
        /// <param name="Password"></param>
        /// <param name="role"></param>
        /// <param name="revUserId"></param>
        /// <param name="TerminalName"></param>
        /// <param name="TerminalIP"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>
        public int Register(DataConnection pclsCache, string PwType, string userId, string UserName, string Password, string role, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            //userID：系统唯一标识

            int roleFlag = 0;
            int ret = 0;
            int Flag = usersMethod.CheckRepeat(pclsCache, userId, PwType);
            //用户名不存在
            if (Flag == 1)
            {
                ret = usersMethod.RegisterRelated(pclsCache, PwType, userId, Password, UserName, role, revUserId, TerminalName, TerminalIP, DeviceType);
            }
            //用户名已存在
            else
            {            
                string UserId = usersMethod.GetIDByInput(pclsCache, PwType, userId);
                //系统唯一标识已产生
                if (UserId != "")
                {
                    List<string> result = usersMethod.GetAllRoleMatch(pclsCache, UserId);
                    //该用户已有角色信息
                    if (result != null)
                    {
                        int flag3 = 0;
                        for (int i = 0; i < result.Count(); i++)
                        {
                            string Role = result[i];
                            if (Role == role)
                            {
                                flag3 = 1;
                                roleFlag = 1;
                            }
                            if (flag3 == 1)
                            {
                                //同一用户名的同一角色已经存在
                                ret = 3; 
                            }

                        }
                        //该角色还没有想要创建的角色
                        if (roleFlag == 0)
                        {
                            string InviteNo = commonMethod.GetNo(pclsCache, 12, "");
                            if (InviteNo != "")
                            {
                                int test = usersMethod.PsRoleMatchSetData(pclsCache, UserId, role, InviteNo, "1", "");
                                if (test == 1)
                                {
                                    ret = 4; // "新建角色成功，密码与您已有账号一致";
                                }
                                else
                                {
                                    ret = 2; // "注册失败！";
                                }
                            }
                        }
                       
                    }                
                }
                //输入的用户名还没有系统唯一标识：在Cm.MstUser表中数据写入成功后，Cm.MstUserDetail表数据写入失败/一些旧用户在创建时只写Cm.MstUser表，而没有写Cm.MstUserDetail表
                else
                {
                    ret = usersMethod.RegisterRelated(pclsCache, PwType, userId, Password, UserName, role, revUserId, TerminalName, TerminalIP, DeviceType);
                }                        
            }
            return ret;
        }

        public int Activition(DataConnection pclsCache, string UserId, string InviteCode, string role)
        {
            int ret = 0;
            int Flag = usersMethod.SetActivition(pclsCache, UserId, role, InviteCode);
            if (Flag == 1)
            {
                ret = 1; // "激活成功";
            }
            else
            {
                ret = 2; // "激活失败";
            }
            return ret;
        }

        public int ChangePassword(DataConnection pclsCache, string OldPassword, string NewPassword, string UserId, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            int ret = 0;
            ret = usersMethod.ChangePassword(pclsCache, UserId, OldPassword, NewPassword, revUserId, TerminalName, TerminalIP, DeviceType);
            return ret;
        }

        #region 弃用 20151101 CSQ 用GetPatientsPlan方法代替
        //public PatientsDataSet GetPatientsList(string DoctorId, string ModuleType, int Plan, int Compliance, int Goal)
        //{
        //    int patientTotalCount = 0;  //某个模块下的患者总数
        //    int planCount = 0;          //已有计划的患者数
        //    int complianceCount = 0;    //依从的患者数
        //    int goalCount = 0;          //达标的患者数
        //    double planRate = 0;
        //    double complianceRateTotal = 0;
        //    double goalRate = 0;

        //    RateTable DT_Rates = new RateTable();

        //    List<PatientListTable> DT_PatientList = new List<PatientListTable>();

        //    List<PatientPlan> DT_Patients = new List<PatientPlan>();

        //    PatientsDataSet DS_Patients = new PatientsDataSet();

        //    try
        //    {
        //        int nowDate = commonMethod.GetServerDate(pclsCache);
        //        DT_Patients = planInfoMethod.GetPatientsPlanByDoctorId(pclsCache, DoctorId, ModuleType);
        //        if (DT_Patients != null)
        //            patientTotalCount = DT_Patients.Count;
        //        else
        //            return DS_Patients;
           
        //        for (int i = 0; i < patientTotalCount; i++)
        //        {
        //            string patientId = DT_Patients[i].PatientId; // item["PatientId"].ToString();
        //            string planNo = DT_Patients[i].PlanNo;  //item["PlanNo"].ToString();
        //            if (planNo != "")
        //            {
        //                planCount++;
        //            }

        //            //HavePlan 0 1 2
        //            if ((Plan == 1 && planNo == "") || (Plan == 2 && planNo != ""))
        //            {
        //                continue;
        //            }
        //            string startDate = DT_Patients[i].StartDate;   //item["StartDate"].ToString();
        //            string totalDays = DT_Patients[i].TotalDays;   //item["TotalDays"].ToString();
        //            string remainingDays = DT_Patients[i].RemainingDays;   //item["RemainingDays"].ToString();
        //            string status = DT_Patients[i].Status;   //item["Status"].ToString();

        //            double process = 0.0;
        //            double complianceRate = 0.0;
        //            //VitalSign
        //            List<string> vitalsigns = new List<string>();

        //            if (planNo != "")
        //            {
        //                complianceRate = planInfoMethod.GetComplianceByDate(pclsCache, planNo, nowDate);
        //                if (complianceRate > 0)
        //                {
        //                    complianceCount++;
        //                }
        //                if (complianceRate < 0)
        //                {
        //                    complianceRate = 0;
        //                }
        //                //Compliance
        //                if (Compliance == 1 && complianceRate <= 0)
        //                {
        //                    continue;
        //                }
        //                if (Compliance == 2 && complianceRate > 0)
        //                {
        //                    continue;
        //                }

        //                //Vitalsign 
        //                string itemType = "Bloodpressure";
        //                string itemCode = "Bloodpressure_1";
        //                int recordDate = nowDate; //nowDate
        //                //recordDate = 20150422;
        //                bool goalFlag = false;
        //                VitalInfo list = vitalInfoMethod.GetSignByDay(pclsCache, patientId, itemType, itemCode, recordDate);
        //                if (list != null)
        //                {
        //                    vitalsigns.Add(list.Value);
        //                }
        //                else
        //                {
        //                    //   vitalsigns.Add("115");
        //                    //ZAM 2015-6-17
        //                    vitalsigns.Add("");
        //                }

        //                TargetByCode targetlist = planInfoMethod.GetTarget(pclsCache, planNo, itemType, itemCode);
        //                if (targetlist != null)
        //                {
        //                    vitalsigns.Add(targetlist.Origin);  //index 4 for Origin value
        //                    vitalsigns.Add(targetlist.Value);  //index 3 for target value
        //                }
        //                else
        //                {
        //                    //vitalsigns.Add("200");
        //                    //vitalsigns.Add("120");
        //                    //ZAM 2015-6-17
        //                    vitalsigns.Add("");
        //                    vitalsigns.Add("");
        //                }
        //                //非法数据判断 zam 2015-5-18
        //                if (list != null && targetlist != null)
        //                {
        //                    double m, n;
        //                    bool misNumeric = double.TryParse(list.Value.ToString(), out m);
        //                    bool nisNumeric = double.TryParse(targetlist.Value.ToString(), out n);
        //                    if (misNumeric && nisNumeric)
        //                    {
        //                        //if (Convert.ToInt32(list[2]) <= Convert.ToInt32(targetlist[3])) //已达标
        //                        if (m <= n)
        //                        {
        //                            goalCount++;
        //                            goalFlag = true;
        //                        }
        //                    }
        //                }
        //                //Goal 
        //                if (Goal == 1 && goalFlag == false)
        //                {
        //                    continue;
        //                }
        //                if (Goal == 2 && goalFlag == true)
        //                {
        //                    continue;
        //                }

        //                //非法数据判断 zam 2015-5-18
        //                if (startDate != "" && totalDays != "" && remainingDays != "")
        //                {
        //                    double m, n;
        //                    bool misNumeric = double.TryParse(totalDays, out m);
        //                    bool nisNumeric = double.TryParse(remainingDays, out n);

        //                    if (misNumeric && nisNumeric)
        //                    {
        //                        //process = (Convert.ToDouble(totalDays) - Convert.ToDouble(remainingDays)) / Convert.ToDouble(totalDays);
        //                        process = m != 0.0 ? (m - n) / m : 0;
        //                    }

        //                }
        //            }

        //            //PhotoAddress
        //            string photoAddress = "";
        //            PatDetailInfo patientInfolist = moduleInfoMethod.PsBasicInfoDetailGetPatientDetailInfo(pclsCache, patientId);
        //            if (patientInfolist != null)
        //            {
        //                photoAddress = patientInfolist.PhotoAddress;

        //            }

        //            string patientName = "";
        //            patientName = usersMethod.GetNameByUserId(pclsCache, patientId);
        //            PatientListTable NewLine = new PatientListTable();
        //            NewLine.PatientId = patientId;
        //            NewLine.PatientName = patientName;
        //            NewLine.photoAddress = photoAddress;
        //            NewLine.PlanNo = planNo;
        //            NewLine.StartDate = startDate;
        //            NewLine.Process = process;
        //            NewLine.RemainingDays = remainingDays;
        //            NewLine.VitalSign = vitalsigns;
        //            NewLine.ComplianceRate = complianceRate;
        //            NewLine.TotalDays = totalDays;
        //            NewLine.Status = status;

        //            DT_PatientList.Add(NewLine);
                                
        //        }
        //        DS_Patients.DT_PatientList = DT_PatientList;
        //        //The main rates for Plan, Compliance , Goal
        //        planRate = patientTotalCount != 0 ? (double)planCount / patientTotalCount : 0;
        //        complianceRateTotal = planCount != 0 ? (double)complianceCount / planCount : 0;
        //        goalRate = planCount != 0 ? (double)goalCount / planCount : 0;
        //        DT_Rates.PlanRate = planRate;
        //        DT_Rates.ComplianceRate = complianceRateTotal;
        //        DT_Rates.GoalRate = goalRate;

        //        DS_Patients.DT_Rates = DT_Rates;
        //        return DS_Patients;
        //    }
        //    catch (Exception ex)
        //    {
        //        HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetPatientsByDoctorId", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
        //        return null;
        //        throw (ex);
        //    }
        //}

        //public PatientsDataSet GetPatientsListTemp(string DoctorId, string ModuleType, int Plan, int Compliance, int Goal, string itemType, string itemCode)
        //{
        //    int patientTotalCount = 0;  //某个模块下的患者总数
        //    int planCount = 0;          //已有计划的患者数
        //    int complianceCount = 0;    //依从的患者数
        //    int goalCount = 0;          //达标的患者数
        //    double planRate = 0;
        //    double complianceRateTotal = 0;
        //    double goalRate = 0;

        //    RateTable DT_Rates = new RateTable();

        //    List<PatientListTable> DT_PatientList = new List<PatientListTable>();

        //    List<PatientPlan> DT_Patients = new List<PatientPlan>();

        //    PatientsDataSet DS_Patients = new PatientsDataSet();

        //    try
        //    {
        //        int nowDate = commonMethod.GetServerDate(pclsCache);

        //        //同时返回依从率 complianceRate = planInfoMethod.GetComplianceByDate(pclsCache, planNo, nowDate);
        //        //同时输出体征的目标值 增加输入string itemType, string itemCode
        //        //同时输出VitalInfo list = vitalInfoMethod.GetSignByDay(pclsCache, patientId, itemType, itemCode, recordDate);
        //        //PatDetailInfo patientInfolist = moduleInfoMethod.PsBasicInfoDetailGetPatientDetailInfo(pclsCache, patientId);
        //        //patientName = usersMethod.GetNameByUserId(pclsCache, patientId);
        //        DT_Patients = planInfoMethod.GetPatientsPlanByDoctorId(pclsCache, DoctorId, ModuleType);
        //        if (DT_Patients != null)
        //            patientTotalCount = DT_Patients.Count;
        //        else
        //            return DS_Patients;

        //        for (int i = 0; i < patientTotalCount; i++)
        //        {
        //            string patientId = DT_Patients[i].PatientId; // item["PatientId"].ToString();
        //            string planNo = DT_Patients[i].PlanNo;  //item["PlanNo"].ToString();
        //            if (planNo != "")
        //            {
        //                planCount++;
        //            }

        //            //HavePlan 0 1 2
        //            if ((Plan == 1 && planNo == "") || (Plan == 2 && planNo != ""))
        //            {
        //                continue;
        //            }
        //            string startDate = DT_Patients[i].StartDate;   //item["StartDate"].ToString();
        //            string totalDays = DT_Patients[i].TotalDays;   //item["TotalDays"].ToString();
        //            string remainingDays = DT_Patients[i].RemainingDays;   //item["RemainingDays"].ToString();
        //            string status = DT_Patients[i].Status;   //item["Status"].ToString();

        //            double process = 0.0;
        //            double complianceRate = 0.0;
        //            //VitalSign
        //            List<string> vitalsigns = new List<string>();

        //            if (planNo != "")
        //            {               
                                                                 
        //                //Vitalsign 
                  
        //                int recordDate = nowDate; //nowDate
        //                //recordDate = 20150422;
        //                bool goalFlag = false;
        //                VitalInfo list = vitalInfoMethod.GetSignByDay(pclsCache, patientId, itemType, itemCode, recordDate);
        //                if (list != null)
        //                {
        //                    vitalsigns.Add(list.Value);
        //                }
        //                else
        //                {
        //                    //   vitalsigns.Add("115");
        //                    //ZAM 2015-6-17
        //                    vitalsigns.Add("");
        //                }

        //                TargetByCode targetlist = planInfoMethod.GetTarget(pclsCache, planNo, itemType, itemCode);
        //                if (targetlist != null)
        //                {
        //                    vitalsigns.Add(targetlist.Origin);  //index 4 for Origin value
        //                    vitalsigns.Add(targetlist.Value);  //index 3 for target value
        //                }
        //                else
        //                {
        //                    //vitalsigns.Add("200");
        //                    //vitalsigns.Add("120");
        //                    //ZAM 2015-6-17
        //                    vitalsigns.Add("");
        //                    vitalsigns.Add("");
        //                }
        //                //非法数据判断 zam 2015-5-18
        //                if (list != null && targetlist != null)
        //                {
        //                    double m, n;
        //                    bool misNumeric = double.TryParse(list.Value.ToString(), out m);
        //                    bool nisNumeric = double.TryParse(targetlist.Value.ToString(), out n);
        //                    if (misNumeric && nisNumeric)
        //                    {
        //                        //if (Convert.ToInt32(list[2]) <= Convert.ToInt32(targetlist[3])) //已达标
        //                        if (m <= n)
        //                        {
        //                            goalCount++;
        //                            goalFlag = true;
        //                        }
        //                    }
        //                }
        //                //Goal 
        //                if (Goal == 1 && goalFlag == false)
        //                {
        //                    continue;
        //                }
        //                if (Goal == 2 && goalFlag == true)
        //                {
        //                    continue;
        //                }

        //                //非法数据判断 zam 2015-5-18
        //                if (startDate != "" && totalDays != "" && remainingDays != "")
        //                {
        //                    double m, n;
        //                    bool misNumeric = double.TryParse(totalDays, out m);
        //                    bool nisNumeric = double.TryParse(remainingDays, out n);

        //                    if (misNumeric && nisNumeric)
        //                    {
        //                        //process = (Convert.ToDouble(totalDays) - Convert.ToDouble(remainingDays)) / Convert.ToDouble(totalDays);
        //                        process = m != 0.0 ? (m - n) / m : 0;
        //                    }

        //                }
        //            }

        //            //PhotoAddress
        //            string photoAddress = "";
        //            PatDetailInfo patientInfolist = moduleInfoMethod.PsBasicInfoDetailGetPatientDetailInfo(pclsCache, patientId);
        //            if (patientInfolist != null)
        //            {
        //                photoAddress = patientInfolist.PhotoAddress;

        //            }

        //            string patientName = "";
        //            patientName = usersMethod.GetNameByUserId(pclsCache, patientId);
        //            PatientListTable NewLine = new PatientListTable();
        //            NewLine.PatientId = patientId;
        //            NewLine.PatientName = patientName;
        //            NewLine.photoAddress = photoAddress;
        //            NewLine.PlanNo = planNo;
        //            NewLine.StartDate = startDate;
        //            NewLine.Process = process;
        //            NewLine.RemainingDays = remainingDays;
        //            NewLine.VitalSign = vitalsigns;
        //            NewLine.ComplianceRate = complianceRate;
        //            NewLine.TotalDays = totalDays;
        //            NewLine.Status = status;

        //            DT_PatientList.Add(NewLine);

        //        }
        //        DS_Patients.DT_PatientList = DT_PatientList;
        //        //The main rates for Plan, Compliance , Goal
        //        planRate = patientTotalCount != 0 ? (double)planCount / patientTotalCount : 0;
        //        complianceRateTotal = planCount != 0 ? (double)complianceCount / planCount : 0;
        //        goalRate = planCount != 0 ? (double)goalCount / planCount : 0;
        //        DT_Rates.PlanRate = planRate;
        //        DT_Rates.ComplianceRate = complianceRateTotal;
        //        DT_Rates.GoalRate = goalRate;

        //        DS_Patients.DT_Rates = DT_Rates;
        //        return DS_Patients;
        //    }
        //    catch (Exception ex)
        //    {
        //        HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetPatientsByDoctorId", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
        //        return null;
        //        throw (ex);
        //    }
        //}
        #endregion

        /// <summary>
        /// 验证用户是否存在
        /// </summary>
        /// <param name="userId">手机号/邮箱等</param>
        /// <param name="PwType"></param>
        /// <returns></returns>
        public int Verification(DataConnection pclsCache, string userId, string PwType)
        {
            int ret = 0;

            string userID = usersMethod.GetIDByInput(pclsCache, PwType, userId);
            if (userID != "" && userID != null)
            {
                ret = 1; //用户存在
            }
            else
            {
                ret = 2;    // "用户不存在！";
            }

            return ret;
        }

        public PatBasicInfo GetPatBasicInfo(DataConnection pclsCache, string UserId)
        {
            try
            {
                string module = "";
                PatBasicInfo patientInfo = new PatBasicInfo();
                PatientBasicInfo patientList = usersMethod.GetPatientBasicInfo(pclsCache, UserId);
                patientInfo.UserId = UserId;
                if (patientList != null)
                {
                    patientInfo.UserName = patientList.UserName;
                    patientInfo.Age = patientList.Age;
                    patientInfo.Gender = patientList.Gender;
                    patientInfo.BloodType = patientList.BloodType;
                    patientInfo.InsuranceType = patientList.InsuranceType;
                    patientInfo.Birthday = patientList.Birthday;
                    patientInfo.GenderText = patientList.GenderText;
                    patientInfo.BloodTypeText = patientList.BloodTypeText;
                    patientInfo.InsuranceTypeText = patientList.InsuranceTypeText;
                    patientInfo.DoctorId = patientList.DoctorId;
                    patientInfo.IDNo = patientList.IDNo;
                }

                List<TypeAndName> modules = moduleInfoMethod.PsBasicInfoDetailGetModulesByPID(pclsCache, UserId);
                for (int i = 0; i < modules.Count; i++)
                {
                    module = module + "|" + modules[i].Name;
                }
                if (module != "")
                {
                    module = module.Substring(1, module.Length - 1);
                }
                patientInfo.Module = module;
                return patientInfo;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetPatBasicInfo", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
                throw ex;
            }
        }

        public PatientDetailInfo GetPatientDetailInfo(DataConnection pclsCache, string UserId)
        {
            try
            {
                string module = "";
                PatientDetailInfo PatientDetailInfo = new PatientDetailInfo();
                PatDetailInfo GetPatientDetailInfoList = moduleInfoMethod.PsBasicInfoDetailGetPatientDetailInfo(pclsCache, UserId);
                PatientDetailInfo.UserId = UserId;
                if (GetPatientDetailInfoList != null)
                {
                    PatientDetailInfo.PhoneNumber = GetPatientDetailInfoList.PhoneNumber;
                    if (PatientDetailInfo.PhoneNumber == null)
                    {
                        PatientDetailInfo.PhoneNumber = "";
                    }
                    PatientDetailInfo.HomeAddress = GetPatientDetailInfoList.HomeAddress;
                    if (PatientDetailInfo.HomeAddress == null)
                    {
                        PatientDetailInfo.HomeAddress = "";
                    }
                    PatientDetailInfo.Occupation = GetPatientDetailInfoList.Occupation;
                    if (PatientDetailInfo.Occupation == null)
                    {
                        PatientDetailInfo.Occupation = "";
                    }
                    PatientDetailInfo.Nationality = GetPatientDetailInfoList.Nationality;
                    if (PatientDetailInfo.Nationality == null)
                    {
                        PatientDetailInfo.Nationality = "";
                    }
                    PatientDetailInfo.EmergencyContact = GetPatientDetailInfoList.EmergencyContact;
                    if (PatientDetailInfo.EmergencyContact == null)
                    {
                        PatientDetailInfo.EmergencyContact = "";
                    }
                    PatientDetailInfo.EmergencyContactPhoneNumber = GetPatientDetailInfoList.EmergencyContactPhoneNumber;
                    if (PatientDetailInfo.EmergencyContactPhoneNumber == null)
                    {
                        PatientDetailInfo.EmergencyContactPhoneNumber = "";
                    }
                    PatientDetailInfo.PhotoAddress = GetPatientDetailInfoList.PhotoAddress;
                    if (PatientDetailInfo.PhotoAddress == null)
                    {
                        PatientDetailInfo.PhotoAddress = "";
                    }
                    PatientDetailInfo.IDNo = GetPatientDetailInfoList.IDNo;
                    if (PatientDetailInfo.IDNo == null)
                    {
                        PatientDetailInfo.IDNo = "";
                    }
                    PatientDetailInfo.Height = GetPatientDetailInfoList.Height;
                    if (PatientDetailInfo.Height == null)
                    {
                        PatientDetailInfo.Height = "";
                    }
                    PatientDetailInfo.Weight = GetPatientDetailInfoList.Weight;
                    if (PatientDetailInfo.Weight == null)
                    {
                        PatientDetailInfo.Weight = "";
                    }
                }

                List<TypeAndName> modules = moduleInfoMethod.PsBasicInfoDetailGetModulesByPID(pclsCache, UserId);
                for (int i = 0; i < modules.Count; i++)
                {
                    module = module + "|" + modules[i].Name;
                }
                if (module != "")
                {
                    module = module.Substring(1, module.Length - 1);
                }
                PatientDetailInfo.Module = module;
                return PatientDetailInfo;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.BasicInfoDetail.GetPatientDetailInfo", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
                throw (ex);
            }

        }


        public DocInfoDetail GetDoctorDetailInfo(DataConnection pclsCache, string UserId)
        {
            return moduleInfoMethod.PsDoctorInfoDetailGetDoctorInfoDetail(pclsCache, UserId);
        }

        public DoctorInfo GetDoctorInfo(DataConnection pclsCache, string DoctorId)
        {
            return usersMethod.GetDoctorInfo(pclsCache, DoctorId);
        }
        public int SetDoctorInfoDetail(DataConnection pclsCache, string Doctor, string CategoryCode, string ItemCode, int ItemSeq, string Value, string Description, int SortNo, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            try
            {
                int IsSaved = 2;
                IsSaved = moduleInfoMethod.PsDoctorInfoDetailSetData(pclsCache, Doctor, CategoryCode, ItemCode, ItemSeq, Value, Description, SortNo, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                return IsSaved;
            }
            catch (Exception ex)
            {
                return 0;
                throw (ex);
            }
        }
        public int SetPsDoctor(DataConnection pclsCache, string UserId, string UserName, int Birthday, int Gender, string IDNo, int InvalidFlag, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            try
            {
                int IsSaved = 2;
                IsSaved = usersMethod.PsDoctorInfoSetData(pclsCache, UserId, UserName, Birthday, Gender, IDNo, InvalidFlag, revUserId, TerminalName, TerminalIP, DeviceType);
                return IsSaved;
            }
            catch (Exception ex)
            {
                return 0;
                throw (ex);
            }
        }

        public int SetPatBasicInfo(DataConnection pclsCache, string UserId, string UserName, int Birthday, int Gender, int BloodType, string IDNo, string DoctorId, string InsuranceType, int InvalidFlag, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            try
            {
                return usersMethod.PsBasicInfoSetData(pclsCache, UserId, UserName, Birthday, Gender, BloodType, IDNo, DoctorId, InsuranceType, InvalidFlag, revUserId, TerminalName, TerminalIP, DeviceType);
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "SetPatBasicInfo", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return 0;
                throw ex;
            }
        }

        /// <summary>
        /// 插入患者详细信息 LY 2015-10-14
        /// </summary>
        /// <param name="Patient"></param>
        /// <param name="CategoryCode"></param>
        /// <param name="ItemCode"></param>
        /// <param name="ItemSeq"></param>
        /// <param name="Value"></param>
        /// <param name="Description"></param>
        /// <param name="SortNo"></param>
        /// <param name="revUserId"></param>
        /// <param name="TerminalName"></param>
        /// <param name="TerminalIP"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>
        public int SetPatBasicInfoDetail(DataConnection pclsCache, string Patient, string CategoryCode, string ItemCode, int ItemSeq, string Value, string Description, int SortNo, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            return new ModuleInfoMethod().PsBasicInfoDetailSetData(pclsCache, Patient, CategoryCode, ItemCode, ItemSeq, Value, Description, SortNo, revUserId, TerminalName, TerminalIP, DeviceType);
        }

        public string GetIDByInputPhone(DataConnection pclsCache, string Type, string Name)
        {
            return new UsersMethod().GetIDByInputPhone(pclsCache, Type, Name);
        }
        public List<Calendar> GetCalendar(DataConnection pclsCache, string DoctorId)
        {
            return usersMethod.GetCalendar(pclsCache, DoctorId);

        }
        public List<PatientListTable> GetPatientsPlan(DataConnection pclsCache, string DoctorId, string Module, string VitalType, string VitalCode)
        {

            List<PatientListTable> items = new List<PatientListTable>();
            if (Module == "{Module}")
            {
                string[] Mu = new string[] { "HM1", "HM2", "HM3" };
                for (int i = 0; i<Mu.Length; i++)
                {
                    List<PatientListTable> item = new List<PatientListTable>();
                    item = new PlanInfoMethod().GetPatientsPlan(pclsCache, DoctorId, Mu[i], VitalType, VitalCode);
                    if (item != null)
                    {
                        items.AddRange(item);
                    }
                }
            }
            else
            {
                items = new PlanInfoMethod().GetPatientsPlan(pclsCache, DoctorId, Module, VitalType, VitalCode);
            }
            for (int i = 0; i < items.Count; i++)
            {
                for(int j=i+1; j<items.Count; j++)
                {
                    if(items[i].PatientId == items[j].PatientId)
                    {
                        items[i].Module = items[i].Module + "/" + items[j].Module;
                        items.RemoveAt(j);
                        j--;
                    }
                }
            }
                return items;

        }

        public int SetComment(DataConnection pclsCache, string DoctorId, string CategoryCode, string Value, string Description, string SortNo, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            return new UsersMethod().SetComment(pclsCache, DoctorId, CategoryCode, Value, Description, SortNo, piUserId, piTerminalName, piTerminalIP, piDeviceType);
        }

        public int PsCalendarSetData(DataConnection pclsCache, string DoctorId, int DateTime, string Period, int SortNo, string Description, int Status, string Redundancy, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            return new PlanInfoMethod().PsCalendarSetData(pclsCache, DoctorId, DateTime, Period, SortNo, Description, Status, Redundancy, revUserId, TerminalName, TerminalIP, DeviceType);
        }

        #region<新增bySYF>
        public List<HealthCoachList> GetHealthCoachList(DataConnection pclsCache)
        {
            return usersMethod.GetHealthCoachList(pclsCache);
        }

        public HealthCoachInfo GetHealthCoachInfo(DataConnection pclsCache, string HealthCoachID)
        {
            return usersMethod.GetHealthCoachInfo(pclsCache, HealthCoachID);
        }

        public int ReserveHealthCoach(DataConnection pclsCache, string DoctorId, string PatientId, string Module, string Description, int Status, DateTime ApplicationTime, DateTime AppointmentTime, string AppointmentAdd, string Redundancy, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            return usersMethod.ReserveHealthCoach(pclsCache, DoctorId, PatientId, Module, Description, Status, ApplicationTime, AppointmentTime, AppointmentAdd, Redundancy, revUserId, TerminalName, TerminalIP, DeviceType);
        }

        public int UpdateReservation(DataConnection pclsCache, string DoctorId, string PatientId, int Status, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            return usersMethod.UpdateReservation(pclsCache, DoctorId, PatientId, Status, revUserId, TerminalName, TerminalIP, DeviceType);
        }

        public List<CommentList> GetCommentList(DataConnection pclsCache, string DoctorId, string CategoryCode)
        {
            return usersMethod.GetCommentList(pclsCache, DoctorId, CategoryCode);
        }

        public List<HealthCoachListByPatient> GetHealthCoachListByPatient(DataConnection pclsCache, string PatientId)
        {
            return usersMethod.GetHealthCoachListByPatient(pclsCache, PatientId);
        }


        public int RemoveHealthCoach(DataConnection pclsCache, string PatientId, string DoctorId, string CategoryCode)
        {
            return usersMethod.RemoveHealthCoach(pclsCache, PatientId, DoctorId, CategoryCode);
        }

        public List<AppoitmentPatient> GetAppoitmentPatientList(DataConnection pclsCache, string healthCoachID, string Status)
        {
            return usersMethod.GetAppoitmentPatientList(pclsCache, healthCoachID, Status);
        }

        public List<string> GetAllRoleMatch(DataConnection pclsCache, string UserId)
        {
            return usersMethod.GetAllRoleMatch(pclsCache, UserId);

        }

        public string GetPhoneNoByUserId(DataConnection pclsCache, string UserId)
        {
            return usersMethod.GetPhoneNoByUserId(pclsCache,UserId);
        }
       
        public string GetBasicInfoDtlValue(DataConnection pclsCache, string UserId, string CategoryCode, string ItemCode, int ItemSeq)
        {
            return usersMethod.GetPatientValue(pclsCache, UserId, CategoryCode, ItemCode, ItemSeq);
        }

        #endregion

        public List<ModulesByPID> GetHModulesByID(DataConnection pclsCache, string PatientId, string DoctorId)
        {
            return new UsersMethod().GetHModulesByID(pclsCache, PatientId, DoctorId);
        }

        public string GetValueByType(DataConnection pclsCache, string UserId, string Type)
        {
            return new UsersMethod().GetValueByType(pclsCache, UserId, Type);
        }

        public int PsConsultationSetData(DataConnection pclsCache, string DoctorId, string PatientId, int SortNo, DateTime ApplicationTime, string HealthCoachId, string Module, string Title, string Description, DateTime ConsultTime, string Solution, int Emergency, int Status, string Redundancy, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            return new UsersMethod().PsConsultationSetData(pclsCache, DoctorId, PatientId, SortNo, ApplicationTime, HealthCoachId, Module, Title, Description, ConsultTime, Solution, Emergency, Status, Redundancy, revUserId, TerminalName, TerminalIP, DeviceType);
        }

        public List<ConsultationStatus> ConsultationGetPatientsByStatus(DataConnection pclsCache, string DoctorId, int Status)
        {
            List<ConsultationStatus> items = new List<ConsultationStatus>();
            if(Status == 7)
            {
                for(int i=3; i>0; i--)
                {
                    List<ConsultationStatus> item = new List<ConsultationStatus>();
                    item = new UsersMethod().ConsultationGetPatientsByStatus(pclsCache, DoctorId, i);
                    if(item != null)
                    {
                        items.AddRange(item);
                    }                   
                }
            }
            else if(Status == 8)
            {
                for (int i=6; i>3; i--)
                {
                    List<ConsultationStatus> item = new List<ConsultationStatus>();
                    item = new UsersMethod().ConsultationGetPatientsByStatus(pclsCache, DoctorId, i);
                    if (item != null)
                    {
                        items.AddRange(item);
                    }
                }
            }
            else
            {
                items = new UsersMethod().ConsultationGetPatientsByStatus(pclsCache, DoctorId, Status);
            }
            return items;
        }

        public int ConsultationChangeStatus(DataConnection pclsCache, string DoctorId, string PatientId, int SortNo, int Status, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            return new UsersMethod().ConsultationChangeStatus(pclsCache, DoctorId, PatientId, SortNo, Status, revUserId, TerminalName, TerminalIP, DeviceType);
        }

        public List<ConsultationDP> ConsultationGetDataByDP(DataConnection pclsCache, string DoctorId, string PatientId)
        {
            return new UsersMethod().ConsultationGetDataByDP(pclsCache, DoctorId, PatientId);
        }

<<<<<<< HEAD
        public List<Doctor> GetDoctorList(DataConnection pclsCache)
        {
            return new UsersMethod().GetDoctorList(pclsCache);
        }
=======

        #region<ConsultationHid Model>
        public List<ConsultationHid> GetConsultationDataByHidPid1(DataConnection pclsCache, string HealthCoachId1)
        {
            return new UsersMethod().GetConsultationDataByHid(pclsCache, HealthCoachId1);
        }

        public List<ConsultationHid> GetConsultationDataByHidPid2(DataConnection pclsCache, string HealthCoachId, int status)
        {
            List<ConsultationHid> items = new List<ConsultationHid>();
            items = new UsersMethod().GetConsultationDataByHid(pclsCache, HealthCoachId);
            List<ConsultationHid> res = new List<ConsultationHid>();

            foreach (ConsultationHid item in items)
            {
                int statustemp = item.Status;
                if (status == 7)
                {
                    if ((statustemp == 1) || (statustemp == 2) || (statustemp == 3))
                    {
                        if (item != null)
                        {
                            res.Add(item);
                        }
                    }
                }
                if (status == 8)
                {
                    if ((statustemp == 4) || (statustemp == 5) || (statustemp == 6))
                    {
                        if (item != null)
                        {
                            res.Add(item);
                        }
                    }
                }
                else
                {
                    if (status == statustemp)
                    {
                        res.Add(item);
                    }
                }
            }
            return res;
        }

        public List<ConsultationHid> GetConsultationDataByHidPid3(DataConnection pclsCache, string HealthCoachId, string PatientId)
        {
            List<ConsultationHid> items = new List<ConsultationHid>();
            items = new UsersMethod().GetConsultationDataByHid(pclsCache, HealthCoachId);
            List<ConsultationHid> res = new List<ConsultationHid>();

            foreach (ConsultationHid item in items)
            {
                if (item.PatientId == PatientId)
                {
                    res.Add(item);
                }
            }
            return res;
        }

        public List<ConsultationHid> GetConsultationDataByHidPid4(DataConnection pclsCache, string HealthCoachId, string PatientId, int status)
        {
            List<ConsultationHid> items = new List<ConsultationHid>();
            items = new UsersMethod().GetConsultationDataByHid(pclsCache, HealthCoachId);
            List<ConsultationHid> res1 = new List<ConsultationHid>();
            List<ConsultationHid> res2 = new List<ConsultationHid>();

            foreach (ConsultationHid item in items)
            {
                if (item.PatientId == PatientId)
                {
                    res1.Add(item);
                }
            }

            foreach (ConsultationHid item in res1)
            {
                int statustemp = item.Status;
                if (status == 7)
                {
                    if ((statustemp == 1) || (statustemp == 2) || (statustemp == 3))
                    {
                        if (item != null)
                        {
                            res2.Add(item);
                        }
                    }
                }
                if (status == 8)
                {
                    if ((statustemp == 4) || (statustemp == 5) || (statustemp == 6))
                    {
                        if (item != null)
                        {
                            res2.Add(item);
                        }
                    }
                }
                else
                {
                    if (status == statustemp)
                    {
                        res2.Add(item);
                    }
                }
            }

            return res2;
        }
        #endregion
>>>>>>> c12f01ed7ca9782af3c513bca526610e52bba453
    }
}