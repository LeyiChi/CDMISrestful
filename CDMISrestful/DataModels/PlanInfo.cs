using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CDMISrestful.CommonLibrary;
using InterSystems.Data.CacheClient;
using System.ComponentModel.DataAnnotations;

namespace CDMISrestful.DataModels
{
    public class ComplianceDate
    {
        public int Date { get; set; }
        public string PlanNo { get; set; }
        public double Compliance { get; set; }
    }

    public class DeleteTask
    {
        public string PlanNo { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string SortNo { get; set; }

    }

    public class TaskDetail
    {
        public string Module { get; set; }
        public string CurativeEffect { get; set; }
        public string SideEffect { get; set; }
        public string Instruction { get; set; }
        public string HealthEffect { get; set; }
        public string Unit { get; set; }
        public string Redundance { get; set; }
    }

    public class Template
    {
        public string DoctorId { get; set; }
        public int TemplateCode { get; set; }
        public string TemplateName { get; set; }
        public string Description { get; set; }
        public DateTime RecordDate { get; set; }
        public string Redundance { get; set; }
        public string piUserId { get; set; }
        public string piTerminalName { get; set; }
        public string piTerminalIP { get; set; }
        public int piDeviceType { get; set; }
    }

    public class TemplateInfo
    {
        public int TemplateCode { get; set; }
        public string TemplateName { get; set; }
        public string Description { get; set; }
        public DateTime RecordDate { get; set; }
        public string Redundance { get; set; }
    }


    public class TemplateInfoDtl
    {
        public string CategoryCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string InvalidFlag { get; set; }
        public string Value { get; set; }
        public string TemplateDescription { get; set; }
        public string Redundance { get; set; }
        public string ParentCode { get; set; }
        public string TaskDescription { get; set; }
        public string GroupHeaderFlag { get; set; }
        public string ControlType { get; set; }
        public string OptionCategory { get; set; }
    }

    public class TemplateDetail
    {
        public string DoctorId { get; set; }
        public int TemplateCode { get; set; }
        public string CategoryCode { get; set; }
        public string ItemCode { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Redundance { get; set; }

        public string piUserId { get; set; }
        public string piTerminalName { get; set; }
        public string piTerminalIP { get; set; }
        public int piDeviceType { get; set; }
    }

    public class Period
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DayCount { get; set; }
    }

    public class Progressrate
    {
        public string RemainingDays { get; set; }
        public string ProgressRate { get; set; }
    }

    public class PlanDeatil
    {
        /// <summary>
        /// 计划名称  “计划”+序号+起止时间  用于计划列表的显示
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// 计划编码
        /// </summary>
        public string PlanNo { get; set; }

        /// <summary>
        /// 起始日期
        /// </summary>
        public int StartDate { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        public int EndDate { get; set; }
    }

    public class SetPlanInfo
    {
        [Required]
        public string PlanNo { get; set; }
        public string PatientId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Module { get; set; }
        [Required]
        public string Status { get; set; }
        public string DoctorId { get; set; }
        public string piUserId { get; set; }
        public string piTerminalName { get; set; }
        public string piTerminalIP { get; set; }
        public int piDeviceType { get; set; }
    }
    public class GPlanInfo
    {
        [Required]
        public string PlanNo { get; set; }
        public string PlanName { get; set; }
        public string PatientId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Module { get; set; }
        [Required]
        public string Status { get; set; }

        public string PlanCompliance { get; set; }

        public string RemainingDays { get; set; }
        public string ProgressRate { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string piUserId { get; set; }
        public string piTerminalName { get; set; }
        public string piTerminalIP { get; set; }
        public int piDeviceType { get; set; }
    }

