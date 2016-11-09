using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CDMISrestful.DataModels;

namespace CDMISrestful.DataViewModels
{
    public class ClinicInfoViewModel
    {
        public List<DiagnosisInfo> DiagnosisInfo_DataViewModel { get; set; }
        public List<ExamInfo> ExamInfo_DataViewModel { get; set; }
        public List<LabTestList> LabTestList_DataViewModel { get; set; }
        public List<DrugRecordList> DrugRecordList_DataViewModel { get; set; }

        public ClinicInfoViewModel()
        {
            DiagnosisInfo_DataViewModel = new List<DiagnosisInfo>();
            ExamInfo_DataViewModel = new List<ExamInfo>();
            LabTestList_DataViewModel = new List<LabTestList>();
            DrugRecordList_DataViewModel = new List<DrugRecordList>();
        }

    }
    public class ClinicalInfoListViewModel
    {
        public List<PsInPatInfo> DT_InPatientInfo { get; set; }
        public List<PsOutPatInfo> DT_OutPatientInfo { get; set; }

        public ClinicalInfoListViewModel()
        {
            DT_InPatientInfo = new List<PsInPatInfo>();
            DT_OutPatientInfo = new List<PsOutPatInfo>();
           
        }

    }
}