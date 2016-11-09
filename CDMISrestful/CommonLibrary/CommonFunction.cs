using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

namespace CDMISrestful.CommonLibrary
{
    public class CommonFunction
    {
        DataConnection pclsCache = new DataConnection();
        /// <summary>
        /// 时间格式转换 GL 2015-10-10
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string TransTime(string time)
        {
            int length = time.Length;
            string result = "";
            try
            {
                switch (length)
                {
                    case 1:
                        result = "00：0" + time;
                        break;
                    case 2:
                        result = "00：" + time;
                        break;
                    case 3:
                        result = "0" + time.Substring(0, 1) + "：" + time.Substring(1, 2);
                        break;
                    case 4:
                        result = time.Substring(0, 2) + "：" + time.Substring(2, 2);  //Substring(起始, 截取长度)
                        break;
                    default: break;
                }

                return result;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "CommonMethod.TransTime", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        //CaculateWeekDay 判断日期是星期几 syf 2015-10-10
        public string CaculateWeekDay(string date)
        {
            string week = "星期一";  //待标记颜色
            try
            {
                string weekDayEn = Convert.ToDateTime(date).DayOfWeek.ToString();
                switch (weekDayEn)
                {
                    case "Monday":
                        week = "星期一";
                        break;
                    case "Tuesday":
                        week = "星期二";
                        break;
                    case "Wednesday":
                        week = "星期三";
                        break;
                    case "Thursday":
                        week = "星期四";
                        break;
                    case "Friday":
                        week = "星期五";
                        break;
                    case "Saturday":
                        week = "星期六";
                        break;
                    case "Sunday":
                        week = "星期日";
                        break;
                    default: break;
                }

                return week;
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "PsCompliance.CaculateWeekDay", "数据库操作异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        //获取服务器日期(解决频繁连接导致的连接未及时关闭) GL 2015-10-13
        public int GetServerDate()
        {
            string serverDate = string.Empty;
            int date = 99999999;
            try
            {
                //ZAM 2015-5-7 频繁连接导致的连接未及时关闭
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
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetServerDate", "CommonFunction error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return date;
                throw (ex);
            }
            finally
            {
                pclsCache.DisConnect();
            }



        }

        /// <summary>
        /// 获取远程调用的IP CSQ 20151026
        /// </summary>
        /// <returns></returns>
        public string getRemoteIPAddress()
        {
            string visitorIP = "";
            visitorIP = HttpContext.Current.Request.UserHostAddress;
            return visitorIP;
        }

        public string getTerminalName()
        {
            //string visitorIP = "";
            string machineName = "";
            //visitorIP = HttpContext.Current.Request.UserHostAddress;
            System.Net.IPHostEntry host = new System.Net.IPHostEntry();
            host = System.Net.Dns.GetHostEntry(HttpContext.Current.Request.UserHostAddress);
            machineName = host.HostName;
            return machineName;
        }
    }
}