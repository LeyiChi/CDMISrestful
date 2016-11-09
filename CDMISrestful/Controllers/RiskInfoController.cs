using CDMISrestful.CommonLibrary;
using CDMISrestful.DataModels;
using CDMISrestful.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CDMISrestful.Controllers
{
     [WebApiTracker]
    [RESTAuthorizeAttribute]
    public class RiskInfoController : ApiController
    {
        static readonly IRiskInfoRepository repository = new RiskInfoRepository();
        DataConnection pclsCache = new DataConnection();

        /// <summary>
        /// 根据收缩压获取血压等级说明 LY 2015-10-13
        /// </summary>
        /// <param name="SBP"></param>
        /// <returns></returns>
        [Route("Api/v1/RiskInfo/GetDescription")]
        public HttpResponseMessage GetDescription(int SBP)
        {
            string ret = repository.GetDescription(pclsCache, SBP);
            return new ExceptionHandler().Common(Request, ret);
        }

        /// <summary>
        /// 插入风险评估结果 LY 2015-10-13
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="AssessmentType"></param>
        /// <param name="AssessmentName"></param>
        /// <param name="AssessmentTime"></param>
        /// <param name="Result"></param>
        /// <param name="revUserId"></param>
        /// <param name="TerminalName"></param>
        /// <param name="TerminalIP"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>

        [Route("Api/v1/RiskInfo/RiskResult")]
        [ModelValidationFilter]
        public HttpResponseMessage PostRiskResult(RiskResult Item)
        {
            int ret = repository.SetRiskResult(pclsCache, Item.UserId, Item.AssessmentType, Item.AssessmentName, Item.AssessmentTime, Item.Result, Item.revUserId, Item.TerminalName, new CommonFunction().getRemoteIPAddress(), Item.DeviceType);
            return new ExceptionHandler().SetData(Request, ret);
        }

     

        /// <summary>
        /// 根据UserId获取最新风险评估结果 LY 2015-10-13
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [Route("Api/v1/RiskInfo/RiskResult")]
        public HttpResponseMessage GetRiskResult(string UserId, string AssessmentType)
        {
            string ret = repository.GetRiskResult(pclsCache, UserId, AssessmentType);
            return new ExceptionHandler().Common(Request, ret);
        }

       

        /// <summary>
        /// 根据UserId获取风险评估结果 WF 20151027
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [Route("Api/v1/RiskInfo/RiskResults")]
        [ModelValidationFilter]
        [RESTAuthorizeAttribute]
        public List<PsTreatmentIndicators> GetPsTreatmentIndicators(string UserId)
        {
            List<PsTreatmentIndicators> ret = repository.GetPsTreatmentIndicators(pclsCache, UserId);
            return ret;
        }

        /// <summary>
        /// Ps.TreatmentIndicators  SetData WF 20151027
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        [Route("Api/v1/RiskInfo/TreatmentIndicators")]

        [ModelValidationFilter]
        public HttpResponseMessage POSTPsTreatmentIndicatorsSetData(RiskResult Item)
        {
            int ret = repository.PsTreatmentIndicatorsSetData(pclsCache, Item.UserId, Item.SortNo, Item.AssessmentType, Item.AssessmentName, Item.AssessmentTime, Item.Result, Item.revUserId, Item.TerminalName, new CommonFunction().getRemoteIPAddress(), Item.DeviceType);
            return new ExceptionHandler().SetData(Request, ret);
        }

        /// <summary>
        /// Ps.Parameters  SetData WF20151027
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
         [Route("Api/v1/RiskInfo/PsParameters")]

        [ModelValidationFilter]
        public HttpResponseMessage PostPsParametersSetData(Parameters Item)
        {
            int ret = repository.PsParametersSetData(pclsCache, Item.Indicators, Item.Id, Item.Name, Item.Value, Item.Unit, Item.revUserId, Item.TerminalName, new CommonFunction().getRemoteIPAddress(), Item.DeviceType);
            return new ExceptionHandler().SetData(Request, ret);
        }

        /// <summary>
        /// 获取评估表Ps.Parameters的具体参数（对应一次评估）  GetParameters WF20151027
        /// </summary>
        /// <param name="Indicators"></param>
        /// <returns></returns>
        [Route("Api/v1/RiskInfo/Parameters")]
        [ModelValidationFilter]
        [RESTAuthorizeAttribute]
        public List<Parameters> GetParameters(string Indicators)
        {
            List<Parameters> ret = repository.GetParameters(pclsCache, Indicators);
            return ret;
        }
        /// <summary>
        /// Ps.TreatmentIndicators GetMaxSortNo
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [Route("Api/v1/RiskInfo/GetMaxSortNo")]
        [ModelValidationFilter]
        [RESTAuthorizeAttribute]
        public HttpResponseMessage GetMaxSortNo(string UserId)
        {
            int ret = repository.GetMaxSortNo(pclsCache, UserId);
            return new ExceptionHandler().Common(Request,ret.ToString());
        }

         /// <summary>
         /// 获取高血压模块评估参数所需输入 SYF 20151117
         /// </summary>
         /// <param name="UserId"></param>
         /// <returns></returns>
        [Route("Api/v1/RiskInfo/M1RiskInput")]
        public M1RiskInput GetM1RiskInput(string UserId)
        {
            return repository.GetM1RiskInput(pclsCache, UserId);
        }

         /// <summary>
        /// 获取心衰模型评估的所有输入 SYF 20151117
         /// </summary>
         /// <param name="UserId"></param>
         /// <returns></returns>
        [Route("Api/v1/RiskInfo/M3RiskInput")]
        public M3RiskInput GetM3RiskInput(string UserId)
        {
            return repository.GetM3RiskInput(pclsCache, UserId);
        }

      /// <summary>
        /// 计算风险评估 SYF 2015-11-16
      /// </summary>
      /// <param name="UserId"></param>
      /// <param name="Module"></param>
      /// <returns></returns>
        [Route("Api/v1/RiskInfo/Risk")]
        public HttpResponseMessage GetRisk(string UserId, string Module)
        {
            if (Module == "M1")
            {
                return new ExceptionHandler().toJson(repository.GetM1Risk(pclsCache, UserId));
            }
            else if(Module == "M3")
            {
                return new ExceptionHandler().toJson(repository.GetM3Risk(pclsCache, UserId));
            }
            else
            {
                return new ExceptionHandler().toJson("无对应评估模型");
            }
            
        }

         /// <summary>
         /// 新建高血压风险评估（修改参数） SYF 20151118
         /// </summary>
         /// <param name="PatientId"></param>
         /// <param name="M1RiskInput"></param>
         /// <param name="RecordDate"></param>
         /// <param name="RecordTime"></param>
         /// <param name="piUserId"></param>
         /// <param name="piTerminalName"></param>
         /// <param name="piTerminalIP"></param>
         /// <param name="piDeviceType"></param>
         /// <returns></returns>
        [Route("Api/v1/RiskInfo/AddM1Risk")]
        public M1Risk AddM1Risk(string PatientId, M1RiskInput M1RiskInput, int RecordDate, int RecordTime, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            return repository.AddM1Risk(pclsCache, PatientId, M1RiskInput, RecordDate, RecordTime, piUserId, piTerminalName, piTerminalIP, piDeviceType);
        }

        /// <summary>
        /// 新建心衰模块风险评估（修改参数） SYF 20151118
        /// </summary>
        /// <param name="PatientId"></param>
        /// <param name="M3RiskInput"></param>
        /// <param name="RecordDate"></param>
        /// <param name="RecordTime"></param>
        /// <param name="piUserId"></param>
        /// <param name="piTerminalName"></param>
        /// <param name="piTerminalIP"></param>
        /// <param name="piDeviceType"></param>
        /// <returns></returns>
        [Route("Api/v1/RiskInfo/AddM3Risk")]
        public M3Risk AddM3Risk(string PatientId, M3RiskInput M3RiskInput, int RecordDate, int RecordTime, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            return repository.AddM3Risk(pclsCache, PatientId, M3RiskInput, RecordDate, RecordTime, piUserId, piTerminalName, piTerminalIP, piDeviceType);
        }
    }
}
