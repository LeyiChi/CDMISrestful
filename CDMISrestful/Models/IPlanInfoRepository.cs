using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CDMISrestful.DataModels;
using CDMISrestful.CommonLibrary;

namespace CDMISrestful.Models
{
    public interface IPlanInfoRepository
    {
        int SetPlan(DataConnection pclsCache, string PlanNo, string PatientId, int StartDate, int EndDate, string Module, int Status, string DoctorId, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType);
        int SetComplianceDetail(DataConnection pclsCache, string PlanNo, int Date, string CategoryCode, string Code, string SortNo, int Status, string Description, string CoUserId, string CoTerminalName, string CoTerminalIP, int CoDeviceType);

        //List<PsDrugRecord> GetPatientDrugRecord(DataConnection pclsCache, string PatientId, string Module);
        int CreateTask(DataConnection pclsCache, string PlanNo, string Type, string Code, string SortNo, string Instruction, string UserId, string TerminalName, string TerminalIP, int DeviceType);

        int SetCompliance(DataConnection pclsCache, string PlanNo, int Date, Double Compliance, string Description, string revUserId, string TerminalName, string TerminalIP, int DeviceType);
        int SetTarget(DataConnection pclsCache, string Plan, string Type, string Code, string Value, string Origin, string Instruction, string Unit, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType);
        GPlanInfo GetPlanInfo(DataConnection pclsCache, string PlanNo);
        TaskComDetailByD GetImplementationByDate(DataConnection pclsCache, string PatientId, string PlanNo, string DateSelected);
        ChartData GetSignInfoByCode(DataConnection pclsCache, string PatientId, string PlanNo, string ItemCode, int StartDate, int EndDate);
        int SetPlanStart(DataConnection pclsCache, string PlanNo, int Status, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType);
        //ImplementationInfo GetImplementationForPadFirst(string PatientId, string Module);
        //ImplementationInfo GetImplementationForPadSecond(string PatientId, string PlanNo);
        List<OverDuePlanDetail> GetOverDuePlanList(DataConnection pclsCache, string DoctorId, string ModuleType);
        //string GetPlanInfobyPID(string PatientId);
        //ImplementationPhone GetImplementationForPhone(string PatientId, string Module);

        //List<GPlanInfo> GetPlanList34ByM(DataConnection pclsCache, string PatientId, string Module);
        List<PsTask> GetTasks(DataConnection pclsCache, string PlanNo, string ParentCode, string Date, string PatientId);
        List<TaskDetail> GetTaskDetails(DataConnection pclsCache, string CategoryCode, string Code);
        int PsTemplateSetData(DataConnection pclsCache, string DoctorId, int TemplateCode, string TemplateName, string Description, DateTime RecordDate, string Redundance, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType);
        int PsTemplateDetailSetData(DataConnection pclsCache, string DoctorId, int TemplateCode, string CategoryCode, string ItemCode, string Value, string Description, string Redundance, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType);
        List<TemplateInfo> GetTemplateList(DataConnection pclsCache, string DoctorId);
        List<TemplateInfoDtl> GetTemplateDetails(DataConnection pclsCache, string DoctorId, string TemplateCode, string ParentCode);
        TargetByCode GetTarget(DataConnection pclsCache, string PlanNo, string Type, string Code);

        //List<ComplianceAllSignsListByPeriod> GetComplianceAllSignsListByPeriod(string UserId, string PlanNo, int StartDate, int EndDate);
        List<ComplianceAllSignsListByPeriod> GetComplianceAllSignsListByPeriod(DataConnection pclsCache, string UserId, string PlanNo, int StartDate, int EndDate, string ItemType, string ItemCode);

        int DeteteTask(DataConnection pclsCache, string Plan, string Type, string Code, string SortNo);

        List<GPlanInfo> GetPlanListByMS(DataConnection pclsCache, string PatientId, string Module, int Status);
        List<TasksForClick> GetTasksForClick(DataConnection pclsCache, string PlanNo, string ParentCode, string Date);
        List<ComplianceDate> GetComplianceListInC(DataConnection pclsCache, string PatientId, string StartDate, string EndDate, string Module);

        List<LogTask> GetDTaskByPlanNo(DataConnection pclsCache, string PlanNo);

        //LogPlan GetLogPlanInfo(DataConnection pclsCache, string PlanNo);

        //List<LogTask> GetAllTaskByLogPlan(DataConnection pclsCache, string PlanNo, int piEdition);

        List<TaskCompliance> GetTaskCompliance(DataConnection pclsCache, string PlanNo);

       
    }
}