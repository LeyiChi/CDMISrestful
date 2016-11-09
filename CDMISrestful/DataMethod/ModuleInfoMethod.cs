using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CDMISrestful.CommonLibrary;
using CDMISrestful.DataModels;
using InterSystems.Data.CacheClient;

namespace CDMISrestful.DataMethod
{
    public class ModuleInfoMethod
    {
        #region <Ps.BasicInfoDetail>
        /// <summary>
        /// SetData LY 2015-10-10 
        /// </summary>
        /// <param name="pclsCache"></param>
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
        public int PsBasicInfoDetailSetData(DataConnection pclsCache, string Patient, string CategoryCode, string ItemCode, int ItemSeq, string Value, string Description, int SortNo, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            int ret = 2;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }
                ret = (int)Ps.BasicInfoDetail.SetData(pclsCache.CacheConnectionObject, Patient, CategoryCode, ItemCode, ItemSeq, Value, Description, SortNo, revUserId, TerminalName, TerminalIP, DeviceType);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ModuleInfoMethod.PsBasicInfoDetailSetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        ///  获取需要知道的项目的值 LY 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <param name="CategoryCode"></param>
        /// <param name="ItemCode"></param>
        /// <param name="ItemSeq"></param>
        /// <returns></returns>
        public string PsBasicInfoDetailGetValue(DataConnection pclsCache, string UserId, string CategoryCode, string ItemCode, int ItemSeq)
        {
            string ret = "";
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }
                ret = (string)Ps.BasicInfoDetail.GetValue(pclsCache.CacheConnectionObject, UserId, CategoryCode, ItemCode, ItemSeq);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ModuleInfoMethod.PsBasicInfoDetailGetValue", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// 得到专员信息 LY 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public TypeAndName PsBasicInfoDetailGetSDoctor(DataConnection pclsCache, string PatientId)
        {
            TypeAndName ret = new TypeAndName();
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }
                InterSystems.Data.CacheTypes.CacheSysList list = null;
                list = Ps.BasicInfoDetail.GetSDoctor(pclsCache.CacheConnectionObject, PatientId);
                if (list != null)
                {
                    ret.Type = list[0];
                    ret.Name = list[1];
                }
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ModuleInfoMethod.PsBasicInfoDetailGetSDoctor", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// 得到病人详细信息 LY 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public PatDetailInfo PsBasicInfoDetailGetPatientDetailInfo(DataConnection pclsCache, string UserId)
        {
            PatDetailInfo ret = new PatDetailInfo();
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }
                InterSystems.Data.CacheTypes.CacheSysList list = null;
                list = Ps.BasicInfoDetail.GetPatientDetailInfo(pclsCache.CacheConnectionObject, UserId);
                if (list != null)
                {
                    ret.PhoneNumber = list[0];
                    ret.HomeAddress = list[1];
                    ret.Occupation = list[2];
                    ret.Nationality = list[3];
                    ret.EmergencyContact = list[4];
                    ret.EmergencyContactPhoneNumber = list[5];
                    ret.PhotoAddress = list[6];
                    ret.IDNo = list[7];
                    ret.Height = list[8];
                    ret.Weight = list[9];
                }
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ModuleInfoMethod.PsBasicInfoDetailGetPatientDetailInfo", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// 获取用户全部详细信息 LY 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <param name="CategoryCode"></param>
        /// <returns></returns>

