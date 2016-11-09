using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CDMISrestful.DataModels;
using CDMISrestful.CommonLibrary;

namespace CDMISrestful.Models
{
    public interface IDictRepository
    {
        List<TypeAndName> GetAllDivisionType(DataConnection pclsCache);
        List<TypeAndName> GetDivisionDeptList(DataConnection pclsCache, string Type);
        List<TypeAndName> GetHypertensionDrugTypeNameList(DataConnection pclsCache);
        List<CmAbsType> GetHypertensionDrug(DataConnection pclsCache) ;
        List<TypeAndName> GetDiabetesDrugTypeNameList(DataConnection pclsCache);
        List<CmAbsType> GetDiabetesDrug(DataConnection pclsCache);
        List<TypeAndName> GetTypeList(DataConnection pclsCache, string Category);
        List<MstBloodPressure> GetBloodPressure(DataConnection pclsCache);
        List<Insurance> GetInsuranceType(DataConnection pclsCache);
        int CmMstTaskSetData(DataConnection pclsCache, string CategoryCode, string Code, string Name, string ParentCode, string Description, int StartDate, int EndDate, int GroupHeaderFlag, int ControlType, string OptionCategory, string revUserId, string TerminalName, string TerminalIP, int DeviceType);
        List<CmMstTask> GetMstTaskByParentCode(DataConnection pclsCache, string ParentCode);
        string GetNo(DataConnection pclsCache, int NumberingType, string TargetDate);

        CmMonitorMethod GetMonitorMethodData(DataConnection pclsCache, string Type, string Code);

    }
}