using CDMISrestful.CommonLibrary;
using CDMISrestful.DataMethod;
using CDMISrestful.DataModels;
using CDMISrestful.DataViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDMISrestful.Models
{
    public class ClinicInfoRepository : IClinicInfoRepository
    {
       
        ClinicInfoMethod clinicInfoMethod = new ClinicInfoMethod();
        public Clinic GetClinicalNewMobile(DataConnection pclsCache, string UserId, DateTime AdmissionDate, DateTime ClinicDate, int Num)
        {
            //最终输出
            Clinic Clinic = new Clinic();
            Clinic.UserId = UserId;
            Clinic.History = new List<ClinicBasicInfoandTime>();
            try
            {

                List<DT_Clinical_All> DT_Clinical_All = new List<DT_Clinical_All>();   //住院,门诊、检查化验。。。混合表
                //DT_Clinical_All.Columns.Add(new DataColumn("精确时间", typeof(DateTime)));
                //DT_Clinical_All.Columns.Add(new DataColumn("时间", typeof(string)));
                //DT_Clinical_All.Columns.Add(new DataColumn("类型", typeof(string)));
                //DT_Clinical_All.Columns.Add(new DataColumn("VisitId", typeof(string)));
                //DT_Clinical_All.Columns.Add(new DataColumn("事件", typeof(string)));
                //DT_Clinical_All.Columns.Add(new DataColumn("关键属性", typeof(string)));  //检查化验的详细查看  例：examnation|

                //颜色由文字决定


                #region 读取两张就诊表，并通过VisitId取出其他数据（检查、化验等）  全部拿出 按时间  VID 类型  排序后，合并

                List<ClinicalTrans> DT_Clinical_ClinicInfo = new List<ClinicalTrans>();
                DT_Clinical_ClinicInfo = clinicInfoMethod.PsClinicalInfoGetClinicalInfoNum(pclsCache, UserId, AdmissionDate, ClinicDate, Num);
                if (DT_Clinical_ClinicInfo != null)
                {
                    #region
                    if (DT_Clinical_ClinicInfo.Count > 1)    //肯定大于0，最后一条是标记，必须传回；大于1表明时间轴还可以加载
                    {

                        #region  拿出临床信息

                        for (int i = 0; i < DT_Clinical_ClinicInfo.Count - 1; i++)  //最后一条是标记，需要单独拿出
                        {
                            string DateShow = Convert.ToDateTime(DT_Clinical_ClinicInfo[i].精确时间).ToString("yyyy年MM月dd日");  //取日期
                            DT_Clinical_All item = new DT_Clinical_All();
                            item.精确时间 = DT_Clinical_ClinicInfo[i].精确时间;
                            item.时间 = DateShow;
                            item.类型 = DT_Clinical_ClinicInfo[i].类型;
                            item.VisitId = DT_Clinical_ClinicInfo[i].VisitId;
                            item.事件 = DT_Clinical_ClinicInfo[i].事件;
                            item.关键属性 = "";
                            DT_Clinical_All.Add(item);
                            if (DT_Clinical_ClinicInfo[i].类型.ToString() == "入院")
                            {
                                //转科处理  转科内容：什么时候从哪里转出，什么时候转到哪里
                                List<ClinicalTrans> DT_Clinical_Trans = new List<ClinicalTrans>();
                                DT_Clinical_Trans = clinicInfoMethod.PsClinicalInfoGetTransClinicalInfo(pclsCache, UserId, DT_Clinical_ClinicInfo[i].VisitId.ToString());
                                if (DT_Clinical_Trans.Count > 0)  //有转科
                                {
                                    for (int n = 0; n < DT_Clinical_Trans.Count; n++)
                                    {
                                        string DateShow1 = Convert.ToDateTime(DT_Clinical_Trans[n].精确时间).ToString("yyyy年MM月dd日");  //取日期
                                        item.精确时间 = DT_Clinical_Trans[n].精确时间;
                                        item.时间 = DateShow1;
                                        item.类型 = "转科";
                                        item.VisitId = DT_Clinical_Trans[n].VisitId;
                                        item.事件 = DT_Clinical_Trans[n].事件;
                                        item.关键属性 = "";
                                        DT_Clinical_All.Add(item);
                                    
                                    }
                                }
                            }


                            if ((DT_Clinical_ClinicInfo[i].类型.ToString() == "入院") || (DT_Clinical_ClinicInfo[i].类型.ToString() == "门诊") || (DT_Clinical_ClinicInfo[i].类型.ToString() == "急诊"))
                            {
                                //诊断检查等
                                List<ClinicalTrans> DT_Clinical_Others = new List<ClinicalTrans>();
                                DT_Clinical_Others = clinicInfoMethod.PsClinicalInfoGetOtherTable(pclsCache, UserId, DT_Clinical_ClinicInfo[i].VisitId.ToString());


                                if (DT_Clinical_Others.Count > 0)
                                {
                                    for (int n = 0; n < DT_Clinical_Others.Count; n++)
                                    {
                                        string DateShow2 = Convert.ToDateTime(DT_Clinical_Others[n].精确时间).ToString("yyyy年MM月dd日");  //取日期
                                        item.精确时间 = DT_Clinical_Others[n].精确时间;
                                        item.时间 = DateShow2;
                                        item.类型 = DT_Clinical_Others[n].类型;
                                        item.VisitId = DT_Clinical_Others[n].VisitId;
                                        item.事件 = DT_Clinical_Others[n].事件;
                                        item.关键属性 = DT_Clinical_Others[n].关键属性;
                                        DT_Clinical_All.Add(item);
                                    }

                                    //DataRow[] rows = DT_Clinical_Others.Select();
                                    //foreach (DataRow row in rows)  // 将查询的结果添加到dt中； 
                                    //{
                                    //    DT_Clinical_All.Rows.Add(row.ItemArray);
                                    //}
                                    //for(int j=0; j<DT_Clinical_Others.Rows.Count;j++)
                                    //{
                                    //     DT_Clinical_All.Rows.Add(DT_Clinical_Others.Rows[j]);
                                    //}
                                }
                            }

                        } //for循环的结尾
                        #endregion


                        //排序   按“精准时间”, “VID”    排序后, “时间”、“VID”相同的合并  【精准时间到s,时间到天】
                        DT_Clinical_All.Sort((x, y) => -(x.时间.CompareTo(y.时间) * 3 + x.VisitId.CompareTo(y.VisitId) * 2 - x.精确时间.CompareTo(y.精确时间)));
                        //dv.Sort = "时间 desc,  VisitId desc, 精确时间 Asc"; //目前采用方案二，
                        //时间轴需要倒序，升序Asc    时间轴最外层 日期倒序 某一天内按照时分升序  注意：遇到同一天 又住院又门诊的，即不同VID  方案：一、不拆开，按时间排即可，问题是会混乱； 二，拆开，时间、VID、精确时间   这样的话，按照目前是在一个方框里 颜色字体大小区分开
                        List<DT_Clinical_All> dtNew = DT_Clinical_All;

                        #region 如果两者“时间”、“VID”相同则合并   时间轴方框标签-完成后遍历每一个方框内的事件确定标签

                        List<ClinicBasicInfoandTime> history = new List<ClinicBasicInfoandTime>();  //总  时间、事件的集合
                        ClinicBasicInfoandTime temphistory = new ClinicBasicInfoandTime();
                        if (dtNew != null)
                        {
                            #region
                            if (dtNew.Count > 0)
                            {
                                string TimeMark = dtNew[0].时间.ToString();
                                string VisitIdMark = dtNew[0].VisitId.ToString();
                                temphistory.Time = TimeMark;
                                temphistory.VisitId = VisitIdMark;

                                List<SomeDayEvent> ItemGroup = new List<SomeDayEvent>();
                                SomeDayEvent SomeDayEvent = new SomeDayEvent();
                                SomeDayEvent.Type = dtNew[0].类型.ToString();    //已有类型集合：入院、出院、转科、门诊、急诊、当前住院中；诊断、检查、化验、用药   【住院中未写入】
                                SomeDayEvent.Time = Convert.ToDateTime(dtNew[0].精确时间).ToString("HH:mm");  //取时分 HH:mm(24) hh:mm(12)
                                SomeDayEvent.Event = dtNew[0].事件.ToString();
                                SomeDayEvent.KeyCode = dtNew[0].关键属性.ToString();
                                ItemGroup.Add(SomeDayEvent);

                                if (dtNew.Count > 1)
                                {
                                    for (int i = 1; i < dtNew.Count; i++)
                                    {
                                        string TimeMark1 = dtNew[i].时间.ToString();
                                        string VisitIdMark1 = dtNew[i].VisitId.ToString();

                                        if (i == dtNew.Count - 1)
                                        {
                                            if ((TimeMark1 == TimeMark) && (VisitIdMark1 == VisitIdMark))
                                            {

                                                SomeDayEvent = new SomeDayEvent();
                                                SomeDayEvent.Type = dtNew[i].类型.ToString();
                                                SomeDayEvent.Time = Convert.ToDateTime(dtNew[i].精确时间).ToString("HH:mm");
                                                SomeDayEvent.Event = dtNew[i].事件.ToString();
                                                SomeDayEvent.KeyCode = dtNew[i].关键属性.ToString();
                                                ItemGroup.Add(SomeDayEvent);

                                                temphistory.ItemGroup = ItemGroup;
                                                history.Add(temphistory);
                                            }
                                            else
                                            {

                                                temphistory.ItemGroup = ItemGroup;
                                                history.Add(temphistory);

                                                temphistory = new ClinicBasicInfoandTime();
                                                temphistory.Time = TimeMark1;
                                                temphistory.VisitId = VisitIdMark1;

                                                ItemGroup = new List<SomeDayEvent>();
                                                SomeDayEvent = new SomeDayEvent();
                                                SomeDayEvent.Type = dtNew[i].类型.ToString();
                                                SomeDayEvent.Time = Convert.ToDateTime(dtNew[i].精确时间).ToString("HH:mm");
                                                SomeDayEvent.Event = dtNew[i].事件.ToString();
                                                SomeDayEvent.KeyCode = dtNew[i].关键属性.ToString();
                                                ItemGroup.Add(SomeDayEvent);

                                                temphistory.ItemGroup = ItemGroup;
                                                history.Add(temphistory);


                                            }
                                        }
                                        else
                                        {

                                            if ((TimeMark1 == TimeMark) && (VisitIdMark1 == VisitIdMark))
                                            {

                                                SomeDayEvent = new SomeDayEvent();
                                                SomeDayEvent.Type = dtNew[i].类型.ToString();
                                                SomeDayEvent.Time = Convert.ToDateTime(dtNew[i].精确时间).ToString("HH:mm");
                                                SomeDayEvent.Event = dtNew[i].事件.ToString();
                                                SomeDayEvent.KeyCode = dtNew[i].关键属性.ToString();
                                                ItemGroup.Add(SomeDayEvent);
                                            }
                                            else
                                            {

                                                temphistory.ItemGroup = ItemGroup;
                                                history.Add(temphistory);

                                                temphistory = new ClinicBasicInfoandTime();
                                                temphistory.Time = TimeMark1;
                                                temphistory.VisitId = VisitIdMark1;

                                                ItemGroup = new List<SomeDayEvent>();
                                                SomeDayEvent = new SomeDayEvent();
                                                SomeDayEvent.Type = dtNew[i].类型.ToString();
                                                SomeDayEvent.Time = Convert.ToDateTime(dtNew[i].精确时间).ToString("HH:mm");
                                                SomeDayEvent.Event = dtNew[i].事件.ToString();
                                                SomeDayEvent.KeyCode = dtNew[i].关键属性.ToString();
                                                ItemGroup.Add(SomeDayEvent);

                                                TimeMark = TimeMark1;
                                                VisitIdMark = VisitIdMark1;
                                            }

                                        }

                                    }
                                }
                                else
                                {
                                    temphistory.ItemGroup = ItemGroup;
                                    history.Add(temphistory);
                                }
                            }
                            #endregion
                        }
                        
                        #endregion


                        #region 时间轴块标签、颜色
                        //类型 入院、出院、转科、门诊、急诊、当前住院中；诊断、检查、化验、用药   【住院中未写入】
                        //标签 入院、出院、转科、门诊、住院中、急诊
                        if(history != null)
                        {
                            #region
                            for (int n = 0; n < history.Count; n++)
                            {
                                for (int m = 0; m < history[n].ItemGroup.Count; m++)
                                {
                                    if ((history[n].ItemGroup[m].Type == "入院") || (history[n].ItemGroup[m].Type == "出院") || (history[n].ItemGroup[m].Type == "转科") || (history[n].ItemGroup[m].Type == "门诊") || (history[n].ItemGroup[m].Type == "急诊") || (history[n].ItemGroup[m].Type == "当前住院中"))
                                    {
                                        history[n].Tag += history[n].ItemGroup[m].Type + "、";
                                    }

                                }

                                if ((history[n].Tag == "") || (history[n].Tag == null))
                                {
                                    //防止门诊、急诊逸出
                                    if (history[n].VisitId.Substring(0, 1) == "I")  //住院
                                    {
                                        history[n].Tag = "住院中";
                                        history[n].Color = clinicInfoMethod.PsClinicalInfoGetColor("住院中");
                                    }
                                    else if (history[n].VisitId.Substring(0, 1) == "O") //门诊
                                    {
                                        history[n].Tag = "";//门诊
                                        history[n].Color = clinicInfoMethod.PsClinicalInfoGetColor("门诊");
                                    }
                                    else if (history[n].VisitId.Substring(0, 1) == "E") //急诊
                                    {
                                        history[n].Tag = "";//急诊
                                        history[n].Color = clinicInfoMethod.PsClinicalInfoGetColor("急诊");
                                    }
                                    //history[n].Tag = "住院中";
                                    //history[n].Color = PsClinicalInfo.GetColor("住院中");
                                }
                                else
                                {
                                    int z = history[n].Tag.IndexOf("、");
                                    //history[n].Color = PsClinicalInfo.GetColor(history[n].Tag.Substring(0, z));   //若有多个标签，颜色取第一个
                                    history[n].Tag = history[n].Tag.Substring(0, history[n].Tag.Length - 1);  //去掉最后的、
                                    //int end= history[n].Tag.LastIndexOf("、")+1;
                                    history[n].Color = clinicInfoMethod.PsClinicalInfoGetColor(history[n].Tag.Substring(history[n].Tag.LastIndexOf("、") + 1));  //若有多个标签，颜色取最后一个
                                }
                            }
                            #endregion
                        }


                        #endregion

                        Clinic.History = history;   //时间轴

                    }  //if (DT_Clinical_ClinicInfo.Rows.Count > 1)的结尾


                    //取出指针标记
                    int mark = DT_Clinical_ClinicInfo.Count - 1;
                    Clinic.AdmissionDateMark = DT_Clinical_ClinicInfo[mark].精确时间.ToString();
                    Clinic.ClinicDateMark = DT_Clinical_ClinicInfo[mark].类型.ToString();

                    //确定是否能继续加载
                    if ((DT_Clinical_ClinicInfo.Count - 1) < Num)
                    {
                        Clinic.mark_contitue = "0";
                    }
                    else
                    {
                        string mark_in = clinicInfoMethod.PsClinicalInfoGetNextInDate(pclsCache, UserId, Clinic.AdmissionDateMark);
                        string mark_out = clinicInfoMethod.PsClinicalInfoGetNextOutDate(pclsCache, UserId, Clinic.ClinicDateMark);
                        if (((mark_in == "") && (mark_out == "")) || ((mark_in == null) && (mark_out == null)))
                        {
                            Clinic.mark_contitue = "0";
                        }
                        else
                        {
                            Clinic.mark_contitue = "1";
                        }
                    }
                    #endregion

                }
                #endregion

                return Clinic;
                //string result_final = JSONHelper.ObjectToJson(Clinic);
                //Context.Response.BinaryWrite(new byte[] { 0xEF, 0xBB, 0xBF });
                //Context.Response.Write(result_final);

                //Context.Response.End();

            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetReminder", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                // return null;
                throw (ex);
            }
        }
        public ClinicInfoViewModel GetClinicInfoDetail(DataConnection pclsCache, string UserId, string Type, string VisitId, string Date)
        {
            try
            {
                ClinicInfoViewModel DT_ClinicInfoDetail = new ClinicInfoViewModel();
                
                //string date = Date.Substring(0, 10) + " " + Date.Substring(10, 8);
                //string date_final = Convert.ToDateTime(date).ToString("yyyy/M/d H:mm:ss");
                //string date_final = Date.Substring(0, 10) + " " + Date.Substring(10, 8);
                string date_final = Date;
                return DT_ClinicInfoDetail = clinicInfoMethod.GetClinicInfoDetail(pclsCache, UserId, Type, VisitId, date_final);
          
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetReminder", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
                throw (ex);
            }
        }
        public List<LabTestDetails> GetLabTestDtlList(DataConnection pclsCache, string UserId, string VisitId, string SortNo)
        {
            try
            {
                List<LabTestDetails> DT_LabTestDetails = new List<LabTestDetails>();     
                return DT_LabTestDetails = clinicInfoMethod.GetLabTestDetails(pclsCache, UserId, VisitId, SortNo);

            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetLabTestDtlList", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
                throw (ex);
            }
        }

        public ClinicalInfoListViewModel GetClinicalInfoList(DataConnection pclsCache, string UserId)
        {
            try
            {
                ClinicInfoViewModel DT_ClinicInfoDetail = new ClinicInfoViewModel();
                return clinicInfoMethod.GetClinicalInfoList(pclsCache, UserId);

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
            try
            {
                string ret = clinicInfoMethod.getLatestHUserIdByHCode(pclsCache, UserId, HospitalCode);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "getLatestHUserIdByHCode", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return "";
                throw ex;
            }
        }
        public List<SymptomsList> GetSymptomsList(DataConnection pclsCache, string UserId, string VisitId)
        {
            try
            {
                return clinicInfoMethod.GetSymptomsList(pclsCache, UserId, VisitId);
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetSymptomsList", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
                throw (ex);
            }
        }
        public List<DiagnosisInfo> GetDiagnosisInfoList(DataConnection pclsCache, string UserId, string VisitId)
        {
            try
            {
                return clinicInfoMethod.PsDiagnosisGetDiagnosisInfo(pclsCache, UserId, VisitId);
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetDiagnosisInfoList", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
                throw (ex);
            }
        }
        public List<ExamInfo> GetExaminationList(DataConnection pclsCache, string UserId, string VisitId)
        {
            try
            {
                return clinicInfoMethod.PsExaminationGetExaminationList(pclsCache, UserId, VisitId);
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetExaminationList", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
                throw (ex);
            }
        }
        public List<ExamDetails> GetExamDtlList(DataConnection pclsCache, string UserId, string VisitId, string SortNo, string ItemCode)
        {
            try
            {
                return clinicInfoMethod.GetExamDetails(pclsCache, UserId, VisitId, SortNo, ItemCode);
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetExamDtlList", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
                throw (ex);
            }
        }
        public List<LabTestList> GetLabTestList(DataConnection pclsCache, string UserId, string VisitId)
        {
            try
            {
                return clinicInfoMethod.GetLabTestList(pclsCache, UserId, VisitId);
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetLabTestList", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
                throw (ex);
            }
        }
        public List<DrugRecordList> GetDrugRecordList(DataConnection pclsCache, string UserId, string VisitId)
        {
            try
            {
                return clinicInfoMethod.GetDrugRecordList(pclsCache, UserId, VisitId);
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetDrugRecordList", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
                throw (ex);
            }
        }

    }
}