using CDMISrestful.CommonLibrary;
using CDMISrestful.DataMethod;
using CDMISrestful.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
//using System.Web.Services;

namespace CDMISrestful.Models
{
    public class VitalInfoRepository : IVitalInfoRepository
    {    
        // 获取病人最新体征情况 GL 2015-10-10       
        public ValueTime GetLatestPatientVitalSigns(DataConnection pclsCache, string UserId, string ItemType, string ItemCode)
        {
            return new VitalInfoMethod().GetLatestPatientVitalSigns(pclsCache, UserId, ItemType, ItemCode);
        }

        // Ps.VitalSigns.SetData GL 2015-10-10      
        public int SetPatientVitalSigns(DataConnection pclsCache, string UserId, int RecordDate, int RecordTime, string ItemType, string ItemCode, string Value, string Unit, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            return new VitalInfoMethod().SetData(pclsCache, UserId, RecordDate, RecordTime, ItemType, ItemCode, Value, Unit, revUserId, TerminalName, TerminalIP, DeviceType);
        }

        //public List<VitalInfo> GetAllSignsByPeriod( string UserId, int StartDate, int EndDate)
        //{
        //    return new VitalInfoMethod().GetAllSignsByPeriod(pclsCache, UserId, StartDate, EndDate);
        //}