        public List<PatBasicInfoDetail> PsBasicInfoDetailGetPatientBasicInfoDetail(DataConnection pclsCache, string UserId, string CategoryCode)
        {
            List<PatBasicInfoDetail> list = new List<PatBasicInfoDetail>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.BasicInfoDetail.GetPatientBasicInfoDetail(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("UserId", CacheDbType.NVarChar).Value = UserId;
                cmd.Parameters.Add("CategoryCode", CacheDbType.NVarChar).Value = CategoryCode;
                cdr = cmd.ExecuteReader();
                int ItemSeq;
                int SortNo;
                int ControlType;
                while (cdr.Read())
                {
                    if (cdr["ItemSeq"].ToString() == "")
                    {
                        ItemSeq = 0;
                    }
                    else
                    {
                        ItemSeq = Convert.ToInt32(cdr["ItemSeq"]);
                    }
                    if (cdr["SortNo"].ToString() == "")
                    {
                        SortNo = 0;
                    }
                    else
                    {
                        SortNo = Convert.ToInt32(cdr["SortNo"]);
                    }
                    if (cdr["ControlType"].ToString() == "")
                    {
                        ControlType = 0;
                    }
                    else
                    {
                        ControlType = Convert.ToInt32(cdr["ControlType"]);
                    }
                    PatBasicInfoDetail NewLine = new PatBasicInfoDetail();
                    NewLine.UserId = cdr["UserId"].ToString();
                    NewLine.CategoryCode = cdr["CategoryCode"].ToString();
                    NewLine.CategoryName = cdr["CategoryName"].ToString();
                    NewLine.ItemCode = cdr["ItemCode"].ToString();
                    NewLine.ItemName = cdr["ItemName"].ToString();
                    NewLine.ParentCode = cdr["ParentCode"].ToString();
                    NewLine.ItemSeq = ItemSeq;
                    NewLine.Value = cdr["Value"].ToString();
                    NewLine.Content = cdr["Content"].ToString();
                    NewLine.Description = cdr["Description"].ToString();
                    NewLine.SortNo = SortNo;
                    NewLine.ControlType = ControlType;
                    NewLine.OptionCategory = cdr["OptionCategory"].ToString();
                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ModuleInfoMethod.PsBasicInfoDetailGetPatientBasicInfoDetail", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        //通过PID得到模块 LY 2015-10-10 
        public List<TypeAndName> PsBasicInfoDetailGetModulesByPID(DataConnection pclsCache, string PatientId)
        {
            List<TypeAndName> list = new List<TypeAndName>();
            CacheCommand cmd = null;
            CacheDataReader cdr = null;
            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Ps.BasicInfoDetail.GetModulesByPID(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("PatientId", CacheDbType.NVarChar).Value = PatientId;
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    TypeAndName NewLine = new TypeAndName();
                    NewLine.Type = cdr["CategoryCode"].ToString();
                    NewLine.Name = cdr["Modules"].ToString();
                    list.Add(NewLine);
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ModuleInfoMethod.PsBasicInfoDetailGetModulesByPID", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        #region <Ps.DoctorInfoDetail>
        /// <summary>
        /// SetData  LY 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="Doctor"></param>
        /// <param name="CategoryCode"></param>
        /// <param name="ItemCode"></param>
        /// <param name="ItemSeq"></param>
        /// <param name="Value"></param>
        /// <param name="Description"></param>
        /// <param name="SortNo"></param>
        /// <param name="piUserId"></param>
        /// <param name="piTerminalName"></param>
        /// <param name="piTerminalIP"></param>
        /// <param name="piDeviceType"></param>
        /// <returns></returns>
        public int PsDoctorInfoDetailSetData(DataConnection pclsCache, string Doctor, string CategoryCode, string ItemCode, int ItemSeq, string Value, string Description, int SortNo, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            int ret = 2;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }
                ret = (int)Ps.DoctorInfoDetail.SetData(pclsCache.CacheConnectionObject, Doctor, CategoryCode, ItemCode, ItemSeq, Value, Description, SortNo, piUserId, piTerminalName, piTerminalIP, piDeviceType);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ModuleInfoMethod.PsDoctorInfoDetailSetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// 得到最大的ItemSeq LY 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="DoctorId"></param>
        /// <param name="CategoryCode"></param>
        /// <param name="ItemCode"></param>
        /// <returns></returns>
        public int PsDoctorInfoDetailGetMaxItemSeq(DataConnection pclsCache, string DoctorId, string CategoryCode, string ItemCode)
        {
            int ret = 0;
            try
            {
                if (!pclsCache.Connect())
                {
                    return ret;
                }
                ret = (int)Ps.DoctorInfoDetail.GetMaxItemSeq(pclsCache.CacheConnectionObject, DoctorId, CategoryCode, ItemCode);
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ModuleInfoMethod.PsDoctorInfoDetailGetMaxItemSeq", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return ret;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// 得到医生详细信息 LY 2015-10-10
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DocInfoDetail PsDoctorInfoDetailGetDoctorInfoDetail(DataConnection pclsCache, string UserId)
        {
            try
            {
                DocInfoDetail ret = new DocInfoDetail();
                if (!pclsCache.Connect())
                {
                    return null;
                }
                InterSystems.Data.CacheTypes.CacheSysList list = null;
                list = Ps.DoctorInfoDetail.GetDoctorInfoDetail(pclsCache.CacheConnectionObject, UserId);
                if (list != null)
                {
                    ret.IDNo = list[0];
                    ret.PhotoAddress = list[1];
                    ret.UnitName = list[2];
                    ret.JobTitle = list[3];
                    ret.Level = list[4];
                    ret.Dept = list[5];
                    ret.ActivatePhotoAddr = list[6];
                    ret.DeptName = list[7];
                }
                return ret;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "ModuleInfoMethod.PsDoctorInfoDetailGetDoctorInfoDetail", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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