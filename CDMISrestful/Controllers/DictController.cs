using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using CDMISrestful.CommonLibrary;
using CDMISrestful.DataModels;
using CDMISrestful.Models;

namespace CDMISrestful.Controllers
{
     [WebApiTracker]
    [RESTAuthorizeAttribute]
    public class DictController : ApiController
    {
        static readonly IDictRepository repository = new DictRepository();
        DataConnection pclsCache = new DataConnection();
        /// <summary>
        /// 获取高血压药物类型名称列表 LY 2015-10-13
        /// </summary>
        /// <returns></returns>
        [Route("Api/v1/Dict/HypertensionDrug/TypeNames")]
        public List<TypeAndName> GetHypertensionDrugTypeNameList()
        {
            return repository.GetHypertensionDrugTypeNameList(pclsCache);
        }

        /// <summary>
        /// 获取高血压药物名称列表 LY 2015-10-13
        /// </summary>
        /// <returns></returns>
        [Route("Api/v1/Dict/HypertensionDrug")]
        [EnableQuery]
        public List<CmAbsType> GetHypertensionDrug()
        {
            return repository.GetHypertensionDrug(pclsCache);
        }

        /// <summary>
        /// 获取糖尿病药物类型名称列表 LY 2015-10-13
        /// </summary>
        /// <returns></returns>
        [Route("Api/v1/Dict/DiabetesDrug/TypeNames")]
        public List<TypeAndName> GetDiabetesDrugTypeNameList()
        {
            return repository.GetDiabetesDrugTypeNameList(pclsCache);
        }

        /// <summary>
        /// 获取糖尿病药物名称列表 LY 2015-10-13
        /// </summary>
        /// <returns></returns>
        [Route("Api/v1/Dict/DiabetesDrug")]
        [EnableQuery]
        public List<CmAbsType> GetDiabetesDrug()
        {
            return repository.GetDiabetesDrug(pclsCache);
            //return ExceptionHandler.toJson(ret);

        }

        /// <summary>
        /// 获取某个分类的类别 LY 2015-10-13
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        [Route("Api/v1/Dict/Type/{Category}")]
        public List<TypeAndName> GetTypeList(string Category)
        {
            return repository.GetTypeList(pclsCache,Category);
        }


        /// <summary>
        /// 获取血压等级字典表的所有信息 LY 2015-10-13
        /// </summary>
        /// <returns></returns>
        [Route("Api/v1/Dict/BloodPressure")]
        [EnableQuery]
        public List<MstBloodPressure> GetBloodPressure()
        {
            return repository.GetBloodPressure(pclsCache);
        }

        /// <summary>
        /// GetInsuranceType
        /// </summary>
        /// <returns></returns>
        [Route("Api/v1/Dict/GetInsuranceType")]
        [ModelValidationFilter]
        public List<Insurance> GetInsuranceType()
        {
            List<Insurance> ret = repository.GetInsuranceType(pclsCache);
            return ret;
        }
        [Route("Api/v1/Dict/CmMstTaskSetData")]
        [ModelValidationFilter]
        [RESTAuthorizeAttribute]
        public HttpResponseMessage CmMstTaskSetData(CmMstTaskSetData CmMstTaskSetData)
        {
            int ret = repository.CmMstTaskSetData(pclsCache, CmMstTaskSetData.CategoryCode, CmMstTaskSetData.Code, CmMstTaskSetData.Name, CmMstTaskSetData.ParentCode, CmMstTaskSetData.Description, CmMstTaskSetData.StartDate, CmMstTaskSetData.EndDate, CmMstTaskSetData.GroupHeaderFlag, CmMstTaskSetData.ControlType, CmMstTaskSetData.OptionCategory, CmMstTaskSetData.revUserId, CmMstTaskSetData.TerminalName, new CommonFunction().getRemoteIPAddress(), CmMstTaskSetData.DeviceType);
            return new ExceptionHandler().SetData(Request, ret);
        }
        [Route("Api/v1/Dict/MstTask")]
        [ModelValidationFilter]
        [RESTAuthorizeAttribute]
        public List<CmMstTask> GetMstTaskByParentCode(string ParentCode)
        {
            List<CmMstTask> ret = repository.GetMstTaskByParentCode(pclsCache, ParentCode);
            return ret;
        }
        [Route("Api/v1/Dict/GetNo")]
        [ModelValidationFilter]
        public HttpResponseMessage GetNo(int NumberingType, string TargetDate)
        {
            string ret = repository.GetNo(pclsCache, NumberingType, TargetDate);
            return new ExceptionHandler().Common(Request,ret);
        }

        /// <summary>
        /// 获取科室所有Type和TypeName字段 SYF 20151109
        /// </summary>
        /// <returns></returns>
        [Route("Api/v1/Dict/DivisionTypes")]
        [ModelValidationFilter]
        public List<TypeAndName> GetAllDivisionType()
        {
            return repository.GetAllDivisionType(pclsCache);
        }
        /// <summary>
        /// 根据Type获取科室Code和Name SYF 20151109
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        [Route("Api/v1/Dict/Divisions")]
        [ModelValidationFilter]
        public List<TypeAndName> GetDivisionDeptList(string Type)
        {
            return repository.GetDivisionDeptList(pclsCache, Type);
        }
    
        /// <summary>
        /// 根据Type和Code获取CmMonitorMethod表中其他字段 syf 20160114
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        [Route("Api/v1/Dict/MonitorMethod")]
        [ModelValidationFilter]
        public CmMonitorMethod GetMonitorMethodData(string Type, string Code)
        {
            return repository.GetMonitorMethodData(pclsCache, Type, Code);
        }

    }
}
