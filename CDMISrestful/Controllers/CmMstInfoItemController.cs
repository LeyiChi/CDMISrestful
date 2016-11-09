//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using System.Web.OData;
//using CDMISrestful.CommonLibrary;
//using CDMISrestful.DataModels;
//using CDMISrestful.Models;

//namespace CDMISrestful.Controllers
//{
//     [WebApiTracker]
//    [RESTAuthorizeAttribute]
//    public class CmMstInfoItemController : ApiController
//    {
//        static readonly ICmMstInfoItemRepository repository = new CmMstInfoItemRepository();

//        /// <summary>
//        /// 路径：有待确定
//        /// 功能：获取Cm.MstInfoItem表的所有数据
//        /// 输入：/
//        /// 输出：全表字段
//        /// 开发者：CSQ
//        /// 开发时间：20150930
//        /// 修改记录1：/
//        /// 修改记录2：/
//        /// </summary>
//        /// <returns></returns>
//        [EnableQuery(PageSize = 10)]
//        public IEnumerable<CmMstInfoItem> GetAll()
//        {
//            return repository.GetAll();
//        }

//        //public CmMstInfoItem GetCmMstInfoItem(string CategoryCode, string Code, int StartDate)
//        //{
//        //    CmMstInfoItem item = repository.Get(CategoryCode, Code, StartDate);
//        //    if (item == null)
//        //    {
//        //        throw new HttpResponseException(HttpStatusCode.NotFound);
//        //    }
//        //    return item;
//        //}

//        public IEnumerable<CmMstInfoItem> GetProductsByCategory(string CategoryCode)
//        {
//            return repository.GetAll().Where(
//                p => string.Equals(p.CategoryCode, CategoryCode, StringComparison.OrdinalIgnoreCase));
//        }

//        /// <summary>
//        /// 删除数据
//        /// </summary>
//        /// <param name="CategoryCode"></param>
//        /// <param name="Code"></param>
//        /// <param name="StartDate"></param>
//        /// <returns></returns>
//        public HttpResponseMessage Delete(string CategoryCode, string Code, int StartDate)
//        {
//            int ret = repository.Remove(CategoryCode, Code, StartDate);
//            return new ExceptionHandler().DeleteData(Request, ret);
//        }

//        /// <summary>
//        /// 更新数据
//        /// </summary>
//        /// <param name="item"></param>
//        /// <returns></returns>
//        //public HttpResponseMessage Put(CmMstInfoItem item)
//        //{
//        //    bool ret = repository.Update(item);
//        //    //return new ExceptionHandler().SetData(ret);
//        //    return ret;
//        //}

//        /// <summary>
//        /// 新增数据
//        /// </summary>
//        /// <param name="item"></param>
//        /// <returns></returns>
//        //[ModelValidationFilter]
//        //public HttpResponseMessage Post(CmMstInfoItem item)
//        //{
//        //    bool ret = repository.AddItem(item);
//        //    //return new ExceptionHandler().SetData(ret);
//        //    return ret;
//        //}
//    }
//}