    public class ComplianceDetail
    {
        [Required]
        public string PlanNo { get; set; }
        [Required]
        public int Date { get; set; }
        public string CategoryCode { get; set; }
        public string Code { get; set; }
        public string SortNo { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
  
        public string piUserId { get; set; }
        public string piTerminalName { get; set; }
        public string piTerminalIP { get; set; }
        public int piDeviceType { get; set; }
    }
    public class SetComplance
    {
        [Required]
        public string PlanNo { get; set; }
        [Required]
        public int Date { get; set; }
        [Required]
        public double Compliance { get; set; }
        public string Description { get; set; }
        public string piUserId { get; set; }
        public string piTerminalName { get; set; }
        public string piTerminalIP { get; set; }
        public int piDeviceType { get; set; }
    }

    public class CreateTask
    {
        [Required]
        public string PlanNo { get; set; }
        public string Type{ get; set; }
        public string Code{ get; set; }
        public string SortNo{ get; set; }
        public string Instruction{ get; set; }
        public string piUserId { get; set; }
        public string piTerminalName { get; set; }
        public string piTerminalIP { get; set; }
        public int piDeviceType { get; set; }
    }

    public class PatientPlan
    {
        public string PatientId { get; set; }
        public string PlanNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TotalDays { get; set; }
        public string RemainingDays { get; set; }
        public string Status { get; set; }
    }

    public class TasksComList
    {
        public string Date { get; set; }
        public string ComplianceValue { get; set; }
        public string TaskType { get; set; }
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string Status { get; set; }
        public string TaskCode { get; set; }
        public string Type { get; set; }
    }

    public class TasksComByPeriodDT
    {
        public string Date { get; set; }
        public string ComplianceValue { get; set; }
        public string TaskType { get; set; }
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
    }

    public class TasksByDate
    {
        public string TaskID { get; set; }
        public string TaskName { get; set; }
        public string Status { get; set; }
    }

    public class TasksByStatus
    {
         public string Id { get; set; }
        public string Status { get; set; }
        public string TaskCode { get; set; }
        public string TaskName { get; set; }
        public string TaskType { get; set; }
        public string Instruction { get; set; }
    }

    public class SignDetailByPeriod
    {
         public string RecordDate { get; set; }
        public string RecordTime { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
    }

    public class ComplianceListByPeriod
    {
        public int Date { get; set; }
        public double Compliance { get; set; }
        public string Description { get; set; }
    }
   

    public class PsTaskByType
    { 
        public string Id {get; set;}
        public string Code {get; set;}
        public string CodeName {get; set;}
        public string Instruction {get; set;}
     
    }
    public class PsTask
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string SortNo { get; set; }
        public string Name { get; set; }
        public string InvalidFlag { get; set; }
        public string Status { get; set; }
        public string Instruction { get; set; }
        public string ParentCode { get; set; }
        public string Description { get; set; }
        public string GroupHeaderFlag { get; set; }
        public string ControlType { get; set; }
        public string OptionCategory { get; set; }
        public string VitalSignValue { get; set; }

    }
    public class TargetByCode
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Origin { get; set; }
        public string Instruction { get; set; }
        public string Unit { get; set; }
        [Required]
        public string Plan { get; set; }
        public string piUserId { get; set; }
        public string piTerminalName { get; set; }
        public string piTerminalIP { get; set; }
        public int piDeviceType { get; set; }
    }

    public class OverDuePlanDetail
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string PhotoAddress { get; set; }
        public string PlanNo { get; set; }
        public string StartDate { get; set; }
        public double Process { get; set; }
        public string RemainingDays { get; set; }
        public List<string> VitalSign { get; set; }
    }

    public class MstBloodPressure
    {
        public string Code { get; set; }                //编码  
        public string Name { get; set; }               //名称：很高、偏高、警戒、正常
        public string Description { get; set; }       //描述
        public int SBP { get; set; }              //收缩压
        public int DBP { get; set; }             //舒张压
        public string PatientClass { get; set; }       //患者类型
        public string Redundance { get; set; }        //冗余
    }

