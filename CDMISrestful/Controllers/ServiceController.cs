using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using CDMISrestful.Models;
using CDMISrestful.CommonLibrary;
using CDMISrestful.DataModels;
using System.Text;
using ServiceStack.Redis;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using CDMISrestful.DataMethod;

namespace CDMISrestful.Controllers
{
     [WebApiTracker]
    [RESTAuthorizeAttribute]
    public class ServiceController : ApiController
    {
        static readonly IServiceRepository repository = new ServiceRepository();
        DataConnection pclsCache = new DataConnection();
        /// <summary>
        /// 发送验证码短信 20151016 CSQ
        /// smsType为verification时，不用输入content，发送验证码短信；
        /// smsType为confirmtoPatient时，发送预约情况给病人，需要content；
        /// smsType为confirmtoHealthCoach时，发送预约情况给专员，需要content。
        /// content的内容示例： 王五，2015年11月11日，海军总医院（目标姓名，预约时间，预约地点）
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="smsType"></param>
        /// <returns></returns>
        public string sendSMS(string mobile, string smsType, string content)//content的内容示例： 王五，2015年11月11日，海军总医院（目标姓名，预约时间，预约地点）
        {
            try
            {

                var Client = new RedisClient("127.0.0.1", 6379);
                var token = "849407bfab0cf4c1a998d3d6088d957b";
                var appId = "0593b3c52f7d4f8aa9f9055585407e16";
                var accountSid = "b839794e66174938828d1b8ea9c58412";
                var tplId = "";
                var param = "";
                var Jsonstring1 = "templateSMS";
                var Jsonstring2 = "appId";
                var Jsonstring3 = "param";
                var Jsonstring4 = "templateId";
                var Jsonstring5 = "to";
                var J6 = "{";
                var flag = false;

                Random rand = new Random();
                var randNum = rand.Next(100000, 1000000);
                if (smsType == "verification")
                {
                    tplId = "14452";
                    param = randNum + "," + 3;
                }
                if (smsType == "confirmtoPatient")
                {
                    tplId = "16420";
                    param = content;
                    flag = true;
                }
                if (smsType == "confirmtoHealthCoach")
                {
                    tplId = "16419";
                    param = content;
                    flag = true;
                }

                string JSONData = J6 + '"' + Jsonstring1 + '"' + ':' + '{' + '"' + Jsonstring2 + '"' + ':' + '"' + appId + '"' + ',' + '"' + Jsonstring3 + '"' + ':' + '"' + param + '"' + ',' + '"' + Jsonstring4 + '"' + ':' + '"' + tplId + '"' + ',' + '"' + Jsonstring5 + '"' + ':' + '"' + mobile + '"' + '}' + '}';

                if (mobile != "" && smsType != "")
                {
                    var Flag1 = Client.Get<string>(mobile + smsType);
                    if (Flag1 == null || flag == true)
                    {
                        Client.Set<int>(mobile + smsType, randNum);
                        Client.Expire(mobile + smsType, 3 * 60);
                        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                        MD5 MD5 = MD5.Create();
                        var md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(accountSid + token + timestamp, "MD5").ToUpper();

                        System.Text.Encoding encode = System.Text.Encoding.ASCII;
                        byte[] bytedata = encode.GetBytes(accountSid + ":" + timestamp);
                        var authorization = Convert.ToBase64String(bytedata, 0, bytedata.Length);
                        var length1 = md5.Length;
                        var length2 = authorization.Length;

                        string Url = "https://api.ucpaas.com/2014-06-30/Accounts/" + accountSid + "/Messages/templateSMS?sig=" + md5;
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                        request.Method = "POST";
                        request.Accept = "application/json";
                        request.ContentType = "application/json;charset=utf-8";
                        request.Headers.Set("Authorization", authorization);
                        //request.ContentLength = 256;
                        //request.Headers.Set("content-type", "application/json;charset=utf-8");
                        byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
                        request.ContentLength = bytes.Length;
                        request.Timeout = 10000;
                        Stream reqstream = request.GetRequestStream();
                        reqstream.Write(bytes, 0, bytes.Length);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        Stream streamReceive = response.GetResponseStream();
                        Encoding encoding = Encoding.UTF8;
                        StreamReader streamReader = new StreamReader(streamReceive, encoding);
                        string strResult = streamReader.ReadToEnd();
                        streamReceive.Dispose();
                        streamReader.Dispose();

                        return strResult;

                    }
                    else
                    {
                        var time = Client.Ttl(mobile + smsType);
                        string codeexist = "您的邀请码已发送，请等待" + time + "后重新获取";
                        return codeexist;
                    }
                }
                return null;
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        Console.WriteLine(text);
                    }
                }
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取验证码 CSQ 20151021
        /// 1：验证码正确
        /// 2：验证码错误
        /// 0：没有验证码或验证码已过期
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="smsType"></param>
        /// <param name="verification"></param>
        /// <returns></returns>
        public HttpResponseMessage checkverification(string mobile, string smsType, string verification)
        {
            string ret = repository.checkverification(mobile, smsType, verification).ToString();
            return new ExceptionHandler().Common(Request, ret);
        }

