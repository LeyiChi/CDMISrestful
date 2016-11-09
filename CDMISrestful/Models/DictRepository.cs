using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CDMISrestful.CommonLibrary;
using CDMISrestful.DataMethod;
using CDMISrestful.DataModels;

namespace CDMISrestful.Models
{
    public class DictRepository : IDictRepository
    {
      
        DictMethod dictMethod = new DictMethod();
        /// <summary>
        /// 获取高血压药物类型名称列表 LY 2015-10-14
        /// </summary>
        /// <returns></returns>
        public List<TypeAndName> GetHypertensionDrugTypeNameList(DataConnection pclsCache)
        {
            return dictMethod.CmMstHypertensionDrugGetTypeList(pclsCache);
        }

        /// <summary>
        /// 获取高血压药物名称列表 LY 2015-10-14
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public List<CmAbsType> GetHypertensionDrug(DataConnection pclsCache)
        {
            return dictMethod.GetHypertensionDrug(pclsCache);
            
        }

        /// <summary>
        /// 获取糖尿病药物类型名称列表 LY 2015-10-14
        /// </summary>
        /// <returns></returns>
        public List<TypeAndName> GetDiabetesDrugTypeNameList(DataConnection pclsCache)
        {
            return dictMethod.CmMstDiabetesDrugGetTypeList(pclsCache);
        }

        /// <summary>
        /// 获取糖尿病药物名称列表 LY 2015-10-14
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public List<CmAbsType> GetDiabetesDrug(DataConnection pclsCache)
        {
            return dictMethod.GetDiabetesDrug(pclsCache);
            
        }

        /// <summary>
        /// 获取某个分类的类别 LY 2015-10-14
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        public List<TypeAndName> GetTypeList(DataConnection pclsCache, string Category)
        {
            return dictMethod.CmMstTypeGetTypeList(pclsCache, Category);
        }

        /// <summary>
        /// 获取血压等级字典表的所有信息 LY 2015-10-13
        /// </summary>
        /// <returns></returns>
        public List<MstBloodPressure> GetBloodPressure(DataConnection pclsCache)
        {
            return new PlanInfoMethod().GetBPGrades(pclsCache);
        }

        public List<Insurance> GetInsuranceType(DataConnection pclsCache)
        {
            try
            {
                return dictMethod.GetInsurance(pclsCache);
            }
            catch (Exception ex)
            {
                HygeiaComUtility.WriteClientLog(HygeiaEnum.LogType.ErrorLog, "GetInsuranceType", "WebService调用异常！ error information : " + ex.Message + Environment.NewLine + ex.StackTrace);
                return null;
                throw (ex);
            }
        }

        public int CmMstTaskSetData(DataConnection pclsCache, string CategoryCode, string Code, string Name, string ParentCode, string Description, int StartDate, int EndDate, int GroupHeaderFlag, int ControlType, string OptionCategory, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            try
            {
                int IsSaved = 2;
                IsSaved = dictMethod.CmMstTaskSetData(pclsCache, CategoryCode, Code, Name, ParentCode, Description, StartDate, EndDate, GroupHeaderFlag, ControlType, OptionCategory, revUserId, TerminalName, TerminalIP, DeviceType);
                return IsSaved;
            }
            catch (Exception ex)
            {
                return 0;
                throw (ex);
            }
        }
        public List<CmMstTask> GetMstTaskByParentCode(DataConnection pclsCache, string ParentCode)
        {
            return dictMethod.GetMstTaskByParentCode(pclsCache, ParentCode);

        }

        public string GetNo(DataConnection pclsCache, int NumberingType, string TargetDate)
        {
            if(TargetDate == "{TargetDate}")
            {
                TargetDate = "";
            }
            return dictMethod.GetNo(pclsCache, NumberingType, TargetDate);
        }

        public List<TypeAndName> GetAllDivisionType(DataConnection pclsCache)
        {
            return dictMethod.GetAllDivisionType(pclsCache);
        }

        public List<TypeAndName> GetDivisionDeptList(DataConnection pclsCache, string Type)
        {
            return dictMethod.GetDivisionDeptList(pclsCache, Type);
        }

        public CmMonitorMethod GetMonitorMethodData(DataConnection pclsCache, string Type, string Code)
        {
            return dictMethod.GetMonitorMethodData(pclsCache, Type, Code);
        }

    }
}