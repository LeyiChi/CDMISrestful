using CDMISrestful.CommonLibrary;
using CDMISrestful.DataModels;
using CDMISrestful.DataViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDMISrestful.Models
{
    public interface IClinicInfoRepository
    {
        Clinic GetClinicalNewMobile(DataConnection pclsCache, string UserId, DateTime AdmissionDate, DateTime ClinicDate, int Num);
        ClinicInfoViewModel GetClinicInfoDetail(DataConnection pclsCache, string UserId, string Type, string VisitId, string Date);
        List<LabTestDetails> GetLabTestDtlList(DataConnection pclsCache, string UserId, string VisitId, string SortNo);
        ClinicalInfoListViewModel GetClinicalInfoList(DataConnection pclsCache, string UserId);
        string getLatestHUserIdByHCode(DataConnection pclsCache, string UserId, string HospitalCode);
        List<SymptomsList> GetSymptomsList(DataConnection pclsCache, string UserId, string VisitId);
        List<DiagnosisInfo> GetDiagnosisInfoList(DataConnection pclsCache, string UserId, string VisitId);
        List<ExamInfo> GetExaminationList(DataConnection pclsCache, string UserId, string VisitId);
        List<ExamDetails> GetExamDtlList(DataConnection pclsCache, string UserId, string VisitId, string SortNo, string ItemCode);
        List<LabTestList> GetLabTestList(DataConnection pclsCache, string UserId, string VisitId);
        List<DrugRecordList> GetDrugRecordList(DataConnection pclsCache, string UserId, string VisitId);

    }
}
