using CDMISrestful.CommonLibrary;
using CDMISrestful.DataModels;
using InterSystems.Data.CacheClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CDMISrestful.DataMethod;

namespace CDMISrestful.DataMethod
{
    public class VitalInfoMethod
    {
        //DataConnection pclsCache = new DataConnection();

        #region < "Ps.VitalSigns" >
        /// <summary>
        /// Ps.VitalSigns.SetData GL 2015-10-10
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RecordDate"></param>
        /// <param name="RecordTime"></param>
        /// <param name="ItemType"></param>
        /// <param name="ItemCode"></param>
        /// <param name="Value"></param>
        /// <param name="Unit"></param>
        /// <param name="revUserId"></param>
        /// <param name="TerminalName"></param>
        /// <param name="TerminalIP"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>
        public int SetData(DataConnection pclsCache, string UserId, int RecordDate, int RecordTime, string ItemType, string ItemCode, string Value, string Unit, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            int ret = 2;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }
                ret = (int)Ps.VitalSigns.SetData(pclsCache.CacheConnectionObject, UserId, RecordDate, RecordTime, ItemType, ItemCode, Value, Unit, revUserId, TerminalName, TerminalIP, DeviceType);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "VitalInfoMethod.SetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return 2;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// 获取某日期之后的有数据的起止时间 GL 2015-10-10
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="date"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public VitalInfo GetVitalSignDates(DataConnection pclsCache, string UserId, int date, int Num)
        {
            try
            {
                VitalInfo item = new VitalInfo();
                InterSystems.Data.CacheTypes.CacheSysList list = null;
                if (!pclsCache.Connect())
                {
                    return null;
                }
                list = Ps.VitalSigns.GetVitalSignDates(pclsCache.CacheConnectionObject, UserId, date, Num);
                if (list != null)
                {
                    item.StartDate = list[0];
                    item.EndDate = list[1];
                }
                return item;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "VitalInfoMethod.GetVitalSignDates", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// 从数据库拿出相关数据 并增加type字段 区别收缩压、舒张压、脉率  用于phone 体征详细时刻表 GL 2015-10-10
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ItemType"></param>
        /// <param name="ItemCode"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public List<VitalInfo> GetTypedSignDetailByPeriod(DataConnection pclsCache, string UserId, string ItemType, string ItemCode, int StartDate, int EndDate)
        {
            {
                List<VitalInfo> items = new List<VitalInfo>();
                CacheCommand cmd = null;
                CacheDataReader cdr = null;
                try
                {
                    if (!pclsCache.Connect())
                    {
                        return null;
                    }
                    cmd = new CacheCommand();
                    cmd = Ps.VitalSigns.GetSignDetailByPeriod(pclsCache.CacheConnectionObject);
                    cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;
                    cmd.Parameters.Add("ItemType", CacheDbType.NVarChar).Value = ItemType;
                    cmd.Parameters.Add("ItemCode", CacheDbType.NVarChar).Value = ItemCode;
                    cmd.Parameters.Add("StartDate", CacheDbType.NVarChar).Value = StartDate;
                    cmd.Parameters.Add("EndDate", CacheDbType.NVarChar).Value = EndDate;

                    cdr = cmd.ExecuteReader();
                    while (cdr.Read())
                    {
                        VitalInfo item = new VitalInfo();
                        string RecordDate = cdr["RecordDate"].ToString();
                        RecordDate = RecordDate.Substring(0, 4) + "-" + RecordDate.Substring(4, 2) + "-" + RecordDate.Substring(6, 2);
                        string SignType = "";
                        if (ItemCode == "Bloodpressure_1") //收缩压
                        {
                            SignType = "1";
                        }
                        else if (ItemCode == "Bloodpressure_2")  //舒张压
                        {
                            SignType = "2";
                        }
                        else   //脉率"Pulserate_1"
                        {
                            SignType = "3";
                        }
                        item.SignType = SignType;
                        item.RecordDate = RecordDate;
                        item.RecordTime = new CommonFunction().TransTime(cdr["RecordTime"].ToString());
                        item.Value = cdr["Value"].ToString();
                        item.Unit = cdr["Unit"].ToString();
                        items.Add(item);
                    }
                    return items;
                }
                catch (Exception ex)
                {
                    HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "VitalInfoMethod.GetTypedSignDetailByPeriod", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        /// <summary>
        /// GL 2015-10-10
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ItemType"></param>
        /// <param name="ItemCode"></param>
        /// <param name="RecordDate"></param>
        /// <returns></returns>
        public VitalInfo GetSignByDay(DataConnection pclsCache, string UserId, string ItemType, string ItemCode, int RecordDate)
        {
            try
            {
                VitalInfo item = new VitalInfo();
                InterSystems.Data.CacheTypes.CacheSysList list = null;
                list = Ps.VitalSigns.GetSignByDay(pclsCache.CacheConnectionObject, UserId, ItemType, ItemCode, RecordDate);
                if (list != null)
                {
                    item.RecordDate = list[0];
                    item.RecordTime = list[1];
                    item.Value = list[2];
                    item.Unit = list[3];
                }
                return item;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "VitalInfoMethod.GetSignByDay", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// 获取患者某项生理参数的RecordDate前的最近一条数据 GL 2015-10-10
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ItemType"></param>
        /// <param name="ItemCode"></param>
        /// <param name="RecordDate"></param>
        /// <returns></returns>
        public VitalInfo GetLatestVitalSignsByDate(DataConnection pclsCache, string UserId, string ItemType, string ItemCode, int RecordDate)
        {
            try
            {
                VitalInfo item = new VitalInfo();
                InterSystems.Data.CacheTypes.CacheSysList list = null;
                list = Ps.VitalSigns.GetLatestVitalSignsByDate(pclsCache.CacheConnectionObject, UserId, ItemType, ItemCode, RecordDate);
                if (list != null)
                {
                    item.RecordDate = list[0];
                    item.RecordTime = list[1];
                    item.Value = list[2];
                    item.Unit = list[3];
                }
                return item;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "VitalInfoMethod.GetLatestVitalSignsByDate", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// 获取病人最新体征情况 GL 2015-10-10
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ItemType"></param>
        /// <param name="ItemCode"></param>
        /// <returns></returns>
        public ValueTime GetLatestPatientVitalSigns(DataConnection pclsCache, string UserId, string ItemType, string ItemCode)
        {
            ValueTime item = new ValueTime();
            try
            {
                if (!pclsCache.Connect())
                {
                    return item;
                }
                InterSystems.Data.CacheTypes.CacheSysList list = null;
                list = Ps.VitalSigns.GetLatestPatientVitalSigns(pclsCache.CacheConnectionObject, UserId, ItemType, ItemCode);
                if (list != null)
                {
                    item.Value = list[0];
                    item.RecordDate = list[1];
                    item.RecordTime = list[2];
                }
                return item;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "VitalInfoMethod.GetLatestPatientVitalSigns", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return item;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        //输出一段时间中记录的所有体征信息
        public List<VitalInfo> GetAllSignsByPeriod(DataConnection pclsCache, string UserId, int StartDate, int EndDate)
        {
            {
                List<VitalInfo> items = new List<VitalInfo>();
                CacheCommand cmd = null;
                CacheDataReader cdr = null;
                try
                {
                    if (!pclsCache.Connect())
                    {
                        return null;
                    }
                    cmd = new CacheCommand();
                    cmd = Ps.VitalSigns.GetAllSignsByPeriod(pclsCache.CacheConnectionObject);
                    cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;
                    cmd.Parameters.Add("StartDate", CacheDbType.NVarChar).Value = StartDate;
                    cmd.Parameters.Add("EndDate", CacheDbType.NVarChar).Value = EndDate;

                    cdr = cmd.ExecuteReader();
                    while (cdr.Read())
                    {
                        VitalInfo item = new VitalInfo();
                        item.RecordDate = cdr["RecordDate"].ToString();
                        item.RecordTime = cdr["RecordTime"].ToString();
                        item.ItemType   = cdr["ItemType"].ToString();
                        item.ItemCode   = cdr["ItemCode"].ToString();
                        item.Value      = cdr["Value"].ToString();
                        item.Unit       = cdr["Unit"].ToString();
                        item.Name = cdr["VitalName"].ToString();
                        items.Add(item);
                    }
                    return items;
                }
                catch (Exception ex)
                {
                    HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "VitalInfoMethod.GetAllSignsByPeriod", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        //WF 20151031
        public List<VitalInfo> GetVitalSignsByPeriod(DataConnection pclsCache, string UserId, int StartDate, int EndDate)
        {
            {
                List<VitalInfo> items = new List<VitalInfo>();
                CacheCommand cmd = null;
                CacheDataReader cdr = null;
                try
                {
                    if (!pclsCache.Connect())
                    {
                        return null;
                    }
                    cmd = new CacheCommand();
                    cmd = Ps.VitalSigns.GetVitalSignsByPeriod(pclsCache.CacheConnectionObject);
                    cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;
                    cmd.Parameters.Add("StartDate", CacheDbType.NVarChar).Value = StartDate;
                    cmd.Parameters.Add("EndDate", CacheDbType.NVarChar).Value = EndDate;

                    cdr = cmd.ExecuteReader();
                    while (cdr.Read())
                    {
                        VitalInfo item = new VitalInfo();
                        item.RecordDate = cdr["RecordDate"].ToString();
                        string time = cdr["RecordTime"].ToString();
                        switch (time.Length)
                        {
                            case 1:
                                item.RecordTime = "00:0" + time;
                                break;
                            case 2:
                                item.RecordTime = "00:" + time;
                                break;
                            case 3:
                                item.RecordTime = "0" + time.Substring(0,1)+":"+time.Substring(1,2);
                                break;
                            case 4:
                                item.RecordTime = time.Substring(0, 2) + ":" + time.Substring(2, 2);
                                break;
                        }

                        //item.RecordTime = cdr["RecordTime"].ToString();
                        item.ItemType = cdr["ItemType"].ToString();
                        item.ItemCode = cdr["ItemCode"].ToString();
                        item.Value = cdr["Value"].ToString();
                        item.Unit = cdr["Unit"].ToString();
                        item.Name = cdr["VitalName"].ToString();

                        items.Add(item);
                    }
                    return items;
                }
                catch (Exception ex)
                {
                    HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "VitalInfoMethod.GetVitalSignsByPeriod", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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


        #endregion
    }
}