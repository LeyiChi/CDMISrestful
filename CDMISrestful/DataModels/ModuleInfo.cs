using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace CDMISrestful.DataModels
{
    public class ModuleInfo
    {
    }
    public class PatDetailInfo
    {
        public string PhoneNumber { get; set; }
        public string HomeAddress { get; set; }
        public string Occupation { get; set; }
        public string Nationality { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyContactPhoneNumber { get; set; }
        public string PhotoAddress { get; set; }
        public string IDNo { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
    }
    public class PatBasicInfoDetail
    {
        public string UserId { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ParentCode { get; set; }
        public int ItemSeq { get; set; }
        public string Value { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public int SortNo { get; set; }
        public int ControlType { get; set; }
        public string OptionCategory { get; set; }
    }
    public class DocInfoDetail
    {
        public string IDNo { get; set; }
        public string PhotoAddress { get; set; }
        public string UnitName { get; set; }
        public string JobTitle { get; set; }
        public string Level { get; set; }
        public string Dept { get; set; }
        public string ActivatePhotoAddr { get; set; }

        public string DeptName { get; set; }

    }
    public class SynBasicInfo
    {
        public List<NewExam> ExamInfo { get; set; }
        public List<NewLabTest> LabTestInfo { get; set; }
    }

    public class BasinInfoDetail 
    {
        public string Patient { get; set; }
        public string CategoryCode { get; set; }
        public string ItemCode { get; set; }
        public int ItemSeq { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int SortNo { get; set; }
        public string revUserId { get; set; }
        public string TerminalName { get; set; }
        public string TerminalIP { get; set; }
        public int DeviceType { get; set; }
    }
}