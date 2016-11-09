using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CDMISrestful.DataModels
{
    public class SetVitalInfo
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "请传入用户Id")]
        public string UserId { get; set; }
        /// <summary>
        /// 记录日期
        /// </summary>
        [Required(ErrorMessage = "请输入记录日期")]
        public string RecordDate { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        [Required(ErrorMessage = "请输入记录时间")]
        public string RecordTime { get; set; }
        /// <summary>
        /// 记录类型
        /// </summary>
        [Required(ErrorMessage = "请输入体征大类")]
        public string ItemType { get; set; }
        /// <summary>
        /// 记录项目
        /// </summary>
        [Required(ErrorMessage = "请输入体征名称")]
        public string ItemCode { get; set; }
        /// <summary>
        /// 项目数据
        /// </summary>
        [Required(ErrorMessage = "请输入体征值")]
        public string Value { get; set; }
        /// <summary>
        /// 项目单位
        /// </summary>
          [Required(ErrorMessage = "请输入体征值单位")]
        public string Unit { get; set; }    
        public string revUserId { get; set; }
        public string TerminalName { get; set; }
        public string TerminalIP { get; set; }
        public int DeviceType { get; set; }
    }

    public class VitalInfo
    {
        /// <summary>
        /// 用户Id
        /// </summary>

        public string UserId { get; set; }
        /// <summary>
        /// 记录日期
        /// </summary>

        public string RecordDate { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>

        public string RecordTime { get; set; }
        /// <summary>
        /// 记录类型
        /// </summary>

        public string ItemType { get; set; }
        /// <summary>
        /// 记录项目
        /// </summary>

        public string ItemCode { get; set; }
        /// <summary>
        /// 项目数据
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 项目单位
        /// </summary>
        public string Unit { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public string SignType { get; set; }
    }

    /// <summary>
    /// 高血压体征详细-日期段
    /// </summary>
    public class SignDetailByP
    {
        /// <summary>
        /// 需要查询起始的时间
        /// </summary>
        public int NextStartDate { get; set; }
        public List<SignDetailByD> SignDetailByDs { get; set; }
        public SignDetailByP()
        {
            SignDetailByDs = new List<SignDetailByD>();
        }
    }

    /// <summary>
    /// 高血压体征详细-某天
    /// </summary>
    public class SignDetailByD
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 星期几
        /// </summary>
        public string WeekDay { get; set; }
        public List<SignDetail> SignDetailList { get; set; }
        public SignDetailByD()
        {
            SignDetailList = new List<SignDetail>();
        }
    }

    /// <summary>
    /// 高血压体征详细-到分 
    /// </summary>
    public class SignDetail
    {
        /// <summary>
        /// 详细时间 到时分
        /// </summary>
        public string DetailTime { get; set; }
        /// <summary>
        /// 收缩压
        /// </summary>
        public string SBPValue { get; set; }
        /// <summary>
        /// 舒张压
        /// </summary>
        public string DBPValue { get; set; }
        /// <summary>
        /// 脉率  "78"
        /// </summary>
        public string PulseValue { get; set; }
        public SignDetail()
        {
            SBPValue = "";
            DBPValue = "";
            PulseValue = "";
        }
    }

    public class VitalSignFromDevice
    {
        public string ubid { get; set; }
        public string mobilephone { get; set; }
        public string sex { get; set; }
        public int age { get; set; }
        public string birthday { get; set; }
        public DailyInfos dailyinfos { get; set; }
    }

    public class DailyInfos
    {
        public string date { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
        public string remark { get; set; }
        public List<BloodPressureInfos> bloodpressureinfos { get; set; }
        public List<BloodSugarInfos> bloodsugarinfos { get; set; }
        public List<ECGInfos> ecginfos { get; set; }
        public List<BreatheInfos> breatheinfos { get; set; }
    }

    public class BloodPressureInfos
    {
        public string time { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public int source { get; set; }
    }

    public class BloodSugarInfos
    {
        public string time { get; set; }
        public int type { get; set; }
        public string glu { get; set; }
        public int source { get; set; }
    }

    public class ECGInfos
    {
        public string time { get; set; }
        public string bpm { get; set; }
        public int source { get; set; }
    }

    public class BreatheInfos
    {
        public string time { get; set; }
        public double oximetry { get; set; }
    }

    public class ValueTime
    {
        public string Value { get; set; }
        public string RecordDate { get; set; }

        public string RecordTime { get; set; }
    }
}