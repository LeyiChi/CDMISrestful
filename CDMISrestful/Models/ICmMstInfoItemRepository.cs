using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CDMISrestful.DataModels;
using CDMISrestful.CommonLibrary;

namespace CDMISrestful.Models
{
    public interface ICmMstInfoItemRepository
    {
        //IEnumerable<CmMstInfoItem> GetAll(DataConnection pclsCache);
        //CmMstInfoItem Get(string CategoryCode, string Code, int StartDate);
        //bool AddItem(DataConnection pclsCache, CmMstInfoItem item);
        int Remove(DataConnection pclsCache, string CategoryCode, string Code, int StartDate);
        //bool Update(CmMstInfoItem item);
     
    }
}