        // 获取某日期之前，一定条数血压（收缩压/舒张压）和脉率的数据详细时刻列表,用于phone，支持继续加载  GL 2015-10-12  
        public SignDetailByP GetSignsDetailByPeriod(DataConnection pclsCache, string PatientId, string Module, int StartDate, int Num)
        {
            SignDetailByP result = new SignDetailByP();

            try
            {
                int CacheStartDate = 0;
                int CacheEndDate = 0;

                //按有数据的天数平移
                VitalInfo dateList = new VitalInfoMethod().GetVitalSignDates(pclsCache, PatientId, StartDate, Num);
                if (dateList != null)
                {
                    if ((dateList.StartDate != null) && (dateList.EndDate != null))
                    {
                        CacheStartDate = Convert.ToInt32(dateList.StartDate);
                        CacheEndDate = Convert.ToInt32(dateList.EndDate);
                        result.NextStartDate = CacheStartDate;
                    }
                    else if ((dateList.StartDate == null) && (dateList.EndDate != null))
                    {
                        CacheEndDate = Convert.ToInt32(dateList.EndDate);
                        result.NextStartDate = -1; //取完的标志

                    }
                    else if ((dateList.StartDate == null) && (dateList.EndDate == null))
                    {
                        result.NextStartDate = -1;
                    }
                }

                //收缩压
                List<VitalInfo> sysInfo = new List<VitalInfo>();
                sysInfo = new VitalInfoMethod().GetTypedSignDetailByPeriod(pclsCache, PatientId, "Bloodpressure", "Bloodpressure_1", CacheStartDate, CacheEndDate);

                //舒张压
                List<VitalInfo> diaInfo = new List<VitalInfo>();
                diaInfo = new VitalInfoMethod().GetTypedSignDetailByPeriod(pclsCache, PatientId, "Bloodpressure", "Bloodpressure_2", CacheStartDate, CacheEndDate);

                //脉率
                List<VitalInfo> pulInfo = new List<VitalInfo>();
                pulInfo = new VitalInfoMethod().GetTypedSignDetailByPeriod(pclsCache, PatientId, "Pulserate", "Pulserate_1", CacheStartDate, CacheEndDate);

                //三张表整合，按时间排序 避免条数可能不一致造成的问题                
                //sysInfo.Merge(diaInfo);
                //sysInfo.Merge(pulInfo);
                if (diaInfo.Count > 0)
                {
                    foreach (VitalInfo item in diaInfo)
                    {
                        sysInfo.Add(item);
                    }
                }
                if (pulInfo.Count > 0)
                {
                    foreach (VitalInfo item in pulInfo)
                    {
                        sysInfo.Add(item);
                    }
                }

                //按RecordDate、RecordTime、SignType排序  再合并成收集需要的形式)
                //将List<>转化为datatable进行排序
                DataTable list = new DataTable();
                list.Columns.Add(new DataColumn("SignType", typeof(string)));
                list.Columns.Add(new DataColumn("RecordDate", typeof(string)));
                list.Columns.Add(new DataColumn("RecordTime", typeof(string)));
                list.Columns.Add(new DataColumn("Value", typeof(string)));
                list.Columns.Add(new DataColumn("Unit", typeof(string)));

                foreach (VitalInfo item in sysInfo)
                {
                    list.Rows.Add(item.SignType, item.RecordDate, item.RecordTime, item.Value, item.Unit);
                }

                DataView dv = list.DefaultView;
                dv.Sort = "RecordDate desc, RecordTime asc,SignType asc";
                DataTable dt_Sort = dv.ToTable();

                //1 收缩压, 2 舒张压, 3 脉率 
                //整理成日期、时刻、数值的形式
                //整理成列表形式 2011/01/03 星期三 
                //08:00 137 95 66
                //09:00 134 78 66
                if (dt_Sort != null)
                {
                    if (dt_Sort.Rows.Count > 0)
                    {
                        SignDetail SignDetail = new SignDetail();
                        SignDetail.DetailTime = dt_Sort.Rows[0]["RecordTime"].ToString();
                        if (dt_Sort.Rows[0]["SignType"].ToString() == "1")
                        {
                            SignDetail.SBPValue = dt_Sort.Rows[0]["Value"].ToString();
                        }
                        else if (dt_Sort.Rows[0]["SignType"].ToString() == "2")
                        {
                            SignDetail.DBPValue = dt_Sort.Rows[0]["Value"].ToString();
                        }
                        else
                        {
                            SignDetail.PulseValue = dt_Sort.Rows[0]["Value"].ToString();
                        }

                        SignDetailByD SignDetailByD = new SignDetailByD();
                        SignDetailByD.Date = dt_Sort.Rows[0]["RecordDate"].ToString();
                        SignDetailByD.WeekDay = new CommonFunction().CaculateWeekDay(dt_Sort.Rows[0]["RecordDate"].ToString());

                        if (dt_Sort.Rows.Count == 1)
                        {
                            SignDetailByD.SignDetailList.Add(SignDetail);
                            result.SignDetailByDs.Add(SignDetailByD);
                        }
                        else
                        {
                            string temp_date = dt_Sort.Rows[0]["RecordDate"].ToString();
                            string temp_hour = dt_Sort.Rows[0]["RecordTime"].ToString();

                            for (int rowsCount = 1; rowsCount < dt_Sort.Rows.Count; rowsCount++)
                            {
                                if (rowsCount != dt_Sort.Rows.Count - 1)
                                {
                                    #region 不是最后一条

                                    if (temp_date == dt_Sort.Rows[rowsCount]["RecordDate"].ToString())
                                    {
                                        #region 同一天
                                        if (temp_hour == dt_Sort.Rows[rowsCount]["RecordTime"].ToString())
                                        {
                                            if (dt_Sort.Rows[rowsCount]["SignType"].ToString() == "1")
                                            {
                                                SignDetail.SBPValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                            }
                                            else if (dt_Sort.Rows[rowsCount]["SignType"].ToString() == "2")
                                            {
                                                SignDetail.DBPValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                            }
                                            else
                                            {
                                                SignDetail.PulseValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                            }
                                        }
                                        else
                                        {
                                            SignDetailByD.SignDetailList.Add(SignDetail);

                                            SignDetail = new SignDetail();
                                            SignDetail.DetailTime = dt_Sort.Rows[rowsCount]["RecordTime"].ToString();
                                            if (dt_Sort.Rows[rowsCount]["SignType"].ToString() == "1")
                                            {
                                                SignDetail.SBPValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                            }
                                            else if (dt_Sort.Rows[rowsCount]["SignType"].ToString() == "2")
                                            {
                                                SignDetail.DBPValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                            }
                                            else
                                            {
                                                SignDetail.PulseValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                            }

                                            temp_hour = dt_Sort.Rows[rowsCount]["RecordTime"].ToString();
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 不同天
                                        SignDetailByD.SignDetailList.Add(SignDetail);
                                        result.SignDetailByDs.Add(SignDetailByD);

                                        SignDetailByD = new SignDetailByD();
                                        SignDetail = new SignDetail();
                                        SignDetail.DetailTime = dt_Sort.Rows[rowsCount]["RecordTime"].ToString();
                                        if (dt_Sort.Rows[rowsCount]["SignType"].ToString() == "1")
                                        {
                                            SignDetail.SBPValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                        }
                                        else if (dt_Sort.Rows[rowsCount]["SignType"].ToString() == "2")
                                        {
                                            SignDetail.DBPValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                        }
                                        else
                                        {
                                            SignDetail.PulseValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                        }
                                        SignDetailByD.Date = dt_Sort.Rows[rowsCount]["RecordDate"].ToString();
                                        SignDetailByD.WeekDay = new CommonFunction().CaculateWeekDay(dt_Sort.Rows[rowsCount]["RecordDate"].ToString());

                                        temp_date = dt_Sort.Rows[rowsCount]["RecordDate"].ToString();
                                        temp_hour = dt_Sort.Rows[rowsCount]["RecordTime"].ToString();

                                        #endregion
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 最后一条

                                    if (temp_date == dt_Sort.Rows[rowsCount]["RecordDate"].ToString())
                                    {
                                        #region 同一天
                                        if (temp_hour == dt_Sort.Rows[rowsCount]["RecordTime"].ToString())
                                        {
                                            if (dt_Sort.Rows[rowsCount]["SignType"].ToString() == "1")
                                            {
                                                SignDetail.SBPValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                            }
                                            else if (dt_Sort.Rows[rowsCount]["SignType"].ToString() == "2")
                                            {
                                                SignDetail.DBPValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                            }
                                            else
                                            {
                                                SignDetail.PulseValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                            }
                                            SignDetailByD.SignDetailList.Add(SignDetail);
                                            result.SignDetailByDs.Add(SignDetailByD);
                                        }
                                        else
                                        {
                                            SignDetailByD.SignDetailList.Add(SignDetail);

                                            SignDetail = new SignDetail();
                                            SignDetail.DetailTime = dt_Sort.Rows[rowsCount]["RecordTime"].ToString();
                                            if (dt_Sort.Rows[rowsCount]["SignType"].ToString() == "1")
                                            {
                                                SignDetail.SBPValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                            }
                                            else if (dt_Sort.Rows[rowsCount]["SignType"].ToString() == "2")
                                            {
                                                SignDetail.DBPValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                            }
                                            else
                                            {
                                                SignDetail.PulseValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                            }

                                            temp_hour = dt_Sort.Rows[rowsCount]["RecordTime"].ToString();
                                            SignDetailByD.SignDetailList.Add(SignDetail);
                                            result.SignDetailByDs.Add(SignDetailByD);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 不同天
                                        SignDetailByD.SignDetailList.Add(SignDetail);
                                        result.SignDetailByDs.Add(SignDetailByD);

                                        SignDetailByD = new SignDetailByD();
                                        SignDetail = new SignDetail();
                                        SignDetail.DetailTime = dt_Sort.Rows[rowsCount]["RecordTime"].ToString();
                                        if (dt_Sort.Rows[rowsCount]["SignType"].ToString() == "1")
                                        {
                                            SignDetail.SBPValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                        }
                                        else if (dt_Sort.Rows[rowsCount]["SignType"].ToString() == "2")
                                        {
                                            SignDetail.DBPValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                        }
                                        else
                                        {
                                            SignDetail.PulseValue = dt_Sort.Rows[rowsCount]["Value"].ToString();
                                        }
                                        SignDetailByD.Date = dt_Sort.Rows[rowsCount]["RecordDate"].ToString();
                                        SignDetailByD.WeekDay = new CommonFunction().CaculateWeekDay(dt_Sort.Rows[rowsCount]["RecordDate"].ToString());

                                        temp_date = dt_Sort.Rows[rowsCount]["RecordDate"].ToString();
                                        SignDetailByD.SignDetailList.Add(SignDetail);
                                        result.SignDetailByDs.Add(SignDetailByD);

                                        #endregion
                                    }

                                    #endregion
                                }

                            }
                        }
                    }
                }
                return result;
                //string a = JSONHelper.ObjectToJson(result);
                //Context.Response.BinaryWrite(new byte[] { 0xEF, 0xBB, 0xBF });
                //Context.Response.Write(a);
                ////Context.Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetSignsDetailByPeriod", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                //return null;
                throw (ex);
            }
        }

        public List<VitalInfo> GetVitalSignsByPeriod(DataConnection pclsCache, string UserId, int StartDate, int EndDate)
        {
            return new VitalInfoMethod().GetVitalSignsByPeriod(pclsCache, UserId, StartDate, EndDate);
        }


    }
}