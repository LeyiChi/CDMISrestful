using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CDMISrestful.CommonLibrary;

namespace CDMISrestful.DataMethod
{
    public class CommonMethod
    {
        #region CmMstNumbering
        /// <summary>
        /// GetNo 自动编号 CSQ 20151010
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
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CommonMethod.GetNo", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return number;
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }
        #endregion

        /// <summary>
        /// GetServerTime 获取服务器时间 CSQ 20151010
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <returns></returns>
        public string GetServerTime(DataConnection pclsCache)
        {
            string serverTime = string.Empty;
            try
            {
                if (!pclsCache.Connect())
                {
                    return serverTime;
                }
                serverTime = Cm.CommonLibrary.GetServerDateTime(pclsCache.CacheConnectionObject);
                serverTime = serverTime.Replace("/", "-");
                return serverTime;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CommonMethod.GetServerTime", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return serverTime;
                throw (ex);
            }
            finally
            {
                pclsCache.DisConnect();
            }
        }

        /// <summary>
        /// 获取服务器日期(解决频繁连接导致的连接未及时关闭)
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <returns></returns>
        public int GetServerDate(DataConnection pclsCache)                 //ZAM 2015-5-13 获取服务器日期(解决频繁连接导致的连接未及时关闭)
        {
            string serverDate = string.Empty;
            int date = 99999999;
            try
            {
                //ZAM 2015-5-7 频繁连接导致的连接未及时关闭
                //if (_cnCache.CacheConnectionObject.State == ConnectionState.Closed)
                //if (_cnCache.CacheConnectionObject.State != ConnectionState.Open)
                {
                    if (!pclsCache.Connect())
                    {
                        return date;
                    }
                }
                serverDate = Cm.CommonLibrary.GetServerDateTime(pclsCache.CacheConnectionObject);    //2014/08/22 15:33:35
                string[] str = serverDate.Split(' ');
                if (str.Length >= 1)
                {
                    serverDate = str[0];
                    serverDate = serverDate.Replace("/", string.Empty);
                    date = Convert.ToInt32(serverDate);
                }

                return date;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CommonMethod.GetServerDate", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return date;
                throw (ex);
            }
            finally
            {
                pclsCache.DisConnect();
            }

        }
    }
}