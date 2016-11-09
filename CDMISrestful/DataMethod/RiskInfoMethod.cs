using CDMISrestful.CommonLibrary;
using CDMISrestful.DataModels;
using InterSystems.Data.CacheClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDMISrestful.DataMethod
{
    public class RiskInfoMethod
    {
        #region < "Ps.TreatmentIndicators" >

        /// <summary>
        /// Ps.TreatmentIndicators.SetData GL 2015-10-10
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SortNo"></param>
        /// <param name="AssessmentType"></param>
        /// <param name="AssessmentName"></param>
        /// <param name="AssessmentTime"></param>
        /// <param name="Result"></param>
        /// <param name="revUserId"></param>
        /// <param name="TerminalName"></param>
        /// <param name="TerminalIP"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>
        public int PsTreatmentIndicatorsSetData(DataConnection pclsCache, string UserId, int SortNo, string AssessmentType, string AssessmentName, DateTime AssessmentTime, string Result, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            int ret = 2;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }
                ret = (int)Ps.TreatmentIndicators.SetData(pclsCache.CacheConnectionObject, UserId, SortNo, AssessmentType, AssessmentName, AssessmentTime, Result, revUserId, TerminalName, TerminalIP, DeviceType);
                //if(ret == 1)
                //{
                //    int Ass = 0;
                //    string Amt = "";
                //    Amt = Ps.DoctorInfoDetail.GetValue(pclsCache.CacheConnectionObject, revUserId, "Score", "AssessmentNum", 1);
                //    if(Amt != null)
                //    {
                //        Ass = Convert.ToInt32(Amt);
                //        Ass++;
                //    }
                //    ret = (int)Ps.DoctorInfoDetail.SetData(pclsCache.CacheConnectionObject, revUserId, "Score", "AssessmentNum", 1, Convert.ToString(Ass), "", 1, revUserId, TerminalName, TerminalIP, DeviceType);

                //}
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "RiskInfoMethod.SetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return 2;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }
        public int PsParametersSetData(DataConnection pclsCache, string Indicators, string Id, string Name, string Value, string Unit,  string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            int ret = 2;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }
                ret = (int)Ps.Parameters.SetData(pclsCache.CacheConnectionObject, Indicators, Id, Name, Value, Unit, revUserId, TerminalName, TerminalIP, DeviceType);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "RiskInfoMethod.PsParametersSetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return 2;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }
        /// <summary>
        /// GL 2015-10-10
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public int GetMaxSortNo(DataConnection pclsCache, string UserId)
        {
            int ret = 0;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;

                }
                ret = (int)Ps.TreatmentIndicators.GetMaxSortNo(pclsCache.CacheConnectionObject, UserId);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "RiskInfoMethod.GetMaxSortNo", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// GL 2015-10-10
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SortNo"></param>
        /// <returns></returns>
        public string GetResult(DataConnection pclsCache, string UserId, int SortNo, string AssessmentType)
        {
            string Result = "";
            try
            {
                if (!pclsCache.Connect())
                {
                    return "";

                }
                Result = Ps.TreatmentIndicators.GetResult(pclsCache.CacheConnectionObject, UserId, SortNo, AssessmentType);
                return Result;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "RiskInfoMethod.GetResult", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return Result;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }
        public List<PsTreatmentIndicators> GetPsTreatmentIndicators(DataConnection pclsCache, string UserId)
        {
            List<PsTreatmentIndicators> list = new List<PsTreatmentIndicators>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.TreatmentIndicators.GetPsTreatmentIndicators(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Add(new PsTreatmentIndicators
                    {
                        SortNo = Convert.ToInt32(cdr["SortNo"]),
                        AssessmentType = cdr["AssessmentType"].ToString(),
                        AssessmentName = cdr["AssessmentName"].ToString(),
                        AssessmentTime = Convert.ToDateTime(cdr["AssessmentTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                        Result = cdr["Result"].ToString(),
                        DocName = cdr["DocName"].ToString(),
                       
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "RiskInfoMethod.GetPsTreatmentIndicators", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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
        public List<Parameters> GetParameters(DataConnection pclsCache, string Indicators)
        {
            List<Parameters> list = new List<Parameters>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.Parameters.GetParameters(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("Indicators", CacheDbType.NVarChar).Value = Indicators;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Add(new Parameters
                    {
                        Id = cdr["Id"].ToString(),
                        Name = cdr["Name"].ToString(),
                        Value = cdr["Value"].ToString(),
                        Unit = cdr["Unit"].ToString()
                       
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "RiskInfoMethod.GetParameters", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        #region<第二层>
        public int SetM1RiskInput(DataConnection pclsCache, string PatientId, M1RiskInput M1RiskInput, int RecordDate, int RecordTime, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            int ret = 0;
            int fret = 1;
            try
            {
                #region//插入PsVitalSigns表
                ret = new VitalInfoMethod().SetData(pclsCache, PatientId, RecordDate, RecordTime, "BodySigns", "Height", M1RiskInput.Height.ToString(), "cm", piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new VitalInfoMethod().SetData(pclsCache, PatientId, RecordDate, RecordTime, "BodySigns", "Weight", M1RiskInput.Weight.ToString(), "kg", piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new VitalInfoMethod().SetData(pclsCache, PatientId, RecordDate, RecordTime, "M1", "QG0018", M1RiskInput.AbdominalGirth.ToString(), "mm", piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new VitalInfoMethod().SetData(pclsCache, PatientId, RecordDate, RecordTime, "M1", "QG0007", M1RiskInput.Heartrate.ToString(), "次/分", piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new VitalInfoMethod().SetData(pclsCache, PatientId, RecordDate, RecordTime, "M1", "QG0013", M1RiskInput.SBP.ToString(), "mmHg", piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new VitalInfoMethod().SetData(pclsCache, PatientId, RecordDate, RecordTime, "M1", "QG0012", M1RiskInput.DBP.ToString(), "mmHg", piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                #endregion

                #region//插入PsBasicInfoDetail表
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M1", "QC0001", 1, M1RiskInput.Parent.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M1", "QF0011", 1, M1RiskInput.Smoke.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M1", "QA0007", 1, M1RiskInput.Stroke.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M1", "QJ0024", 1, M1RiskInput.Lvh.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M1", "QB0045", 1, M1RiskInput.Diabetes.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M1", "QD0002", 1, M1RiskInput.Treat.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M1", "QA0004", 1, M1RiskInput.Heartattack.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M1", "QA0005", 1, M1RiskInput.Af.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M1", "QA0002", 1, M1RiskInput.Chd.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M1", "QA0006", 1, M1RiskInput.Valve.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M1", "QH0002", 1, M1RiskInput.Tcho.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M1", "QH0001", 1, M1RiskInput.Creatinine.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M1", "QH0003", 1, M1RiskInput.Hdlc.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                #endregion

                return fret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "RiskInfoMethod.SetM1RiskInput", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return 0;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        public int SetM3RiskInput(DataConnection pclsCache, string PatientId, M3RiskInput M3RiskInput, int RecordDate, int RecordTime, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            int ret = 0;
            int fret = 1;
            try
            {
                #region//插入PsVitalSigns表
                ret = new VitalInfoMethod().SetData(pclsCache, PatientId, RecordDate, RecordTime, "BodySigns", "Height", M3RiskInput.Height.ToString(), "cm", piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new VitalInfoMethod().SetData(pclsCache, PatientId, RecordDate, RecordTime, "BodySigns", "Weight", M3RiskInput.Weight.ToString(), "kg", piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new VitalInfoMethod().SetData(pclsCache, PatientId, RecordDate, RecordTime, "M3", "QG0013", M3RiskInput.SBP.ToString(), "mmHg", piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                #endregion

                #region//插入PsBasicInfoDetail表
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M3", "QJ0025", 1, M3RiskInput.EF.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M3", "QF0011", 1, M3RiskInput.Smoke.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M3", "QB0045", 1, M3RiskInput.Diabetes.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M3", "QH0001", 1, M3RiskInput.Creatinine.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M3", "QJ0008", 1, M3RiskInput.NYHA.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M3", "QB0046", 1, M3RiskInput.Lung.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M3", "QB0047", 1, M3RiskInput.HF18.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M3", "QE0008", 1, M3RiskInput.Beta.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                ret = new UsersMethod().BasicInfoDetailSetData(pclsCache, PatientId, "M3", "QE0009", 1, M3RiskInput.AA.ToString(), "", 1, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                if (ret == 0)
                {
                    fret = 0;
                }
                #endregion

                return fret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "RiskInfoMethod.SetM3RiskInput", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return 0;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }
        #endregion
    }
}