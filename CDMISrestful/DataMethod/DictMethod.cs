using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CDMISrestful.CommonLibrary;
using CDMISrestful.DataModels;
using InterSystems.Data.CacheClient;

namespace CDMISrestful.DataMethod
{
    public class DictMethod
    {
        #region CmMstType
        /// <summary>
        /// 获取某个分类的类别 CSQ 20151010
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="Category"></param>
        /// <returns></returns>
        public List<TypeAndName> CmMstTypeGetTypeList(DataConnection pclsCache, string Category)
        {
            List<TypeAndName> list = new List<TypeAndName>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    //MessageBox.Show("Cache数据库连接失败");
                    return null;
                }

                cmd = new CacheCommand();
                cmd = Cm.MstType.GetTypeList(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("Category", CacheDbType.NVarChar).Value = Category;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Add(new TypeAndName { Type = cdr["Type"].ToString(), Name = cdr["Name"].ToString() });
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "DictMethod.GetTypeList", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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
       
        #region CmMstLifeStyleDetail
        /// <summary>
        /// 获取生活方式细节 CSQ 20151010
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="Module"></param>
        /// <returns></returns>
        public List<LifeStyleDetail> GetLifeStyleDetail(DataConnection pclsCache, string Module)
        {
            List<LifeStyleDetail> list = new List<LifeStyleDetail>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Cm.MstLifeStyleDetail.GetLifeStyleDetail(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("Module", CacheDbType.NVarChar).Value = Module;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Add(new LifeStyleDetail
                    {
                        StyleId = cdr["StyleId"].ToString(),
                        Name = cdr["Name"].ToString(),
                        CurativeEffect = cdr["CurativeEffect"].ToString(),
                        SideEffect = cdr["SideEffect"].ToString(),
                        Instruction = cdr["Instruction"].ToString(),
                        HealthEffect = cdr["HealthEffect"].ToString(),
                        Unit = cdr["Unit"].ToString(),
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "DictMethod.GetLifeStyleDetail", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        #region Cm.MstInsurance
        /// <summary>
        /// GetInsurance CSQ 20151010
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <returns></returns>
        public List<Insurance> GetInsurance(DataConnection pclsCache)
        {
            List<Insurance> list = new List<Insurance>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }

                cmd = new CacheCommand();
                cmd = Cm.MstInsurance.GetInsuranceType(pclsCache.CacheConnectionObject);
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Add(new Insurance
                    {
                        Code = cdr["Code"].ToString(),
                        Name = cdr["Name"].ToString(),
                        InputCode = cdr["InputCode"].ToString(),
                        Redundance = cdr["Redundance"].ToString()
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "DictMethod.GetInsurance", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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


        #region Cm.MstInfoItem
        /// <summary>
        /// CSQ 20151010
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <returns></returns>
        public List<CmMstInfoItem> GetCmMstInfoItem(DataConnection pclsCache)
        {
            List<CmMstInfoItem> CmMstInfoItemList = new List<CmMstInfoItem>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = Cm.MstInfoItem.GetInfoItem(pclsCache.CacheConnectionObject);
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    CmMstInfoItemList.Add(new CmMstInfoItem
                    {
                        CategoryCode = cdr["CategoryCode"].ToString(),
                        Code = cdr["Code"].ToString(),
                        Name = cdr["Name"].ToString(),
                        ParentCode = cdr["ParentCode"].ToString(),
                        SortNo = Convert.ToInt32(cdr["SortNo"].ToString()),
                        StartDate = Convert.ToInt32(cdr["StartDate"].ToString()),
                        EndDate = Convert.ToInt32(cdr["EndDate"].ToString()),
                        GroupHeaderFlag = Convert.ToInt32(cdr["GroupHeaderFlag"].ToString()),
                        ControlType = cdr["ControlType"].ToString(),
                        OptionCategory = cdr["OptionCategory"].ToString(),
                        RevUserId = "",
                        TerminalName = "",
                        TerminalIP = "",
                        DeviceType = 0
                    });
                }
                return CmMstInfoItemList;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "DictMethod.GetInfoItem", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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
        /// GetMstInfoItemByCategoryCode CSQ 2015-10-10 
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="CategoryCode"></param>
        /// <returns></returns>
        public List<MstInfoItemByCategoryCode> GetMstInfoItemByCategoryCode(DataConnection pclsCache, string CategoryCode)
        {
            List<MstInfoItemByCategoryCode> list = new List<MstInfoItemByCategoryCode>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    //MessageBox.Show("Cache数据库连接失败");
                    return null;
                }

                cmd = new CacheCommand();
                cmd = Cm.MstInfoItem.GetMstInfoItemByCategoryCode(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("CategoryCode", CacheDbType.NVarChar).Value = CategoryCode;

                cdr = cmd.ExecuteReader();
                int SortNo;
                int GroupHeaderFlag;
                while (cdr.Read())
                {
                    if (cdr["SortNo"].ToString() == "")
                    {
                        SortNo = 0;
                    }
                    else
                    {
                        SortNo = Convert.ToInt32(cdr["SortNo"]);
                    }
                    if (cdr["GroupHeaderFlag"].ToString() == "")
                    {
                        GroupHeaderFlag = 0;
                    }
                    else
                    {
                        GroupHeaderFlag = Convert.ToInt32(cdr["GroupHeaderFlag"]);
                    }

                    list.Add(new MstInfoItemByCategoryCode
                    {
                        Code = cdr["Code"].ToString(),
                        Name = cdr["Name"].ToString(),
                        ParentCode = cdr["ParentCode"].ToString(),
                        SortNo = SortNo,
                        GroupHeaderFlag = GroupHeaderFlag,
                        ControlType = cdr["ControlType"].ToString(),
                        OptionCategory = cdr["OptionCategory"].ToString()
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "DictMethod.GetMstInfoItemByCategoryCode", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        #region Cm.MstHypertensionDrug
        /// <summary>
        /// GetTypeList 返回所有类型代码及名称 CSQ 20151010
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <returns></returns>
        public List<TypeAndName> CmMstHypertensionDrugGetTypeList(DataConnection pclsCache)
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
                cmd = Cm.MstHypertensionDrug.GetTypeList(pclsCache.CacheConnectionObject);
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Add(new TypeAndName
                    {
                        Type = cdr["Type"].ToString(),
                        Name = cdr["TypeName"].ToString()
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "DictMethod.GetTypeList", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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
        /// GetHypertensionDrug 返回所有数据信息 CSQ 20151010
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <returns></returns>
        public List<CmAbsType> GetHypertensionDrug(DataConnection pclsCache)
        {
            List<CmAbsType> list = new List<CmAbsType>();

            int int_InvalidFlag = 0;

            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    //MessageBox.Show("Cache数据库连接失败");
                    return null;
                }

                cmd = new CacheCommand();
                cmd = Cm.MstHypertensionDrug.GetHypertensionDrug(pclsCache.CacheConnectionObject);
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    if (cdr["InvalidFlag"].ToString() == "")
                    {
                        int_InvalidFlag = 0;
                    }
                    else
                    {
                        int_InvalidFlag = Convert.ToInt32(cdr["InvalidFlag"]);
                    }

                    list.Add(new CmAbsType
                    {
                        Type = cdr["Type"].ToString(),
                        Code = cdr["Code"].ToString(),
                        TypeName = cdr["TypeName"].ToString(),
                        Name = cdr["Name"].ToString(),
                        InputCode = cdr["InputCode"].ToString(),
                        SortNo = Convert.ToInt32(cdr["SortNo"]),
                        Redundance = cdr["Redundance"].ToString(),
                        InvalidFlag = int_InvalidFlag
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "DictMethod.GetHypertensionDrug", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        #region CmMstDiabetesDrug GetTypeList
        /// <summary>
        ///  CmMstDiabetesDrugGetTypeList 返回所有类型代码及名称 CSQ 20151010
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <returns></returns>
        public List<TypeAndName> CmMstDiabetesDrugGetTypeList(DataConnection pclsCache)
        {
            List<TypeAndName> list = new List<TypeAndName>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    //MessageBox.Show("Cache数据库连接失败");
                    return null;
                }

                cmd = new CacheCommand();
                cmd = Cm.MstDiabetesDrug.GetTypeList(pclsCache.CacheConnectionObject);
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Add(new TypeAndName { Type = cdr["Type"].ToString(), Name = cdr["TypeName"].ToString() });
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "DictMethod.CmMstDiabetesDrugGetTypeList", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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
        ///  GetDiabetesDrug 返回所有数据信息 CSQ 20151010
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <returns></returns>
        public List<CmAbsType> GetDiabetesDrug(DataConnection pclsCache)
        {
            List<CmAbsType> list = new List<CmAbsType>();

            int int_InvalidFlag = 0;

            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    //MessageBox.Show("Cache数据库连接失败");
                    return null;
                }

                cmd = new CacheCommand();
                cmd = Cm.MstDiabetesDrug.GetDiabetesDrug(pclsCache.CacheConnectionObject);
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    if (cdr["InvalidFlag"].ToString() == "")
                    {
                        int_InvalidFlag = 0;
                    }
                    else
                    {
                        int_InvalidFlag = Convert.ToInt32(cdr["InvalidFlag"]);
                    }

                    list.Add(new CmAbsType
                    {
                        Type = cdr["Type"].ToString(),
                        Code = cdr["Code"].ToString(),
                        TypeName = cdr["TypeName"].ToString(),
                        Name = cdr["Name"].ToString(),
                        InputCode = cdr["InputCode"].ToString(),
                        SortNo = Convert.ToInt32(cdr["SortNo"]),
                        Redundance = cdr["Redundance"].ToString(),
                        InvalidFlag = int_InvalidFlag
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "DictMethod.GetDiabetesDrug", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        #region CmMstBloodPressure

        #endregion


        #region<Cm.MstTask>
        public int CmMstTaskSetData(DataConnection pclsCache, string CategoryCode, string Code, string Name, string ParentCode, string Description, int StartDate, int EndDate, int GroupHeaderFlag, int ControlType, string OptionCategory, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            int IsSaved = 2;
            try
            {
                if (!pclsCache.Connect())
                {
                    //MessageBox.Show("Cache数据库连接失败");
                    return IsSaved;

                }
                IsSaved = (int)Cm.MstTask.SetData(pclsCache.CacheConnectionObject, CategoryCode, Code, Name, ParentCode, Description, StartDate, EndDate, GroupHeaderFlag, ControlType, OptionCategory, revUserId, TerminalName, TerminalIP, DeviceType);

                return IsSaved;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "保存失败！");
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "DictMethod.CmMstTaskSetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return IsSaved;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }
        public List<CmMstTask> GetMstTaskByParentCode(DataConnection pclsCache, string ParentCode)
        {
            List<CmMstTask> list = new List<CmMstTask>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    return null;
                }
                cmd = new CacheCommand();
                cmd = Cm.MstTask.GetMstTaskByParentCode(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("DoctorId", CacheDbType.NVarChar).Value = ParentCode;

                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Add(new CmMstTask
                    {
                        CategoryCode = cdr["CategoryCode"].ToString(),
                        Code = cdr["Code"].ToString(),
                        Name = cdr["Name"].ToString(),
                        ParentCode = cdr["ParentCode"].ToString(),
                        Description = cdr["Description"].ToString(),
                        StartDate = Convert.ToInt32(cdr["StartDate"]),
                        EndDate = Convert.ToInt32(cdr["EndDate"]),
                        GroupHeaderFlag = Convert.ToInt32(cdr["GroupHeaderFlag"]),
                        ControlType = Convert.ToInt32(cdr["ControlType"]),
                        OptionCategory = cdr["OptionCategory"].ToString(),
                 
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "UsersMethod.GetMstTaskByParentCode", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        #region<Cm.MstNumbering>
        /// <summary>
        /// syf 20151029
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="NumberingType"></param>
        /// <param name="TargetDate"></param>
        /// <returns></returns>
        public string GetNo(DataConnection pclsCache, int NumberingType, string TargetDate)
        {
            string number = "";
            try
            {
                if (!pclsCache.Connect())
                {
                    //MessageBox.Show("Cache数据库连接失败");
                    return number;

                }
                number = Cm.MstNumbering.GetNo(pclsCache.CacheConnectionObject, NumberingType, TargetDate);
                return number;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "获取编号失败！");
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "DictMethod.GetNo", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return number;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }
        #endregion

        #region<Cm.MstDivision>
        /// <summary>
        /// 获取科室所有Type和TypeName字段 SYF 20151109
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <returns></returns>
        public List<TypeAndName> GetAllDivisionType(DataConnection pclsCache)
        {
            List<TypeAndName> list = new List<TypeAndName>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    //MessageBox.Show("Cache数据库连接失败");
                    return null;
                }

                cmd = new CacheCommand();
                cmd = Cm.MstDivision.GetAllType(pclsCache.CacheConnectionObject);
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Add(new TypeAndName { Type = cdr["Type"].ToString(), Name = cdr["TypeName"].ToString() });
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "DictMethod.GetAllDivisionType", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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
        /// SYf 20151109
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public List<TypeAndName> GetDivisionDeptList(DataConnection pclsCache, string Type)
        {
            List<TypeAndName> list = new List<TypeAndName>();

            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    //MessageBox.Show("Cache数据库连接失败");
                    return null;
                }

                cmd = new CacheCommand();
                cmd = Cm.MstDivision.GetDeptList(pclsCache.CacheConnectionObject);
                cmd.Parameters.Add("Type", CacheDbType.NVarChar).Value = Type;
                cdr = cmd.ExecuteReader();
                while (cdr.Read())
                {
                    list.Add(new TypeAndName { Type = cdr["Code"].ToString(), Name = cdr["Name"].ToString() });
                }
                return list;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "DictMethod.GetDivisionDeptList", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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

        #region<Cm.MonitorMethod>
        public CmMonitorMethod GetMonitorMethodData(DataConnection pclsCache, string Type, string Code)
        {
            try
            {
                CmMonitorMethod item = new CmMonitorMethod();
                if (!pclsCache.Connect())
                {
                    return null;
                }
                InterSystems.Data.CacheTypes.CacheSysList list = null;
                list = Cm.MonitorMethod.GetData(pclsCache.CacheConnectionObject, Type, Code);
                if (list != null)
                {
                    item.TypeName = list[0];
                    item.Name = list[1];
                    item.Method = list[2];
                    item.Description = list[3];
                    item.SortNo = Convert.ToInt32(list[4]);
                    item.Redundance = list[5];
                }
                return item;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "DictMethod.GetMonitorMethodData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
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