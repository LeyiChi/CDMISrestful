using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CDMISrestful.DataModels
{
    public class ClinicInfo
    {

    }
    //public class GetClinicalNewMobile
    //{

    //    [Required(ErrorMessage = "请传入UserId")]
    //    public string UserId { get; set; }
    //    [Required(ErrorMessage = "请输入AdmissionDate")]
    //    //[RegularExpression(@"/^[-_A-Za-z0-9]+@([_A-Za-z0-9]+\.)+[A-Za-z0-9]{2,3}$/" @"/^1[3|4|5|7|8][0-9]\d{4,8}$/)]
    //    public DateTime AdmissionDate { get; set; }
    //    [Required(ErrorMessage = "请输入ClinicDate")]
    //    public DateTime ClinicDate { get; set; }
    //    [Required(ErrorMessage = "请输入Num")]
    //    public int Num { get; set; }
    //}

    //public class GetClinicInfoDetail
    //{

    //    [Required(ErrorMessage = "请传入UserId")]
    //    public string UserId { get; set; }
    //    [Required(ErrorMessage = "请输入Type")]
    //    //[RegularExpression(@"/^[-_A-Za-z0-9]+@([_A-Za-z0-9]+\.)+[A-Za-z0-9]{2,3}$/" @"/^1[3|4|5|7|8][0-9]\d{4,8}$/)]
    //    public string Type { get; set; }
    //    [Required(ErrorMessage = "请输入VisitId")]
    //    public string VisitId { get; set; }
    //    [Required(ErrorMessage = "请输入Date")]
    //    public string Date { get; set; }
    //}
    //public class GetLabTestDtlList
    //{

    //    [Required(ErrorMessage = "请传入UserId")]
    //    public string UserId { get; set; }
    //    [Required(ErrorMessage = "请输入VisitId")]
    //    //[RegularExpression(@"/^[-_A-Za-z0-9]+@([_A-Za-z0-9]+\.)+[A-Za-z0-9]{2,3}$/" @"/^1[3|4|5|7|8][0-9]\d{4,8}$/)]
    //    public string VisitId { get; set; }
    //    [Required(ErrorMessage = "请输入SortNo")]
    //    public string SortNo { get; set; }

    //}
    public class ClinicalTrans
    {
        public DateTime 精确时间 { get; set; }
        public string 类型 { get; set; }
        public string VisitId { get; set; }
        public string 事件 { get; set; }
        public string 关键属性 { get; set; }
    }
    public class ClinicalTemp
    {
        public int SortNo { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime DisChargeDate { get; set; }
        public string HospitalName { get; set; }
        public string DepartmentName { get; set; }
    }
    public class DiagnosisInfo
    {
        public string VisitId { get; set; }
        public string DiagnosisType { get; set; }
        public string DiagnosisTypeName { get; set; }
        public string DiagnosisNo { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public string DiagnosisCode { get; set; }
        public string DiagnosisName { get; set; }
        public string Description { get; set; }
        public string RecordDate { get; set; }
        public string RecordDateShow { get; set; }
        public string Creator { get; set; }
        public string RecordDateCom { get; set; }
    }

    public class ExamInfo
    {
        public string VisitId { get; set; }
        public string SortNo { get; set; }
        public string ExamType { get; set; }
        public string ExamTypeName { get; set; }
        public string ExamDate { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ExamPara { get; set; }
        public string Description { get; set; }
        public string Impression { get; set; }
        public string Recommendation { get; set; }
        public string IsAbnormalCode { get; set; }
        public string IsAbnormal { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string ReqortDate { get; set; }
        public string ImageURL { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string Creator { get; set; }
        public string ExamDateCom { get; set; }
    }

    public class LabTestList
    {
        public string VisitId { get; set; }
        public string SortNo { get; set; }
        public string LabItemType { get; set; }
        public string LabItemTypeName { get; set; }
        public string LabItemCode { get; set; }
        public string LabItemName { get; set; }
        public string LabTestDate { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string ReportDate { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string Creator { get; set; }
        public string LabTestDateCom { get; set; }
    }

    public class DrugRecordList
    {
        public string VisitId { get; set; }
        public string OrderNo { get; set; }
        public string OrderSubNo { get; set; }
        public string RepeatIndicatorCode { get; set; }
        public string RepeatIndicator { get; set; }
        public string OrderClassCode { get; set; }
        public string OrderClass { get; set; }
        public string OrderCode { get; set; }
        public string OrderContent { get; set; }
        public string Dosage { get; set; }
        public string DosageUnitsCode { get; set; }
        public string DosageUnits { get; set; }
        public string AdministrationCode { get; set; }
        public string Administration { get; set; }
        public string StartDateTime { get; set; }
        public string StopDateTime { get; set; }
        public string Frequency { get; set; }
        public string FreqCounter { get; set; }
        public string FreqInteval { get; set; }
        public string FreqIntevalUnitCode { get; set; }
        public string FreqIntevalUnit { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string Creator { get; set; }
        public string StartDateTimeCom { get; set; }
    }

    public class DrugRecord
    {
        public string VisitId { get; set; }
        public string OrderNo { get; set; }
        public string OrderSubNo { get; set; }
        public string RepeatIndicatorCode { get; set; }
        public string RepeatIndicator { get; set; }
        public string OrderClassCode { get; set; }
        public string OrderClass { get; set; }
        public string OrderCode { get; set; }
        public string OrderContent { get; set; }
        public string Dosage { get; set; }
        public string DosageUnitsCode { get; set; }
        public string DosageUnits { get; set; }
        public string AdministrationCode { get; set; }
        public string Administration { get; set; }
        public string StartDateTime { get; set; }
        public string StopDateTime { get; set; }
        public string Frequency { get; set; }
        public string FreqCounter { get; set; }
        public string FreqInteval { get; set; }
        public string FreqIntevalUnitCode { get; set; }
        public string FreqIntevalUnit { get; set; }
        public string HistoryContent { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }
    public class PsDrugRecord
    {
        public string VisitId { get; set; }
        public string OrderNo { get; set; }
        public string OrderSubNo { get; set; }
        public string RepeatIndicator { get; set; }
        public string OrderClass { get; set; }
        public string OrderCode { get; set; }
        public string DrugName { get; set; }
        public string CurativeEffect { get; set; }
        public string SideEffect { get; set; }
        public string Instruction { get; set; }
        public string HealthEffect { get; set; }
        public string Unit { get; set; }
        public string OrderContent { get; set; }
        public string Dosage { get; set; }
        public string DosageUnits { get; set; }
        public string Administration { get; set; }
        public string StartDateTime { get; set; }
        public string StopDateTime { get; set; }
        public string Frequency { get; set; }
    }

    /// <summary>
    /// 读取最新的检查信息，用于慢病信息页面问卷的填写
    /// </summary>
    public class NewExam
    {
        public string Name1 { get; set; }
        public string Value1 { get; set; }
        public string Name2 { get; set; }
        public string Value2 { get; set; }
        public string Name3 { get; set; }
        public string Value3 { get; set; }

        public string Date { get; set; }

    }

    /// <summary>
    /// 读取最新的化验信息，用于慢病信息页面问卷的填写
    /// </summary>
    public class NewLabTest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public DateTime Date { get; set; }
    }


    public class Clinic
    {
        public string UserId { get; set; }

        public List<ClinicBasicInfoandTime> History { get; set; }

        public string AdmissionDateMark { get; set; }

        public string ClinicDateMark { get; set; }

        public string mark_contitue { get; set; }

        public Clinic()
        {
            History = new List<ClinicBasicInfoandTime>();
        }
    }
    public class ClinicBasicInfoandTime
    {
        public string Time { get; set; }       //时间轴左侧日期：年月日  

        public List<SomeDayEvent> ItemGroup { get; set; }     //时间轴右侧事件集 

        public string Tag { get; set; }   //标签：入院、出院、住院中、转科、门诊、急诊    右侧事件右上标

        public string Color { get; set; }   //颜色：主要按照标签Tag赋颜色，但应避免同一天多个标签，导致颜色不定的情况-默认解决方案：取第一种

        public string VisitId { get; set; }

        public ClinicBasicInfoandTime()
        {
            ItemGroup = new List<SomeDayEvent>();
        }

    }
    public class SomeDayEvent
    {
        public string Time { get; set; }         //时间 某天下的 时分 截取自 精确时间
        public string Type { get; set; }         //类型
        public string Event { get; set; }        //事件
        public string KeyCode { get; set; }         //关键主键（用于查看详细）
    }
    public class DT_Clinical_All
    {
        public DateTime 精确时间 { get; set; }     //精确时间
        public string 时间 { get; set; }            //时间
        public string 类型 { get; set; }            //类型
        public string VisitId { get; set; }
        public string 事件 { get; set; }          //事件
        public string 关键属性 { get; set; }            //关键属性
    }
    public class LabTestDetails
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int IsAbnormalCode { get; set; }
        public string IsAbnormal { get; set; }
        public string UnitCode { get; set; }
        public string Unit { get; set; }
        public string Creator { get; set; }

    }
    public class PsInPatInfo
    {
        public string VisitId { get; set; }
        public string SortNo { get; set; }
        public string AdmissionDate { get; set; }
        public string DischargeDate { get; set; }
        public string HospitalCode { get; set; }
        public string HospitalName { get; set; }
        public string Department { get; set; }
        public string DepartmentName { get; set; }
        public string Doctor { get; set; }
        public string Creator { get; set; }

    }
    public class PsOutPatInfo
    {
        public string VisitId { get; set; }
        public string ClinicDate { get; set; }
        public string HospitalCode { get; set; }
        public string HospitalName { get; set; }
        public string Department { get; set; }
        public string DepartmentName { get; set; }
        public string Doctor { get; set; }
        public string Creator { get; set; }

    }
    public class SymptomsList
    {
        public string VisitId { get; set; }
        public string SynptomsNo { get; set; }
        public string SymptomsType { get; set; }
        public string SymptomsTypeName { get; set; }
        public string SymptomsCode { get; set; }
        public string SymptomsName { get; set; }
        public string Description { get; set; }
        public string RecordDate { get; set; }
        public string RecordTime { get; set; }
        public string Creator { get; set; }

    }
    public class ExamDetails
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int IsAbnormalCode { get; set; }
        public string IsAbnormal { get; set; }
        public string UnitCode { get; set; }
        public string Unit { get; set; }
        public string Creator { get; set; }

    }
}