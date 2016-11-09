using CDMISrestful.CommonLibrary;
using CDMISrestful.DataModels;
using CDMISrestful.DataViewModels;
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
    public class ClinicInfoController : ApiController
    {
        static readonly IClinicInfoRepository repository = new ClinicInfoRepository();
        DataConnection pclsCache = new DataConnection();
        /// <summary>
        /// 获取目前最新Num条临床数据
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="AdmissionDate"></param>
        /// <param name="ClinicDate"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        [Route("Api/v1/ClinicInfo/GetClinicalNewMobile")]
        [ModelValidationFilter]
        
        public Clinic GetClinicalNewMobile(string UserId, DateTime AdmissionDate, DateTime ClinicDate, int Num)
        {
            Clinic ret = repository.GetClinicalNewMobile(pclsCache,UserId, AdmissionDate, ClinicDate, Num);
            return ret;
        }
        /// <summary>
        /// 获取临床大类信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Type"></param>
        /// <param name="VisitId"></param>
        /// <param name="Date"></param>
        /// <returns></returns>
        [Route("Api/v1/ClinicInfo/GetClinicInfoDetail")]
        [ModelValidationFilter]

        //public HttpResponseMessage LogOn(string PwType, string username, string password, string role)
        public ClinicInfoViewModel GetClinicInfoDetail(string UserId, string Type, string VisitId, string Date)
        {
            ClinicInfoViewModel ret = repository.GetClinicInfoDetail(pclsCache, UserId, Type, VisitId, Date);
            return ret;
        }
        /// <summary>
        /// 获取化验参数列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="VisitId"></param>
        /// <param name="SortNo"></param>
        /// <returns></returns>
        [Route("Api/v1/ClinicInfo/GetLabTestDtlList")]
        [ModelValidationFilter]

        //public HttpResponseMessage LogOn(string PwType, string username, string password, string role)
        public List<LabTestDetails> GetLabTestDtlList(string UserId, string VisitId, string SortNo)
        {
            List<LabTestDetails> ret = repository.GetLabTestDtlList(pclsCache, UserId, VisitId, SortNo);
            return ret;
        }
        /// <summary>
        /// 获取患者的就诊信息 Table:Ps.InPatientInfo和Ps.OutPatientInfo Author:WF 2015-10-28"
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [Route("Api/v1/ClinicInfo/GetClinicalInfoList")]
        [ModelValidationFilter]

        //public HttpResponseMessage LogOn(string PwType, string username, string password, string role)
        public ClinicalInfoListViewModel GetClinicalInfoList(string UserId)
        {
            ClinicalInfoListViewModel ret = repository.GetClinicalInfoList(pclsCache, UserId);
            return ret;
        }
        /// <summary>
        /// 获取患者在就诊医院的就诊号 Table：Ps.UserIdMatch Author:WF  2015-10-28
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="HospitalCode"></param>
        /// <returns></returns>
        [Route("Api/v1/ClinicInfo/GetLatestHUserIdByHCode")]
        [ModelValidationFilter]

        //public HttpResponseMessage LogOn(string PwType, string username, string password, string role)
        public HttpResponseMessage GetLatestHUserIdByHCode(string UserId, string HospitalCode)
        {
            string ret = repository.getLatestHUserIdByHCode(pclsCache, UserId, HospitalCode);
            return new ExceptionHandler().Common(Request,ret);
        }
        /// <summary>
        /// 已有症状列表 Table:Ps.Symptoms  Author:WF 2015-10-28
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="VisitId"></param>
        /// <returns></returns>
        [Route("Api/v1/ClinicInfo/GetSymptomsList")]
        [ModelValidationFilter]

        //public HttpResponseMessage LogOn(string PwType, string username, string password, string role)
        public List<SymptomsList> GetSymptomsList(string UserId, string VisitId)
        {
            List<SymptomsList> ret = repository.GetSymptomsList(pclsCache, UserId, VisitId);
            return ret;
        }
        /// <summary>
        /// 根据患者Id，获取诊断信息 Table:Ps.Diagnosis Author:WF 2015-10-28
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="VisitId"></param>
        /// <returns></returns>
        [Route("Api/v1/ClinicInfo/GetDiagnosisInfoList")]
        [ModelValidationFilter]

        //public HttpResponseMessage LogOn(string PwType, string username, string password, string role)
        public List<DiagnosisInfo> GetDiagnosisInfoList(string UserId, string VisitId)
        {
            List<DiagnosisInfo> ret = repository.GetDiagnosisInfoList(pclsCache, UserId, VisitId);
            return ret;
        }
        /// <summary>
        /// 根据患者Id,获取患者的检查信息  Table:Ps.Examination  Author:WF  2015-10-28
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="VisitId"></param>
        /// <returns></returns>
        [Route("Api/v1/ClinicInfo/GetExaminationList")]
        [ModelValidationFilter]

        //public HttpResponseMessage LogOn(string PwType, string username, string password, string role)
        public List<ExamInfo> GetExaminationList(string UserId, string VisitId)
        {
            List<ExamInfo> ret = repository.GetExaminationList(pclsCache, UserId, VisitId);
            return ret;
        }
        /// <summary>
        /// 获取检查参数列表 Table:Ps.ExamDetails  Author:WF 2015-10-28
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="VisitId"></param>
        /// <param name="SortNo"></param>
        /// <param name="ItemCode"></param>
        /// <returns></returns>
        [Route("Api/v1/ClinicInfo/GetExamDtlList")]
        [ModelValidationFilter]

        //public HttpResponseMessage LogOn(string PwType, string username, string password, string role)
        public List<ExamDetails> GetExamDtlList(string UserId, string VisitId, string SortNo, string ItemCode)
        {
            List<ExamDetails> ret = repository.GetExamDtlList(pclsCache, UserId, VisitId, SortNo, ItemCode);
            return ret;
        }
        /// <summary>
        /// 根据患者Id, 获取患者的化验信息 Table:Ps.LabTest  Author:WF  2015-10-28
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="VisitId"></param>
        /// <returns></returns>
        [Route("Api/v1/ClinicInfo/GetLabTestList")]
        [ModelValidationFilter]

        //public HttpResponseMessage LogOn(string PwType, string username, string password, string role)
        public List<LabTestList> GetLabTestList(string UserId, string VisitId)
        {
            List<LabTestList> ret = repository.GetLabTestList(pclsCache, UserId, VisitId);
            return ret;
        }

        [Route("Api/v1/ClinicInfo/GetDrugRecordList")]
        [ModelValidationFilter]
        //public HttpResponseMessage LogOn(string PwType, string username, string password, string role)
        public List<DrugRecordList> GetDrugRecordList(string UserId, string VisitId)
        {
            List<DrugRecordList> ret = repository.GetDrugRecordList(pclsCache, UserId, VisitId);
            return ret;
        }
    }
}
