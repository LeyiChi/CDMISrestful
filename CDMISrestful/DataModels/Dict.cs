using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CDMISrestful.DataModels
{
    public class Dict
    {
    }

    public class LifeStyleDetail
    {
        public string StyleId{get;set;}
        public string Name{get;set;}
        public string CurativeEffect{get;set;}
        public string SideEffect{get;set;}
        public string Instruction{get;set;}
        public string HealthEffect{get;set;}
        public string Unit{get;set;}
        public string Redundance{get;set;}        
    }

    public class Insurance
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string InputCode { get; set; }
        public string Redundance { get; set; }
        //public string InvalidFlag { get; set; }
    }

    public class MstInfoItemByCategoryCode
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }
        public int SortNo { get; set; }
        public int GroupHeaderFlag { get; set; }
        public string ControlType { get; set; }
        public string OptionCategory { get; set; }

    }

    public class CmAbsType
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
        public string InputCode { get; set; }
        public int SortNo { get; set; }
        public string Redundance { get; set; }
        public int InvalidFlag { get; set; }
    }

    public class CmMstTaskSetData
    {

        [Required(ErrorMessage = "请传入CategoryCode")]
        public string CategoryCode { get; set; }
        [Required(ErrorMessage = "请输入Code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "请输入Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "请输入ParentCode")]
        public string ParentCode { get; set; }
        [Required(ErrorMessage = "请输入Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "请输入StartDate")]
        public int StartDate { get; set; }
        [Required(ErrorMessage = "请输入EndDate")]
        public int EndDate { get; set; }
        [Required(ErrorMessage = "请输入GroupHeaderFlag")]
        public int GroupHeaderFlag { get; set; }
        [Required(ErrorMessage = "请输入ControlType")]
        public int ControlType { get; set; }
        [Required(ErrorMessage = "请输入OptionCategory")]
        public string OptionCategory { get; set; }
        [Required(ErrorMessage = "请输入revUserId")]
        public string revUserId { get; set; }
        [Required(ErrorMessage = "请输入TerminalName")]
        public string TerminalName { get; set; }
        [Required(ErrorMessage = "请输入TerminalIP")]
        public string TerminalIP { get; set; }
        [Required(ErrorMessage = "请输入DeviceType")]
        public int DeviceType { get; set; }
    }
    public class CmMstTask
    {
        public string CategoryCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }
        public string Description { get; set; }
        public int StartDate { get; set; }
        public int EndDate { get; set; }
        public int GroupHeaderFlag { get; set; }
        public int ControlType { get; set; }
        public string OptionCategory { get; set; }
      
    }

    public class CmMonitorMethod
    {
        public string TypeName { get; set; }
        public string Name { get; set; }
        public string Method { get; set; }
        public string Description { get; set; }
        public int SortNo { get; set; }
        public string Redundance { get; set; }

    }

}