using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CDMISrestful.CommonLibrary;
using CDMISrestful.DataModels;
using InterSystems.Data.CacheClient;

namespace CDMISrestful.Models
{
    public class CmMstInfoItemRepository : ICmMstInfoItemRepository
    {
        //private List<CmMstInfoItem> CmMstInfoItemList = new List<CmMstInfoItem>();
     

        //public IEnumerable<CmMstInfoItem> GetAll()
        //{
        //    List<CmMstInfoItem> CmMstInfoItemList = new List<CmMstInfoItem>();
        //    GetCmMstInfoItem(CmMstInfoItemList);
        //    return CmMstInfoItemList;
        //}

        //public CmMstInfoItem Get(string CategoryCode, string Code, int StartDate)
        //{
        //    List<CmMstInfoItem> CmMstInfoItemList = new List<CmMstInfoItem>();
        //    GetCmMstInfoItem(CmMstInfoItemList);
        //    return CmMstInfoItemList.Find(p => p.CategoryCode == CategoryCode && p.Code == Code && p.StartDate == StartDate);
        //}

        public int Remove(DataConnection pclsCache, string CategoryCode, string Code, int StartDate)
        {
             int ret = 3;
             try
             {
                 if (!pclsCache.Connect())
                 {
                     //MessageBox.Show("Cache数据库连接失败");
                     return ret;

                 }
                 ret = (int)Cm.MstInfoItem.DeleteData(pclsCache.CacheConnectionObject, CategoryCode, Code);
                 return ret;
             }
             catch (Exception ex)
             {
                 //MessageBox.Show(ex.ToString(), "保存失败！");
                 HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CmMstInfoItem.DeleteData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                 return ret;
             }
             finally
             {
                 pclsCache.DisConnect();
             }
        }

        //public bool Update(CmMstInfoItem item)
        //{
        //    if (item == null)
        //    {
        //        throw new ArgumentNullException("item");
        //    }
        //    bool ret = SetData(item);
        //    return ret;
        //}

        //public bool AddItem(CmMstInfoItem item)
        //{
        //    if (item == null)
        //    {
        //        throw new ArgumentNullException("item");
        //    }
        //    bool ret = SetData(item);
        //    return ret;
        //}

        #region functions
        private void GetCmMstInfoItem(DataConnection pclsCache, List<CmMstInfoItem> CmMstInfoItemList)
        {
            CacheCommand cmd = null;
            CacheDataReader cdr = null;

            try
            {
                if (!pclsCache.Connect())
                {
                    return;
                }
                //cmd = new CacheCommand();
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
                                            TerminalName ="",
                                            TerminalIP = "",
                                            DeviceType = 0                                       
                    });
                }
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CmMstInfoItem.GetInfoItem", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return;
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

        private bool SetData(DataConnection pclsCache, CmMstInfoItem item)
        {
            bool IsSaved = false;
            try
            {
                if (!pclsCache.Connect())
                {
                    //MessageBox.Show("Cache数据库连接失败");
                    return IsSaved;

                }
                int flag = (int)Cm.MstInfoItem.SetData(pclsCache.CacheConnectionObject, item.CategoryCode, item.Code, item.Name, item.ParentCode, item.SortNo, item.StartDate, item.EndDate, item.GroupHeaderFlag, item.ControlType, item.OptionCategory, item.RevUserId, item.TerminalName, item.TerminalIP, item.DeviceType);
                if (flag == 1)
                {
                    IsSaved = true;
                }
                return IsSaved;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "保存失败！");
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CmMstInfoItem.SetData", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return IsSaved;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }
        #endregion
    }
}