    public class Graph         //图的主要点数据
    {
        //日期
        public string Date { get; set; }       //日期，到天
        //图-测量任务，体征数据部分
        public string SignValue { get; set; }          //Y值
        public string SignGrade { get; set; }          //Y值级别  暂时只用来确定颜色，后期可作文字显示 “偏高、很高等”
        public string SignColor { get; set; }         //点颜色  
        public string SignShape { get; set; }         //点形状  
        public string SignDescription { get; set; }    //点的气球文本   样式——日期  <br> 收缩压/舒张压 mmHg  <br> 脉率 次/分
        //图-其他任务依从情况（包括用药、生活方式等）
        public string DrugValue { get; set; }         //画在下部图，保持Y=1
        public string DrugBullet { get; set; }       //客制化颜色 用图片-部分完成 "amcharts-images/drug.png" 半白半黑图片
        public string DrugColor { get; set; }        //药的其他颜色-完全未完成、完成	
        public string DrugDescription { get; set; }       //任务依从情况描述 "部分完成；未吃:阿司匹林、青霉素；已吃：钙片、板蓝根"  使用叉勾图标
        //暂时用不到的
        //public string BPBullet { get; set; }       //客制化血压点 "amcharts-images/star.png"
        //public string timeDetail { get; set; }    //最新测试的具体时间，到min
        public Graph()
        {
            //初始化  初始化为无记录状态，还是未完成任务状态？
            //暂时未完成任务状态  因为从PsCompliance取出那天，最初默认的就是未完成任务！
            //SignValue = "";  //string默认初始化为""，所以不需要再赋值
            //SignGrade = "";
            // SignColor = "";
            // SignShape = "";
            //SignDescription = "";
            DrugValue = "";            //可能没有用药或其他任务
            DrugBullet = "";         //初始化  时间肯定有  默认所有任务（生理测量、用药）为未完成任务状态
            DrugColor = "";  //白色 "#FFFFFF"
            DrugDescription = "";  //可能无任务，也可能任务未完成 不确定状态  "无记录";
        }
    }

    /// <summary>
    /// 任务依从情况
    /// </summary>

    public class CompliacneDetailByD
    {
        public string Date { get; set; }
        public string drugBullet { get; set; }
        public string drugColor { get; set; }
        public string Events { get; set; }
    }

    /// <summary>
    /// 某天任务依从情况详细 用于弹框显示该天全部详情  目前按类别来分
    /// </summary>
    public class TaskComDetailByD
    {
        public string Date { get; set; }
        public string WeekDay { get; set; }
        public string ComplianceValue { get; set; }
        /// <summary>
        /// 体征测量
        /// </summary>
        public List<VitalTaskCom> VitalTaskComList { get; set; }
        /// <summary>
        /// 生活方式和用药情况的共同集合类
        /// </summary>
        public List<TaskComByType> TaskComByTypeList { get; set; }
        public TaskComDetailByD()
        {
            VitalTaskComList = new List<VitalTaskCom>();

            TaskComByTypeList = new List<TaskComByType>();
        }
    }

    /// <summary>
    /// 任务详细依从情况类  适用于体征测量和用药情况 
    /// </summary>
    public class TaskComByType
    {
        /// <summary>
        /// 任务类型：体征测量 生活方式 用药情况   
        /// </summary>
        public string TaskType { get; set; }
        public List<TaskCom> TaskComList { get; set; }
        public TaskComByType()
        {
            TaskComList = new List<TaskCom>();
        }
    }

    /// <summary>
    /// 任务详细依从情况类
    /// </summary>
    public class TaskCom
    {
        /// <summary>
        /// 体征名称 药物名称 生活方式名称  
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 是否完成 对应勾叉
        /// </summary>
        public string TaskStatus { get; set; }
    }

    /// <summary>
    /// 体征任务详细情况类
    /// </summary>
    public class VitalTaskCom
    {
        /// <summary>
        /// 是否完成 对应勾叉
        /// </summary>
        public string Status { get; set; }
        public string Time { get; set; }
        public string SignName { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
    }

