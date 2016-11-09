using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CDMISrestful.CommonLibrary;
using InterSystems.Data.CacheClient;
using CDMISrestful.DataModels;
using CDMISrestful.DataViewModels;


namespace CDMISrestful.DataMethod
{

    public class ClinicInfoMethod
    {
        #region <PsClinicalInfo>
        /// <summary>
        /// 住院-转科处理 LY 2015-10-10  
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <param name="VisitId"></param>
        /// <returns></returns>
        public List<ClinicalTrans> PsClinicalInfoGetTransClinicalInfo(DataConnection pclsCache, string UserId, string VisitId)
        {
            //最终输出
            List<ClinicalTrans> List_Trans = new List<ClinicalTrans>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                //取出原始数据
                List<ClinicalTemp> List_Temp = new List<ClinicalTemp>();
                cmd = new CacheCommand();
                cmd = Ps.InPatientInfo.GetInfobyVisitId(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;
                cmd.Parameters.Add("VisitId", CacheDbType.NVarChar).Value = VisitId;
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    ClinicalTemp NewLine = new ClinicalTemp();
                    NewLine.SortNo = Convert.ToInt32(cdr["SortNo"]);
                    NewLine.AdmissionDate = Convert.ToDateTime(cdr["AdmissionDate"]);
                    NewLine.DisChargeDate = Convert.ToDateTime(cdr["DischargeDate"]);
                    NewLine.HospitalName = cdr["HospitalName"].ToString();
                    NewLine.DepartmentName = cdr["DepartmentName"].ToString();
                    List_Temp.Add(NewLine);
                }
                //有转科 
                //转科处理  转科内容：什么时候从哪里转出，什么时候转到哪
                if (List_Temp.Count > 1)
                {
                    for (int n = 0; n < List_Temp.Count - 1; n++)
                    {
                        //只科室
                        string things = List_Temp[n].DepartmentName + "(转出)" + "  ";
                        things += List_Temp[n + 1].DepartmentName + "(转入)";
                        ClinicalTrans NewLine = new ClinicalTrans();
                        NewLine.精确时间 = List_Temp[n + 1].AdmissionDate;
                        NewLine.类型 = "转科";
                        NewLine.VisitId = VisitId;
                        NewLine.事件 = things;
                        List_Trans.Add(NewLine);
                    }
                }
                return List_Trans;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ClinicInfoMethod.GetClinicalInfo", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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
        /// 检查化验等信息 LY 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <param name="VisitId"></param>
        /// <returns></returns>
        public List<ClinicalTrans> PsClinicalInfoGetOtherTable(DataConnection pclsCache, string UserId, string VisitId)
        {
            //输出表
            List<ClinicalTrans> List_Trans = new List<ClinicalTrans>();
            try
            {
                //诊断
                List<DiagnosisInfo> List_Diagnosis = new List<DiagnosisInfo>();
                List_Diagnosis = PsDiagnosisGetDiagnosisInfo(pclsCache, UserId, VisitId);
                if (List_Diagnosis != null)
                {
                    for (int i = 0; i < List_Diagnosis.Count; i++)
                    {
                         ClinicalTrans NewLine = new ClinicalTrans();
                         NewLine.精确时间 = Convert.ToDateTime(List_Diagnosis[i].RecordDate);
                         NewLine.类型 = "诊断";
                         NewLine.VisitId = VisitId;
                         NewLine.事件 = "诊断：" + List_Diagnosis[i].TypeName;
                         NewLine.关键属性 = "DiagnosisInfo" + "|" + VisitId + "|" + Convert.ToDateTime(List_Diagnosis[i].RecordDate).ToString("yyyy-MM-ddHH:mm:ss");
                         List_Trans.Add(NewLine);
                     }
                }
                
                //检查
                List<ExamInfo> List_Examination = new List<ExamInfo>();
                List_Examination = PsExaminationGetExaminationList(pclsCache, UserId, VisitId);
                if (List_Examination != null)
                {
                    for (int i = 0; i < List_Examination.Count; i++)
                    {
                        ClinicalTrans NewLine = new ClinicalTrans();
                        NewLine.精确时间 = Convert.ToDateTime(List_Examination[i].ExamDate);
                        NewLine.类型 = "检查";
                        NewLine.VisitId = VisitId;
                        NewLine.事件 = "检查：" + List_Examination[i].ExamTypeName;
                        NewLine.关键属性 = "ExaminationInfo" + "|" + VisitId + "|" + Convert.ToDateTime(List_Examination[i].ExamDate).ToString("yyyy-MM-ddHH:mm:ss");
                        List_Trans.Add(NewLine);
                    }
                }

                //化验
                List<LabTestList> List_LabTest = new List<LabTestList>();
                List_LabTest = GetLabTestList(pclsCache, UserId, VisitId);
                if (List_LabTest != null)
                {
                    for (int i = 0; i < List_LabTest.Count; i++)
                    {
                        ClinicalTrans NewLine = new ClinicalTrans();
                        NewLine.精确时间 = Convert.ToDateTime(List_LabTest[i].LabTestDate);
                        NewLine.类型 = "化验";
                        NewLine.VisitId = VisitId;
                        NewLine.事件 = "化验：" + List_LabTest[i].LabItemName;
                        NewLine.关键属性 = "LabTestInfo" + "|" + VisitId + "|" + Convert.ToDateTime(List_LabTest[i].LabTestDate).ToString("yyyy-MM-ddHH:mm:ss");
                        List_Trans.Add(NewLine);
                    }
                }

                //用药    
                List<DrugRecord> List_DrugRecord = new List<DrugRecord>();
                List_DrugRecord = GetDrugRecord(pclsCache, UserId, VisitId);
                if (List_DrugRecord != null)
                {
                    for (int i = 0; i < List_DrugRecord.Count; i++)
                    {
                        ClinicalTrans NewLine = new ClinicalTrans();
                        NewLine.精确时间 = Convert.ToDateTime(List_DrugRecord[i].StartDateTime);
                        NewLine.类型 = "用药";
                        NewLine.VisitId = VisitId;
                        NewLine.事件 = "用药：" + List_DrugRecord[i].HistoryContent;
                        NewLine.关键属性 = "DrugRecord" + "|" + VisitId + "|" + Convert.ToDateTime(List_DrugRecord[i].StartDateTime).ToString("yyyy-MM-ddHH:mm:ss");
                        List_Trans.Add(NewLine);
                    }
                }

                return List_Trans;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ClinicInfoMethod.GetClinicalInfo", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
            }
        }

        /// <summary>
        /// 获取门诊的下一日期 LY 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <param name="ClinicDate"></param>
        /// <returns></returns>
        public string PsClinicalInfoGetNextOutDate(DataConnection pclsCache, string UserId, string ClinicDate)
        {
            string ret = "";
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }
                ret = (string)Ps.OutPatientInfo.GetNextDatebyDate(pclsCache.CacheConnectionObject, UserId, Convert.ToDateTime(ClinicDate).ToString("yyyy-MM-dd HH:mm:ss"));
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.OutPatientInfo.GetNextDatebyDate", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// 获取住院的下一日期 LY 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <param name="AdmissionDate"></param>
        /// <returns></returns>
        public string PsClinicalInfoGetNextInDate(DataConnection pclsCache, string UserId, string AdmissionDate)
        {
            string ret = "";
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }
                ret = (string)Ps.InPatientInfo.GetNextDatebyDate(pclsCache.CacheConnectionObject, UserId, Convert.ToDateTime(AdmissionDate).ToString("yyyy-MM-dd HH:mm:ss"));
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ClinicInfoMethod.GetNextDatebyDate", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }


        /// <summary>
        /// 颜色分配：根据首标签决定 LY 2015-10-10
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string PsClinicalInfoGetColor(string type)
        {
            string colorShow = "clolor_default";
            try
            {
                switch (type)
                {
                    case "入院": colorShow = "clolor_in";
                        break;
                    case "出院": colorShow = "clolor_in";
                        break;
                    case "转科": colorShow = "clolor_trans";
                        break;
                    case "门诊": colorShow = "clolor_out";
                        break;
                    case "急诊": colorShow = "clolor_emer";
                        break;
                    case "住院中": colorShow = "clolor_inHos";
                        break;
                    case "当前住院中": colorShow = "clolor_inHos";
                        break;
                    default: break;
                }
                return colorShow;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ClinicInfoMethod.GetColor", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        #endregion
        #region <PsDiagnosis>
        /// <summary>
        /// 得到诊断信息 LY 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <param name="VisitId"></param>
        /// <returns></returns>
        public List<DiagnosisInfo> PsDiagnosisGetDiagnosisInfo(DataConnection pclsCache, string UserId, string VisitId)
        {
            List<DiagnosisInfo> list = new List<DiagnosisInfo>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Diagnosis.GetDiagnosisInfo(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;
                cmd.Parameters.Add("VisitId", CacheDbType.NVarChar).Value = VisitId;
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    string RecordDateShow = "";
                    string str1 = cdr["RecordDate"].ToString();
                    if (str1 != "")
                    {
                        RecordDateShow = str1.Substring(0, 4) + "-" + str1.Substring(4, 2) + "-" + str1.Substring(6, 2);
                    }
                    DiagnosisInfo NewLine = new DiagnosisInfo();
                    NewLine.VisitId = cdr["VisitId"].ToString();
                    NewLine.DiagnosisType = cdr["DiagnosisType"].ToString();
                    NewLine.DiagnosisTypeName = cdr["DiagnosisTypeName"].ToString();
                    NewLine.DiagnosisNo = cdr["DiagnosisNo"].ToString();
                    NewLine.Type = cdr["Type"].ToString();
                    NewLine.TypeName = cdr["TypeName"].ToString();
                    NewLine.DiagnosisCode = cdr["DiagnosisCode"].ToString();
                    NewLine.DiagnosisName = cdr["DiagnosisName"].ToString();
                    NewLine.Description = cdr["Description"].ToString();
                    NewLine.RecordDate = cdr["RecordDate"].ToString();
                    NewLine.RecordDateShow = RecordDateShow;
                    NewLine.Creator = cdr["Creator"].ToString();
                    NewLine.RecordDateCom = Convert.ToDateTime(cdr["RecordDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ClinicInfoMethod.GetDiagnosisInfo", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        #region <PsExamination>
        /// <summary>
        /// 得到检查信息 LY 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="piUserId"></param>
        /// <param name="piVisitId"></param>
        /// <returns></returns>
        public List<ExamInfo> PsExaminationGetExaminationList(DataConnection pclsCache, string piUserId, string piVisitId)
        {
            List<ExamInfo> list = new List<ExamInfo>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Examination.GetExaminationList(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("piUserId", CacheDbType.NVarChar).Value = piUserId;
                cmd.Parameters.Add("piVisitId", CacheDbType.NVarChar).Value = piVisitId;
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    string ReportDateShow = "";
                    if (cdr["ReportDate"].ToString() == "9999/1/1 0:00:00")
                    {
                        ReportDateShow = "";
                    }
                    else
                    {
                        ReportDateShow = cdr["ReportDate"].ToString();
                    }
                    ExamInfo NewLine = new ExamInfo();
                    NewLine.VisitId = cdr["VisitId"].ToString();
                    NewLine.SortNo = cdr["SortNo"].ToString();
                    NewLine.ExamType = cdr["ExamType"].ToString();
                    NewLine.ExamTypeName = cdr["ExamTypeName"].ToString();
                    NewLine.ExamDate = cdr["ExamDate"].ToString();
                    NewLine.ItemCode = cdr["ItemCode"].ToString();
                    NewLine.ItemName = cdr["ItemName"].ToString();
                    NewLine.ExamPara = cdr["ExamPara"].ToString();
                    NewLine.Description = cdr["Description"].ToString();
                    NewLine.Impression = cdr["Impression"].ToString();
                    NewLine.Recommendation = cdr["Recommendation"].ToString();
                    NewLine.IsAbnormalCode = cdr["IsAbnormalCode"].ToString();
                    NewLine.IsAbnormal = cdr["IsAbnormal"].ToString();
                    NewLine.StatusCode = cdr["StatusCode"].ToString();
                    NewLine.Status = cdr["Status"].ToString();
                    NewLine.ReqortDate = ReportDateShow;
                    NewLine.ImageURL = cdr["ImageURL"].ToString();
                    NewLine.DeptCode = cdr["DeptCode"].ToString();
                    NewLine.DeptName = cdr["DeptName"].ToString();
                    NewLine.Creator = cdr["Creator"].ToString();
                    NewLine.ExamDateCom = Convert.ToDateTime(cdr["ExamDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ClinicInfoMethod.GetExaminationList", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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
        /// CSQ 20150714 //20151012 SYF
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<NewExam> GetNewExamForM1(DataConnection pclsCache, string UserId, string Module)
        {
            List<NewExam> list = new List<NewExam>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                if(Module == "M1")
                {
                    cmd = Ps.Examination.GetNewExamForM1(pclsCache.CacheConnectionObject);
                }
                
                cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    NewExam NewLine = new NewExam();
                    NewLine.Name1 = cdr["Name1"].ToString();
                    NewLine.Value1 = cdr["Value1"].ToString();
                    NewLine.Name2 = cdr["Name2"].ToString();
                    NewLine.Value2 = cdr["Value2"].ToString();
                    NewLine.Name3 = cdr["Name3"].ToString();
                    NewLine.Value3 = cdr["Value3"].ToString();
                    NewLine.Date = cdr["Date"].ToString();
                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ClinicInfoMethod.GetNewExam", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        public List<ExamDetails> GetExamDetails(DataConnection pclsCache, string UserId, string VisitId, string SortNo, string ItemCode)
        {
            List<ExamDetails> list = new List<ExamDetails>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.ExamDetails.GetExamDetails(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;
                cmd.Parameters.Add("VisitId", CacheDbType.NVarChar).Value = VisitId;
                cmd.Parameters.Add("SortNo", CacheDbType.Int).Value = SortNo;
                cmd.Parameters.Add("ItemCode", CacheDbType.NVarChar).Value = ItemCode;
                cdr = cmd.ExecuteReader();

                while (cdr.Read())
                {
                    list.Add(new ExamDetails() {
                        Code            = cdr["Code"].ToString(),
                        Name            = cdr["Name"].ToString(),
                        Value           = cdr["Value"].ToString(),
                        IsAbnormalCode  = Convert.ToInt32(cdr["IsAbnormalCode"].ToString()),
                        IsAbnormal      = cdr["IsAbnormal"].ToString(),
                        UnitCode        = cdr["UnitCode"].ToString(),
                        Unit            = cdr["Unit"].ToString(),
                        Creator         = cdr["Creator"].ToString()

                    });
                    //list.Rows.Add(cdr["Code"].ToString(), cdr["Name"].ToString(), cdr["Value"].ToString(),Convert.ToInt32(cdr["IsAbnormalCode"].ToString()), cdr["IsAbnormal"].ToString(),cdr["UnitCode"].ToString(), cdr["Unit"].ToString(), cdr["Creator"].ToString());
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PsExamDetails.GetExamDetails", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        #region Ps.LabTest
        //SYF 2015-10-10 WF 20151027
        /// <summary>
        /// 根据piUserId和piVisitId获取化验结果
        /// </summary>
        /// <param name="pclsCacheList"></param>
        /// <param name="piUserId"></param>
        /// <param name="piVisitId"></param>
        /// <returns></returns>
        public List<LabTestList> GetLabTestList(DataConnection pclsCache, string piUserId, string piVisitId)
        {

            List<LabTestList> list = new List<LabTestList>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.LabTest.GetLabTestList(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("piUserId", CacheDbType.NVarChar).Value = piUserId;
                cmd.Parameters.Add("piVisitId", CacheDbType.NVarChar).Value = piVisitId;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    string ReportDateShow = "";
                    if (cdr["ReportDate"].ToString() == "9999/1/1 0:00:00")
                    {
                        ReportDateShow = "";
                    }
                    else
                    {
                        ReportDateShow = cdr["ReportDate"].ToString();
                    }
                    LabTestList NewLine = new LabTestList();
                    NewLine.VisitId = cdr["VisitId"].ToString();
                    NewLine.SortNo = cdr["SortNo"].ToString();
                    NewLine.LabItemType = cdr["LabItemType"].ToString();
                    NewLine.LabItemTypeName = cdr["LabItemTypeName"].ToString();
                    NewLine.LabItemCode = cdr["LabItemCode"].ToString();
                    NewLine.LabItemName = cdr["LabItemName"].ToString();
                    NewLine.LabTestDate = cdr["LabTestDate"].ToString();
                    NewLine.StatusCode = cdr["StatusCode"].ToString();
                    NewLine.Status = cdr["Status"].ToString();
                    NewLine.ReportDate = ReportDateShow;
                    NewLine.DeptCode = cdr["DeptCode"].ToString();
                    NewLine.DeptName = cdr["DeptName"].ToString();
                    NewLine.Creator = cdr["Creator"].ToString();
                    NewLine.LabTestDateCom = Convert.ToDateTime(cdr["LabTestDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ClinicInfoMethod.GetLabTestList", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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
        /// 得到某些最新的化验结果 LY 2015-10-14
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<NewLabTest> GetNewLabTest(DataConnection pclsCache, string UserId, string Module)
        {
            List<NewLabTest> list = new List<NewLabTest>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                if (Module == "M1")
                {
                    cmd = Ps.LabTestDetails.GetNewLabTestForM1(pclsCache.CacheConnectionObject);
                }
                else if(Module == "M2")
                {
                    cmd = Ps.LabTestDetails.GetNewLabTestForM2(pclsCache.CacheConnectionObject);
                }
                else if (Module == "M3")
                {
                    cmd = Ps.LabTestDetails.GetNewLabTestForM3(pclsCache.CacheConnectionObject);
                }
                
                cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    NewLabTest NewLine = new NewLabTest();
                    NewLine.Code = cdr["Code"].ToString();
                    NewLine.Name = cdr["Name"].ToString();
                    NewLine.Value = cdr["Value"].ToString();
                    NewLine.Date = Convert.ToDateTime(cdr["Date"]);
                    //string ForDate = Convert.ToString(NewLine.Date);
                    //ForDate = ForDate.Replace("T", " ");
                    //NewLine.Date = Convert.ToDateTime(ForDate);
                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PsLabTestDetails.GetNewLabTest", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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
        /// 获取患者某次化验的所有详细信息 LY 2015-10-14
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <param name="VisitId"></param>
        /// <param name="SortNo"></param>
        /// <returns></returns>
        public List<LabTestDetails> GetLabTestDetails(DataConnection pclsCache, string UserId, string VisitId, string SortNo)
        {
            List<LabTestDetails> list = new List<LabTestDetails>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.LabTestDetails.GetLabTestDetails(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;
                cmd.Parameters.Add("VisitId", CacheDbType.NVarChar).Value = VisitId;
                cmd.Parameters.Add("SortNo", CacheDbType.NVarChar).Value = SortNo;
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    LabTestDetails NewLine = new LabTestDetails();
                    NewLine.Code = cdr["Code"].ToString();
                    NewLine.Name = cdr["Name"].ToString();
                    NewLine.Value = cdr["Value"].ToString();
                    NewLine.IsAbnormalCode = Convert.ToInt32(cdr["IsAbnormalCode"].ToString());
                    NewLine.IsAbnormal = cdr["IsAbnormal"].ToString();
                    NewLine.UnitCode = cdr["UnitCode"].ToString();
                    NewLine.Unit = cdr["Unit"].ToString();
                    NewLine.Creator = cdr["Creator"].ToString();
                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PsLabTestDetails.GetLabTestDetails", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        
        #region Ps.DrugRecord
        /// <summary>
        /// SYF 20151012
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="piUserId"></param>
        /// <param name="piVisitId"></param>
        /// <returns></returns>
        public List<DrugRecord> GetDrugRecord(DataConnection pclsCache, string piUserId, string piVisitId)
        {
            List<DrugRecord> list = new List<DrugRecord>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.DrugRecord.GetDrugRecordList(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("piUserId", CacheDbType.NVarChar).Value = piUserId;
                cmd.Parameters.Add("piVisitId", CacheDbType.NVarChar).Value = piVisitId;
                cdr = cmd.ExecuteReader();

                while (cdr.Read())
                {
                    int OrderSubNo = 0;
                    int FreqCounter = 0;
                    int FreqInteval = 0;
                    if (cdr["OrderSubNo"].ToString() != "")
                    {
                        OrderSubNo = Convert.ToInt32(cdr["OrderSubNo"].ToString());
                    }

                    if (OrderSubNo == 1)       //只拿出OrderSubNo为1的记录
                    {
                        if (cdr["FreqCounter"].ToString() != "")
                        {
                            FreqCounter = Convert.ToInt32(cdr["FreqCounter"].ToString());
                        }
                        if (cdr["FreqInteval"].ToString() != "")
                        {
                            FreqInteval = Convert.ToInt32(cdr["FreqInteval"].ToString());
                        }

                        //用药开始时间处理（时间轴标准）
                        string StartDate = DateTime.Parse(cdr["StartDateTime"].ToString()).ToString("yyyyMMdd");
                        //用药结束时间处理
                        string EndDate = cdr["StopDateTime"].ToString();
                        string Content = "";
                        Content += cdr["OrderContent"].ToString();
                        #region
                        DrugRecord NewLine = new DrugRecord();
                        NewLine.VisitId = cdr["VisitId"].ToString();
                        NewLine.OrderNo = cdr["OrderNo"].ToString();
                        NewLine.OrderSubNo = cdr["OrderSubNo"].ToString();
                        NewLine.RepeatIndicatorCode = cdr["RepeatIndicatorCode"].ToString();
                        NewLine.RepeatIndicator = cdr["RepeatIndicator"].ToString();
                        NewLine.OrderClassCode = cdr["OrderClassCode"].ToString();
                        NewLine.OrderClass = cdr["OrderClass"].ToString();
                        NewLine.OrderCode = cdr["OrderCode"].ToString();
                        NewLine.OrderContent = cdr["OrderContent"].ToString();
                        NewLine.Dosage = cdr["Dosage"].ToString();
                        NewLine.DosageUnitsCode = cdr["DosageUnitsCode"].ToString();
                        NewLine.DosageUnits = cdr["DosageUnits"].ToString();
                        NewLine.AdministrationCode = cdr["AdministrationCode"].ToString();
                        NewLine.Administration = cdr["Administration"].ToString();
                        NewLine.StartDateTime = cdr["StartDateTime"].ToString();
                        NewLine.StopDateTime = cdr["StopDateTime"].ToString();
                        NewLine.Frequency = cdr["Frequency"].ToString();
                        NewLine.FreqCounter = cdr["FreqCounter"].ToString();
                        NewLine.FreqInteval = cdr["FreqInteval"].ToString();
                        NewLine.FreqIntevalUnitCode = cdr["FreqIntevalUnitCode"].ToString();
                        NewLine.FreqIntevalUnit = cdr["FreqIntevalUnit"].ToString();
                        NewLine.HistoryContent = Content;
                        NewLine.StartDate = StartDate;
                        NewLine.StopDate = EndDate;

                        list.Add(NewLine);
                        #endregion
                    }                   
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ClinicInfoMethod.GetDrugRecordList", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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
        /// syf 2015-10-12 获取某病人所有药嘱
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="piUserId"></param>
        /// <param name="Module"></param>
        /// <returns></returns>
        public List<PsDrugRecord> GetPsDrugRecord(DataConnection pclsCache, string piUserId, string Module)
        {
            List<PsDrugRecord> list = new List<PsDrugRecord>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.DrugRecord.GetPsDrugRecord(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("piUserId", CacheDbType.NVarChar).Value = piUserId;
                cmd.Parameters.Add("Module", CacheDbType.NVarChar).Value = Module;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    PsDrugRecord NewLine = new PsDrugRecord();
                    NewLine.VisitId = cdr["VisitId"].ToString();
                    NewLine.OrderNo = cdr["OrderNo"].ToString();
                    NewLine.OrderSubNo = cdr["OrderSubNo"].ToString();
                    NewLine.RepeatIndicator = cdr["RepeatIndicator"].ToString();
                    NewLine.OrderClass = cdr["OrderClass"].ToString();
                    NewLine.OrderCode = cdr["OrderCode"].ToString();
                    NewLine.DrugName = cdr["DrugName"].ToString();
                    NewLine.CurativeEffect = cdr["CurativeEffect"].ToString();
                    NewLine.SideEffect = cdr["SideEffect"].ToString();
                    NewLine.Instruction = cdr["Instruction"].ToString();
                    NewLine.HealthEffect = cdr["HealthEffect"].ToString();
                    NewLine.Unit = cdr["Unit"].ToString();
                    NewLine.OrderContent = cdr["OrderContent"].ToString();
                    NewLine.Dosage = cdr["Dosage"].ToString();
                    NewLine.DosageUnits = cdr["DosageUnits"].ToString();
                    NewLine.Administration = cdr["Administration"].ToString();
                    NewLine.StartDateTime = cdr["StartDateTime"].ToString();
                    NewLine.StopDateTime = cdr["StopDateTime"].ToString();
                    NewLine.Frequency = cdr["Frequency"].ToString();
                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ClinicInfoMethod.GetPsDrugRecord", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        //syf 2015-10-10  wf20151027
        /// <summary>
        /// 根据piUserId和piVisitId获取用药结果
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="piUserId"></param>
        /// <param name="piVisitId"></param>
        /// <returns></returns>
        public List<DrugRecordList> GetDrugRecordList(DataConnection pclsCache, string piUserId, string piVisitId)
        {
            List<DrugRecordList> list = new List<DrugRecordList>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.DrugRecord.GetDrugRecordList(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("piUserId", CacheDbType.NVarChar).Value = piUserId;
                cmd.Parameters.Add("piVisitId", CacheDbType.NVarChar).Value = piVisitId;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    string startTime = "";
                    if (cdr["StartDateTime"].ToString() != null && cdr["StartDateTime"].ToString() != "")
                    {
                        startTime = Convert.ToDateTime(cdr["StartDateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    string endTime = "";
                    if (cdr["StopDateTime"].ToString() != null && cdr["StopDateTime"].ToString() != "")
                    {
                        endTime = Convert.ToDateTime(cdr["StopDateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    DrugRecordList NewLine = new DrugRecordList();
                    NewLine.VisitId = cdr["VisitId"].ToString();
                    NewLine.OrderNo = cdr["OrderNo"].ToString();
                    NewLine.OrderSubNo = cdr["OrderSubNo"].ToString();
                    NewLine.RepeatIndicatorCode = cdr["RepeatIndicatorCode"].ToString();
                    NewLine.RepeatIndicator = cdr["RepeatIndicator"].ToString();
                    NewLine.OrderClassCode = cdr["OrderClassCode"].ToString();
                    NewLine.OrderClass = cdr["OrderClass"].ToString();
                    NewLine.OrderCode = cdr["OrderCode"].ToString();
                    NewLine.OrderContent = cdr["OrderContent"].ToString();
                    NewLine.Dosage = cdr["Dosage"].ToString();
                    NewLine.DosageUnitsCode = cdr["DosageUnitsCode"].ToString();
                    NewLine.DosageUnits = cdr["DosageUnits"].ToString();
                    NewLine.AdministrationCode = cdr["AdministrationCode"].ToString();
                    NewLine.Administration = cdr["Administration"].ToString();
                    NewLine.StartDateTime = startTime;
                    NewLine.StopDateTime = endTime;
                    NewLine.Frequency = cdr["Frequency"].ToString();
                    NewLine.FreqCounter = cdr["FreqCounter"].ToString();
                    NewLine.FreqInteval = cdr["FreqInteval"].ToString();
                    NewLine.FreqIntevalUnitCode = cdr["FreqIntevalUnitCode"].ToString();
                    NewLine.FreqIntevalUnit = cdr["FreqIntevalUnit"].ToString();
                    NewLine.DeptCode = cdr["DeptCode"].ToString();
                    NewLine.DeptName = cdr["DeptName"].ToString();
                    NewLine.Creator = cdr["Creator"].ToString();
                    NewLine.StartDateTimeCom = Convert.ToDateTime(cdr["StartDateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ClinicInfoMethod.GetDrugRecordList", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        /// <summary>
        /// 住院（入院、出院）、门诊、急诊——住院拆分成入院、出院，再排序，共取出Num条 LY 2015-10-12
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <param name="AdmissionDate"></param>
        /// <param name="ClinicDate"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public List<ClinicalTrans> PsClinicalInfoGetClinicalInfoNum(DataConnection pclsCache, string UserId, DateTime AdmissionDate, DateTime ClinicDate, int Num)
        {
            //实际最终输出
            List<ClinicalTrans> NewTable = new List<ClinicalTrans>(); //最终输出（表格形式）【总不为空：会输出标记AdmissionDateMark和ClinicDateMark】
            DateTime AdmissionDateMark = new DateTime();  //指针标记 放在NewTable最后     住院
            DateTime ClinicDateMark = new DateTime();   //门诊
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                List<ClinicalTrans> list = new List<ClinicalTrans>();
                #region 住院、门诊 取数据，放在list中
                //Ps.InPatientInfo  取入院时间与出院时间  
                cmd = new CacheCommand();
                cmd = Ps.InPatientInfo.GetInfobyDate(pclsCache.CacheConnectionObject);  //只取了Num数量的VId,还需要取完整
                cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;
                cmd.Parameters.Add("AdmissionDate", CacheDbType.NVarChar).Value = AdmissionDate;
                cmd.Parameters.Add("Num", CacheDbType.NVarChar).Value = Num;
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    //住院-入院（AdmissionDate肯定不为空）
                    ClinicalTrans NewLine = new ClinicalTrans();
                    NewLine.精确时间 = Convert.ToDateTime(cdr["AdmissionDate"]);
                    NewLine.类型 = "入院";
                    NewLine.VisitId = cdr["VisitId"].ToString();
                    NewLine.事件 = cdr["HospitalName"].ToString() + "：" + cdr["DepartmentName"].ToString() + "（入院）";
                    list.Add(NewLine);
                    //住院-出院（DischargeDate为1900/1/1 0:00:00，表示目前正在住院）  【注意：应该取该Vid下最大sortNo】   出院时间必须提前取出，考虑门诊在住院期间的情况
                    DateTime DischargeDate;
                    DischargeDate = (DateTime)Ps.InPatientInfo.GetDischargeDate(pclsCache.CacheConnectionObject, UserId, cdr["VisitId"].ToString());
                    if (DischargeDate != null)
                    {
                        if (DischargeDate.ToString() == "9999/1/1 0:00:00")
                        {
                            ClinicalTrans NewLine_1 = new ClinicalTrans();
                            DischargeDate = DateTime.Now; //取当前时间
                            NewLine_1.精确时间 = DischargeDate;
                            NewLine_1.类型 = "当前住院中";
                            NewLine_1.VisitId = cdr["VisitId"].ToString();
                            NewLine_1.事件 = cdr["HospitalName"].ToString() + "：" + cdr["DepartmentName"].ToString() + "（当前住院中）";
                            list.Add(NewLine_1);
                        }
                        else
                        {
                            //已经出院
                            ClinicalTrans NewLine_1 = new ClinicalTrans();
                            NewLine_1.精确时间 = DischargeDate;
                            NewLine_1.类型 = "出院";
                            NewLine_1.VisitId = cdr["VisitId"].ToString();
                            NewLine_1.事件 = cdr["HospitalName"].ToString() + "：" + cdr["DepartmentName"].ToString() + "（出院）";
                            list.Add(NewLine_1);
                        }
                    }
                }
                //Ps.OutPatientInfo 包括门诊和急诊
                cmd = new CacheCommand();
                cmd = Ps.OutPatientInfo.GetInfobyDate(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;
                cmd.Parameters.Add("ClinicDate", CacheDbType.NVarChar).Value = ClinicDate;
                cmd.Parameters.Add("Num", CacheDbType.NVarChar).Value = Num;
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    //就诊时间（ClinicDate肯定不为空）
                    //门诊
                    if (cdr["VisitId"].ToString().Substring(0, 1) == "O")
                    {
                        ClinicalTrans NewLine = new ClinicalTrans();
                        NewLine.精确时间 = Convert.ToDateTime(cdr["ClinicDate"]);
                        NewLine.类型 = "门诊";
                        NewLine.VisitId = cdr["VisitId"].ToString();
                        NewLine.事件 = cdr["HospitalName"].ToString() + "：" + cdr["DepartmentName"].ToString() + "（门诊）";
                        list.Add(NewLine);
                    }
                    //急诊
                    else
                    {
                        ClinicalTrans NewLine = new ClinicalTrans();
                        NewLine.精确时间 = Convert.ToDateTime(cdr["ClinicDate"]);
                        NewLine.类型 = "急诊";
                        NewLine.VisitId = cdr["VisitId"].ToString();
                        NewLine.事件 = cdr["HospitalName"].ToString() + "：" + cdr["DepartmentName"].ToString() + "（急诊）";
                        list.Add(NewLine);
                    }
                }
                #endregion
                #region list有数据时→排序,取Num条，并确定标记 （注意：极端情况-住院期间嵌套很多门诊  解决方案，后期判断取够或到底为止，满足 门诊日期<入院日期）
                //就诊有数据时，排序→确定标记（取出<=Num条数据）
                if(list != null)
                {
                    #region
                    if (list.Count > 0)   //保证有数据
                    {
                        //住院、门诊按时间排序
                        list.Sort((x, y) => -x.精确时间.CompareTo(y.精确时间));
                        int NumMark = 0;
                        int TypeMark = 0;     //TypeMark：遇到入院，变为1；出院变为0 （确定是否在住院期间）
                        int j = 0;
                        int CoNum = 0;  //从dtNew中拿出的实际条数
                        for (j = 0; j < list.Count; j++)  //for循环是在，内容执行完成后，j才加1；若中途break
                        {
                            if ((list[j].类型 == "出院") || (list[j].类型 == "当前住院中"))
                            {
                                TypeMark = 1; //表明在住院
                            }
                            if (list[j].类型 == "入院")
                            {
                                TypeMark = 0; //表明某一住院阶段结束
                            }
                            if (((list[j].类型 == "门诊") && (TypeMark == 0)) || ((list[j].类型 == "急诊") && (TypeMark == 0))) { NumMark++; } //在住院期间的门诊不作为NumMark的计数
                            if (list[j].类型 == "入院") { NumMark++; }
                            if (NumMark == Num)
                            {
                                CoNum = j + 1; //break的时候 j不会继续自加直接跳出for循环
                                break;
                            }
                        }
                        if (CoNum == 0)   //不是所需条数达到Num、break的情况
                        {
                            CoNum = j;
                        }
                        //分析：最后一条 要么是门诊（不在住院阶段），要么入院
                        //表格输出（注意入院和出院拆开了，一条变两条影响计数，所以用j,而不是Num)
                        for (int i = 0; i < CoNum; i++)     //j+1的情况：是有剩余   【有问题：只有门诊的话 ，只取到j】
                        {
                            //NewTable.ImportRow((DataRow)rows[i]);
                            ClinicalTrans NewLine = list[i];
                            NewTable.Add(NewLine);
                        }
                        //获取两张表的当前标记  由于表默认取出（是按时间倒序，所以应该去最后一条，才是标记
                        //住院
                        List<ClinicalTrans> InRows = NewTable.FindAll(delegate(ClinicalTrans x)
                        {
                            return x.类型.Contains("入院");
                        });
                        InRows.Sort((x, y) => -x.精确时间.CompareTo(y.精确时间));
                        if (InRows == null || InRows.Count == 0)
                        {
                            AdmissionDateMark = AdmissionDate;
                        }
                        else
                        {
                            AdmissionDateMark = DateTime.Parse(InRows[InRows.Count - 1].精确时间.ToString());
                        }
                        //门诊
                        List<ClinicalTrans> OutRows = NewTable.FindAll(delegate(ClinicalTrans x)
                        {
                            return x.类型.Contains("诊");
                        });
                        OutRows.Sort((x, y) => -x.精确时间.CompareTo(y.精确时间));
                        if (OutRows == null || OutRows.Count == 0)
                        {
                            ClinicDateMark = ClinicDate;
                        }
                        else
                        {
                            ClinicDateMark = DateTime.Parse(OutRows[OutRows.Count - 1].精确时间.ToString());
                        }
                        //将两张表当前指针标记，放在表的最后，用VisitTypeName="标志"，做区分
                        ClinicalTrans Pointer = new ClinicalTrans();
                        Pointer.精确时间 = AdmissionDateMark;
                        Pointer.类型 = ClinicDateMark.ToString();
                        Pointer.VisitId = "指针";
                        Pointer.事件 = "";
                        NewTable.Add(Pointer);
                    }
                    else
                    {
                        ClinicalTrans Pointer = new ClinicalTrans();
                        Pointer.精确时间 = AdmissionDate;
                        Pointer.类型 = ClinicDate.ToString();
                        Pointer.VisitId = "指针";
                        Pointer.事件 = "";
                        NewTable.Add(Pointer);
                    }
                    #endregion
                }
                
                #endregion
                //DateSort、LeftShow、VisitTypeName、VisitId、Location
                return NewTable;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PsClinicalInfo.GetClinicalInfo", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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
        /// 诊断、检查、化验、用药信息查看/用于时间轴
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <param name="Type"></param>
        /// <param name="VisitId"></param>
        /// <param name="Date"></param>
        /// <returns></returns>
        public ClinicInfoViewModel GetClinicInfoDetail(DataConnection pclsCache, string UserId, string Type, string VisitId, string Date)
        {
            ClinicInfoViewModel list = new ClinicInfoViewModel();
            List<DiagnosisInfo> DiagnosisInfo_DataViewModel = null;
            List<ExamInfo> ExamInfo_DataViewModel = null;
            List<LabTestList> LabTestList_DataViewModel = null;
            List<DrugRecordList> DrugRecordList_DataViewModel = null;
            string condition = "";
            try
            {
                switch (Type)
                {
                    case "DiagnosisInfo": DiagnosisInfo_DataViewModel = new ClinicInfoMethod().PsDiagnosisGetDiagnosisInfo(pclsCache, UserId, VisitId);//诊断表
                        List<DiagnosisInfo> DiagnosisInfo_DataViewModel_copy = new List<DiagnosisInfo>();
                        foreach (DiagnosisInfo item in DiagnosisInfo_DataViewModel)
                        {
                            if (item.RecordDateCom == Date)
                            {
                                DiagnosisInfo_DataViewModel_copy.Add(item);
                            }
                        }
                        list.DiagnosisInfo_DataViewModel = DiagnosisInfo_DataViewModel_copy;
                        break;

                    case "ExaminationInfo": ExamInfo_DataViewModel = new ClinicInfoMethod().PsExaminationGetExaminationList(pclsCache, UserId, VisitId); //检查表（有子表）
                        List<ExamInfo> ExamInfo_DataViewModel_copy = new List<ExamInfo>();
                        foreach (ExamInfo item in ExamInfo_DataViewModel)
                        {
                            if (item.ExamDateCom == Date)
                            {
                                ExamInfo_DataViewModel_copy.Add(item);
                            }
                        }
                        list.ExamInfo_DataViewModel = ExamInfo_DataViewModel_copy;
                        break;

                    case "LabTestInfo": LabTestList_DataViewModel = new ClinicInfoMethod().GetLabTestList(pclsCache, UserId, VisitId); //化验表（有子表）
                        List<LabTestList> LabTestList_DataViewModel_copy = new List<LabTestList>();
                        foreach (LabTestList item in LabTestList_DataViewModel)
                        {
                            if (item.LabTestDateCom == Date)
                            {
                                LabTestList_DataViewModel_copy.Add(item);
                            }
                        }
                        list.LabTestList_DataViewModel = LabTestList_DataViewModel_copy;
                        break;
                    case "DrugRecord": DrugRecordList_DataViewModel = new ClinicInfoMethod().GetDrugRecordList(pclsCache, UserId, VisitId); //用药
                        condition = "StartDateTimeCom = '" + Date + "'";
                        list.DrugRecordList_DataViewModel = DrugRecordList_DataViewModel;
                        break;
                    default: break;
                }

                //list肯定不为空
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ClinicInfoMethod.GetClinicInfoDetail", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }

            finally
            {
                pclsCache.DisConnect();
            }
        }
        public List<PsInPatInfo> PsInPatientInfoGetInfobyId(DataConnection pclsCache, string UserId)
        {
            List<PsInPatInfo> list = new List<PsInPatInfo>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }

                cmd = new CacheCommand();
                cmd = Ps.InPatientInfo.GetInfobyId(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;
                //cmd.Parameters.Add("InvalidFlag", CacheDbType.Int).Value = InvalidFlag;
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Add(new PsInPatInfo() {
                        VisitId         = cdr["VisitId"].ToString(),
                        SortNo          = cdr["SortNo"].ToString(),
                        AdmissionDate   = cdr["AdmissionDate"].ToString(),
                        DischargeDate   = cdr["DischargeDate"].ToString(),
                        HospitalCode    = cdr["HospitalCode"].ToString(),
                        HospitalName    = cdr["HospitalName"].ToString(),
                        Department      = cdr["Department"].ToString(),
                        DepartmentName  = cdr["DepartmentName"].ToString(),
                        Doctor          = cdr["Doctor"].ToString(),
                        Creator         = cdr["Creator"].ToString()

                    });
                    //list.Rows.Add(cdr["VisitId"].ToString(), cdr["SortNo"].ToString(), cdr["AdmissionDate"].ToString(), cdr["DischargeDate"].ToString(), cdr["HospitalCode"].ToString(), cdr["HospitalName"].ToString(), cdr["Department"].ToString(), cdr["DepartmentName"].ToString(), cdr["Doctor"].ToString(), cdr["Creator"].ToString());
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PsInPatientInfo.GetInfobyId", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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
        public List<PsOutPatInfo> PsOutPatientInfoGetInfobyId(DataConnection pclsCache, string UserId)
        {
            List<PsOutPatInfo> list = new List<PsOutPatInfo>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.OutPatientInfo.GetInfobyId(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Add(new PsOutPatInfo() {
                        VisitId         = cdr["VisitId"].ToString(),
                        ClinicDate      = cdr["ClinicDate"].ToString(),
                        HospitalCode    = cdr["HospitalCode"].ToString(),
                        HospitalName    = cdr["HospitalName"].ToString(),
                        Department      = cdr["Department"].ToString(),
                        DepartmentName  = cdr["DepartmentName"].ToString(),
                        Doctor          = cdr["Doctor"].ToString(),
                        Creator         = cdr["Creator"].ToString()
                     
                    });
                    //list.Rows.Add(cdr["VisitId"].ToString(), cdr["ClinicDate"].ToString(), cdr["HospitalCode"].ToString(), cdr["HospitalName"].ToString(), cdr["Department"].ToString(), cdr["DepartmentName"].ToString(), cdr["Doctor"].ToString(), cdr["Creator"].ToString());
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.OutPatientInfo.GetInfobyId", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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
        public ClinicalInfoListViewModel GetClinicalInfoList(DataConnection pclsCache, string UserId)
        {
            try
            {
                ClinicalInfoListViewModel DS_ClinicalInfo = new ClinicalInfoListViewModel();
                List<PsInPatInfo> DT_InPatientInfo = new List<PsInPatInfo>();
                List<PsOutPatInfo> DT_OutPatientInfo = new List<PsOutPatInfo>();

                DT_InPatientInfo = PsInPatientInfoGetInfobyId(pclsCache, UserId);
                DT_OutPatientInfo = PsOutPatientInfoGetInfobyId(pclsCache, UserId);
                if (DT_InPatientInfo != null)
                {
                    DS_ClinicalInfo.DT_InPatientInfo = DT_InPatientInfo;
                }
                if (DT_OutPatientInfo != null)
                {
                    DS_ClinicalInfo.DT_OutPatientInfo = DT_OutPatientInfo;
                }
                return DS_ClinicalInfo;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetClinicalInfoList", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
                throw (ex);
            }
        }
        public string getLatestHUserIdByHCode(DataConnection pclsCache, string UserId, string HospitalCode)
        {
            string HUserId = "";
            try
            {
                if (!pclsCache.Connect())
                {
                    return "";
                }
                HUserId = Ps.UserIdMatch.GetLatestHUserIdByHospitalCode(pclsCache.CacheConnectionObject, UserId, HospitalCode);

                return HUserId;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PsUserIdMatch.getLatestHUserIdByHCode", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return "";
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }
        public List<SymptomsList> GetSymptomsList(DataConnection pclsCache, string UserId, string VisitId)
        {
            List<SymptomsList> list = new List<SymptomsList>();


            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Symptoms.GetSymptomsList(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;
                cmd.Parameters.Add("VisitId", CacheDbType.NVarChar).Value = VisitId;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Add(new SymptomsList() {
                        VisitId             = cdr["VisitId"].ToString(),
                        SynptomsNo          = cdr["SynptomsNo"].ToString(),
                        SymptomsType        = cdr["SymptomsType"].ToString(),
                        SymptomsTypeName    = cdr["SymptomsTypeName"].ToString(),
                        SymptomsCode        = cdr["SymptomsCode"].ToString(),
                        SymptomsName        = cdr["SymptomsName"].ToString(),
                        Description         = cdr["Description"].ToString(),
                        RecordDate          = cdr["RecordDate"].ToString(),
                        RecordTime          = cdr["RecordTime"].ToString(),
                        Creator             = cdr["Creator"].ToString()
                    }); 
                    //list.Rows.Add(cdr["VisitId"].ToString(), cdr["SynptomsNo"].ToString(), cdr["SymptomsType"].ToString(), cdr["SymptomsTypeName"].ToString(), cdr["SymptomsCode"].ToString(), cdr["SymptomsName"].ToString(), cdr["Description"].ToString(), cdr["RecordDate"].ToString(), cdr["RecordTime"].ToString(), cdr["Creator"].ToString());
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "Ps.Symptoms.GetSymptomsList", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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
     
    }
}