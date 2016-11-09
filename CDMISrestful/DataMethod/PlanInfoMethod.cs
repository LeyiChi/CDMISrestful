using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CDMISrestful.CommonLibrary;
using InterSystems.Data.CacheClient;
using CDMISrestful.DataModels;
using System.Data;

namespace CDMISrestful.DataMethod
{
    public class PlanInfoMethod
    {
        #region<PsCalendar>
        public int PsCalendarSetData(DataConnection pclsCache, string DoctorId, int DateTime, string Period, int SortNo, string Description, int Status, string Redundancy, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            int ret = 0;

            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }

                ret = (int)Ps.Calendar.SetData(pclsCache.CacheConnectionObject, DoctorId, DateTime, Period, SortNo, Description, Status, Redundancy, revUserId, TerminalName, TerminalIP, DeviceType);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.ChangePlanStatus", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        #endregion

        #region PsTemplate
        //setdata CSQ 20151026
        public int PsTemplateSetData(DataConnection pclsCache, string DoctorId, int TemplateCode, string TemplateName, string Description, DateTime RecordDate, string Redundance,string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            int ret = 3;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }

                ret = (int)Ps.Template.SetData(pclsCache.CacheConnectionObject, DoctorId, TemplateCode, TemplateName, Description, RecordDate, Redundance, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.Template.SetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        //setdata CSQ 20151026
        public int PsTemplateDetailSetData(DataConnection pclsCache, string DoctorId, int TemplateCode, string CategoryCode, string ItemCode, string Value, string Description,string Redundance, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            int ret = 3;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }
                string Key = DoctorId + "||" + TemplateCode;
                ret = (int)Ps.TemplateDetail.SetData(pclsCache.CacheConnectionObject, Key, CategoryCode, ItemCode, Value, Description, Redundance, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.TemplateDetail.SetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        public List<TemplateInfo> GetTemplateList(DataConnection pclsCache, string DoctorId)
        {
            List<TemplateInfo> list = new List<TemplateInfo>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Template.GetTemplateList(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("DoctorId", CacheDbType.NVarChar).Value = DoctorId;             

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    TemplateInfo NewLine = new TemplateInfo();             
                    NewLine.TemplateCode = Convert.ToInt32(cdr["TemplateCode"]);
                    NewLine.TemplateName = cdr["TemplateName"].ToString();
                    NewLine.Description = cdr["Description"].ToString();
                    NewLine.RecordDate = Convert.ToDateTime(cdr["RecordDate"]);
                    NewLine.Redundance = cdr["Redundance"].ToString();         
                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetPatientsPlanByDoctorId", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }

        public List<TemplateInfoDtl> GetTemplateDetails(DataConnection pclsCache, string DoctorId, string TemplateCode, string ParentCode)
        {
            List<TemplateInfoDtl> list = new List<TemplateInfoDtl>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.TemplateDetail.GetTemplateDetails(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("DoctorId", CacheDbType.NVarChar).Value = DoctorId;
                cmd.Parameters.Add("TemplateCode", CacheDbType.NVarChar).Value = TemplateCode;
                cmd.Parameters.Add("ParentCode", CacheDbType.NVarChar).Value = ParentCode;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    TemplateInfoDtl NewLine = new TemplateInfoDtl();
                    NewLine.CategoryCode = cdr["CategoryCode"].ToString();
                    NewLine.Code = cdr["Code"].ToString();
                    NewLine.Name = cdr["Name"].ToString();
                    NewLine.InvalidFlag = cdr["InvalidFlag"].ToString();
                    NewLine.Value = cdr["Value"].ToString();
                    NewLine.TemplateDescription = cdr["TemplateDescription"].ToString();
                    NewLine.Redundance = cdr["Redundance"].ToString();
                    NewLine.ParentCode = cdr["ParentCode"].ToString();
                    NewLine.TaskDescription = cdr["TaskDescription"].ToString();
                    NewLine.GroupHeaderFlag = cdr["GroupHeaderFlag"].ToString();
                    NewLine.ControlType = cdr["ControlType"].ToString();
                    NewLine.OptionCategory = cdr["OptionCategory"].ToString();
                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetTemplateDetails", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }

        #endregion

        #region PsPlan
        //SYF 20151010
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PlanNo"></param>
        /// <param name="PatientId"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Module"></param>
        /// <param name="Status"></param>
        /// <param name="DoctorId"></param>
        /// <param name="piUserId"></param>
        /// <param name="piTerminalName"></param>
        /// <param name="piTerminalIP"></param>
        /// <param name="piDeviceType"></param>
        /// <returns></returns>
        public int PsPlanSetData(DataConnection pclsCache, string PlanNo, string PatientId, int StartDate, int EndDate, string Module, int Status, string DoctorId, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            int ret = 2;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }

                ret = (int)Ps.Plan.SetData(pclsCache.CacheConnectionObject, PlanNo, PatientId, StartDate, EndDate, Module, Status, DoctorId, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.PsPlanSetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        //SYF 20151010
        /// <summary>
        /// 插入一条新纪录到Ps.Plan表
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PlanNo"></param>
        /// <param name="Status"></param>
        /// <param name="piUserId"></param>
        /// <param name="piTerminalName"></param>
        /// <param name="piTerminalIP"></param>
        /// <param name="piDeviceType"></param>
        /// <returns></returns>
        public int ChangePlanStatus(DataConnection pclsCache, string PlanNo, int Status, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            int ret = 0;

            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }

                ret = (int)Ps.Plan.PlanStart(pclsCache.CacheConnectionObject, PlanNo, Status, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.ChangePlanStatus", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }


        /// <summary>
        /// 获取某计划的相关信息 SYF 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PlanStartDate"></param>
        /// <returns></returns>
        public Period GetWeekPeriod(DataConnection pclsCache, int PlanStartDate)
        {
            try
            {
                Period ret = new Period();
                if (!pclsCache.Connect())
                {
                    return null;
                }
                InterSystems.Data.CacheTypes.CacheSysList list = null;
                list = Ps.Plan.GetWeekPeriod(pclsCache.CacheConnectionObject, PlanStartDate);
                if (list != null)
                {
                    ret.StartDate = list[0];
                    ret.EndDate = list[1];
                    ret.DayCount = list[2];
                }
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetWeekPeriod", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }


        /// <summary>
        /// 获取某计划的进度和剩余天数 SYF 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PlanNo"></param>
        /// <returns></returns>
        public Progressrate GetProgressRate(DataConnection pclsCache, string PlanNo)
        {
            try
            {
                Progressrate ret = new Progressrate();
                if (!pclsCache.Connect())
                {
                    return null;
                }
                InterSystems.Data.CacheTypes.CacheSysList list = null;
                list = Ps.Plan.GetProgressRate(pclsCache.CacheConnectionObject, PlanNo);
                if (list != null)
                {
                    ret.RemainingDays = list[0];
                    ret.ProgressRate = list[1];
                }
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetProgressRate", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }


        /// <summary>
        /// 获取某计划的相关信息 SYF 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PlanNo"></param>
        /// <returns></returns>
        public GPlanInfo GetPlanInfo(DataConnection pclsCache, string PlanNo)
        {
            try
            {
                GPlanInfo ret = new GPlanInfo();
                if (!pclsCache.Connect())
                {
                    return null;
                }
                InterSystems.Data.CacheTypes.CacheSysList list = null;
                list = Ps.Plan.GetPlanInfo(pclsCache.CacheConnectionObject, PlanNo);
                if (list != null)
                {
                    ret.PlanNo = list[0];
                    ret.PatientId = list[1];
                    ret.StartDate = list[2];
                    ret.EndDate = list[3];
                    ret.Module = list[4];
                    ret.Status = list[5];
                    ret.DoctorId = list[6];
                }
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetPlanInfo", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }


        


        /// <summary>
        /// SYF 2015-10-10 获取健康专员负责的所有患者最新结束(status = 4)计划列表
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="DoctorId"></param>
        /// <param name="Module"></param>
        /// <returns></returns>
        public List<PatientPlan> GetOverDuePlanByDoctorId(DataConnection pclsCache, string DoctorId, string Module)
        {
            List<PatientPlan> list = new List<PatientPlan>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Plan.GetOverDuePlanByDoctorId(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("DoctorId", CacheDbType.NVarChar).Value = DoctorId;
                cmd.Parameters.Add("Module", CacheDbType.NVarChar).Value = Module;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    PatientPlan NewLine = new PatientPlan();
                    NewLine.PatientId = cdr["PatientId"].ToString();
                    NewLine.PlanNo = cdr["PlanNo"].ToString();
                    NewLine.StartDate = cdr["StartDate"].ToString();
                    NewLine.EndDate = cdr["EndDate"].ToString();
                    NewLine.TotalDays = cdr["TotalDays"].ToString();
                    NewLine.RemainingDays = cdr["RemainingDays"].ToString();
                    NewLine.Status = cdr["Status"].ToString();
                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetOverDuePlanByDoctorId", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }


        /// <summary>
        /// 获取某模块正在执行的计划 Syf 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PatientId"></param>
        /// <param name="Module"></param>
        /// <returns></returns>
        public GPlanInfo GetExecutingPlanByM(DataConnection pclsCache, string PatientId, string Module)
        {
            try
            {
                GPlanInfo ret = new GPlanInfo();
                if (!pclsCache.Connect())
                {
                    return null;
                }
                InterSystems.Data.CacheTypes.CacheSysList list = null;
                list = Ps.Plan.GetExecutingPlan(pclsCache.CacheConnectionObject, PatientId);
                if (list != null)
                {
                    ret.PlanNo = list[0];
                    ret.PatientId = list[1];
                    ret.StartDate = list[2];
                    ret.EndDate = list[3];
                    ret.Module = list[4];
                    ret.Status = list[5];
                    ret.DoctorId = list[6];
                }
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetExecutingPlanByM", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }


        /// <summary>
        /// 获取正在执行的计划 SYF 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public GPlanInfo GetExecutingPlan(DataConnection pclsCache, string PatientId)
        {
            try
            {
                GPlanInfo ret = new GPlanInfo();
                if (!pclsCache.Connect())
                {
                    return null;
                }
                InterSystems.Data.CacheTypes.CacheSysList list = null;
                list = Ps.Plan.GetExecutingPlan(pclsCache.CacheConnectionObject, PatientId);
                if (list != null)
                {
                    ret.PlanNo = list[0];
                    ret.PatientId = list[1];
                    ret.StartDate = list[2];
                    ret.EndDate = list[3];
                    ret.Module = list[4];
                    ret.Status = list[5];
                    ret.DoctorId = list[6];
                }
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetExecutingPlan", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }




        //GetEndingPlan 获取某模块已经结束的计划 GL 2015-10-12
        public List<PlanDeatil> GetEndingPlan(DataConnection pclsCache, string PatientId, string Module)
        {
            List<PlanDeatil> items = new List<PlanDeatil>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Plan.GetPlanList4ByM(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PatientId", CacheDbType.NVarChar).Value = PatientId;
                cmd.Parameters.Add("Module", CacheDbType.NVarChar).Value = Module;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    PlanDeatil item = new PlanDeatil();
                    item.PlanNo = cdr["PlanNo"].ToString();
                    item.StartDate = Convert.ToInt32(cdr["StartDate"]);
                    item.EndDate = Convert.ToInt32(cdr["EndDate"]);
                    items.Add(item);
                }
                return items;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetEndingPlan", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }

        //更新计划状态 GL 2015-10-13
        public int PlanStart(DataConnection pclsCache, string PlanNo, int Status, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            int ret = 2;

            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }

                ret = (int)Ps.Plan.PlanStart(pclsCache.CacheConnectionObject, PlanNo, Status, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.Plan.PlanStart", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        public List<GPlanInfo> GetPlanListByMS(DataConnection pclsCache, string PatientId, string Module, int Status)
        {
            List<GPlanInfo> list = new List<GPlanInfo>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Plan.GetPlanListByMS(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PatientId", CacheDbType.NVarChar).Value = PatientId;
                cmd.Parameters.Add("Module", CacheDbType.NVarChar).Value = Module;
                cmd.Parameters.Add("Status", CacheDbType.Int).Value = Status;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    GPlanInfo NewLine = new GPlanInfo();
                    NewLine.PlanNo = cdr["PlanNo"].ToString();
                    NewLine.PlanName = cdr["PlanName"].ToString();
                    NewLine.PatientId = cdr["PatientId"].ToString();
                    NewLine.StartDate = cdr["StartDate"].ToString(); 
                    NewLine.EndDate = cdr["EndDate"].ToString();
                    NewLine.Module = cdr["Module"].ToString();
                    NewLine.Status = cdr["Status"].ToString();
                    NewLine.PlanCompliance = cdr["PlanCompliance"].ToString();
                    NewLine.RemainingDays = cdr["RemainingDays"].ToString();
                    NewLine.ProgressRate = cdr["ProgressRate"].ToString();
                    NewLine.DoctorId = cdr["DoctorId"].ToString();
                    NewLine.DoctorName = cdr["DoctorName"].ToString();
                  
                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetPlanListByMS", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }
        #endregion


        #region Ps.Compliance

        /// <summary>
        /// CSQ 20151027 写数据
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="Parent"></param>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <param name="CoUserId"></param>
        /// <param name="CoTerminalName"></param>
        /// <param name="CoTerminalIP"></param>
        /// <param name="CoDeviceType"></param>
        /// <returns></returns>
        public int PsComplianceDetailSetData(DataConnection pclsCache, string PlanNo, int Date, string CategoryCode,string Code,string SortNo,int Status,string Description, string CoUserId, string CoTerminalName, string CoTerminalIP, int CoDeviceType)
        {
            int ret = 2;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }

                ret = (int)Ps.ComplianceDetail.SetData(pclsCache.CacheConnectionObject, PlanNo, Date, CategoryCode, Code,SortNo,Status,Description, CoUserId, CoTerminalName, CoTerminalIP, CoDeviceType);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.PsComplianceDetailSetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }


        /// <summary>
        ///  CSQ 20151027 插入PsCompliance某条数据
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PatientId"></param>
        /// <param name="Date"></param>
        /// <param name="PlanNo"></param>
        /// <param name="Compliance"></param>
        /// <param name="revUserId"></param>
        /// <param name="TerminalName"></param>
        /// <param name="TerminalIP"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>
        public int PsComplianceSetData(DataConnection pclsCache, string PlanNo, int Date, Double Compliance, string Description, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            int ret = 0;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }

                ret = (int)Ps.Compliance.SetData(pclsCache.CacheConnectionObject, PlanNo, Date, Compliance, Description,revUserId, TerminalName, TerminalIP, DeviceType);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.PsComplianceSetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }


        /// <summary>
        /// 某一天所有任务的依从情况 DataTable数据库形式   用于点击事件显示详细   SYF 20151010
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PatientId"></param>
        /// <param name="PlanNo"></param>
        /// <param name="Date"></param>
        /// <returns></returns>
        public List<TasksComList> GetTasksComListByDate(DataConnection pclsCache, string PatientId, string PlanNo, int Date)
        {
            List<TasksComList> list = new List<TasksComList>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Compliance.GetTasksComListByDate(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PatientId", CacheDbType.NVarChar).Value = PatientId;
                cmd.Parameters.Add("PlanNo", CacheDbType.NVarChar).Value = PlanNo;
                cmd.Parameters.Add("Date", CacheDbType.Int).Value = Date;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    TasksComList NewLine = new TasksComList();
                    NewLine.Date = cdr["Date"].ToString();
                    NewLine.ComplianceValue = cdr["ComplianceValue"].ToString();
                    NewLine.TaskType = cdr["TaskType"].ToString();
                    NewLine.TaskId = cdr["TaskId"].ToString();
                    NewLine.TaskName = cdr["TaskName"].ToString();
                    NewLine.Status = cdr["Status"].ToString();
                    NewLine.TaskCode = cdr["TaskCode"].ToString();
                    NewLine.Type = cdr["Type"].ToString();
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetTasksComListByDate", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }


        /// <summary>
        /// 某段时间所有任务的依从情况  DataTable数据库形式  SYF 20151010
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PatientId"></param>
        /// <param name="PlanNo"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public List<TasksComByPeriodDT> GetTasksComByPeriodDT(DataConnection pclsCache, string PatientId, string PlanNo, int StartDate, int EndDate)
        {

            List<TasksComByPeriodDT> list = new List<TasksComByPeriodDT>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Compliance.GetTasksComByPeriod(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PatientId", CacheDbType.NVarChar).Value = PatientId;
                cmd.Parameters.Add("PlanNo", CacheDbType.NVarChar).Value = PlanNo;
                cmd.Parameters.Add("StartDate", CacheDbType.Int).Value = StartDate;
                cmd.Parameters.Add("EndDate", CacheDbType.Int).Value = EndDate;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    TasksComByPeriodDT NewLine = new TasksComByPeriodDT();
                    NewLine.Date = cdr["Date"].ToString();
                    NewLine.ComplianceValue = cdr["ComplianceValue"].ToString();
                    NewLine.TaskType = cdr["TaskType"].ToString();
                    NewLine.TaskId = cdr["TaskId"].ToString();
                    NewLine.TaskName = cdr["TaskName"].ToString();
                    NewLine.Status = cdr["Status"].ToString();
                    NewLine.Type = cdr["Type"].ToString();
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetTasksComByPeriodDT", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }

       
        /// <summary>
        /// 通过Ps.Compliance中的date获取当天某项生理参数值，形成系列  DataTable 形式syf 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PatientId"></param>
        /// <param name="PlanNo"></param>
        /// <param name="ItemType"></param>
        /// <param name="ItemCode"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public List<SignDetailByPeriod> GetSignDetailByPeriod(DataConnection pclsCache, string PatientId, string PlanNo, string ItemType, string ItemCode, int StartDate, int EndDate)
        {
            List<SignDetailByPeriod> list = new List<SignDetailByPeriod>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;


            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }

                cmd = new CacheCommand();
                cmd = Ps.ComplianceDetail.GetSignDetailByPeriod(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PatientId", CacheDbType.NVarChar).Value = PatientId;
                cmd.Parameters.Add("PlanNo", CacheDbType.NVarChar).Value = PlanNo;
                cmd.Parameters.Add("ItemType", CacheDbType.NVarChar).Value = ItemType;
                cmd.Parameters.Add("ItemCode", CacheDbType.NVarChar).Value = ItemCode;
                cmd.Parameters.Add("StartDate", CacheDbType.NVarChar).Value = StartDate;
                cmd.Parameters.Add("EndDate", CacheDbType.NVarChar).Value = EndDate;

                cdr = cmd.ExecuteReader();

                while (cdr.Read())
                {
                    SignDetailByPeriod NewLine = new SignDetailByPeriod();
                    NewLine.RecordDate = cdr["RecordDate"].ToString();
                    NewLine.RecordTime = cdr["RecordTime"].ToString();
                    NewLine.Value = cdr["Value"].ToString();
                    NewLine.Unit = cdr["Unit"].ToString();
                }

                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetSignDetailByPeriod", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// WF 20151030 获取某计划的某段时间(包括端点)的依从率列表
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PatientId"></param>
        /// <param name="PlanNo"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public List<ComplianceListByPeriod> GetComplianceListByPeriod(DataConnection pclsCache, string PlanNo, int StartDate, int EndDate)
        {
            List<ComplianceListByPeriod> list = new List<ComplianceListByPeriod>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Compliance.GetComplianceListByPeriod(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PlanNo", CacheDbType.NVarChar).Value = PlanNo;
                cmd.Parameters.Add("StartDate", CacheDbType.Int).Value = StartDate;
                cmd.Parameters.Add("EndDate", CacheDbType.Int).Value = EndDate;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    ComplianceListByPeriod NewLine = new ComplianceListByPeriod();
                    NewLine.Date = Convert.ToInt32(cdr["Date"]);
                    NewLine.Compliance = Convert.ToDouble(cdr["Compliance"]);
                    NewLine.Description = cdr["Description"].ToString();
                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetComplianceListByPeriod", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// CSQ 20151031 获取患者某计划内某天的依从率
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PatientId"></param>
        /// <param name="Date"></param>
        /// <param name="PlanNo"></param>
        /// <returns></returns>
        public double GetComplianceByDate(DataConnection pclsCache, string PlanNo, int Date)
        {
            double ret = 0.0;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }

                ret = (double)Ps.Compliance.GetComplianceByDate(pclsCache.CacheConnectionObject, PlanNo, Date);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetComplianceByDay", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// 计算某段时间的总依从率 syf 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PatientId"></param>
        /// <param name="PlanNo"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public string GetCompliacneRate(DataConnection pclsCache, string PatientId, string PlanNo, int StartDate, int EndDate)
        {
            string compliacneRate = "";
            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }

                cmd = new CacheCommand();
                cmd = Ps.Compliance.GetComplianceListByPeriod(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PatientId", CacheDbType.NVarChar).Value = PatientId;
                cmd.Parameters.Add("PlanNo", CacheDbType.NVarChar).Value = PlanNo;
                cmd.Parameters.Add("StartDate", CacheDbType.NVarChar).Value = StartDate;
                cmd.Parameters.Add("EndDate", CacheDbType.NVarChar).Value = EndDate;

                cdr = cmd.ExecuteReader();

                double sum = 0;
                int count = 0;
                while (cdr.Read())
                {
                    sum += Convert.ToDouble(cdr["Compliance"]);
                    count++;
                }

                if (count != 0)
                {
                    compliacneRate = (Math.Round(sum / count, 2, MidpointRounding.AwayFromZero) * 100).ToString(); //保留整数

                }

                return compliacneRate;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetCompliacneRate", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }

        //所有任务的依从情况简化版 不再全部显示，只对完成数量就行统计  pad、phone使用中 GL 2015-10-12
        public List<CompliacneDetailByD> GetTasksComCountByPeriod(DataConnection pclsCache, string PatientId, string PlanNo, int StartDate, int EndDate)
        {

            List<CompliacneDetailByD> resultList = new List<CompliacneDetailByD>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                //DataTable list = new DataTable();
                //list = PsCompliance.GetTasksComByPeriodDT(pclsCache, PatientId, PlanNo, StartDate, EndDate);
                List<TasksComByPeriodDT> list0 = GetTasksComByPeriodDT(pclsCache, PatientId, PlanNo, StartDate, EndDate);

                DataTable list = new DataTable();
                list.Columns.Add(new DataColumn("Date", typeof(string)));
                list.Columns.Add(new DataColumn("ComplianceValue", typeof(double)));
                list.Columns.Add(new DataColumn("TaskType", typeof(string))); //中文
                list.Columns.Add(new DataColumn("TaskId", typeof(string)));
                list.Columns.Add(new DataColumn("TaskName", typeof(string)));
                list.Columns.Add(new DataColumn("Status", typeof(int)));
                list.Columns.Add(new DataColumn("Type", typeof(string))); //英文
                if (list0 != null)
                {
                    foreach (TasksComByPeriodDT item in list0)
                    {
                        list.Rows.Add(item.Date, item.ComplianceValue, item.TaskType, item.TaskId, item.TaskName, item.Status, item.Type);
                    }
                }
                //确保排序
                DataView dv = list.DefaultView;
                dv.Sort = "Date Asc, Type desc, Status Asc"; //体征s 生活l 用药d   前提：某计划内任务维持不变  即计划内每天的任务是一样的
                DataTable list_sort = dv.ToTable();
                list_sort.Rows.Add("end", 0, "", "", "", 0);  //用于最后一天输出

                if (list_sort.Rows.Count > 1)
                {
                    string temp_date = list_sort.Rows[0]["Date"].ToString();
                    string temp_type = list_sort.Rows[0]["TaskType"].ToString();  //中文
                    int complete = 0; int count = 0;  //完成数量统计


                    string temp_str = "";
                    temp_str += "该天依从率：" + list_sort.Rows[0]["ComplianceValue"].ToString() + "<br>";
                    temp_str += "<b><span style='font-size:14px;'>" + list_sort.Rows[0]["TaskType"].ToString() + "：</span></b>";

                    CompliacneDetailByD CompliacneDetailByD = new CompliacneDetailByD();
                    CompliacneDetailByD.Date = list_sort.Rows[0]["Date"].ToString();
                    //CompliacneDetailByD.ComplianceValue = list_sort.Rows[0]["ComplianceValue"].ToString();

                    if (Convert.ToDouble(list_sort.Rows[0]["ComplianceValue"]) == 0)  //点的颜色由该天依从率决定
                    {
                        CompliacneDetailByD.drugBullet = "";
                        CompliacneDetailByD.drugColor = "#DADADA";
                    }
                    else if (Convert.ToDouble(list_sort.Rows[0]["ComplianceValue"]) == 1)
                    {
                        CompliacneDetailByD.drugBullet = "";
                        CompliacneDetailByD.drugColor = "#777777";
                    }
                    else
                    {
                        CompliacneDetailByD.drugBullet = "amcharts-images/drug.png";
                        CompliacneDetailByD.drugColor = "";
                    }


                    if (Convert.ToInt32(list_sort.Rows[0]["Status"]) == 1)  //某天某项任务的完成情况
                    {
                        complete++;
                        count++;
                    }
                    else
                    {
                        count++;
                    }


                    //只有一条数据
                    if (list_sort.Rows.Count == 2)
                    {
                        temp_str += complete + "/" + count;
                        CompliacneDetailByD.Events = temp_str;
                        resultList.Add(CompliacneDetailByD);
                    }

                    //＞一条数据
                    if (list_sort.Rows.Count > 2)
                    {
                        for (int i = 1; i <= list_sort.Rows.Count - 1; i++)
                        {
                            if (temp_date == list_sort.Rows[i]["Date"].ToString())  //同一天
                            {
                                if (temp_type == list_sort.Rows[i]["TaskType"].ToString())     //同天同任务类型
                                {
                                    if (Convert.ToInt32(list_sort.Rows[i]["Status"]) == 1)  //某天某项任务的完成情况
                                    {
                                        complete++;
                                        count++;
                                    }
                                    else
                                    {
                                        count++;
                                    }
                                }
                                else   //同天不同任务类型
                                {
                                    temp_str += complete + "/" + count;
                                    complete = 0; count = 0;  //清空统计量
                                    temp_str += "<br><b><span style='font-size:14px;'>" + list_sort.Rows[i]["TaskType"].ToString() + "：</span></b>";

                                    if (Convert.ToInt32(list_sort.Rows[i]["Status"]) == 1)  //某天某项任务的完成情况
                                    {
                                        complete++;
                                        count++;
                                    }
                                    else
                                    {
                                        count++;
                                    }

                                    temp_type = list_sort.Rows[i]["TaskType"].ToString();
                                }

                            }
                            else   //不同天
                            {
                                //上一天输出

                                temp_str += complete + "/" + count;
                                complete = 0; count = 0;  //清空统计量
                                CompliacneDetailByD.Events = temp_str;
                                resultList.Add(CompliacneDetailByD);

                                if (list_sort.Rows[i]["Date"].ToString() != "end")
                                {
                                    //获取新一天
                                    CompliacneDetailByD = new CompliacneDetailByD();
                                    CompliacneDetailByD.Date = list_sort.Rows[i]["Date"].ToString();
                                    //CompliacneDetailByD.ComplianceValue = list_sort.Rows[i]["ComplianceValue"].ToString();

                                    if (Convert.ToDouble(list_sort.Rows[i]["ComplianceValue"]) == 0)  //某天依从率
                                    {
                                        CompliacneDetailByD.drugBullet = "";
                                        CompliacneDetailByD.drugColor = "#DADADA";
                                    }
                                    else if (Convert.ToDouble(list_sort.Rows[i]["ComplianceValue"]) == 1)
                                    {
                                        CompliacneDetailByD.drugBullet = "";
                                        CompliacneDetailByD.drugColor = "#777777";
                                    }
                                    else
                                    {
                                        CompliacneDetailByD.drugBullet = "amcharts-images/drug.png";
                                        CompliacneDetailByD.drugColor = "";
                                    }

                                    temp_str = "";
                                    temp_str += "该天依从率：" + list_sort.Rows[i]["ComplianceValue"].ToString() + "<br>";
                                    temp_str += "<b><span style='font-size:14px;'>" + list_sort.Rows[i]["TaskType"].ToString() + "：</span></b>";

                                    if (Convert.ToInt32(list_sort.Rows[i]["Status"]) == 1)  //某天某项任务的完成情况
                                    {
                                        complete++;
                                        count++;
                                    }
                                    else
                                    {
                                        count++;
                                    }

                                    temp_date = list_sort.Rows[i]["Date"].ToString();
                                    temp_type = list_sort.Rows[i]["TaskType"].ToString();
                                }
                            }
                        }

                    }

                }

                return resultList;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetTasksComCountByPeriod", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }

        //某天所有任务的依从情况 整理加工 GL 2015-10-12
        public TaskComDetailByD GetImplementationByDate(DataConnection pclsCache, string PatientId, string PlanNo, int Date)
        {
            TaskComDetailByD TaskComDetailByD = new TaskComDetailByD();
            try
            {
                TaskComDetailByD.Date = Date.ToString().Substring(0, 4) + "-" + Date.ToString().Substring(4, 2) + "-" + Date.ToString().Substring(6, 2);
                TaskComDetailByD.WeekDay = new CommonFunction().CaculateWeekDay(TaskComDetailByD.Date);

                List<TasksComList> ComplianceList = new List<TasksComList>();
                ComplianceList = GetTasksComListByDate(pclsCache, PatientId, PlanNo, Date);

                #region 后期可能用于优化
                //先读任务表，读取体征，拿出新数据；再读药物表，超过三个则省略号
                //DataTable TaskList = new DataTable();
                //TaskList = PsTask.GetTaskList(pclsCache, PlanNo);

                ////读取体征，拿出当天最新数据
                //string condition = " Type = 'VitalSign'";
                //DataRow[] VitalSignRows = TaskList.Select(condition);

                //CacheSysList VitalSignList = new InterSystems.Data.CacheTypes.CacheSysList(System.Text.Encoding.Unicode, true, true);
                //for (int j=0; j < VitalSignRows.Length; j++)
                //{
                //    string code = VitalSignRows[j]["Type"].ToString();
                //    string[] sArray = code.Split(new char[] { '|' });;//拆分
                //    string type = sArray[0].ToString();
                //    VitalSignList = new InterSystems.Data.CacheTypes.CacheSysList(System.Text.Encoding.Unicode, true, true);
                //    VitalSignList = PsVitalSigns.GetSignByDay(pclsCache, PatientId, code, type, Date);
                //    if (VitalSignList != null)
                //    {

                //    }
                //}





                //体征
                /*
                string condition = " Type = 'VitalSign'";
                DataRow[] VitalSignRows = ComplianceList.Select(condition);

                List<TaskCom> TaskComList = new List<TaskCom>();
                TaskCom TaskCom = new TaskCom();
                for (int j = 0; j < VitalSignRows.Length; j++)
                {
                    VitalTaskCom = new VitalTaskCom();
                    VitalTaskCom.SignName = VitalSignRows[j]["TaskName"].ToString();
                    VitalTaskCom.Status = VitalSignRows[j]["Status"].ToString();
                    if (TaskCom.TaskStatus == "1")
                    {
                        string code = VitalSignRows[j]["TaskCode"].ToString();
                        string[] sArray = code.Split(new char[] { '|' }); ;//拆分
                        string type = sArray[0].ToString();
                        //CacheSysList VitalSignList = new InterSystems.Data.CacheTypes.CacheSysList(System.Text.Encoding.Unicode, true, true);
                        CacheSysList VitalSignList = new InterSystems.Data.CacheTypes.CacheSysList(System.Text.Encoding.Unicode, true, true);
                        VitalSignList = PsVitalSigns.GetSignByDay(pclsCache, PatientId, code, type, Date);
                        if (VitalSignList != null)
                        {
                            
                            VitalTaskCom.Time = PulList[1].ToString();
                            VitalTaskCom.Value = PulList[2].ToString();
                            VitalTaskCom.Unit = PulList[3].ToString();
                            
                        } 
                    }
                    VitalTaskComList.Add(VitalTaskCom);
                }
                TaskComDetailByD.VitalTaskComList = VitalTaskComList;

                
                 string vitalCondition = " Type = 'VitalSign'";
                    DataRow[] VitalSignRows = ComplianceList.Select(vitalCondition);

                    if ((VitalSignRows != null) && (VitalSignRows.Length >= 2))
                    {



                        if (VitalSignRows.Length == 2)  //只有血压
                        {

                        }
                        else //血压和脉率
                        {

                        }
                    }
                
                */
                #endregion

                //取出当天的体征测量 若有与测试任务拼接好了
                //先写死取的生理参数
                List<VitalTaskCom> VitalTaskComList = new List<VitalTaskCom>();
                VitalTaskCom VitalTaskCom = new VitalTaskCom();

                string Module = "";
                GPlanInfo planInfo = GetPlanInfo(pclsCache, PlanNo);
                if (planInfo != null)
                {
                    Module = planInfo.Module;
                }

                if (Module == "M1")
                {
                    #region  高血压模块  需要考虑没有脉率任务的情况

                    //血压任务肯定有
                    int BPTime = 0;
                    int mark = 0;
                    string SysValue = "";
                    string DiaValue = "";
                    string Unit = "";

                    //string conditionBP1 = " TaskCode = 'Bloodpressure|Bloodpressure_1'";
                    List<TasksComList> BP1Rows = new List<TasksComList>();
                    if (ComplianceList != null)
                    {
                        foreach (TasksComList item in ComplianceList)
                        {
                            if (item.TaskCode == "Bloodpressure|Bloodpressure_1")
                            {
                                BP1Rows.Add(item);
                            }
                        }
                    }
                    if ((BP1Rows != null) && (BP1Rows.Count == 1))
                    {
                        if (BP1Rows[0].Status == "1")
                        {
                            VitalInfo SysList = new VitalInfoMethod().GetSignByDay(pclsCache, PatientId, "Bloodpressure", "Bloodpressure_1", Date);
                            if (SysList != null)
                            {
                                mark = 1;
                                BPTime = Convert.ToInt32(SysList.RecordTime);  //时刻数据库是"1043"形式，需要转换  取两者最新的那个时间好了 即谁大取谁
                                SysValue = SysList.Value;
                                Unit = SysList.Unit;
                            }
                        }
                    }

                    // string conditionBP2 = " TaskCode = 'Bloodpressure|Bloodpressure_2'";
                    List<TasksComList> BP2Rows = new List<TasksComList>();
                    if (ComplianceList != null)
                    {
                        foreach (TasksComList item in ComplianceList)
                        {
                            if (item.TaskCode == "Bloodpressure|Bloodpressure_2")
                            {
                                BP2Rows.Add(item);
                            }
                        }
                    }
                    if ((BP2Rows != null) && (BP2Rows.Count == 1))
                    {
                        if (BP2Rows[0].Status == "1")
                        {
                            VitalInfo DiaList = new VitalInfoMethod().GetSignByDay(pclsCache, PatientId, "Bloodpressure", "Bloodpressure_2", Date);
                            if (DiaList != null)
                            {
                                mark = 1;
                                int BPTime1 = Convert.ToInt32(DiaList.RecordTime);
                                if (BPTime <= BPTime1)
                                {
                                    BPTime = BPTime1;
                                }
                                DiaValue = DiaList.Value;
                            }
                        }
                    }

                    VitalTaskCom = new VitalTaskCom();
                    VitalTaskCom.SignName = "血压";
                    if (mark == 1)
                    {
                        VitalTaskCom.Status = "1";
                        VitalTaskCom.Time = new CommonFunction().TransTime(BPTime.ToString());
                        VitalTaskCom.Value = SysValue + "/" + DiaValue;
                        VitalTaskCom.Unit = Unit;
                    }
                    else
                    {
                        VitalTaskCom.Status = "0";
                    }
                    VitalTaskComList.Add(VitalTaskCom);



                    //脉率任务可能没没有，需要确认
                    //string conditionPR = " TaskCode = 'Pulserate|Pulserate_1'";
                    List<TasksComList> PulserateRows = new List<TasksComList>();
                    if (ComplianceList != null)
                    {
                        foreach (TasksComList item in ComplianceList)
                        {
                            if (item.TaskCode == "Pulserate|Pulserate_1")
                            {
                                BP2Rows.Add(item);
                            }
                        }
                    }
                    if ((PulserateRows != null) && (PulserateRows.Count == 1))
                    {
                        VitalTaskCom = new VitalTaskCom();
                        VitalTaskCom.SignName = "脉率";

                        if (PulserateRows[0].Status == "1")
                        {
                            VitalInfo PulList = new VitalInfoMethod().GetSignByDay(pclsCache, PatientId, "Pulserate", "Pulserate_1", Date);
                            if (PulList != null)
                            {

                                VitalTaskCom.Status = "1";
                                VitalTaskCom.Time = new CommonFunction().TransTime(PulList.RecordTime);
                                VitalTaskCom.Value = PulList.Value;
                                VitalTaskCom.Unit = PulList.Unit;
                            }
                            else
                            {
                                VitalTaskCom.Status = "0";

                            }
                        }
                        else
                        {
                            VitalTaskCom.Status = "0";

                        }
                        VitalTaskComList.Add(VitalTaskCom);
                    }
                    #endregion
                }

                TaskComDetailByD.VitalTaskComList = VitalTaskComList;

                TaskComByType TaskComByType = new TaskComByType();
                List<TaskCom> TaskComList = new List<TaskCom>();
                TaskCom TaskCom = new TaskCom();

                //生活方式 
                //string condition = " Type = 'LifeStyle'";
                List<TasksComList> LifeStyleRows = new List<TasksComList>();
                if (ComplianceList != null)
                {
                    foreach (TasksComList item in ComplianceList)
                    {
                        if (item.Type == "LifeStyle")
                        {
                            LifeStyleRows.Add(item);
                        }
                    }
                }
                if ((LifeStyleRows != null) && (LifeStyleRows.Count > 0))
                {
                    TaskComByType = new TaskComByType();
                    TaskComByType.TaskType = "生活方式";
                    TaskComList = new List<TaskCom>();
                    TaskCom = new TaskCom();

                    foreach (TasksComList item in LifeStyleRows)
                    {
                        TaskCom = new TaskCom();
                        TaskCom.TaskName = item.TaskName;
                        TaskCom.TaskStatus = item.Status;
                        TaskComList.Add(TaskCom);
                    }
                    TaskComByType.TaskComList = TaskComList;
                    TaskComDetailByD.TaskComByTypeList.Add(TaskComByType);
                }

                //用药情况
                //condition = " Type = 'Drug'";
                List<TasksComList> DrugRows = new List<TasksComList>();
                if (ComplianceList != null)
                {
                    foreach (TasksComList item in ComplianceList)
                    {
                        if (item.Type == "Drug")
                        {
                            DrugRows.Add(item);
                        }
                    }
                }
                if ((DrugRows != null) && (DrugRows.Count > 0))
                {
                    TaskComByType = new TaskComByType();
                    TaskComByType.TaskType = "用药情况";
                    TaskComList = new List<TaskCom>();
                    TaskCom = new TaskCom();
                    foreach (TasksComList item in DrugRows)
                    {
                        TaskCom = new TaskCom();
                        TaskCom.TaskName = item.TaskName;
                        TaskCom.TaskStatus = item.Status;
                        TaskComList.Add(TaskCom);
                    }
                    TaskComByType.TaskComList = TaskComList;
                    TaskComDetailByD.TaskComByTypeList.Add(TaskComByType);
                }
                return TaskComDetailByD;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PsCompliance.GetImplementationByDate", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        public double GetComplianceByPlanNo(DataConnection pclsCache, string PlanNo)
        {
            double ret=0;
            try
            {
                ret = Convert.ToDouble(Ps.Compliance.GetComplianceByPlanNo(pclsCache.CacheConnectionObject, PlanNo));
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetComplianceByPlanNo", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        public List<ItemCompliance> GetItemCompliance(DataConnection pclsCache, string PlanNo, string CategoryCode, string Code)
        {
            List<ItemCompliance> list = new List<ItemCompliance>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.ComplianceDetail.GetItemCompliance(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PlanNo", CacheDbType.NVarChar).Value = PlanNo;
                cmd.Parameters.Add("CategoryCode", CacheDbType.NVarChar).Value = CategoryCode;
                cmd.Parameters.Add("Code", CacheDbType.NVarChar).Value = Code;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    ItemCompliance NewLine = new ItemCompliance();
                    NewLine.CategoryCode   = cdr["CategoryCode"].ToString();
                    NewLine.Code           = cdr["Code"].ToString();
                    NewLine.Name           = cdr["Name"].ToString();
                    NewLine.Date           = Convert.ToInt32(cdr["Date"]);
                    NewLine.Status         = cdr["Status"].ToString();
                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetItemCompliance", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }
        #endregion

        #region<PsTask>
        //CSQ 20151025
        public int PsTaskSetData(DataConnection pclsCache, string PlanNo,  string Type, string Code,string SortNo, string Instruction, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            int ret = 2;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }

                ret = (int)Ps.Task.SetData(pclsCache.CacheConnectionObject, PlanNo,  Type, Code,SortNo, Instruction, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.Task.SetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

     
        //CSQ 20151026 Cm.MstTaskDetail获取数据
        public List<TaskDetail> GetTaskDetails(DataConnection pclsCache, string CategoryCode, string Code)
        {
            List<TaskDetail> list = new List<TaskDetail>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Cm.MstTaskDetail.GetTaskDetail(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("CategoryCode", CacheDbType.NVarChar).Value = CategoryCode;
                cmd.Parameters.Add("Code", CacheDbType.NVarChar).Value = Code;


                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Add(new TaskDetail
                    {
                        Module = cdr["Module"].ToString(),
                        CurativeEffect = cdr["CurativeEffect"].ToString(),
                        SideEffect = cdr["SideEffect"].ToString(),
                        Instruction = cdr["Instruction"].ToString(),
                        HealthEffect = cdr["HealthEffect"].ToString(),
                        Unit = cdr["Unit"].ToString(),
                        Redundance = cdr["Redundance"].ToString(),                      
                    });

                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Cm.MstTask.GetTaskDetail", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }

        //CSQ 20151025
        public List<PsTask> GetTasks(DataConnection pclsCache, string PlanNo,string ParentCode,string Date,string PatientId)
        {
            List<PsTask> list = new List<PsTask>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Task.GetTask(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PlanNo", CacheDbType.NVarChar).Value = PlanNo;
                cmd.Parameters.Add("ParentCode", CacheDbType.NVarChar).Value = ParentCode;
                cmd.Parameters.Add("Date", CacheDbType.NVarChar).Value = Date;
                cmd.Parameters.Add("PatientId", CacheDbType.NVarChar).Value = PatientId;

                
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Add(new PsTask
                    {                   
                        Type = cdr["Type"].ToString(),
                        Code = cdr["Code"].ToString(),
                        SortNo = cdr["SortNo"].ToString(),
                        Name = cdr["Name"].ToString(),
                        InvalidFlag = cdr["InvalidFlag"].ToString(),
                        Status = cdr["Status"].ToString(),
                        Instruction = cdr["Instruction"].ToString(),
                        ParentCode = cdr["ParentCode"].ToString(),
                        Description = cdr["Description"].ToString(),
                        GroupHeaderFlag = cdr["GroupHeaderFlag"].ToString(),
                        ControlType = cdr["ControlType"].ToString(),
                        OptionCategory = cdr["OptionCategory"].ToString(),
                        VitalSignValue = cdr["VitalSignValue"].ToString()
                    });

                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.Task.GetTask", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }

        public int DeteteTask(DataConnection pclsCache, string Plan, string Type, string Code, string SortNo)
        {
            int ret = 0;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }
                ret = (int)Ps.Task.Delete(pclsCache.CacheConnectionObject, Plan, Type, Code, SortNo);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.DeteteTask", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        //syf 20150104 只输入PlanNo获取某个计划下的所有任务
        public List<PsTask> GetAllTask(DataConnection pclsCache, string PlanNo)
        {
            List<PsTask> list = new List<PsTask>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Task.GetAllTask(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PlanNo", CacheDbType.NVarChar).Value = PlanNo;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Add(new PsTask
                    {
                        Type = cdr["Type"].ToString(),
                        Code = cdr["Code"].ToString(),
                        Name = cdr["Name"].ToString(),
                        Instruction = cdr["Instruction"].ToString(),
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetAllTask", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }

        #endregion

        #region<PsTarget>
        //WF 20151010
        public int PsTargetSetData(DataConnection pclsCache, string Plan, string Type, string Code, string Value, string Origin, string Instruction, string Unit, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            int ret = 2;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }

                ret = (int)Ps.Target.SetData(pclsCache.CacheConnectionObject, Plan, Type, Code, Value, Origin, Instruction, Unit, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.Target.SetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }
        
        //WF 20151010  只针对一种参数   使用
        public TargetByCode GetTarget(DataConnection pclsCache, string PlanNo, string Type, string Code)
        {
            TargetByCode targetByCode = new TargetByCode();
          
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                InterSystems.Data.CacheTypes.CacheSysList CacheList = Ps.Target.GetTarget(pclsCache.CacheConnectionObject, PlanNo, Type, Code);
                if (CacheList != null)
                {
                    targetByCode.Type = CacheList[1].ToString();
                    targetByCode.Code = CacheList[2].ToString();
                    targetByCode.Value = CacheList[3].ToString();
                    targetByCode.Origin = CacheList[4].ToString();
                    targetByCode.Instruction = CacheList[5].ToString();
                    targetByCode.Unit = CacheList[6].ToString();
                }
                return targetByCode;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PsTarget.GetTarget", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        #endregion

        #region <CmMstBloodPressure>
        /// <summary>
        /// 获取某血压值的级别 LY 2015-10-12
        /// </summary>
        /// <param name="BPType"></param>
        /// <param name="Value"></param>
        /// <param name="Reference"></param>
        /// <returns></returns>
        public string GetSignBPGrade(string BPType, int Value, List<MstBloodPressure> Reference)
        {
            //算法：将值Value与分级的值一一相比，注意  >Value；   <= Value <；   <=Value 等区间，确定其级别（取左边的值）  目前Name少了一个，暂时默认少了偏高
            string Name = "不明范围";  //待标记颜色
            try
            {
                if (BPType == "Bloodpressure_1")  //收缩压 3  标准肯定不只1个值！
                {
                    //按照血压规定  分级保证>2
                    if (Value < Reference[0].SBP)
                    {
                        Name = "错误";
                    }
                    if (Value > Reference[Reference.Count - 1].SBP)
                    {
                        Name = "错误";
                    }
                    if (Name == "不明范围")
                    {
                        if (Reference.Count >= 2)  //前两步已经保证了，数量肯定》2   
                        {
                            for (int i = 0; i <= Reference.Count - 2; i++)  //要求个数>=2
                            {
                                if ((Value >= Reference[i].SBP) && (Value < Reference[i + 1].SBP)) //左闭右开
                                {
                                    Name = Reference[i].Name;  //名字就低
                                    break;
                                }
                            }
                        }
                    }
                }
                else  //舒张压
                {
                    if (Value < Reference[0].DBP)
                    {
                        Name = "错误";
                    }
                    if (Value > Reference[Reference.Count - 1].DBP)
                    {
                        Name = "错误";
                    }
                    if (Name == "不明范围")
                    {
                        if (Reference.Count >= 2)  //前两步已经保证了，数量肯定》2
                        {
                            for (int i = 0; i <= Reference.Count - 2; i++)  //要求个数>=2
                            {
                                if ((Value >= Reference[i].DBP) && (Value < Reference[i + 1].DBP))
                                {
                                    Name = Reference[i].Name;
                                    break;
                                }
                            }
                        }
                    }
                }
                return Name;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CmMstBloodPressure.GetSignBPGrade", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// 根据收缩压获取血压等级说明 LY 2015-10-12
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="SBP"></param>
        /// <returns></returns>
        public string GetDescription(DataConnection pclsCache, int SBP)
        {
            string ret = "";
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }
                ret = (string)Cm.MstBloodPressure.GetDescription(pclsCache.CacheConnectionObject, SBP);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Cm.MstBloodPressure.GetDescription", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// 从数据库获取血压分级规则 LY 2015-10-12
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <returns></returns>
        public List<MstBloodPressure> GetBPGrades(DataConnection pclsCache)
        {
            List<MstBloodPressure> result = new List<MstBloodPressure>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Cm.MstBloodPressure.GetBPGrades(pclsCache.CacheConnectionObject);
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    MstBloodPressure MstBloodPressure = new MstBloodPressure();
                    MstBloodPressure.Code = cdr["Code"].ToString();
                    MstBloodPressure.Name = cdr["Name"].ToString();
                    MstBloodPressure.Description = cdr["Description"].ToString();
                    MstBloodPressure.SBP = Convert.ToInt32(cdr["SBP"]);
                    MstBloodPressure.DBP = Convert.ToInt32(cdr["DBP"]);
                    MstBloodPressure.PatientClass = cdr["PatientClass"].ToString();
                    MstBloodPressure.Redundance = cdr["Redundance"].ToString();
                    result.Add(MstBloodPressure);
                }
                return result;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CmMstBloodPressure.GetBPGrades", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// 获取血压点和数据图区块的颜色 LY 2015-10-12
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetBPColor(string name, string type)
        {
            string colorShow = "";  //默认颜色
            try
            {
                if (type == "bullet")
                {
                    colorShow = "#FFC78E";
                    switch (name)
                    {
                        case "很高": colorShow = "#930000";  //红色
                            break;
                        case "偏高": colorShow = "#CC0000";  //
                            break;
                        case "警戒": colorShow = "#0000cc"; //
                            break;
                        case "正常": colorShow = "#2894FF";  //
                            break;
                        case "偏低": colorShow = "#FFC78E";  //
                            break;
                        default: break;
                    }
                }
                else   //fill
                {
                    colorShow = "#8080C0";
                    switch (name)
                    {
                        case "很高": colorShow = "#FF0000";  //深红色
                            break;
                        case "偏高": colorShow = "#FF60AF";  //微红
                            break;
                        case "警戒": colorShow = "#FFA042"; //橙色
                            break;
                        case "正常": colorShow = "#00DB00";  //绿色
                            break;
                        case "偏低": colorShow = "#8080C0";  //微紫
                            break;
                        default: break;
                    }
                }
                return colorShow;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CmMstBloodPressure.GetBPColor", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        #endregion

        #region DataMethod 第二层
        /// <summary>
        /// 生成收缩压/舒张压/脉率点图，并分级 LY 2015-10-12
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <param name="PlanNo"></param>
        /// <param name="Code"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        public List<Graph> GetSignInfoByM1(DataConnection pclsCache, string UserId, string PlanNo, string Code, int StartDate, int EndDate, List<MstBloodPressure> reference)
        {
            List<Graph> graphList = new List<Graph>();
            //获取系统时间  数据库连接，这样写，应该对的
            string serverTime = "";
            if (pclsCache.Connect())
            {
                serverTime = Convert.ToDateTime(Cm.CommonLibrary.GetServerDateTime(pclsCache.CacheConnectionObject)).ToString("yyyyMMdd");
            }
            pclsCache.DisConnect();
            try
            {
                //输入的code是拼接字段
                //string[] strCode = Code.Split(new char[] { '|' });
                //string ItemType = strCode[0];
                //string ItemCode = strCode[1];
                //M1的关注含下列几表   PsCompliance.GetSignDetailByPeriod方法保证几表条数相等 即使某天没体征数据，也会输出""字符串  
                //收缩压表
                List<SignDetailByPeriod> sysInfo = new List<SignDetailByPeriod>();
                sysInfo = GetSignDetailByPeriod(pclsCache, UserId, PlanNo, "Bloodpressure", "Bloodpressure_1", StartDate, EndDate);
                //RecordDate、RecordTime、Value、Unit
                //舒张压表
                List<SignDetailByPeriod> diaInfo = new List<SignDetailByPeriod>();
                diaInfo = GetSignDetailByPeriod(pclsCache, UserId, PlanNo, "Bloodpressure", "Bloodpressure_2", StartDate, EndDate);
                //脉率表
                List<SignDetailByPeriod> pulInfo = new List<SignDetailByPeriod>();
                pulInfo = GetSignDetailByPeriod(pclsCache, UserId, PlanNo, "Pulserate", "Pulserate_1", StartDate, EndDate);
                //三张表都有数据
                if ((sysInfo != null)&&(diaInfo != null)&&(pulInfo != null))
                {
                    if ((sysInfo.Count == diaInfo.Count) && (sysInfo.Count == pulInfo.Count) && (sysInfo.Count > 0))
                    {
                        for (int rowsCount = 0; rowsCount < sysInfo.Count; rowsCount++)
                        {
                            Graph Graph = new Graph();
                            Graph.Date = sysInfo[rowsCount].RecordDate;
                            #region 值、等级、颜色
                            //值、等级、颜色、描述文本
                            if ((Code == "Bloodpressure|Bloodpressure_1") && (reference != null)) //血压要求 reference 不为空
                            {
                                #region 收缩压
                                Graph.SignValue = "#";
                                if (sysInfo[rowsCount].Value != "")
                                {
                                    Graph.SignValue = sysInfo[rowsCount].Value;
                                }
                                //Graph.SignValue = sysInfo.Rows[rowsCount]["Value"].ToString();
                                if (Graph.SignValue != "#")
                                {
                                    Graph.SignGrade = GetSignBPGrade("Bloodpressure_1", Convert.ToInt32(Graph.SignValue), reference);
                                    Graph.SignColor = GetBPColor(Graph.SignGrade, "bullet");
                                    //判断是否都有值
                                    if ((sysInfo[rowsCount].Value != "") && (diaInfo[rowsCount].Value != "") && (pulInfo[rowsCount].Value != ""))
                                    {
                                        Graph.SignDescription = "血压：<b><span style='font-size:14px;'>" + sysInfo[rowsCount].Value + "</span></b>/" + diaInfo[rowsCount].Value + "mmHg<br>脉搏：" + pulInfo[rowsCount].Value + "次/分";
                                    }
                                    else if ((sysInfo[rowsCount].Value != "") || (diaInfo[rowsCount].Value != "") && (pulInfo[rowsCount].Value == ""))
                                    {
                                        Graph.SignDescription = "血压：<b><span style='font-size:14px;'>" + sysInfo[rowsCount].Value + "</span></b>/" + diaInfo[rowsCount].Value + "mmHg";
                                    }
                                    else if ((sysInfo[rowsCount].Value == "") && (diaInfo[rowsCount].Value == "") && (pulInfo[rowsCount].Value != ""))
                                    {
                                        Graph.SignDescription = "脉搏：" + pulInfo[rowsCount].Value + "次/分";
                                    }
                                    //Graph.SignDescription = "血压：<b><span style='font-size:14px;'>" + sysInfo.Rows[rowsCount]["Value"].ToString() + "</span></b>/" + diaInfo.Rows[rowsCount]["Value"].ToString() + "mmHg<br>脉搏：" + pulInfo.Rows[rowsCount]["Value"].ToString() + "次/分";
                                    //"[[category]]<br>血压：<b><span style='font-size:14px;'>[[SBPvalue]] </span></b>/[[DBPvalue]]mmHg<br>脉搏：66次/分"
                                }
                                else
                                {
                                    Graph.SignGrade = "";
                                    Graph.SignColor = "";
                                    Graph.SignDescription = "";
                                }
                                #endregion
                            }
                            else if ((Code == "Bloodpressure|Bloodpressure_2") && (reference != null))  //舒张压
                            {
                                #region 舒张压
                                Graph.SignValue = "#";
                                if (diaInfo[rowsCount].Value != "")
                                {
                                    Graph.SignValue = diaInfo[rowsCount].Value;
                                }
                                //Graph.SignValue = diaInfo.Rows[rowsCount]["Value"].ToString();
                                if (Graph.SignValue != "#")
                                {
                                    Graph.SignGrade = GetSignBPGrade("Bloodpressure_2", Convert.ToInt32(Graph.SignValue), reference);
                                    Graph.SignColor = GetBPColor(Graph.SignGrade, "bullet");
                                    //Graph.SignDescription = "血压：" + sysInfo.Rows[rowsCount]["Value"].ToString() + "/<b><span style='font-size:14px;'>" + diaInfo.Rows[rowsCount]["Value"].ToString() + "</span></b>mmHg<br>脉搏：" + pulInfo.Rows[rowsCount]["Value"].ToString() + "次/分";
                                    //判断是否都有值
                                    if ((sysInfo[rowsCount].Value != "") && (diaInfo[rowsCount].Value != "") && (pulInfo[rowsCount].Value != ""))
                                    {
                                        Graph.SignDescription = "血压：" + sysInfo[rowsCount].Value + "/<b><span style='font-size:14px;'>" + diaInfo[rowsCount].Value + "</span></b>mmHg<br>脉搏：" + pulInfo[rowsCount].Value + "次/分";
                                    }
                                    else if ((sysInfo[rowsCount].Value != "") || (diaInfo[rowsCount].Value != "") && (pulInfo[rowsCount].Value == ""))
                                    {
                                        Graph.SignDescription = "血压：" + sysInfo[rowsCount].Value + "/<b><span style='font-size:14px;'>" + diaInfo[rowsCount].Value + "</span></b>mmHg";
                                    }
                                    else if ((sysInfo[rowsCount].Value == "") && (diaInfo[rowsCount].Value == "") && (pulInfo[rowsCount].Value != ""))
                                    {
                                        Graph.SignDescription = "脉搏：" + pulInfo[rowsCount].Value + "次/分";
                                    }
                                }
                                else
                                {
                                    Graph.SignGrade = "";
                                    Graph.SignColor = "";
                                    Graph.SignDescription = "";
                                }
                                #endregion
                            }
                            else if (Code == "Pulserate|Pulserate_1")  //脉率
                            {
                                #region 脉率
                                Graph.SignValue = "#";
                                if (pulInfo[rowsCount].Value != "")
                                {
                                    Graph.SignValue = pulInfo[rowsCount].Value;
                                }
                                //判断是否都有值
                                if ((sysInfo[rowsCount].Value != "") && (diaInfo[rowsCount].Value != "") && (pulInfo[rowsCount].Value != ""))
                                {
                                    Graph.SignDescription = "血压：" + sysInfo[rowsCount].Value + "/" + diaInfo[rowsCount].Value + "mmHg<br>脉率：<b><span style='font-size:14px;'>" + pulInfo[rowsCount].Value + "</span></b>次/分";
                                }
                                else if ((sysInfo[rowsCount].Value != "") || (diaInfo[rowsCount].Value != "") && (pulInfo[rowsCount].Value == ""))
                                {
                                    Graph.SignDescription = "血压：" + sysInfo[rowsCount].Value + "/" + diaInfo[rowsCount].Value + "mmHg";
                                }
                                else if ((sysInfo[rowsCount].Value == "") && (diaInfo[rowsCount].Value == "") && (pulInfo[rowsCount].Value != ""))
                                {
                                    Graph.SignDescription = "脉率：<b><span style='font-size:14px;'>" + pulInfo[rowsCount].Value + "</span></b>次/分";
                                }
                                if (Graph.SignValue != "#")
                                {
                                    //脉率的分级 写死
                                    if (Convert.ToDouble(Graph.SignValue) < 60)  //过慢
                                    {
                                        Graph.SignGrade = "过慢";
                                        Graph.SignColor = "#8080C0"; //微紫
                                    }
                                    else if (Convert.ToDouble(Graph.SignValue) > 100) //过快
                                    {
                                        Graph.SignGrade = "过快";
                                        Graph.SignColor = "#FF60AF";  //微红
                                    }
                                    else //成人 60~100之间包括端点 正常
                                    {
                                        Graph.SignGrade = "正常";
                                        Graph.SignColor = "#00DB00";  //绿色
                                    }
                                }
                                else
                                {
                                    Graph.SignGrade = "";
                                    Graph.SignColor = "";
                                    Graph.SignDescription = "";
                                }
                                #endregion
                            }
                            #endregion
                            //形状
                            if (rowsCount != sysInfo.Count - 1)
                            {
                                Graph.SignShape = "round";
                                Graph.SignShape = "round";
                            }
                            else
                            {
                                if (serverTime == Graph.Date)  //当天的血压点形状用菱形
                                {
                                    Graph.SignShape = "diamond";
                                    Graph.SignShape = "diamond";
                                }
                                else
                                {
                                    Graph.SignShape = "round";
                                    Graph.SignShape = "round";
                                }
                            }
                            graphList.Add(Graph);
                        }
                    }
                }
                //有血压任务，没有脉率
                return graphList;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CmMstBloodPressure.GetSignInfoByBP", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
            }
        }

        /// <summary>
        /// 输出用于图形的分级区域 LY 2015-10-12
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PlanNo"></param>
        /// <param name="Code"></param>
        /// <param name="Reference"></param>
        /// <returns></returns>
        public GraphGuide GetGuidesByCode(DataConnection pclsCache, string PlanNo, string Code, List<MstBloodPressure> Reference)
        {
            GraphGuide GraphGuide = new GraphGuide();   //输出
            List<Guide> GuideList = new List<Guide>();
            try
            {
                if ((Code == "Bloodpressure|Bloodpressure_1") && (Reference != null))
                {
                    #region 收缩压
                    GraphGuide.minimum = Convert.ToDouble(Reference[0].SBP);
                    GraphGuide.maximum = Convert.ToDouble(Reference[Reference.Count - 1].SBP);
                    //InterSystems.Data.CacheTypes.CacheSysList SysTarget = null;
                    TargetByCode SysTarget = GetTarget(pclsCache, PlanNo, "Bloodpressure", "Bloodpressure_1");
                    if (SysTarget != null)
                    {
                        //初始值
                        Guide originalGuide = new Guide();
                        originalGuide.value = SysTarget.Origin.ToString(); //值或起始值
                        originalGuide.toValue = "#CC0000";           //终值或""
                        originalGuide.label = "";      //中文定义 目标线、偏低、偏高等
                        //originalGuide.label = "起始" + "：" + originalGuide.value;
                        originalGuide.lineColor = "#FF5151";          //直线颜色  目标线  初始线
                        originalGuide.lineAlpha = "1";//直线透明度 0全透~1
                        originalGuide.dashLength = "8"; //虚线密度  4  8
                        originalGuide.color = "#CC0000";    //字体颜色
                        originalGuide.fontSize = "8"; //字体大小  默认14
                        originalGuide.position = "right"; //字体位置 right left
                        originalGuide.inside = "";//坐标系的内或外  false
                        originalGuide.fillAlpha = "";
                        originalGuide.fillColor = "";
                        GuideList.Add(originalGuide);
                        GraphGuide.original = originalGuide.value + "mmHg";
                        //目标值
                        Guide tagetGuide = new Guide();
                        tagetGuide.value = SysTarget.Value.ToString();
                        tagetGuide.toValue = "#CC0000";
                        tagetGuide.label = "";
                        //tagetGuide.label = "目标" + "：" + tagetGuide.value;
                        tagetGuide.lineColor = "#CC0000";
                        tagetGuide.lineAlpha = "1";
                        tagetGuide.dashLength = "4";
                        tagetGuide.color = "#CC0000";
                        tagetGuide.fontSize = "14";
                        tagetGuide.position = "right";
                        tagetGuide.inside = "";
                        tagetGuide.fillAlpha = "";
                        tagetGuide.fillColor = "";
                        GuideList.Add(tagetGuide);
                        GraphGuide.target = tagetGuide.value + "mmHg";
                    }
                    //风险范围
                    for (int i = 0; i <= Reference.Count - 2; i++)
                    {
                        //收缩压
                        Guide SysGuide = new Guide();
                        SysGuide.value = Reference[i].SBP.ToString(); //起始值
                        SysGuide.toValue = Reference[i + 1].SBP.ToString();                //终值
                        SysGuide.label = Reference[i].Name;          //偏低、偏高等
                        SysGuide.lineColor = "";     //直线颜色  目标线  初始线
                        SysGuide.lineAlpha = "";   //直线透明度 0全透~1
                        SysGuide.dashLength = "";  //虚线密度  4  8
                        SysGuide.color = "#CC0000";     //字体颜色
                        SysGuide.fontSize = "14";   //字体大小  默认14
                        SysGuide.position = "right";    //字体位置 right left
                        SysGuide.inside = "true";      //坐标系的内或外  false
                        SysGuide.fillAlpha = "0.1";
                        SysGuide.fillColor = GetBPColor(SysGuide.label, "fill");   //GetFillColor
                        GuideList.Add(SysGuide);
                    }
                    //一般线
                    for (int i = 0; i <= Reference.Count - 1; i++)
                    {
                        //收缩压
                        Guide SysGuide = new Guide();
                        SysGuide.value = Reference[i].SBP.ToString();
                        SysGuide.toValue = "#CC0000";
                        SysGuide.label = Reference[i].SBP.ToString();
                        SysGuide.lineColor = "#CC0000";
                        SysGuide.lineAlpha = "0.15";
                        SysGuide.dashLength = "";
                        SysGuide.color = "#CC0000";
                        SysGuide.fontSize = "8";
                        SysGuide.position = "left";
                        SysGuide.inside = "";
                        SysGuide.fillAlpha = "";
                        SysGuide.fillColor = "";
                        GuideList.Add(SysGuide);
                    }
                    #endregion
                }
                else if ((Code == "Bloodpressure|Bloodpressure_2") && (Reference != null))
                {
                    #region 舒张压
                    GraphGuide.minimum = Convert.ToDouble(Reference[0].DBP);
                    GraphGuide.maximum = Convert.ToDouble(Reference[Reference.Count - 1].DBP);
                    //InterSystems.Data.CacheTypes.CacheSysList DiaTarget = null;
                    TargetByCode DiaTarget = GetTarget(pclsCache, PlanNo, "Bloodpressure", "Bloodpressure_2");
                    if (DiaTarget != null)
                    {
                        //初始值
                        Guide originalGuide = new Guide();
                        originalGuide.value = DiaTarget.Origin.ToString(); //值或起始值
                        originalGuide.toValue = "#CC0000";           //终值或""
                        originalGuide.label = "";      //中文定义 目标线、偏低、偏高等
                        //originalGuide.label = "起始" + "：" + originalGuide.value;
                        originalGuide.lineColor = "#FF5151";          //直线颜色  目标线  初始线
                        originalGuide.lineAlpha = "1";//直线透明度 0全透~1
                        originalGuide.dashLength = "8"; //虚线密度  4  8
                        originalGuide.color = "#CC0000";    //字体颜色
                        originalGuide.fontSize = "8"; //字体大小  默认14
                        originalGuide.position = "right"; //字体位置 right left
                        originalGuide.inside = "";//坐标系的内或外  false
                        originalGuide.fillAlpha = "";
                        originalGuide.fillColor = "";
                        GuideList.Add(originalGuide);
                        GraphGuide.original = originalGuide.value + "mmHg";
                        //目标值
                        Guide tagetGuide = new Guide();
                        tagetGuide.value = DiaTarget.Value.ToString();
                        tagetGuide.toValue = "#CC0000";
                        tagetGuide.label = "";
                        //tagetGuide.label = "目标" + "：" + tagetGuide.value;
                        tagetGuide.lineColor = "#CC0000";
                        tagetGuide.lineAlpha = "1";
                        tagetGuide.dashLength = "4";
                        tagetGuide.color = "#CC0000";
                        tagetGuide.fontSize = "14";
                        tagetGuide.position = "right";
                        tagetGuide.inside = "";
                        tagetGuide.fillColor = "";
                        tagetGuide.fillAlpha = "";
                        GuideList.Add(tagetGuide);
                        GraphGuide.target = tagetGuide.value + "mmHg";
                    }
                    //风险范围
                    for (int i = 0; i <= Reference.Count - 2; i++)
                    {
                        //舒张压
                        Guide DiaGuide = new Guide();
                        DiaGuide.value = Reference[i].DBP.ToString();
                        DiaGuide.toValue = Reference[i + 1].DBP.ToString();
                        DiaGuide.label = Reference[i].Name;
                        DiaGuide.lineColor = "";
                        DiaGuide.lineAlpha = "";
                        DiaGuide.dashLength = "";
                        DiaGuide.color = "#CC0000";
                        DiaGuide.fontSize = "14";
                        DiaGuide.position = "right";
                        DiaGuide.inside = "true";
                        DiaGuide.fillAlpha = "0.1";
                        DiaGuide.fillColor = GetBPColor(DiaGuide.label, "fill");
                        GuideList.Add(DiaGuide);
                    }
                    //一般线
                    for (int i = 0; i <= Reference.Count - 1; i++)
                    {
                        //舒张压
                        Guide DiaGuide = new Guide();
                        DiaGuide.value = Reference[i].DBP.ToString();
                        DiaGuide.toValue = "#CC0000";
                        DiaGuide.label = Reference[i].DBP.ToString();
                        DiaGuide.lineColor = "#CC0000";
                        DiaGuide.lineAlpha = "0.15";
                        DiaGuide.dashLength = "";
                        DiaGuide.color = "#CC0000";
                        DiaGuide.fontSize = "8";
                        DiaGuide.position = "left";
                        DiaGuide.inside = "";
                        DiaGuide.fillAlpha = "";
                        DiaGuide.fillColor = "";
                        GuideList.Add(DiaGuide);
                    }
                    #endregion
                }
                else if (Code == "Pulserate|Pulserate_1") //脉率没有 初始值和目标值
                {
                    #region 脉率
                    GraphGuide.minimum = 30;
                    GraphGuide.maximum = 150;

                    //风险范围
                    //正常
                    Guide PulseGuide = new Guide();
                    PulseGuide.value = "60";
                    PulseGuide.toValue = "100";
                    PulseGuide.label = "正常";
                    PulseGuide.lineColor = "";
                    PulseGuide.lineAlpha = "";
                    PulseGuide.dashLength = "";
                    PulseGuide.color = "#00DB00";  //字的颜色
                    PulseGuide.fontSize = "14";
                    PulseGuide.position = "right";
                    PulseGuide.inside = "true";
                    PulseGuide.fillAlpha = "0.1";
                    PulseGuide.fillColor = "#2894FF";  //区域颜色
                    GuideList.Add(PulseGuide);
                    //偏高 #CC0000
                    PulseGuide = new Guide();
                    PulseGuide.value = "100";
                    PulseGuide.toValue = "150";
                    PulseGuide.label = "偏高";
                    PulseGuide.lineColor = "";
                    PulseGuide.lineAlpha = "";
                    PulseGuide.dashLength = "";
                    PulseGuide.color = "#FF60AF";
                    PulseGuide.fontSize = "14";
                    PulseGuide.position = "right";
                    PulseGuide.inside = "true";
                    PulseGuide.fillAlpha = "0.1";
                    PulseGuide.fillColor = "#CC0000";
                    GuideList.Add(PulseGuide);
                    //偏低
                    PulseGuide = new Guide();
                    PulseGuide.value = "30";
                    PulseGuide.toValue = "60";
                    PulseGuide.label = "偏低";
                    PulseGuide.lineColor = "";
                    PulseGuide.lineAlpha = "";
                    PulseGuide.dashLength = "";
                    PulseGuide.color = "#8080C0";
                    PulseGuide.fontSize = "14";
                    PulseGuide.position = "right";
                    PulseGuide.inside = "true";
                    PulseGuide.fillAlpha = "0.1";
                    PulseGuide.fillColor = "#FFC78E";
                    GuideList.Add(PulseGuide);
                    //一般线
                    //30
                    PulseGuide = new Guide();
                    PulseGuide.value = "30";
                    PulseGuide.toValue = "#CC0000";
                    PulseGuide.label = "30";
                    PulseGuide.lineColor = "#CC0000";
                    PulseGuide.lineAlpha = "0.15";
                    PulseGuide.dashLength = "";
                    PulseGuide.color = "#CC0000";
                    PulseGuide.fontSize = "8";
                    PulseGuide.position = "left";
                    PulseGuide.inside = "";
                    PulseGuide.fillAlpha = "";
                    PulseGuide.fillColor = "";
                    GuideList.Add(PulseGuide);
                    //60
                    PulseGuide = new Guide();
                    PulseGuide.value = "60";
                    PulseGuide.toValue = "#CC0000";
                    PulseGuide.label = "60";
                    PulseGuide.lineColor = "#CC0000";
                    PulseGuide.lineAlpha = "0.15";
                    PulseGuide.dashLength = "";
                    PulseGuide.color = "#CC0000";
                    PulseGuide.fontSize = "8";
                    PulseGuide.position = "left";
                    PulseGuide.inside = "";
                    PulseGuide.fillAlpha = "";
                    PulseGuide.fillColor = "";
                    GuideList.Add(PulseGuide);
                    //100
                    PulseGuide = new Guide();
                    PulseGuide.value = "100";
                    PulseGuide.toValue = "#CC0000";
                    PulseGuide.label = "100";
                    PulseGuide.lineColor = "#CC0000";
                    PulseGuide.lineAlpha = "0.15";
                    PulseGuide.dashLength = "";
                    PulseGuide.color = "#CC0000";
                    PulseGuide.fontSize = "8";
                    PulseGuide.position = "left";
                    PulseGuide.inside = "";
                    PulseGuide.fillAlpha = "";
                    PulseGuide.fillColor = "";
                    GuideList.Add(PulseGuide);
                    #endregion
                }
                GraphGuide.GuideList = GuideList;
                return GraphGuide;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CmMstBloodPressure.GetGuidesByCode", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
            }
        }

        #endregion

        /// <summary>
        /// GetSignByPeriod LS 2015-03-30  只针对一种参数   syf 20151029
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <param name="ItemType"></param>
        /// <param name="ItemCode"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public List<SignByPeriod> GetSignByPeriod(DataConnection pclsCache, string UserId, string ItemType, string ItemCode, int StartDate, int EndDate)
        {

            List<SignByPeriod> list = new List<SignByPeriod>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }

                cmd = new CacheCommand();
                cmd = Ps.VitalSigns.GetSignByPeriod(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;
                cmd.Parameters.Add("ItemType", CacheDbType.NVarChar).Value = ItemType;
                cmd.Parameters.Add("ItemCode", CacheDbType.NVarChar).Value = ItemCode;
                cmd.Parameters.Add("StartDate", CacheDbType.NVarChar).Value = StartDate;
                cmd.Parameters.Add("EndDate", CacheDbType.NVarChar).Value = EndDate;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    SignByPeriod NewLine = new SignByPeriod();
                    NewLine.RecordDate = cdr["RecordDate"].ToString();
                    NewLine.RecordTime = cdr["RecordTime"].ToString();
                    NewLine.Value = cdr["Value"].ToString();
                    NewLine.Unit = cdr["Unit"].ToString();
                    list.Add(NewLine);
                }

                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetSignByPeriod", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }

        //20151101 wf 健康专员首页加载患者列表
        public List<PatientListTable> GetPatientsPlan(DataConnection pclsCache, string DoctorId, string Module, string VitalType, string VitalCode)
        {
            List<PatientListTable> list = new List<PatientListTable>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Plan.GetPatientsPlan(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("DoctorId", CacheDbType.NVarChar).Value = DoctorId;
                cmd.Parameters.Add("Module", CacheDbType.NVarChar).Value = Module;
                cmd.Parameters.Add("VitalType", CacheDbType.NVarChar).Value = VitalType;
                cmd.Parameters.Add("VitalCode", CacheDbType.NVarChar).Value = VitalCode;
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    if (cdr["PatientId"].ToString() == string.Empty)
                    {
                        continue;
                    }
                    PatientListTable NewLine = new PatientListTable();
                    NewLine.PatientId = cdr["PatientId"].ToString();
                    NewLine.PatientName = cdr["PatientName"].ToString();
                    NewLine.photoAddress = cdr["PhotoAddress"].ToString(); 
                    NewLine.PlanNo = cdr["PlanNo"].ToString();
                    NewLine.StartDate = cdr["StartDate"].ToString();
                    NewLine.EndDate = cdr["EndDate"].ToString();
                    NewLine.Process = Convert.ToDouble(cdr["Process"]);
                    NewLine.TotalDays = cdr["TotalDays"].ToString();
                    NewLine.RemainingDays = cdr["RemainingDays"].ToString();
                    NewLine.Status = cdr["Status"].ToString();
                    
                   
                    if (cdr["ComplianceRate"].ToString() == "")
                    {
                        NewLine.ComplianceRate = 0;
                    }
                    else
                    {
                        NewLine.ComplianceRate = Convert.ToDouble(cdr["ComplianceRate"]);
                    }
                    //NewLine.ComplianceRate = Convert.ToDouble(cdr["ComplianceRate"]);
                    NewLine.VitalValue = cdr["VitalValue"].ToString();
                    NewLine.VitalUnit = cdr["VitalUnit"].ToString();
                    NewLine.TargetOrigin = cdr["TargetOrigin"].ToString();
                    NewLine.TargetValue = cdr["TargetValue"].ToString();
                    NewLine.SMSCount = cdr["SMSCount"].ToString();
                    NewLine.Module = cdr["Module"].ToString();

                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetPatientsPlan", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// 获取某个病人某个模块指定时间段内的依从率 SYF 20151102
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PatientId"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Module"></param>
        /// <returns></returns>
        public List<ComplianceDate> GetComplianceListInC(DataConnection pclsCache, string PatientId, string StartDate, string EndDate, string Module)
        {
            List<ComplianceDate> ComplianceList = new List<ComplianceDate>();
            try
            {
                List<GPlanInfo> list1 = new List<GPlanInfo>();
                List<ComplianceListByPeriod> list2 = new List<ComplianceListByPeriod>();

                list1 = new PlanInfoMethod().GetPlanListByMS(pclsCache, PatientId, Module, 0);
                int i = 0;
                int j = 0;//为了找两个特殊的计划，即包括输入的StartDate和EndDate的两个计划，后面取依从率就在这两个计划之间取
                int k = 0;
                if (list1 != null)
                {
                    for (; i < list1.Count; i++)// && (int.Parse(list1[i].EndDate) >= int.Parse(StartDate))
                    {
                        if ((int.Parse(list1[i].StartDate) <= int.Parse(StartDate)))
                        {
                            break;
                        }
                    }
                    if (i == list1.Count)
                    {
                        i--;
                    }

                    for (; j < list1.Count; j++)//(int.Parse(list1[j].StartDate) <= int.Parse(EndDate)) && 
                    {
                        if ((int.Parse(list1[j].EndDate) >= int.Parse(EndDate)))
                        {
                            break;
                        }
                    }
                    if (j == list1.Count)
                    {
                        j--;
                    }
                
                    for (k = i; k >= j; k--)
                    {//每次取一个计划的信息，但每个计划里面还是有多条信息
                        int SD = int.Parse(list1[k].StartDate) > int.Parse(StartDate) ? int.Parse(list1[k].StartDate) : int.Parse(StartDate);
                        int ED = int.Parse(list1[k].EndDate) < int.Parse(EndDate) ? int.Parse(list1[k].EndDate) : int.Parse(EndDate);
                        list2 = new PlanInfoMethod().GetComplianceListByPeriod(pclsCache, list1[k].PlanNo, SD, ED);
                        if (list2 != null)
                        {
                            for (int m = 0; m < list2.Count; m++)
                            {
                                ComplianceDate oneday = new ComplianceDate();   //输出
                                oneday.Date = list2[m].Date;
                                oneday.PlanNo = list1[k].PlanNo;
                                oneday.Compliance = list2[m].Compliance;
                                ComplianceList.Add(oneday);
                            }
                        }
                    }
                }
                return ComplianceList;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetComplianceListInC", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
            }
        }

        #region 暂时不用
        //public List<Parameters> GetParameters(DataConnection pclsCache, string Indicators)
        //{
        //    List<Parameters> list = new List<Parameters>();

        //    CacheCommand cmd = null;
        //    CacheDataReader cdr = null;

        //    try
        //    {
        //        if (!pclsCache.Connect())
        //        {
        //            return null;
        //        }
        //        cmd = new CacheCommand();
        //        cmd = Ps.Parameters.GetParameters(pclsCache.CacheConnectionObject);
        //        cmd.Parameters.Add("Indicators", CacheDbType.NVarChar).Value = Indicators;

        //        cdr = cmd.ExecuteReader();
        //        while (cdr.Read())
        //        {
        //            list.Add(new Parameters
        //            {
        //                Id = cdr["Id"].ToString(),
        //                Name = cdr["Name"].ToString(),
        //                Value = cdr["Value"].ToString(),
        //                Unit = cdr["Unit"].ToString()

        //            });
        //        }
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "RiskInfoMethod.GetParameters", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
        //        return null;
        //    }
        //    finally
        //    {
        //        if ((cdr != null))
        //        {
        //            cdr.Close();
        //            cdr.Dispose(true);
        //            cdr = null;
        //        }
        //        if ((cmd != null))
        //        {
        //            cmd.Parameters.Clear();
        //            cmd.Dispose();
        //            cmd = null;
        //        }
        //        pclsCache.DisConnect();
        //    }
        //}

        ////GetPlanList34ByM 获取某模块患者的正在执行的和结束的计划列表 GL 2015-10-12
        //public List<GPlanInfo> GetPlanList34ByM(DataConnection pclsCache, string PatientId, string Module)
        //{
        //    List<GPlanInfo> result = new List<GPlanInfo>();
        //    try
        //    {
        //        GPlanInfo list = GetExecutingPlanByM(pclsCache, PatientId, Module);
        //        if (list != null)
        //        {
        //            //GPlanInfo PlanDeatil = new GPlanInfo();
        //            //PlanDeatil.PlanNo = list.PlanNo;
        //            //PlanDeatil.StartDate = Convert.ToInt32(list.StartDate);
        //            //PlanDeatil.EndDate = Convert.ToInt32(list.EndDate);
        //            string temp = list.StartDate.ToString().Substring(0, 4) + "/" + list.StartDate.ToString().Substring(4, 2) + "/" + list.StartDate.ToString().Substring(6, 2);
        //            string temp1 = list.EndDate.ToString().Substring(0, 4) + "/" + list.EndDate.ToString().Substring(4, 2) + "/" + list.EndDate.ToString().Substring(6, 2);
        //            list.PlanName = "当前计划：" + temp + "-" + temp1;
        //            list.PlanCompliance = GetComplianceByPlanNo(pclsCache, list.PlanNo).ToString();
        //            Progressrate temp2 = new PlanInfoMethod().GetProgressRate(pclsCache, list.PlanNo);
        //            list.RemainingDays = "";
        //            if(temp2 != null)
        //            {
        //                list.RemainingDays = temp2.RemainingDays;
        //            }
        //        }
        //        else
        //        {
        //            //PlanDeatil PlanDeatil = new PlanDeatil();
        //            //PlanDeatil.PlanNo = "";
        //            list.PlanName = "当前计划";
        //        }
        //        result.Add(list);


        //        List<PlanDeatil> endingPlanList = new List<PlanDeatil>();
        //        endingPlanList = GetEndingPlan(pclsCache, PatientId, Module);
        //        if (endingPlanList!= null)
        //        {
        //            foreach (PlanDeatil item in endingPlanList)
        //            {
        //                GPlanInfo PlanDeatil = new GPlanInfo();
        //                PlanDeatil.PlanNo = item.PlanNo;
        //                PlanDeatil.StartDate = item.StartDate.ToString();
        //                PlanDeatil.EndDate = item.EndDate.ToString();
        //                string temp = PlanDeatil.StartDate.ToString().Substring(0, 4) + "/" + PlanDeatil.StartDate.ToString().Substring(4, 2) + "/" + PlanDeatil.StartDate.ToString().Substring(6, 2);
        //                string temp1 = PlanDeatil.EndDate.ToString().Substring(0, 4) + "/" + PlanDeatil.EndDate.ToString().Substring(4, 2) + "/" + PlanDeatil.EndDate.ToString().Substring(6, 2);
        //                PlanDeatil.PlanName = "往期：" + temp + "-" + temp1;
        //                PlanDeatil.PlanCompliance = GetComplianceByPlanNo(pclsCache, list.PlanNo).ToString();
        //                PlanDeatil.RemainingDays = "";
        //                Progressrate temp2 = new PlanInfoMethod().GetProgressRate(pclsCache, list.PlanNo);
        //                if (temp2 != null)
        //                {
        //                    PlanDeatil.RemainingDays = temp2.RemainingDays;
        //                }
        //                result.Add(PlanDeatil);
        //            }
        //        }
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetPlanList34ByM", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
        //        return null;
        //    }
        //    finally
        //    {
        //        pclsCache.DisConnect();
        //    }
        //}

        ////GetPlanList34ByM 获取某模块患者的正在执行的和结束的计划列表 GL 2015-10-12
        //public List<GPlanInfo> GetPlanList34ByM(DataConnection pclsCache, string PatientId, string Module)
        //{
        //    List<GPlanInfo> result = new List<GPlanInfo>();
        //    try
        //    {
        //        GPlanInfo list = GetExecutingPlanByM(pclsCache, PatientId, Module);
        //        if (list != null)
        //        {
        //            //GPlanInfo PlanDeatil = new GPlanInfo();
        //            //PlanDeatil.PlanNo = list.PlanNo;
        //            //PlanDeatil.StartDate = Convert.ToInt32(list.StartDate);
        //            //PlanDeatil.EndDate = Convert.ToInt32(list.EndDate);
        //            string temp = list.StartDate.ToString().Substring(0, 4) + "/" + list.StartDate.ToString().Substring(4, 2) + "/" + list.StartDate.ToString().Substring(6, 2);
        //            string temp1 = list.EndDate.ToString().Substring(0, 4) + "/" + list.EndDate.ToString().Substring(4, 2) + "/" + list.EndDate.ToString().Substring(6, 2);
        //            list.PlanName = "当前计划：" + temp + "-" + temp1;
        //            list.PlanCompliance = GetComplianceByPlanNo(pclsCache, list.PlanNo).ToString();
        //            list.RemainingDays = new PlanInfoMethod().GetProgressRate(pclsCache, list.PlanNo).RemainingDays;
        //        }
        //        else
        //        {
        //            //PlanDeatil PlanDeatil = new PlanDeatil();
        //            //PlanDeatil.PlanNo = "";
        //            list.PlanName = "当前计划";
        //        }
        //        result.Add(list);


        //        List<PlanDeatil> endingPlanList = new List<PlanDeatil>();
        //        endingPlanList = GetEndingPlan(pclsCache, PatientId, Module);
        //        foreach (PlanDeatil item in endingPlanList)
        //        {
        //            GPlanInfo PlanDeatil = new GPlanInfo();
        //            PlanDeatil.PlanNo = item.PlanNo;
        //            PlanDeatil.StartDate = item.StartDate.ToString();
        //            PlanDeatil.EndDate = item.EndDate.ToString();
        //            string temp = PlanDeatil.StartDate.ToString().Substring(0, 4) + "/" + PlanDeatil.StartDate.ToString().Substring(4, 2) + "/" + PlanDeatil.StartDate.ToString().Substring(6, 2);
        //            string temp1 = PlanDeatil.EndDate.ToString().Substring(0, 4) + "/" + PlanDeatil.EndDate.ToString().Substring(4, 2) + "/" + PlanDeatil.EndDate.ToString().Substring(6, 2);
        //            PlanDeatil.PlanName = "往期：" + temp + "-" + temp1;
        //            PlanDeatil.PlanCompliance = GetComplianceByPlanNo(pclsCache, list.PlanNo).ToString();
        //            PlanDeatil.RemainingDays = new PlanInfoMethod().GetProgressRate(pclsCache, list.PlanNo).RemainingDays;
        //            result.Add(PlanDeatil);
        //        }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetPlanList34ByM", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
        //        return null;
        //    }
        //    finally
        //    {
        //        pclsCache.DisConnect();
        //    }
        //}

        ///// <summary>
        ///// SYF 20151010 获取健康专员负责的所有患者列表
        ///// </summary>
        ///// <param name="pclsCache"></param>
        ///// <param name="DoctorId"></param>
        ///// <param name="Module"></param>
        ///// <returns></returns>
        //public List<PatientPlan> GetPatientsPlanByDoctorId(DataConnection pclsCache, string DoctorId, string Module)
        //{
        //    List<PatientPlan> list = new List<PatientPlan>();
        //    CacheCommand cmd = null;
        //    CacheDataReader cdr = null;
        //    try
        //    {
        //        if (!pclsCache.Connect())
        //        {
        //            return null;
        //        }
        //        cmd = new CacheCommand();
        //        cmd = Ps.Plan.GetPatientsPlanByDoctorId(pclsCache.CacheConnectionObject);
        //        cmd.Parameters.Add("DoctorId", CacheDbType.NVarChar).Value = DoctorId;
        //        cmd.Parameters.Add("Module", CacheDbType.NVarChar).Value = Module;

        //        cdr = cmd.ExecuteReader();
        //        while (cdr.Read())
        //        {
        //            if (cdr["PatientId"].ToString() == string.Empty)
        //            {
        //                continue;
        //            }
        //            PatientPlan NewLine = new PatientPlan();
        //            NewLine.PatientId = cdr["PatientId"].ToString();
        //            NewLine.PlanNo = cdr["PlanNo"].ToString();
        //            NewLine.StartDate = cdr["StartDate"].ToString();
        //            NewLine.EndDate = cdr["EndDate"].ToString();
        //            NewLine.TotalDays = cdr["TotalDays"].ToString();
        //            NewLine.RemainingDays = cdr["RemainingDays"].ToString();
        //            NewLine.Status = cdr["Status"].ToString();
        //            list.Add(NewLine);
        //        }
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetPatientsPlanByDoctorId", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
        //        return null;
        //    }
        //    finally
        //    {
        //        if ((cdr != null))
        //        {
        //            cdr.Close();
        //            cdr.Dispose(true);
        //            cdr = null;
        //        }
        //        if ((cmd != null))
        //        {
        //            cmd.Parameters.Clear();
        //            cmd.Dispose();
        //            cmd = null;
        //        }
        //        pclsCache.DisConnect();
        //    }
        //}
        #endregion

        #region<Ps.Log表>
        /// <summary>
        /// 获取某一版本计划的信息 syf 20151222
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PlanNo"></param>
        /// <returns></returns>
        public LogPlan GetLogPlanInfo(DataConnection pclsCache, string PlanNo)
        {
            try
            {
                LogPlan ret = new LogPlan();
                if (!pclsCache.Connect())
                {
                    return null;
                }
                InterSystems.Data.CacheTypes.CacheSysList list = null;
                list = Ps.LogPlan.GetLogPlanInfo(pclsCache.CacheConnectionObject, PlanNo);
                if (list != null)
                {
                    ret.PlanNo = list[0];
                    ret.Edition = list[1];
                    ret.PatientId = list[2];
                    ret.StartDate = list[3];
                    ret.EndDate = list[4];
                    ret.Module = list[5];
                    ret.Status = list[6];
                    ret.DoctorId = list[7];
                }
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetLogPlanInfo", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        public LogPlan GetLogPlanByEdition(DataConnection pclsCache, string PlanNo, int Edition)
        {
            try
            {
                LogPlan ret = new LogPlan();
                if (!pclsCache.Connect())
                {
                    return null;
                }
                InterSystems.Data.CacheTypes.CacheSysList list = null;
                list = Ps.LogPlan.GetLogPlanByEdition(pclsCache.CacheConnectionObject, PlanNo, Edition);
                if (list != null)
                {
                    ret.PlanNo = list[0];
                    ret.Edition = list[1];
                    ret.PatientId = list[2];
                    ret.StartDate = list[3];
                    ret.EndDate = list[4];
                    ret.Module = list[5];
                    ret.Status = list[6];
                    ret.DoctorId = list[7];
                }
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetLogPlanByEdition", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// 施宇帆 2015-12-22 获取某版本计划下所有任务的信息
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PlanNo"></param>
        /// <param name="piEdition"></param>
        /// <returns></returns>
        public List<LogTask> GetAllTaskByLogPlan(DataConnection pclsCache, string PlanNo, int piEdition)
        {
            List<LogTask> list = new List<LogTask>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.LogTask.GetAllTaskByLogPlan(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PlanNo", CacheDbType.NVarChar).Value = PlanNo;
                cmd.Parameters.Add("piEdition", CacheDbType.NVarChar).Value = piEdition;
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    LogTask NewLine = new LogTask();
                    NewLine.Type = cdr["Type"].ToString();
                    NewLine.Code = cdr["Code"].ToString();
                    NewLine.SortNo = cdr["SortNo"].ToString();
                    NewLine.Edition = cdr["Edition"].ToString();
                    NewLine.Instruction = cdr["Instruction"].ToString();

                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetAllTaskByLogPlan", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                if ((cdr != null))
                {
                    cdr.Close();
                    cdr.Dispose(true);
                    cdr = null;
                }
                if ((cmd != null))
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }
                pclsCache.DisConnect();
            }
        }


        #endregion

        //输入PlanNo获取该计划下所有任务的完成情况（即依从情况） 施宇帆 20150104
        public List<TaskCompliance> GetTaskCompliance(DataConnection pclsCache, string PlanNo)
        {

            List<TaskCompliance> TaskCompliance = new List<TaskCompliance>();
            try
            {
                List<PsTask> list1 = new List<PsTask>();
                list1 = new PlanInfoMethod().GetAllTask(pclsCache, PlanNo);
                if(list1 != null)
                {
                    for(int i=0; i<list1.Count; i++)
                    {
                        List<ItemCompliance> list2 = new List<ItemCompliance>();
                        list2 = new PlanInfoMethod().GetItemCompliance(pclsCache, PlanNo, list1[i].Type, list1[i].Code);
                        if(list2.Count != 0)
                        {
                            TaskCompliance NewLine = new TaskCompliance();
                            NewLine.AllDays = 0;
                            NewLine.DoDays = 0;
                            NewLine.UndoDays = 0;
                            NewLine.CategoryCode = list2[0].CategoryCode;
                            NewLine.Code = list2[0].Code;
                            NewLine.Name = list2[0].Name;
                            NewLine.Instruction = list1[i].Instruction;
                            for(int j=0; j<list2.Count; j++)
                            {
                                if(list2[j].Status == "0")
                                {
                                    NewLine.UndoDays++;
                                }
                                else if(list2[j].Status == "1")
                                {
                                    NewLine.DoDays++;
                                }
                                NewLine.AllDays++;
                            }
                            TaskCompliance.Add(NewLine);
                        }
                    }
                }
                return TaskCompliance;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetTaskCompliance", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
            }
        }

        #region<CmMstTask>
        public CmMstTaskN GetCmTaskItemInfo(DataConnection pclsCache, string CategoryCode, string Code)
        {
            try
            {
                CmMstTaskN ret = new CmMstTaskN();
                if (!pclsCache.Connect())
                {
                    return null;
                }
                InterSystems.Data.CacheTypes.CacheSysList list = null;
                list = Cm.MstTask.GetCmTaskItemInfo(pclsCache.CacheConnectionObject, CategoryCode, Code);
                if (list != null)
                {
                    ret.Name = list[0];
                    ret.ParentCode = list[1];
                    ret.Description = list[2];
                    ret.GroupHeaderFlag = Convert.ToInt32(list[3]);
                    ret.ControlType = Convert.ToInt32(list[4]);
                    ret.DateTime = list[5];
                    ret.UserId = list[6];
                    ret.UserName = list[7];
                }
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PlanInfoMethod.GetCmTaskItemInfo", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }
        #endregion

    }
}