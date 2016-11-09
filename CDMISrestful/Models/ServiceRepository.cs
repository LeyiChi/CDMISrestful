using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using ServiceStack.Redis;
using CDMISrestful.CommonLibrary;
using CDMISrestful.DataModels;
using CDMISrestful.DataMethod;

namespace CDMISrestful.Models
{
    public class ServiceRepository : IServiceRepository
    {
       
        public string sendSMS(string mobile, string smsType)
        {
            try
            {

                var Client = new RedisClient("127.0.0.1", 6379);
                var token = "849407bfab0cf4c1a998d3d6088d957b";
                var appId = "0593b3c52f7d4f8aa9f9055585407e16";
                var accountSid = "b839794e66174938828d1b8ea9c58412";
                var tplId = "14452";
                var Jsonstring1 = "templateSMS";
                var Jsonstring2 = "appId";
                var Jsonstring3 = "param";
                var Jsonstring4 = "templateId";
                var Jsonstring5 = "to";
                var J6 = "{";

                Random rand = new Random();
                var randNum = rand.Next(100000, 1000000);
                var param = randNum + "," + 3;

                string JSONData = J6 + '"' + Jsonstring1 + '"' + ':' + '{' + '"' + Jsonstring2 + '"' + ':' + '"' + appId + '"' + ',' + '"' + Jsonstring3 + '"' + ':' + '"' + param + '"' + ',' + '"' + Jsonstring4 + '"' + ':' + '"' + tplId + '"' + ',' + '"' + Jsonstring5 + '"' + ':' + '"' + mobile + '"' + '}' + '}';

                if (mobile != "" && smsType != "")
                {
                    var Flag1 = Client.Get<string>(mobile + smsType);
                    if (Flag1 == null)
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

        public int checkverification(string mobile, string smsType, string verification)  //Verification是前端传进来的验证码
        {
            try
            {
                var Client = new RedisClient("127.0.0.1", 6379);
                var verify = Client.Get<string>(mobile + smsType);
                if (verify != null)
                {
                    if (verify == verification)
                    {
                        return 1;//验证码正确
                    }
                    else
                    {
                        return 2;//验证码错误
                    }
                }
                else
                {
                    return 0;//没有验证码或验证码已过期
                }
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "匹配验证码功能错误", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return 3;
                throw (ex);
            }
        }

       /// <summary>
       /// 推送 通知
       /// </summary>
       /// <param name="platform"></param>
       /// <param name="Alias"></param>
       /// <param name="notification"></param>
       /// <returns></returns>
        public string PushNotification(string platform, string Alias, string notification) // platform 是平台，不输入的时候默认为全部（安卓和ios）,可以单独输入android或者ios,不支持winphone
        {                                                                                  // Alias 是别名，用于定位推送，输入为空时会推送给全部用户
            try                                                                            // notification是要推送的消息内容
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
                    string JSONData = "";

                    if (platform == "")
                    {
                        platform = "all";
                    }
                    if (Alias == "")
                    {
                        Alias = "all";
                        JSONData = J5 + '"' + J1 + '"' + ':' + '"' + platform + '"' + ',' + '"' + J2 + '"' + ':' + '"' + Alias + '"' + ',' + '"' + J3 + '"' + ':' + '{' + '"' + J4 + '"' + ':' + '"' + notification + '"' + '}' + '}';
                    }
                    else
                    {
                        JSONData = J5 + '"' + J1 + '"' + ':' + '"' + platform + '"' + ',' + '"' + J2 + '"' + ':' + '{' + '"' + J6 + '"' + ':' + '[' + '"' + Alias + '"' + ']' + '}' + ',' + '"' + J3 + '"' + ':' + '{' + '"' + J4 + '"' + ':' + '"' + notification + '"' + '}' + '}';
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

                    return strResult;
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
        public string GetPatientInfo(DataConnection pclsCache, string PatientId)
        {
            List<TypeAndName> List = new List<TypeAndName>();
            List.Add(new TypeAndName
            {
                Type = "ubid",
                Name = "DW0101111447049158"
            });
            PatBasicInfo BasicInfo = new UsersRepository().GetPatBasicInfo(pclsCache, PatientId);
            if (BasicInfo == null)
                return "病人不存在";
            List.Add(new TypeAndName
            {
                Type = "name",
                Name = BasicInfo.UserName
            });
            List.Add(new TypeAndName
            {
                Type = "age",
                Name = BasicInfo.Age
            });
            List.Add(new TypeAndName
            {
                Type = "sex",
                Name = BasicInfo.Gender
            });
            string PhoneNumber = new UsersMethod().GetPhoneNoByUserId(pclsCache, PatientId);
            List.Add(new TypeAndName
            {
                Type = "mobilephone",
                Name = PhoneNumber
            });
            ValueTime H = new ValueTime();
            H = new VitalInfoRepository().GetLatestPatientVitalSigns(pclsCache, PatientId, "Height", "Height_1");
            string Height = H.Value;

            ValueTime W = new ValueTime();
            W = new VitalInfoRepository().GetLatestPatientVitalSigns(pclsCache, PatientId, "Weight", "Weight_1");
            string Weight = W.Value;

            ValueTime Wa = new ValueTime();
            Wa = new VitalInfoRepository().GetLatestPatientVitalSigns(pclsCache, PatientId, "Waistline", "Waistline_1");
            string Waistline = Wa.Value;
            if (Height == null)
                Height = "";
            if (Weight == null)
                Weight = "";
            List.Add(new TypeAndName
            {
                Type = "height",
                Name = Height
            });
            List.Add(new TypeAndName
            {
                Type = "weight",
                Name = Weight
            });
            List.Add(new TypeAndName
            {
                Type = "birthday",
                Name = BasicInfo.Birthday.Insert(4,"-").Insert(7,"-")
            });
            if (Waistline != null)
                List.Add(new TypeAndName
                {
                    Type = "waistline",
                    Name = Waistline
                });
            try
            {
                string BasicJson = "{";
                foreach (TypeAndName Line in List)
                {
                    BasicJson = BasicJson + '"' + Line.Type + '"' + ':' + '"' + Line.Name + '"' + ',';
                }
                BasicJson = BasicJson.TrimEnd(',');
                BasicJson += "}";
                byte[] bytedata = Encoding.UTF8.GetBytes(BasicJson);
                string content = Convert.ToBase64String(bytedata, 0, bytedata.Length);
                content = "type=docapp&action=004&content=" + content + "&contentkey=#HQ*" + content;
                string Url = "http://qacsupport.duapp.com/port/port.php";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                request.ContentLength = bytes.Length;
                //request.Timeout = 10000;
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
        /// 浙大接收接口处理 LY 2015-10-31
        /// </summary>
        /// <param name="VitalSigns"></param>
        /// <param name="revUserId"></param>
        /// <param name="TerminalName"></param>
        /// <param name="TerminalIP"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>
        public int VitalSignFromZKY(DataConnection pclsCache, VitalSignFromDevice VitalSigns)
        {
            string UserId = new UsersMethod().GetIDByInput(pclsCache, "PhoneNo", VitalSigns.mobilephone);
            string revUserId = UserId;
            string TerminalName = "DeviceFromZKY";
            string TerminalIP = new CommonFunction().getRemoteIPAddress();
            int DeviceType = 4;
            int RecordDate = Convert.ToInt32(VitalSigns.dailyinfos.date.Replace("-",""));
            int ret = 0;
            foreach (BloodPressureInfos Line in VitalSigns.dailyinfos.bloodpressureinfos)
            {
                if (Line.high != "" && Line.low != "" && Line.time != "")
                {
                    int RecordTime = Convert.ToInt32(Line.time.Substring(0, 5).Replace(":", ""));
                    ret = new VitalInfoRepository().SetPatientVitalSigns(pclsCache, UserId, RecordDate, RecordTime, "Bloodpressure", "Bloodpressure_1", Line.high, "mmHg", revUserId, TerminalName, TerminalIP, DeviceType);
                    if (ret == 0)
                        return ret;
                    ret = new VitalInfoRepository().SetPatientVitalSigns(pclsCache, UserId, RecordDate, RecordTime, "Bloodpressure", "Bloodpressure_2", Line.low, "mmHg", revUserId, TerminalName, TerminalIP, DeviceType);
                    if (ret == 0)
                        return ret;
                }
            }
            foreach (BloodSugarInfos Line in VitalSigns.dailyinfos.bloodsugarinfos)
            {
                if (Line.glu != "" && Line.time != "")
                {
                    int RecordTime = Convert.ToInt32(Line.time.Substring(0, 5).Replace(":", ""));
                    if (Line.type == 0)
                        ret = new VitalInfoRepository().SetPatientVitalSigns(pclsCache, UserId, RecordDate, RecordTime, "BloodSugar", "BloodSugar_10", Line.glu, "mmol/l", revUserId, TerminalName, TerminalIP, DeviceType);
                    if (Line.type == 1)
                        ret = new VitalInfoRepository().SetPatientVitalSigns(pclsCache, UserId, RecordDate, RecordTime, "BloodSugar", "BloodSugar_11", Line.glu, "mmol/l", revUserId, TerminalName, TerminalIP, DeviceType);
                    if (ret == 0)
                        return ret;
                }
            }
            foreach (ECGInfos Line in VitalSigns.dailyinfos.ecginfos)
            {
                if (Line.bpm != "" && Line.time != "")
                {
                    int RecordTime = Convert.ToInt32(Line.time.Substring(0, 5).Replace(":", ""));
                    ret = new VitalInfoRepository().SetPatientVitalSigns(pclsCache, UserId, RecordDate, RecordTime, "HeartRate", "HeartRate_1", Line.bpm, "次/分", revUserId, TerminalName, TerminalIP, DeviceType);
                    if (ret == 0)
                        return ret;
                }
            }
            foreach (BreatheInfos Line in VitalSigns.dailyinfos.breatheinfos)
            if (Line.time != "")
            {
                int RecordTime = Convert.ToInt32(Line.time.Substring(0, 5).Replace(":", ""));
                ret = new VitalInfoRepository().SetPatientVitalSigns(pclsCache, UserId, RecordDate, RecordTime, "BreathStatus", "Oximetry", Line.oximetry.ToString(), "", revUserId, TerminalName, TerminalIP, DeviceType);
                if (ret == 0)
                    return ret;
            }
            return ret;
        }
    }
}