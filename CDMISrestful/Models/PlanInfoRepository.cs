using CDMISrestful.CommonLibrary;
using CDMISrestful.DataMethod;
using CDMISrestful.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDMISrestful.Models
{
    public class PlanInfoRepository : IPlanInfoRepository
    {
      

        //CSQ 20151026 获取任务子表数据
        public List<TaskDetail> GetTaskDetails(DataConnection pclsCache, string CategoryCode, string Code)
        {
            return new PlanInfoMethod().GetTaskDetails(pclsCache, CategoryCode, Code);
        }

        //专员建立计划模板 父表写数 csq 20151026
        public int PsTemplateSetData(DataConnection pclsCache, string DoctorId, int TemplateCode, string TemplateName, string Description, DateTime RecordDate, string Redundance, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            return new PlanInfoMethod().PsTemplateSetData(pclsCache, DoctorId, TemplateCode, TemplateName, Description, RecordDate, Redundance,  piUserId, piTerminalName, piTerminalIP, piDeviceType);
        }

         //专员建立计划模板 子表写数 csq 20151026
        public int PsTemplateDetailSetData(DataConnection pclsCache, string DoctorId, int TemplateCode, string CategoryCode, string ItemCode, string Value, string Description, string Redundance, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            return new PlanInfoMethod().PsTemplateDetailSetData(pclsCache, DoctorId, TemplateCode, CategoryCode, ItemCode, Value,Description, Redundance, piUserId, piTerminalName, piTerminalIP, piDeviceType);
        }

        
        //获取专员计划模板列表 CSQ 20151026
        public List<TemplateInfo> GetTemplateList(DataConnection pclsCache, string DoctorId)
        {
            return new PlanInfoMethod().GetTemplateList(pclsCache, DoctorId);
        }

        //获取专员某个计划模板的详细信息 CSQ 20151026
        public List<TemplateInfoDtl> GetTemplateDetails(DataConnection pclsCache, string DoctorId, string TemplateCode, string ParentCode)
        {
            return new PlanInfoMethod().GetTemplateDetails(pclsCache, DoctorId, TemplateCode, ParentCode);
        }

        //Ps.Plan.SetData GL 2015-10-13
        public int SetPlan(DataConnection pclsCache, string PlanNo, string PatientId, int StartDate, int EndDate, string Module, int Status, string DoctorId, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            return new PlanInfoMethod().PsPlanSetData(pclsCache, PlanNo, PatientId, StartDate, EndDate, Module, Status, DoctorId, piUserId, piTerminalName, piTerminalIP, piDeviceType);
        }

        //Ps.ComplianceDetail.SetData CSQ 20151027
        public int SetComplianceDetail(DataConnection pclsCache, string PlanNo, int Date, string CategoryCode, string Code, string SortNo, int Status, string Description, string CoUserId, string CoTerminalName, string CoTerminalIP, int CoDeviceType)
        {
            return new PlanInfoMethod().PsComplianceDetailSetData(pclsCache, PlanNo, Date, CategoryCode, Code,SortNo, Status, Description, CoUserId, CoTerminalName, CoTerminalIP, CoDeviceType);
        }

        public TargetByCode GetTarget(DataConnection pclsCache, string PlanNo, string Type, string Code)
        {
            return new PlanInfoMethod().GetTarget(pclsCache, PlanNo, Type, Code);
        }



        //创建计划 GL 2015-10-13
        public int CreateTask(DataConnection pclsCache, string PlanNo, string Type, string Code, string SortNo, string Instruction, string UserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            try
            {
                int ret = 3;                      
                ret = new PlanInfoMethod().PsTaskSetData(pclsCache, PlanNo,  Type, Code, SortNo,Instruction, UserId, TerminalName, TerminalIP, DeviceType);
                
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CreateTask ", "PlanInfoRepository！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return 2;
                throw (ex);
            }
        }

        //Ps.Compliance.SetData CSQ 20151027
        public int SetCompliance(DataConnection pclsCache, string PlanNo, int Date, Double Compliance, string Description, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            return new PlanInfoMethod().PsComplianceSetData(pclsCache, PlanNo, Date, Compliance, Description, revUserId, TerminalName, TerminalIP, DeviceType);
        }


        //Ps.Target.SetData CSQ 20151027
        public int SetTarget(DataConnection pclsCache, string Plan, string Type, string Code, string Value, string Origin, string Instruction, string Unit, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            return new PlanInfoMethod().PsTargetSetData(pclsCache, Plan, Type, Code, Value, Origin, Instruction, Unit, piUserId, piTerminalName, piTerminalIP, piDeviceType);
        }

        //获取某计划的进度和剩余天数（取PlanNo，StartDate，EndDate） GL 2015-10-13
        public GPlanInfo GetPlanInfo(DataConnection pclsCache, string PlanNo)
        {
           GPlanInfo ret = new PlanInfoMethod().GetPlanInfo(pclsCache, PlanNo);
           ret.PlanCompliance = new PlanInfoMethod().GetComplianceByPlanNo(pclsCache, PlanNo).ToString();
           return ret;
        }

        //通过某计划的日期，获取该天的任务完成详情 用于图上点点击时弹框内容 GL 2015-10-13
        public TaskComDetailByD GetImplementationByDate(DataConnection pclsCache, string PatientId, string PlanNo, string DateSelected)
        {
            TaskComDetailByD TaskComDetailByD = new TaskComDetailByD(); //voidDateTime
            //string str_result = "";  //最终的输出-ImplementationInfo转化成json格式
            try
            {
                //DateSelected形式"20150618" 或"15/06/18"  目前使用前者
                int Date = Convert.ToInt32(DateSelected);
                TaskComDetailByD = new PlanInfoMethod().GetImplementationByDate(pclsCache, PatientId, PlanNo, Convert.ToInt32(Date));

                //str_result = JSONHelper.ObjectToJson(TaskComDetailByD);
                //Context.Response.BinaryWrite(new byte[] { 0xEF, 0xBB, 0xBF });
                //Context.Response.Write(str_result);
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                //Context.Response.End();
                return TaskComDetailByD;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetImplementationByDate", "PlanInfoRepository error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                //return null;
                throw (ex);
            }
        }

        //获取某体征的数据和画图信息（收缩压、舒张压、脉率) Pad和Phone都要用 GL 2015-10-13
        //关于输入 StartDate，EndDate  Pad首次没有拿出StartDate，EndDate    Phone拿出了 这样规划比较好
        public ChartData GetSignInfoByCode(DataConnection pclsCache, string PatientId, string PlanNo, string ItemCode, int StartDate, int EndDate)
        {
            ChartData ChartData = new ChartData();
            List<Graph> GraphList = new List<Graph>();
            GraphGuide GraphGuide = new GraphGuide();
            List<MstBloodPressure> reference = new List<MstBloodPressure>();

            try
            {
                string Module = "";
                GPlanInfo planInfo = new PlanInfoMethod().GetPlanInfo(pclsCache, PlanNo);
                if (planInfo != null)
                {
                    Module = planInfo.Module;

                }

                if (Module == "M1")
                {
                    if ((ItemCode == "Bloodpressure|Bloodpressure_1") || (ItemCode == "Bloodpressure|Bloodpressure_2"))
                    {
                        reference = new PlanInfoMethod().GetBPGrades(pclsCache);
                    }

                    GraphList = new PlanInfoMethod().GetSignInfoByM1(pclsCache, PatientId, PlanNo, ItemCode, StartDate, EndDate, reference);

                    //初始值、目标值、分级规则加工
                    if (GraphList != null)
                    {
                        if (GraphList.Count > 0)
                        {
                            GraphGuide = new PlanInfoMethod().GetGuidesByCode(pclsCache, PlanNo, ItemCode, reference);
                            ChartData.GraphGuide = GraphGuide;
                        }
                    }
                }

                //读取任务列表  必有测量任务，其他任务（例如吃药）可能没有  20151027 需要修改
                //List<PsTask> TaskList = new PlanInfoMethod().GetTaskList(pclsCache, PlanNo);
                List<PsTask> TaskList = new List<PsTask>();
                List<PsTask> VitalSignRows = new List<PsTask>();
                foreach (PsTask item in TaskList)
                {
                    if (item.Type == "VitalSign")
                    {
                        VitalSignRows.Add(item);
                    }
                }
                //其他任务依从情况
                List<CompliacneDetailByD> TasksComByPeriod = new List<CompliacneDetailByD>();
                //是否有其他任务
                if (TaskList.Count == VitalSignRows.Count)
                {
                    ChartData.OtherTasks = "0";
                }
                else
                {
                    ChartData.OtherTasks = "1";
                    TasksComByPeriod = new PlanInfoMethod().GetTasksComCountByPeriod(pclsCache, PatientId, PlanNo, StartDate, EndDate);
                    if ((TasksComByPeriod != null) && (TasksComByPeriod.Count == GraphList.Count))
                    {
                        for (int rowsCount = 0; rowsCount < TasksComByPeriod.Count; rowsCount++)
                        {
                            GraphList[rowsCount].DrugValue = "1";   //已经初始化过
                            GraphList[rowsCount].DrugBullet = TasksComByPeriod[rowsCount].drugBullet;
                            GraphList[rowsCount].DrugColor = TasksComByPeriod[rowsCount].drugColor;
                            GraphList[rowsCount].DrugDescription = TasksComByPeriod[rowsCount].Events;
                        }
                    }
                }
                ChartData.GraphList = GraphList;


                return ChartData;
                //string a = JSONHelper.ObjectToJson(ChartData);
                //Context.Response.BinaryWrite(new byte[] { 0xEF, 0xBB, 0xBF });
                //Context.Response.Write(a);
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                //Context.Response.End();
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetSignInfoByCode", "PlanInfoRepository error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
                //throw (ex);
            }
        }

        //更新计划状态 GL 2015-10-13
        public int SetPlanStart(DataConnection pclsCache, string PlanNo, int Status, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            return new PlanInfoMethod().PlanStart(pclsCache, PlanNo, Status, piUserId, piTerminalName, piTerminalIP, piDeviceType);
        }

        #region 暂时不用

        ////根据患者Id，获取药物治疗列表 GL 2015-10-13
        //public List<PsDrugRecord> GetPatientDrugRecord(DataConnection pclsCache, string PatientId, string Module)
        //{
        //    try
        //    {
        //        List<PsDrugRecord> list = new List<PsDrugRecord>();
        //        list = new ClinicInfoMethod().GetPsDrugRecord(pclsCache, PatientId, Module);
        //        if (list != null)
        //        {
        //            if (list.Count > 0) //排序(降序)
        //            {
        //                list.Sort((x, y) => -(x.StartDateTime).CompareTo(y.StartDateTime));
        //            }
        //        }
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetPatientDrugRecord", "PlanInfoRepository error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
        //        return null;
        //        throw (ex);
        //    }
        //}

        ////获取计划完成情况（Pad)-首次进入页面 PlanNo为空 GL 2015-10-13
        //public ImplementationInfo GetImplementationForPadFirst(string PatientId, string Module)
        //{
        //    ImplementationInfo ImplementationInfo = new ImplementationInfo();
        //    string str_result = "";  //最终的输出-ImplementationInfo转化成json格式
        //    try
        //    {
        //        //与模块特性无关的公共项 ——病人基本信息、计划列表、计划进度、体征切换  不同模块可共用
        //        string PlanNo = "";
        //        //病人基本信息-姓名、头像..
        //        PatDetailInfo patientList = new ModuleInfoMethod().PsBasicInfoDetailGetPatientDetailInfo(pclsCache, PatientId);
        //        if (patientList != null)
        //        {
        //            ImplementationInfo.PatientInfo.PatientName = patientList.PhoneNumber;

        //            PatDetailInfo BasicInfoDetail = new ModuleInfoMethod().PsBasicInfoDetailGetPatientDetailInfo(pclsCache, PatientId);
        //            if (BasicInfoDetail != null)
        //            {
        //                if (BasicInfoDetail.PhotoAddress != null)
        //                {
        //                    ImplementationInfo.PatientInfo.ImageUrl = BasicInfoDetail.PhotoAddress;
        //                }
        //                else
        //                {
        //                    ImplementationInfo.PatientInfo.ImageUrl = "";  //js端意外不能识别null
        //                }

        //            }
        //        }

        //        //刚进入页面加载计划列表 (始终存在第一条-当前计划）
        //        ImplementationInfo.PlanList = new PlanInfoMethod().GetPlanList34ByM(pclsCache, PatientId, Module);

        //        PlanNo = ImplementationInfo.PlanList[0].PlanNo; //肯定会存在 

        //        #region  存在正在执行的计划

        //        if ((PlanNo != "") && (PlanNo != null))  //存在正在执行的计划
        //        {
        //            //剩余天数和进度
        //            Progressrate PRlist = new PlanInfoMethod().GetProgressRate(pclsCache, PlanNo);
        //            if (PRlist != null)
        //            {
        //                ImplementationInfo.RemainingDays = PRlist.RemainingDays;
        //                ImplementationInfo.ProgressRate = (Convert.ToDouble(PRlist.ProgressRate) * 100).ToString();

        //                ImplementationInfo.StartDate = ImplementationInfo.PlanList[0].StartDate;
        //                ImplementationInfo.EndDate = ImplementationInfo.PlanList[0].EndDate;
        //            }

        //            //正在执行计划的最近一周的依从率
        //            Period weekPeriod = new PlanInfoMethod().GetWeekPeriod(pclsCache, ImplementationInfo.PlanList[0].StartDate);
        //            if (weekPeriod != null)
        //            {
        //                ImplementationInfo.CompliacneValue = "最近一周依从率为：" + new PlanInfoMethod().GetCompliacneRate(pclsCache, PatientId, PlanNo, Convert.ToInt32(weekPeriod.StartDate), Convert.ToInt32(weekPeriod.EndDate)) + "%";
        //            }

        //            //读取任务列表  20151027 需要修改
        //            //List<PsTask> TaskList = new PlanInfoMethod().GetTaskList(pclsCache, PlanNo);
        //            List<PsTask> TaskList = new List<PsTask>();
        //            //ImplementationInfo.TaskList = PsTask.GetSpTaskList(pclsCache, PlanNo);

        //            //测量-体征切换下拉框  
        //            List<PsTask> VitalSignRows = new List<PsTask>();
        //            foreach (PsTask item in TaskList)
        //            {
        //                if (item.Type == "VitalSign")
        //                {
        //                    VitalSignRows.Add(item);
        //                }
        //            }
        //            List<SignShow> SignList = new List<SignShow>();
        //            foreach (PsTask item in VitalSignRows)
        //            {
        //                SignShow SignShow = new SignShow();
        //                SignShow.SignName = item.Name;
        //                SignShow.SignCode = item.Code;
        //                SignList.Add(SignShow);
        //            }
        //            ImplementationInfo.SignList = SignList;


        //            List<MstBloodPressure> reference = new List<MstBloodPressure>();
        //            ChartData ChartData = new ChartData();
        //            List<Graph> GraphList = new List<Graph>();
        //            GraphGuide GraphGuide = new GraphGuide();

        //            if (Module == "M1")  //后期维护的话，在这里添加不同的模块判断
        //            {

        //                //高血压模块  体征测量-血压（收缩压、舒张压）、脉率   【默认显示-收缩压，血压必有，脉率可能有】  
        //                List<PsTask> HyperTensionRows = new List<PsTask>();
        //                foreach (PsTask item in TaskList)
        //                {
        //                    if ((item.Code == "Bloodpressure|Bloodpressure_1") || (item.Code == "Bloodpressure|Bloodpressure_2") || (item.Code == "Pulserate|Pulserate_1"))
        //                    {
        //                        HyperTensionRows.Add(item);
        //                    }
        //                }
        //                //注意：需要兼容之前没有脉率的情况
        //                if ((HyperTensionRows != null) && (HyperTensionRows.Count >= 2))  //M1 收缩压（默认显示）、舒张压、脉率  前两者肯定有，脉率不一定有
        //                {
        //                    //从数据库获取血压的分级规则，脉率的分级原则写死在webservice
        //                    reference = new PlanInfoMethod().GetBPGrades(pclsCache);

        //                    //首次进入，默认加载收缩压
        //                    GraphList = new PlanInfoMethod().GetSignInfoByM1(pclsCache, PatientId, PlanNo, "Bloodpressure|Bloodpressure_1", ImplementationInfo.PlanList[0].StartDate, ImplementationInfo.PlanList[0].EndDate, reference);

        //                    //初始值、目标值、分级规则加工
        //                    if (GraphList.Count > 0)
        //                    {
        //                        GraphGuide = new PlanInfoMethod().GetGuidesByCode(pclsCache, PlanNo, "Bloodpressure|Bloodpressure_1", reference);
        //                        ChartData.GraphGuide = GraphGuide;
        //                    }
        //                }

        //            }
        //            else
        //            {

        //            }


        //            //必有测量任务，其他任务（例如吃药）可能没有

        //            //其他任务依从情况  所有模块共有的
        //            List<CompliacneDetailByD> TasksComByPeriod = new List<CompliacneDetailByD>();
        //            //是否有其他任务
        //            //string condition1 = " Type not in ('VitalSign,')";
        //            if (TaskList.Count == VitalSignRows.Count)
        //            {
        //                ChartData.OtherTasks = "0";
        //            }
        //            else
        //            {
        //                ChartData.OtherTasks = "1";
        //                TasksComByPeriod = new PlanInfoMethod().GetTasksComCountByPeriod(pclsCache, PatientId, PlanNo, ImplementationInfo.PlanList[0].StartDate, ImplementationInfo.PlanList[0].EndDate);
        //                if ((TasksComByPeriod != null) && (TasksComByPeriod.Count == GraphList.Count)) //体征的数据条数一定等于其他任务的条数（天数） ，都是按照compliance的date统计的
        //                {
        //                    for (int rowsCount = 0; rowsCount < TasksComByPeriod.Count; rowsCount++)
        //                    {
        //                        GraphList[rowsCount].DrugValue = "1";   //已经初始化过
        //                        GraphList[rowsCount].DrugBullet = TasksComByPeriod[rowsCount].drugBullet;
        //                        GraphList[rowsCount].DrugColor = TasksComByPeriod[rowsCount].drugColor;
        //                        GraphList[rowsCount].DrugDescription = TasksComByPeriod[rowsCount].Events;//+ "<br><a onclick= shuang shuang zz(); shuang shuang;>详细</a>"
        //                    }
        //                }
        //            }

        //            ChartData.GraphList = GraphList;
        //            ImplementationInfo.ChartData = ChartData;
        //        }

        //        #endregion

        //        //str_result = JSONHelper.ObjectToJson(ImplementationInfo);
        //        //Context.Response.BinaryWrite(new byte[] { 0xEF, 0xBB, 0xBF });
        //        //Context.Response.Write(str_result);
        //        //HttpContext.Current.ApplicationInstance.CompleteRequest();
        //        ////Context.Response.End();
        //        return ImplementationInfo;
        //    }
        //    catch (Exception ex)
        //    {
        //        HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetImplementationForPadFirst", "PlanInfoRepository error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
        //        //return null;
        //        throw (ex);
        //    }
        //}

        ///// <summary>
        ///// 需要修改 20151029
        ///// </summary>
        ///// <param name="UserId"></param>
        ///// <param name="PlanNo"></param>
        ///// <param name="StartDate"></param>
        ///// <param name="EndDate"></param>
        ///// <param name="ItemType"></param>
        ///// <param name="ItemCode"></param>
        ///// <returns></returns>
        //public List<ComplianceAllSignsListByPeriod> GetComplianceAllSignsListByPeriod(string UserId, string PlanNo, int StartDate, int EndDate)
        //{
        //    List<ComplianceAllSignsListByPeriod> items = new List<ComplianceAllSignsListByPeriod>();
        //    //依从率
        //    List<ComplianceListByPeriod> items1 = new PlanInfoMethod().GetComplianceListByPeriod(pclsCache, PlanNo, StartDate, EndDate);
        //    //体征
        //    //List<SignByPeriod> items2 = new PlanInfoMethod().GetSignByPeriod(pclsCache,UserId,ItemType,ItemCode,StartDate,EndDate);
        //    List<VitalInfo> items2 = new VitalInfoMethod().GetAllSignsByPeriod(pclsCache, UserId, StartDate, EndDate);                   

        //    for (int i = 0; i < items1.Count;i++ )
        //    {
        //        ComplianceAllSignsListByPeriod item = new ComplianceAllSignsListByPeriod();
        //        item.Date = items1[i].Date.ToString();
        //        item.Compliance = Math.Round(items1[i].Compliance*100,0)+"%";
        //        item.Description = items1[i].Description;
        //        if(items1[i].Compliance==1)
        //        {
        //            item.BulletColor = "#77777";
        //        }
        //        else if (items1[i].Compliance == 0)
        //        {
        //            item.BulletColor = "#DADADA";
        //        }
        //        else
        //        {
        //            item.CustomBullet = "img/amcharts/customBullet.png";
        //        }
        //        item.BulletValue = "1";

        //        List<PsTask> tasks = new PlanInfoMethod().GetTasks(pclsCache,PlanNo, "T", item.Date, "");
        //        string str = "";
        //        for (int j = 0; j < tasks.Count;j++ )
        //        {
        //            if (j % 2 == 0)
        //            {
        //                str = str + tasks[j].Name +"："+ tasks[j].Status+"   ";

        //            }
        //            else
        //            {
        //                str = str + tasks[j].Name +"：" + tasks[j].Status + "<br>";
        //            }
        //        }
        //        item.Task = str; //需要修改
        //        if (items2 != null)
        //        {
        //            List<VitalInfo> vitalSigns = items2.FindAll(s => s.RecordDate == item.Date);
        //            if (vitalSigns != null)
        //            {
        //                string strValue = "";
        //                for (int k = 0; k < vitalSigns.Count; k++)
        //                {
        //                    if (vitalSigns[k].ItemCode == "Bloodpressure_1")
        //                    {
        //                        item.BloodPressure_1 = vitalSigns[k].Value;
        //                        item.BloodPressure_2 = "#";
        //                        item.Pulserate_1 = "#";

        //                    }
        //                    else if (vitalSigns[k].ItemCode == "Bloodpressure_2")
        //                    {
        //                        item.BloodPressure_1 = "#";
        //                        item.BloodPressure_2 = vitalSigns[k].Value;
        //                        item.Pulserate_1 = "#";
        //                    }
        //                    else if (vitalSigns[k].ItemCode == "Pulserate_1")
        //                    {
        //                        item.BloodPressure_1 = "#";
        //                        item.BloodPressure_2 = "#";
        //                        item.Pulserate_1 = vitalSigns[k].Value;
        //                    }
        //                    else
        //                    {
        //                        item.BloodPressure_1 = "#";
        //                        item.BloodPressure_2 = "#";
        //                        item.Pulserate_1 = "#";
        //                    }
        //                    strValue = strValue + vitalSigns[k].Name + "：" + vitalSigns[k].Value + "<br>";
        //                }
        //                item.Value = strValue; //需要修改
        //            }
        //            else
        //            {
        //                item.Value = "#";
        //            }
        //        }
        //        items.Add(item);
        //    }

        //    return items;
        //}

        ////获取计划完成情况（Pad)-查看往期计划 GL 2015-10-13
        //public ImplementationInfo GetImplementationForPadSecond(string PatientId, string PlanNo)
        //{
        //    ImplementationInfo ImplementationInfo = new ImplementationInfo();
        //    //string str_result = "";
        //    string Module = "";
        //    try
        //    {
        //        //Pad保证PlanNo输入不为空  为空的表示无当前计划，则显示无执行即可，无需连接网络服务
        //        if ((PlanNo != "") && (PlanNo != null)) //存在正在执行的计划
        //        {
        //            //获取计划的相关信息
        //            int planStatus = 0;
        //            GPlanInfo planInfo = new PlanInfoMethod().GetPlanInfo(pclsCache, PlanNo);
        //            if (planInfo != null)
        //            {
        //                planStatus = Convert.ToInt32(planInfo.Status);
        //                Module = planInfo.Module;
        //                ImplementationInfo.StartDate = Convert.ToInt32(planInfo.StartDate);
        //                ImplementationInfo.EndDate = Convert.ToInt32(planInfo.EndDate);
        //            }

        //            if (planStatus == 3) //是正在执行的当前计划
        //            {
        //                //剩余天数和进度
        //                Progressrate PRlist = new PlanInfoMethod().GetProgressRate(pclsCache, PlanNo);
        //                if (PRlist != null)
        //                {
        //                    ImplementationInfo.RemainingDays = PRlist.RemainingDays;
        //                    ImplementationInfo.ProgressRate = (Convert.ToDouble(PRlist.ProgressRate) * 100).ToString();
        //                }

        //                //最近一周的依从率
        //                Period weekPeriod = new PlanInfoMethod().GetWeekPeriod(pclsCache, ImplementationInfo.StartDate);
        //                if (weekPeriod != null)
        //                {
        //                    ImplementationInfo.CompliacneValue = "最近一周依从率为：" + new PlanInfoMethod().GetCompliacneRate(pclsCache, PatientId, PlanNo, Convert.ToInt32(weekPeriod.StartDate), Convert.ToInt32(weekPeriod.EndDate)) + "%";
        //                }
        //            }
        //            else  //已经结束计划
        //            {
        //                ImplementationInfo.RemainingDays = "0";
        //                ImplementationInfo.ProgressRate = "100";
        //                ImplementationInfo.CompliacneValue = "整个计划依从率为：" + new PlanInfoMethod().GetCompliacneRate(pclsCache, PatientId, PlanNo, ImplementationInfo.StartDate, ImplementationInfo.EndDate) + "%";
        //            }

        //            #region  读取任务执行情况，体征、用药等

        //            //读取任务列表 20151027 需要修改
        //            //List<PsTask> TaskList = new PlanInfoMethod().GetTaskList(pclsCache, PlanNo);
        //            List<PsTask> TaskList = new List<PsTask>();
        //            //ImplementationInfo.TaskList = PsTask.GetSpTaskList(pclsCache, PlanNo);

        //            //测量-体征切换下拉框  
        //            List<PsTask> VitalSignRows = new List<PsTask>();
        //            foreach (PsTask item in TaskList)
        //            {
        //                if (item.Type == "VitalSign")
        //                {
        //                    VitalSignRows.Add(item);
        //                }
        //            }

        //            List<SignShow> SignList = new List<SignShow>();
        //            foreach (PsTask item in VitalSignRows)
        //            {
        //                SignShow SignShow = new SignShow();
        //                SignShow.SignName = item.Name;
        //                SignShow.SignCode = item.Code;
        //                SignList.Add(SignShow);
        //            }
        //            ImplementationInfo.SignList = SignList;



        //            List<MstBloodPressure> reference = new List<MstBloodPressure>();
        //            ChartData ChartData = new ChartData();
        //            List<Graph> GraphList = new List<Graph>();
        //            GraphGuide GraphGuide = new GraphGuide();

        //            if (Module == "M1")  //后期维护的话，在这里添加不同的模块判断
        //            {

        //                //高血压模块  体征测量-血压（收缩压、舒张压）、脉率   【默认显示-收缩压，血压必有，脉率可能有】  
        //                List<PsTask> HyperTensionRows = new List<PsTask>();
        //                foreach (PsTask item in TaskList)
        //                {
        //                    if ((item.Code == "Bloodpressure|Bloodpressure_1") || (item.Code == "Bloodpressure|Bloodpressure_2") || (item.Code == "Pulserate|Pulserate_1"))
        //                    {
        //                        HyperTensionRows.Add(item);
        //                    }
        //                }

        //                //注意：需要兼容之前没有脉率的情况
        //                if ((HyperTensionRows != null) && (HyperTensionRows.Count >= 2))  //M1 收缩压（默认显示）、舒张压、脉率  前两者肯定有，脉率不一定有
        //                {
        //                    //获取血压的分级规则，脉率的分级原则写死在webservice
        //                    reference = new PlanInfoMethod().GetBPGrades(pclsCache);

        //                    //首次进入，默认加载收缩压
        //                    GraphList = new PlanInfoMethod().GetSignInfoByM1(pclsCache, PatientId, PlanNo, "Bloodpressure|Bloodpressure_1", ImplementationInfo.StartDate, ImplementationInfo.EndDate, reference);

        //                    //初始值、目标值、分级规则加工
        //                    if (GraphList.Count > 0)
        //                    {
        //                        GraphGuide = new PlanInfoMethod().GetGuidesByCode(pclsCache, PlanNo, "Bloodpressure|Bloodpressure_1", reference);
        //                        ChartData.GraphGuide = GraphGuide;
        //                    }
        //                }
        //            }


        //            //必有测量任务，其他任务（例如吃药）可能没有

        //            //其他任务依从情况
        //            List<CompliacneDetailByD> TasksComByPeriod = new List<CompliacneDetailByD>();
        //            //是否有其他任务
        //            //string condition1 = " Type not in ('VitalSign,')";
        //            if (TaskList.Count == VitalSignRows.Count)
        //            {
        //                ChartData.OtherTasks = "0";
        //            }
        //            else
        //            {
        //                ChartData.OtherTasks = "1";
        //                TasksComByPeriod = new PlanInfoMethod().GetTasksComCountByPeriod(pclsCache, PatientId, PlanNo, ImplementationInfo.StartDate, ImplementationInfo.EndDate);
        //                if ((TasksComByPeriod != null) && (TasksComByPeriod.Count == GraphList.Count))
        //                {
        //                    for (int rowsCount = 0; rowsCount < TasksComByPeriod.Count; rowsCount++)
        //                    {
        //                        GraphList[rowsCount].DrugValue = "1";
        //                        GraphList[rowsCount].DrugBullet = TasksComByPeriod[rowsCount].drugBullet;
        //                        GraphList[rowsCount].DrugColor = TasksComByPeriod[rowsCount].drugColor;
        //                        GraphList[rowsCount].DrugDescription = TasksComByPeriod[rowsCount].Events;
        //                    }
        //                }
        //            }

        //            ChartData.GraphList = GraphList;
        //            ImplementationInfo.ChartData = ChartData;

        //            #endregion
        //        }

        //        //str_result = JSONHelper.ObjectToJson(ImplementationInfo);
        //        //Context.Response.BinaryWrite(new byte[] { 0xEF, 0xBB, 0xBF });
        //        //Context.Response.Write(str_result);
        //        //HttpContext.Current.ApplicationInstance.CompleteRequest();
        //        ////Context.Response.End();
        //        return ImplementationInfo;
        //    }
        //    catch (Exception ex)
        //    {
        //        HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetImplementationForPadSecond", "PlanInfoRepository error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
        //        //return null;
        //        throw (ex);
        //    }
        //}

        ////获取病人当前计划以及健康专员 "PlanNo|DoctorId" GL 2015-10-13
        //public string GetPlanInfobyPID(string PatientId)
        //{
        //    string Info = string.Empty;
        //    try
        //    {
        //        //ZAM Bug Fix 2015-6-2
        //        GPlanInfo Plan = new PlanInfoMethod().GetExecutingPlan(pclsCache, PatientId);
        //        string PlanNo = string.Empty;
        //        string DoctorId = string.Empty;
        //        string DoctorName = string.Empty;

        //        if (Plan != null && Plan.PlanNo != null) //Plan.Count > 0
        //        {
        //            PlanNo = Plan.PlanNo;
        //        }
        //        TypeAndName Doctor = new ModuleInfoMethod().PsBasicInfoDetailGetSDoctor(pclsCache, PatientId);
        //        if (Doctor != null && Doctor.Type != null) //Doctor.Count > 1
        //        {
        //            DoctorId = Doctor.Type;
        //            DoctorName = Doctor.Name;
        //        }
        //        Info = PlanNo + "|" + DoctorId + "|" + DoctorName;
        //        return Info;
        //    }
        //    catch (Exception ex)
        //    {
        //        HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetPlanInfobyPID", "PlanInfoRepository error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
        //        return Info;
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        pclsCache.DisConnect();
        //    }
        //}

        //获取计划完成情况（Phone)-查看当前计划近一周的情况 GL 2015-10-13     
        //public ImplementationPhone GetImplementationForPhone(string PatientId, string Module)
        //{
        //    ImplementationPhone ImplementationPhone = new ImplementationPhone();
        //    //string str_result = "";
        //    try
        //    {
        //        //病人基本信息-头像、姓名.. (由于手机版只针对换换咋用户，基本信息可不用获取
        //        // CacheSysList patientList = PsBasicInfo.GetPatientBasicInfo(pclsCache, PatientId);
        //        //if (patientList != null)
        //        //{
        //        //ImplementationPhone.PatientInfo.PatientName = patientList[0];
        //        //}

        //        int planStartDate = 0;
        //        int planEndDate = 0;
        //        string PlanNo = "";

        //        GPlanInfo planInfo = new PlanInfoMethod().GetExecutingPlanByM(pclsCache, PatientId, Module);
        //        if (planInfo != null)
        //        {
        //            PlanNo = planInfo.PlanNo;
        //            planStartDate = Convert.ToInt32(planInfo.StartDate);
        //            planEndDate = Convert.ToInt32(planInfo.EndDate);  //未用到
        //        }

        //        if ((PlanNo != "") && (PlanNo != null)) //存在正在执行的计划
        //        {
        //            ImplementationPhone.NowPlanNo = PlanNo;

        //            //剩余天数和进度
        //            Progressrate PRlist = new PlanInfoMethod().GetProgressRate(pclsCache, PlanNo);
        //            if (PRlist != null)
        //            {
        //                ImplementationPhone.RemainingDays = PRlist.RemainingDays;  //"距离本次计划结束还剩"+PRlist[0]+"天";
        //                ImplementationPhone.ProgressRate = (Convert.ToDouble(PRlist.ProgressRate) * 100).ToString();  //"进度："++"%";
        //            }

        //            //最近一周的依从率
        //            Period weekPeriod = new PlanInfoMethod().GetWeekPeriod(pclsCache, planStartDate);
        //            if (weekPeriod != null)
        //            {
        //                ImplementationPhone.CompliacneValue = new PlanInfoMethod().GetCompliacneRate(pclsCache, PatientId, PlanNo, Convert.ToInt32(weekPeriod.StartDate), Convert.ToInt32(weekPeriod.EndDate));
        //                ImplementationPhone.StartDate = Convert.ToInt32(weekPeriod.StartDate);  //用于获取血压的详细数据
        //                ImplementationPhone.EndDate = Convert.ToInt32(weekPeriod.EndDate);
        //            }

        //            #region  读取任务执行情况，血压、用药-最近一周的数据

        //            //读取任务  phone版 此函数其他任务也显示  20151027 需要修改
        //            //List<PsTask> TaskList = new PlanInfoMethod().GetTaskList(pclsCache, PlanNo);
        //            List<PsTask> TaskList = new List<PsTask>();

        //            //测量-体征切换下拉框  
        //            List<PsTask> VitalSignRows = new List<PsTask>();
        //            foreach (PsTask item in TaskList)
        //            {
        //                if (item.Type == "VitalSign")
        //                {
        //                    VitalSignRows.Add(item);
        //                }
        //            }
        //            List<SignShow> SignList = new List<SignShow>();
        //            foreach (PsTask item in VitalSignRows)
        //            {
        //                SignShow SignShow = new SignShow();
        //                SignShow.SignName = item.Name;
        //                SignShow.SignCode = item.Code;
        //                SignList.Add(SignShow);
        //            }
        //            ImplementationPhone.SignList = SignList;


        //            List<MstBloodPressure> reference = new List<MstBloodPressure>();
        //            ChartData ChartData = new ChartData();
        //            List<Graph> GraphList = new List<Graph>();
        //            GraphGuide GraphGuide = new GraphGuide();

        //            if (Module == "M1")  //后期维护的话，在这里添加不同的模块判断
        //            {
        //                List<PsTask> HyperTensionRows = new List<PsTask>();
        //                foreach (PsTask item in TaskList)
        //                {
        //                    if ((item.Code == "Bloodpressure|Bloodpressure_1") || (item.Code == "Bloodpressure|Bloodpressure_2") || (item.Code == "Pulserate|Pulserate_1"))
        //                    {
        //                        HyperTensionRows.Add(item);
        //                    }
        //                }
        //                //注意：需要兼容之前没有脉率的情况
        //                if ((HyperTensionRows != null) && (HyperTensionRows.Count >= 2))  //M1 收缩压（默认显示）、舒张压、脉率  前两者肯定有，脉率不一定有
        //                {
        //                    //获取血压的分级规则，脉率的分级原则写死在webservice
        //                    reference = new PlanInfoMethod().GetBPGrades(pclsCache);

        //                    //首次进入，默认加载收缩压
        //                    GraphList = new PlanInfoMethod().GetSignInfoByM1(pclsCache, PatientId, PlanNo, "Bloodpressure|Bloodpressure_1", Convert.ToInt32(weekPeriod.StartDate), Convert.ToInt32(weekPeriod.EndDate), reference);

        //                    //初始值、目标值、分级规则加工
        //                    if (GraphList.Count > 0)
        //                    {
        //                        GraphGuide = new PlanInfoMethod().GetGuidesByCode(pclsCache, PlanNo, "Bloodpressure|Bloodpressure_1", reference);
        //                        ChartData.GraphGuide = GraphGuide;
        //                    }
        //                }

        //            }
        //            else
        //            {

        //            }

        //            //必有测量任务，其他任务（例如吃药）可能没有
        //            //其他任务依从情况 //是否有其他任务
        //            List<CompliacneDetailByD> TasksComByPeriod = new List<CompliacneDetailByD>();
        //            //string condition1 = " Type not in ('VitalSign,')";
        //            if (TaskList.Count == VitalSignRows.Count)
        //            {
        //                ChartData.OtherTasks = "0";
        //            }
        //            else
        //            {
        //                ChartData.OtherTasks = "1";
        //                TasksComByPeriod = new PlanInfoMethod().GetTasksComCountByPeriod(pclsCache, PatientId, PlanNo, Convert.ToInt32(weekPeriod.StartDate), Convert.ToInt32(weekPeriod.EndDate));
        //                if ((TasksComByPeriod != null) && (TasksComByPeriod.Count == GraphList.Count))
        //                {
        //                    for (int rowsCount = 0; rowsCount < TasksComByPeriod.Count; rowsCount++)
        //                    {
        //                        GraphList[rowsCount].DrugValue = "1";   //已经初始化过
        //                        GraphList[rowsCount].DrugBullet = TasksComByPeriod[rowsCount].drugBullet;
        //                        GraphList[rowsCount].DrugColor = TasksComByPeriod[rowsCount].drugColor;
        //                        GraphList[rowsCount].DrugDescription = TasksComByPeriod[rowsCount].Events;//+ "<br><a onclick= shuang shuang zz(); shuang shuang;>详细</a>"
        //                    }
        //                }
        //            }


        //            #endregion

        //            ChartData.GraphList = GraphList;
        //            ImplementationPhone.ChartData = ChartData;
        //        }

        //        //str_result = JSONHelper.ObjectToJson(ImplementationPhone);
        //        //Context.Response.BinaryWrite(new byte[] { 0xEF, 0xBB, 0xBF });
        //        //Context.Response.Write(str_result);
        //        //HttpContext.Current.ApplicationInstance.CompleteRequest();
        //        ////Context.Response.End();
        //        return ImplementationPhone;
        //    }
        //    catch (Exception ex)
        //    {
        //        HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetImplementationForPhone", "PlanInfoRepository error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
        //        //return null;
        //        throw (ex);
        //    }
        //}
        #endregion


        //获取健康专员负责的所有患者（最新结束但未达标的）计划列表 GL 2015-10-13
        public List<OverDuePlanDetail> GetOverDuePlanList(DataConnection pclsCache, string DoctorId, string ModuleType)
        {
            List<OverDuePlanDetail> PlanList = new List<OverDuePlanDetail>();
            try
            {
                int nowDate = new CommonFunction().GetServerDate();

                List<PatientPlan> DT_Patients = new PlanInfoMethod().GetOverDuePlanByDoctorId(pclsCache, DoctorId, ModuleType);
                if (DT_Patients == null)
                {
                    return null;
                }
                foreach (PatientPlan item in DT_Patients)
                {
                    string patientId = item.PatientId;
                    string planNo = item.PlanNo;
                    string startDate = item.StartDate;
                    string endDate = item.EndDate;
                    string totalDays = item.TotalDays;
                    string remainingDays = item.RemainingDays;

                    double process = 0.0;
                    //VitalSign
                    List<string> vitalsigns = new List<string>();

                    if (planNo != "")
                    {
                        //double complianceRate = PsCompliance.GetComplianceByDay(pclsCache, patientId, nowDate, planNo);

                        string itemType = "Bloodpressure";
                        string itemCode = "Bloodpressure_1";
                        int recordDate = Convert.ToInt32(endDate);
                        VitalInfo list = new VitalInfoMethod().GetLatestVitalSignsByDate(pclsCache, patientId, itemType, itemCode, recordDate);
                        if (list != null)
                        {
                            vitalsigns.Add(list.Value);
                        }

                        TargetByCode targetlist = new PlanInfoMethod().GetTarget(pclsCache, planNo, itemType, itemCode);
                        if (targetlist != null)
                        {
                            vitalsigns.Add(targetlist.Value);  //value
                        }
                        //非法数据判断 zam 2015-5-18
                        //OverDue Check
                        if (list != null && targetlist != null)
                        {
                            double m, n;
                            bool misNumeric = double.TryParse(list.Value, out m);
                            bool nisNumeric = double.TryParse(targetlist.Value, out n);
                            if (misNumeric && nisNumeric)
                            {
                                //if (Convert.ToInt32(list[2]) <= Convert.ToInt32(targetlist[3])) //已达标
                                if (m <= n)
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    //PhotoAddress
                    string photoAddress = "";
                    PatDetailInfo patientInfolist = new ModuleInfoMethod().PsBasicInfoDetailGetPatientDetailInfo(pclsCache, patientId);
                    if (patientInfolist != null)
                    {
                        photoAddress = patientInfolist.PhotoAddress;
                    }

                    string patientName = "";
                    patientName = new UsersMethod().GetNameByUserId(pclsCache, patientId);

                    OverDuePlanDetail PlanItem = new OverDuePlanDetail();
                    PlanItem.PatientId = patientId;
                    PlanItem.PatientName = patientName;
                    PlanItem.PhotoAddress = photoAddress;
                    PlanItem.PlanNo = planNo;
                    PlanItem.StartDate = startDate;
                    PlanItem.Process = process;
                    PlanItem.RemainingDays = remainingDays;
                    PlanItem.VitalSign = vitalsigns;

                    PlanList.Add(PlanItem);
                }
                return PlanList;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetOverDuePlanList", "PlanInfoRepository error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
                throw (ex);
            }
        }     

        public List<PsTask> GetTasks(DataConnection pclsCache, string PlanNo, string ParentCode, string Date, string PatientId)
        {
            return new PlanInfoMethod().GetTasks(pclsCache, PlanNo, ParentCode,Date,PatientId);
        }

        public List<TasksForClick> GetTasksForClick(DataConnection pclsCache, string PlanNo, string ParentCode, string Date)
        {
            List<TasksForClick> tasks = new List<TasksForClick>();
            List<PsTask> tasksOne = new PlanInfoMethod().GetTasks(pclsCache, PlanNo, ParentCode, Date, "");
            if (tasksOne != null)
            {
                for (int i = 0; i < tasksOne.Count; i++)
                {
                    if (tasksOne[i].InvalidFlag == "1")
                    {
                        TasksForClick task = new TasksForClick();
                        task.Type = tasksOne[i].Type;
                        task.Code = tasksOne[i].Code;
                        task.Name = tasksOne[i].Name;
                        task.Status = tasksOne[i].Status;
                        List<PsTask> tasksTwo = new PlanInfoMethod().GetTasks(pclsCache, PlanNo, tasksOne[i].Code, Date, "");
                        if (tasksTwo != null)
                        {
                            for (int j = 0; j < tasksTwo.Count; j++)
                            {
                                TasksForClickDtl dtl = new TasksForClickDtl();
                                dtl.Code = tasksTwo[j].Code;
                                dtl.Name = tasksTwo[j].Name;
                                dtl.Status = tasksTwo[j].Status;
                                task.SubTasks.Add(dtl);
                            }
                        }
                        tasks.Add(task);
                    }
                }
            }
            return tasks;
        }

        /// <summary>
        /// 需要修改 20151029
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="PlanNo"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="ItemType"></param>
        /// <param name="ItemCode"></param>
        /// <returns></returns>
        public List<ComplianceAllSignsListByPeriod> GetComplianceAllSignsListByPeriod(DataConnection pclsCache, string UserId, string PlanNo, int StartDate, int EndDate, string ItemType, string ItemCode)
        {
            List<ComplianceAllSignsListByPeriod> items = new List<ComplianceAllSignsListByPeriod>();
            //依从率
            List<ComplianceListByPeriod> items1 = new PlanInfoMethod().GetComplianceListByPeriod(pclsCache, PlanNo, StartDate, EndDate);
            //体征
            List<SignByPeriod> items2 = new PlanInfoMethod().GetSignByPeriod(pclsCache,UserId,ItemType,ItemCode,StartDate,EndDate);
            //List<VitalInfo> items2 = new VitalInfoMethod().GetAllSignsByPeriod(pclsCache, UserId, StartDate, EndDate);

            if (items1 != null)
            {
                for (int i = 0; i < items1.Count; i++)
                {
                    ComplianceAllSignsListByPeriod item = new ComplianceAllSignsListByPeriod();
                    item.Date = items1[i].Date.ToString();
                    item.Compliance = Math.Round(items1[i].Compliance , 2)*100 + "%";
                    item.Description = items1[i].Description;
                    if (items1[i].Compliance == 1)
                    {
                        item.BulletColor = "#77777";
                    }
                    else if (items1[i].Compliance == 0)
                    {
                        item.BulletColor = "#DADADA";
                    }
                    else
                    {
                        item.CustomBullet = "img/amcharts/customBullet.png";
                    }
                    item.BulletValue = "1";

                    List<PsTask> tasks = new PlanInfoMethod().GetTasks(pclsCache, PlanNo, "T", item.Date, "");
                    string str = "";
                    if (tasks != null)
                    {
                        for (int j = 0; j < tasks.Count; j++)
                        {
                            if (j % 2 == 0)
                            {
                                //str = str + tasks[j].Name + "：" + tasks[j].Status + "   ";
                                str = str + tasks[j].Name + "   ";

                            }
                            else
                            {
                                //str = str + tasks[j].Name + "：" + tasks[j].Status + "<br>";
                                str = str + tasks[j].Name + "<br>";

                            }
                        }
                    }
                    item.Task = str; //需要修改
                    if (items2 != null)
                    {
                        SignByPeriod vitalSign = items2.Find(s => s.RecordDate == item.Date);
                        if (vitalSign != null)
                        {
                            item.VitalCode = ItemCode;
                            item.Value = vitalSign.Value; //需要修改
                        }
                        else
                        {
                            item.VitalCode = ItemCode;
                            item.Value = "#"; //需要修改
                        }
                    }
                    items.Add(item);
                }
            }
            return items;
        }
            

        //根据主键删除Ps.Task一条数据 SYF 2015-10-29
        public int DeteteTask(DataConnection pclsCache, string Plan, string Type, string Code, string SortNo)
        {
            return new PlanInfoMethod().DeteteTask(pclsCache, Plan, Type, Code, SortNo);

        }

        public List<GPlanInfo> GetPlanListByMS(DataConnection pclsCache, string PatientId, string Module, int Status)
        {
            return new PlanInfoMethod().GetPlanListByMS(pclsCache, PatientId, Module, Status);
        }

        public List<ComplianceDate> GetComplianceListInC(DataConnection pclsCache, string PatientId, string StartDate, string EndDate, string Module)
        {
            return new PlanInfoMethod().GetComplianceListInC(pclsCache, PatientId, StartDate, EndDate, Module);
        }

        public List<TaskCompliance> GetTaskCompliance(DataConnection pclsCache, string PlanNo)
        {
            return new PlanInfoMethod().GetTaskCompliance(pclsCache, PlanNo);
        }

        /// <summary>
        /// 获取计划修改前后的不同任务 syf 20151223
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PlanNo"></param>
        /// <returns></returns>
        public List<LogTask> GetDTaskByPlanNo(DataConnection pclsCache, string PlanNo)
        {
            //1为最新修改的PlanTask，2是修改前的
            LogPlan Plan1 = new LogPlan();
            Plan1 = new PlanInfoMethod().GetLogPlanInfo(pclsCache, PlanNo);
            if (Plan1 == null)
            {
                return null;
            }
            else
            {
                int Edition1 = -1;
                int Edition2 = -1;
                Edition1 = Convert.ToInt32(Plan1.Edition);
                if (Edition1 >= 1)
                {
                    Edition2 = Edition1 - 1;


                    List<LogTask> list1 = new List<LogTask>();
                    List<LogTask> list2 = new List<LogTask>();
                    List<LogTask> list3 = new List<LogTask>();
                    List<LogTask> list4 = new List<LogTask>();
                    list1 = new PlanInfoMethod().GetAllTaskByLogPlan(pclsCache, PlanNo, Edition1);
                    list2 = new PlanInfoMethod().GetAllTaskByLogPlan(pclsCache, PlanNo, Edition2);
                    if ((list1 != null) && (list2 != null))
                    {
                        #region<按顺序比>
                        //    int len = Math.Min(list1.Count, list2.Count);
                        //    for (int i = 0; i < len; i++)
                        //    {
                        //        if((list1[i].Code != list2[i].Code) || (list1[i].Type != list2[i].Type) ||(list1[i].Instruction != list2[i].Instruction))
                        //        {
                        //            list3.Add(list1[i]);
                        //            list3.Add(list2[i]);
                        //        }
                        //    }
                        //    //输出公共部分不同的

                        //    //其余多出来部分
                        //    if(list1.Count > len)
                        //    {
                        //        for(int j=len; j<list1.Count; j++)
                        //        {
                        //            list3.Add(list1[j]);
                        //        }
                        //    }
                        //    if (list2.Count > len)
                        //    {
                        //        for (int j=len ; j <list2.Count; j++)
                        //        {
                        //            list3.Add(list2[j]);
                        //        }
                        //    }
                        #endregion
                        //for (int x = 0; x < list1.Count;x++ )
                        //{
                        //    list1[x].Edition = "1";
                        //}
                        //for (int y = 0; y < list2.Count; y++)
                        //{
                        //    list2[y].Edition = "1";
                        //}
                        list3 = list1.Except(list2, new CDMISrestful.DataModels.LogTask.StudentListEquality()).ToList();
                        //if(list3 != null)
                        //{
                        //    for (int x = 0; x < list3.Count; x++)
                        //    {
                        //        list3[x].Edition = Convert.ToString(Edition1);
                        //    }
                        //}
                        list4 = list2.Except(list1, new CDMISrestful.DataModels.LogTask.StudentListEquality()).ToList();
                        //if (list4 != null)
                        //{
                        //    for (int x = 0; x < list4.Count; x++)
                        //    {
                        //        list4[x].Edition = Convert.ToString(Edition2);
                        //    }
                        //}

                        list3.AddRange(list4);

                        if(list3 != null)
                        {
                            for(int z=0; z<list3.Count; z++)
                            {
                                //string ForName = "";
                                CmMstTaskN CmMstTaskN = new CmMstTaskN();
                                CmMstTaskN = new PlanInfoMethod().GetCmTaskItemInfo(pclsCache, list3[z].Type, list3[z].Code);
                                if(CmMstTaskN != null)
                                {
                                    list3[z].TaskName = CmMstTaskN.Name;
                                }
                            }
                        }
                    }

                    string EndDate1 = Plan1.EndDate;
                    LogPlan Plan2 = new LogPlan();
                    Plan2 = new PlanInfoMethod().GetLogPlanByEdition(pclsCache, PlanNo, Edition2);
                    string EndDate2 = "";
                    if(Plan2 != null)
                    {
                        EndDate2 = Plan2.EndDate;
                    }
                    
                    if (EndDate1 != EndDate2)
                    {
                        LogTask ForEnd1 = new LogTask();
                        LogTask ForEnd2 = new LogTask();
                        ForEnd1.PlanEndDate = EndDate1;
                        ForEnd2.PlanEndDate = EndDate2;
                        list3.Add(ForEnd1);
                        list3.Add(ForEnd2);
                    }
                    list3.Sort();
                    return list3;

                }
                else
                {
                    return null;
                }
            }
        }

       //public LogPlan GetLogPlanInfo(DataConnection pclsCache, string PlanNo)
       // {
       //     LogPlan ret = new PlanInfoMethod().GetLogPlanInfo(pclsCache, PlanNo);
       //     return ret;
       // }

       // public List<LogTask> GetAllTaskByLogPlan(DataConnection pclsCache, string PlanNo, int piEdition)
       //{
       //    return new PlanInfoMethod().GetAllTaskByLogPlan(pclsCache, PlanNo, piEdition);
       //}
    }
}