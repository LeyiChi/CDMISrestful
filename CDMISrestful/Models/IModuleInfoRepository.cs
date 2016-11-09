using CDMISrestful.CommonLibrary;
using CDMISrestful.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDMISrestful.Models
{
    public interface IModuleInfoRepository
    {
        List<PatBasicInfoDetail> GetItemInfoByPIdAndModule(DataConnection pclsCache, string UserId, string CategoryCode);

        List<MstInfoItemByCategoryCode> GetMstInfoItemByCategoryCode(DataConnection pclsCache, string CategoryCode);
        SynBasicInfo SynBasicInfoDetail(DataConnection pclsCache, string UserId, string Module);
    }
}