    /// <summary>
    /// Pad和Web版通用  区别：Web显示计划的任务列表，Pad不
    /// </summary>
    public class ImplementationInfo
    {
        public PatientInfo1 PatientInfo { get; set; }          //病人基本信息-姓名、头像
        public string ProgressRate { get; set; }              //此计划的进度
        public string RemainingDays { get; set; }            //此计划的剩余天数
        public string CompliacneValue { get; set; }         //此计划 最近一周的依从率 或 整个计划依从率
        public int StartDate { get; set; }                //计划的时间起始，作为体征切换的时间输入
        public int EndDate { get; set; }

        //是否有其他任务（除体征测量之外）的标志位？   
        //public string OtherTasks { get; set; }    // 1有 0没有
        public List<PlanDeatil> PlanList { get; set; }     //所有计划列表(正在实施中的 和 已经结束的)
        public List<Task> TaskList { get; set; }          //此计划的任务列表
        public ChartData ChartData { get; set; }        //画图数据-血压、脉率值，分级情况，依从情况
        public List<SignShow> SignList { get; set; }          //图上体征切换显示
        public ImplementationInfo()
        {
            PatientInfo = new PatientInfo1();          //初始化
            PlanList = new List<PlanDeatil>();
            TaskList = new List<Task>();
            ChartData = new ChartData();
            SignList = new List<SignShow>();
        }

    }

    /// <summary>
    /// Phone版
    /// </summary>
    public class ImplementationPhone
    {
        public string NowPlanNo { get; set; }       //正在执行的计划编号 ""则无
        public string ProgressRate { get; set; }       //进度
        public string RemainingDays { get; set; }       //剩余天数
        public string CompliacneValue { get; set; }       //最近一周的依从率
        public ChartData ChartData { get; set; }
        public int StartDate { get; set; }    //最近一周的时间起始，作为血压详细查看（时刻）的时间输入
        public int EndDate { get; set; }
        public List<SignShow> SignList { get; set; }          //图上体征切换显示
        public ImplementationPhone()
        {
            ChartData = new ChartData();
            SignList = new List<SignShow>();
        }
    }

    /// <summary>
    /// 病人基本信息
    /// </summary>
    public class PatientInfo1
    {
        public string PatientName { get; set; } //姓名
        public string ImageUrl { get; set; }   //头像
    }

    /// <summary>
    /// 画图数据集合
    /// </summary>
    public class ChartData
    {
        public List<Graph> GraphList { get; set; }   //图：点
        public GraphGuide GraphGuide { get; set; }  //图：血。压分级区域和最大最小值 种类：收缩压、舒张压
        //是否有其他任务（除体征测量之外）的标志位？   
        public string OtherTasks { get; set; }    // 1有 0没有
        public ChartData()
        {
            GraphList = new List<Graph>();
            GraphGuide = new GraphGuide();
            OtherTasks = "0";
        }
    }



    /// <summary>
    /// 血压分级区域和最大最小值
    /// </summary>
    public class GraphGuide
    {
        public List<Guide> GuideList { get; set; }   //血压分级区域
        public string original { get; set; }      //初始值
        public string target { get; set; }        //目标值
        public double minimum { get; set; }       //Y值下限
        public double maximum { get; set; }       //Y值上限
        public GraphGuide()
        {
            GuideList = new List<Guide>();
        }
    }

    /// <summary>
    /// 图的区域划分-风险分级、目标线、初始线    目标线、初始线 字体、线密度不同  分级区域颜色不同，文字不同
    /// </summary>
    public class Guide
    {
        //变量-来自数据库
        public string value { get; set; }       //值或起始值
        public string toValue { get; set; }       //终值或""
        public string label { get; set; }        //中文定义 目标线、偏低、偏高等

        //恒量-根据图设定
        public string lineColor { get; set; }       //直线颜色  目标线  初始线
        public string lineAlpha { get; set; }       //直线透明度 0全透~1
        public string dashLength { get; set; }       //虚线密度  4  8

        public string color { get; set; }            //字体颜色
        public string fontSize { get; set; }       //字体大小  默认14
        public string position { get; set; }       //字体位置 right left
        public string inside { get; set; }        //坐标系的内或外  false

