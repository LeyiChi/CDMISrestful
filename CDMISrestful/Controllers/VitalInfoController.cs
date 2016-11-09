using CDMISrestful.DataModels;
using CDMISrestful.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CDMISrestful.CommonLibrary;
using System.Web.OData;

namespace CDMISrestful.Controllers
{
     [WebApiTracker]
    [RESTAuthorizeAttribute]
    public class VitalInfoController : ApiController
    {
        static readonly IVitalInfoRepository repository = new VitalInfoRepository();
        DataConnection pclsCache = new DataConnection();
        /// <summary>
        /// GetLatestPatientVitalSigns 获取病人某项生理参数最新值 GL 2015-10-12
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ItemType"></param>
        /// <param name="ItemCode"></param>
        /// <returns></returns>
        [Route("Api/v1/VitalInfo/VitalSign")]
        public ValueTime GetLatestPatientVitalSigns(string UserId, string ItemType, string ItemCode)
        {
            ValueTime ret = repository.GetLatestPatientVitalSigns(pclsCache, UserId, ItemType, ItemCode);
            //return new ExceptionHandler().Common(Request, ret);
            return ret;
        }

        /// <summary>
        /// SetPatientVitalSigns  GL 2015-10-12  
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("Api/v1/VitalInfo/VitalSign")]
        [ModelValidationFilter]
        public HttpResponseMessage PostPatientVitalSigns(SetVitalInfo item)
        {
            int ret = repository.SetPatientVitalSigns(pclsCache, item.UserId, Convert.ToInt32(item.RecordDate), Convert.ToInt32(item.RecordTime), item.ItemType, item.ItemCode, item.Value, item.Unit, item.revUserId, item.TerminalName, new CommonFunction().getRemoteIPAddress(), item.DeviceType);
            return new ExceptionHandler().SetData(Request,ret);
        }

        /// <summary>
        /// WF 20151031 获取体征信息 一段时间内的所有体征信息 同一天同一种体征可以有多条输出
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        [Route("Api/v1/VitalInfo/VitalSigns")]
        [EnableQuery]
        [HttpGet]
        public List<VitalInfo> GetVitalSignsByPeriod(string UserId, int StartDate, int EndDate)
        {
            return repository.GetVitalSignsByPeriod(pclsCache, UserId, StartDate, EndDate);
        }

        ///// <summary>
        ///// 根据患者Id，输入的起始日期和截止日期获取该段时间内记录的所有体征数据 CSQ 20151031
        ///// </summary>
        ///// <param name="UserId"></param>
        ///// <param name="StartDate"></param>
        ///// <param name="EndDate"></param>
        ///// <returns></returns>
        //[Route("Api/v1/VitalInfo/VitalSigns")]        
        //public List<VitalInfo> GetAllSignsByPeriod(string UserId, int StartDate, int EndDate)
        //{
        //    return repository.GetAllSignsByPeriod(UserId, StartDate, EndDate);
        //}

        ///// <summary>
        ///// GetSignsDetailByPeriod 获取某日期之前，一定条数血压（收缩压/舒张压）和脉率的数据详细时刻列表,用于phone，支持继续加载  GL 2015-10-12  
        ///// </summary>
        ///// <param name="PatientId"></param>
        ///// <param name="Module"></param>
        ///// <param name="StartDate"></param>
        ///// <param name="Num"></param>
        //[Route("Api/v1/VitalInfo/VitalSigns")]
        //[HttpGet]
        //public SignDetailByP GetSignsDetailByPeriod(string PatientId, string Module, int StartDate, int Num)
        //{
        //    return repository.GetSignsDetailByPeriod(PatientId, Module, StartDate, Num);
        //}

    }
}