        /// <summary>
        /// 推送消息 通知   platform 是平台，不输入的时候默认为全部（安卓和ios）,可以单独输入android或者ios,不支持winphone
        ///  Alias 是别名，用于定位推送，输入为空时会推送给全部用户 接受者Id
        ///  notification是要推送的消息内容 title是要推送的内容的标题，目前只有android可以用(platform必须填android，否则标题无效)， ID为发送者的UID
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="Alias"></param>
        /// <param name="notification"></param>
        /// <returns></returns>
        [Route("Api/v1/Service/PushNotification")]
        public string GetNotification(string platform, string Alias, string notification, string title, string id) // platform 是平台，不输入的时候默认为全部（安卓和ios）,可以单独输入android或者ios,不支持winphone
        {                                                                                  // Alias 是别名，用于定位推送，输入为空时会推送给全部用户,输入目标用户的UID即可
            try                                                                            // notification是要推送的消息内容 title是要推送的内容的标题，目前只有android可以用(platform必须填android，否则标题无效)， ID为发送者的UID
            {
                if (notification != "")
                {
                    string APPKEY = "d78aa00fb6d8b1f6d156696b";
                    string MasterSecret = "3b3fd68d30b426d351d840b1";
                    string J1 = "platform";
                    string J2 = "audience";
                    string J3 = "notification";
                    string J4 = "alert";
                    string J5 = "{";
                    string J6 = "alias";
                    string J7 = "title";
                    string J8 = "android";
                    string J9 = "ios";
                    string J10 = "extras";
                    string J11 = "type";
                    string J12 = "SenderID";
                    string JSONData = "";
                    // + ',' + '"' + J7 + '"' + ':' + '"' + J8 + '"'
                    if (platform == "")
                    {
                        platform = "all";
                        if (Alias == "")
                        {
                            Alias = "all";
                            JSONData = J5 + '"' + J1 + '"' + ':' + '"' + platform + '"' + ',' + '"' + J2 + '"' + ':' + '"' + Alias + '"' + ',' + '"' + J3 + '"' + ':' + '{' + '"' + J4 + '"' + ':' + '"' + notification + '"' + '}' + '}';
                        }
                        else
                        {
                            JSONData = J5 + '"' + J1 + '"' + ':' + '"' + platform + '"' + ',' + '"' + J2 + '"' + ':' + '{' + '"' + J6 + '"' + ':' + '[' + '"' + Alias + '"' + ']' + '}' + ',' + '"' + J3 + '"' + ':' + '{' + '"' + J8 + '"' + ':' + '{' + '"' + J4 + '"' + ':' + '"' + notification + '"' + ',' + '"' + J7 + '"' + ':' + '"' + title + '"' + ',' + '"' + J10 + '"' + ':' + '{' + '"' + J11 + '"' + ':' + '"' + title + '"' + ',' + '"' + J12 + '"' + ':' + '"' + id + '"' + '}' + '}' + ',' + '"' + J9 + '"' + ':' + '{' + '"' + J4 + '"' + ':' + '"' + notification + '"' + ',' + '"' + J10 + '"' + ':' + '{' + '"' + J11 + '"' + ':' + '"' + title + '"' + ',' + '"' + J12 + '"' + ':' + '"' + id + '"' + '}' + '}' + '}' + '}';
                        }
                    }
                    else if (platform == "android")
                    {
                        if (Alias == "")
                        {
                            Alias = "all";
                            JSONData = J5 + '"' + J1 + '"' + ':' + '"' + platform + '"' + ',' + '"' + J2 + '"' + ':' + '"' + Alias + '"' + ',' + '"' + J3 + '"' + ':' + '{' + '"' + J8 + '"' + ':' + '{' + '"' + J4 + '"' + ':' + '"' + notification + '"' + ',' + '"' + J7 + '"' + ':' + '"' + title + '"' + ',' + '"' + J10 + '"' + ':' + '{' + '"' + J11 + '"' + ':' + '"' + title + '"' + '}' + '}' + '}' + '}';
                        }
                        else
                        {
                            JSONData = J5 + '"' + J1 + '"' + ':' + '"' + platform + '"' + ',' + '"' + J2 + '"' + ':' + '{' + '"' + J6 + '"' + ':' + '[' + '"' + Alias + '"' + ']' + '}' + ',' + '"' + J3 + '"' + ':' + '{' + '"' + J8 + '"' + ':' + '{' + '"' + J4 + '"' + ':' + '"' + notification + '"' + ',' + '"' + J7 + '"' + ':' + '"' + title + '"' + ',' + '"' + J10 + '"' + ':' + '{' + '"' + J11 + '"' + ':' + '"' + title + '"' + ',' + '"' + J12 + '"' + ':' + '"' + id + '"' + '}' + '}' + '}' + '}';
                        }
                    }
                    else if (platform == "ios")
                    {
                        if (Alias == "")
                        {
                            Alias = "all";
                            JSONData = J5 + '"' + J1 + '"' + ':' + '"' + platform + '"' + ',' + '"' + J2 + '"' + ':' + '"' + Alias + '"' + ',' + '"' + J3 + '"' + ':' + '{' + '"' + J9 + '"' + ':' + '{' + '"' + J4 + '"' + ':' + '"' + notification + '"' + ',' + '"' + J10 + '"' + ':' + '{' + '"' + J11 + '"' + ':' + '"' + title + '"' + '}' + '}' + '}' + '}';
                        }
                        else
                        {
                            JSONData = J5 + '"' + J1 + '"' + ':' + '"' + platform + '"' + ',' + '"' + J2 + '"' + ':' + '{' + '"' + J6 + '"' + ':' + '[' + '"' + Alias + '"' + ']' + '}' + ',' + '"' + J3 + '"' + ':' + '{' + '"' + J9 + '"' + ':' + '{' + '"' + J4 + '"' + ':' + '"' + notification + '"' + ',' + '"' + J10 + '"' + ':' + '{' + '"' + J11 + '"' + ':' + '"' + title + '"' + ',' + '"' + J12 + '"' + ':' + '"' + id + '"' + '}' + '}' + '}' + '}';
                        }
                    }

                    System.Text.Encoding encode = System.Text.Encoding.ASCII;
                    byte[] bytedata = encode.GetBytes(APPKEY + ":" + MasterSecret);
                    var Authorization = "Basic" + " " + Convert.ToBase64String(bytedata, 0, bytedata.Length);

                    string Url = "https://api.jpush.cn/v3/push";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                    request.Method = "POST";
                    request.Accept = "application/json";
                    request.Headers.Set("Authorization", Authorization);
                    //request.ContentLength = 256;
                    //request.Headers.Set("content-type", "application/json;charset=utf-8");
                    byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
                    request.ContentLength = bytes.Length;
                    request.Timeout = 10000;
                    Stream reqstream = request.GetRequestStream();
                    reqstream.Write(bytes, 0, bytes.Length);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream streamReceive = response.GetResponseStream();
                    Encoding encoding = Encoding.UTF8;
                    StreamReader streamReader = new StreamReader(streamReceive, encoding);
                    string strResult = streamReader.ReadToEnd();
                    streamReceive.Dispose();
                    streamReader.Dispose();
                    int SetN = 0;
                    string NotificationType = "";
                    if (id == "Administrator")
                    {
                        NotificationType = "系统";
                    }
                    else
                    {
                        NotificationType = "用户";
                    }
                   //var NowDay = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
                   //var NowTime = DateTime.Now.ToString("hh:mm:ss");
                   //var timestamp = NowDay + " " + NowTime;
                    var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                   SetN = new MessageMethod().PsNotificationSetData(pclsCache, Alias, NotificationType, title, notification, timestamp, id, "0", "", id, "system", new CommonFunction().getRemoteIPAddress(), 0);
                   return strResult + "_"+SetN;
                }
                else
                {
                    return "没有推送内容";
                }
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        Console.WriteLine(text);
                    }
                }
                return ex.Message;
            }
        }

        /// <summary>
        /// 浙大输出接口 LY 2015-10-29
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetPatientInfo(string PatientId)
        {
            string ret = repository.GetPatientInfo(pclsCache, PatientId);
            return new ExceptionHandler().Common(Request, ret);
        }

        /// <summary>
        /// 浙大接收接口处理 LY 2015-10-31
        /// </summary>
        /// <param name="VitalSigns"></param>
        /// <param name="revUserId"></param>
        /// <param name="TerminalName"></param>
        /// <param name="TerminalIP"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>
        public HttpResponseMessage VitalSignFromZKY(VitalSignFromDevice VitalSigns)
        {
            int ret = 9;
            var HeaderList = Request.Headers.ToList();
            string HeaderContent = "";
            KeyValuePair<string, IEnumerable<string>> Header = HeaderList.Find(delegate(KeyValuePair<string, IEnumerable<string>> x)
            {
                return x.Key == "Token";
            });
            if (Header.Key != null)
                HeaderContent = Header.Value.First();
            if (HeaderContent != "#zjuBME319*")
                return new ExceptionHandler().SetData(Request, ret);
            ret = repository.VitalSignFromZKY(pclsCache, VitalSigns);
            if (ret == 1)
            {
                string UserId = new UsersMethod().GetIDByInput(pclsCache, "PhoneNo", VitalSigns.mobilephone);
                string Note = repository.PushNotification("android", UserId, "新的体征信息已输入，请查看");
            }
            return new ExceptionHandler().SetData(Request, ret);
        }
    }
}