        public string fillAlpha { get; set; }       //区域透明度
        public string fillColor { get; set; }       //
        //public string balloonText { get; set; }       //气球弹出框   

    }

    /// <summary>
    /// 某任务的详细属性
    /// </summary>
    public class TaskDeatil
    {
        public string TaskType { get; set; }
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string Instruction { get; set; }

    }

    /// <summary>
    /// 某类型任务的集合
    /// </summary>
    public class Task
    {
        public string TaskType { get; set; }
        public List<TaskDeatil> TaskDeatilList { get; set; }
        public Task()
        {
            TaskDeatilList = new List<TaskDeatil>();
        }
    }

    /// <summary>
    /// 图上体征切换显示
    /// </summary>
    public class SignShow
    {
        public string SignName { get; set; }
        public string SignCode { get; set; }
    }
    public class ComplianceAllSignsListByPeriod
    {
        public string Date { get; set; }
        public string Compliance { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string Task { get; set; }
        public string BulletValue { get; set; }
        public string BulletColor { get; set; }
        public string CustomBullet { get; set; }
        public string VitalCode { get; set; }

        //public string BloodPressure_1 { get; set; }
        //public string BloodPressure_2 { get; set; }
        //public string Pulserate_1 { get; set; }
    }

    public class SignByPeriod
    {
        public string RecordDate { get; set; }
        public string RecordTime { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
    }

    public class TasksForClickDtl
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }

    public class TasksForClick
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public List<TasksForClickDtl> SubTasks { get; set; }

        public TasksForClick()
        {
            SubTasks = new List<TasksForClickDtl>();
        }
    }

    public class CalendarSetData
    {
        public string DoctorId { get; set; }
        public int DateTime { get; set; }

        public string Period { get; set; }
        public int SortNo { get; set; }

        public string Description { get; set; }
        public int Status { get; set; }

        public string Redundancy { get; set; }
        public string revUserId { get; set; }

        public string TerminalName { get; set; }
        public string TerminalIP { get; set; }

        public int DeviceType { get; set; }
    }

    public class LogPlan
    {
        public string PlanNo { get; set; }
        public string Edition { get; set; }

        public string PatientId { get; set; }
        public string StartDate { get; set; }

        public string EndDate { get; set; }
        public string Module { get; set; }

        public string Status { get; set; }
        public string DoctorId { get; set; }

    }

    public class LogTask:IComparable
    {

        public string TaskName { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }



        public string SortNo { get; set; }
        public string Edition { get; set; }

        public string Instruction { get; set; }

        public string PlanEndDate { get; set; }

        public class StudentListEquality : IEqualityComparer<LogTask>
        {
            public bool Equals(LogTask x, LogTask y)
            {
                if((x.Type==y.Type) && (x.Code==y.Code) && (x.Instruction==y.Instruction) && (x.PlanEndDate==y.PlanEndDate) )
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }

            public int GetHashCode(LogTask obj)
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return obj.ToString().GetHashCode();
                }
            }
        }  

       public int CompareTo(object obj) 
       {
            int result;
            try
            {
                LogTask info = obj as LogTask;
                return string.Compare(this.Code, info.Code);
            }
            catch (Exception ex)
            { 
                 throw new Exception(ex.Message); 
            }
        }

    }

    public class ItemCompliance
    {
        public string CategoryCode { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }
        public int Date { get; set; }

        public string Status { get; set; }

    }

    public class TaskCompliance
    {
        public string CategoryCode { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }
        public int AllDays { get; set; }

        public int DoDays { get; set; }

        public int UndoDays { get; set; }

        public string Instruction { get; set; }

    }

    public class CmMstTaskN
    {
        public string Name { get; set; }
        public string ParentCode { get; set; }

        public string Description { get; set; }
        public int GroupHeaderFlag { get; set; }

        public int ControlType { get; set; }

        public string DateTime { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

    }

}