//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using CDMISrestful.DataModels;
//using CDMISrestful.Models;
//using CDMISrestful.CommonLibrary;

//namespace CDMISrestful.Controllers
//{
//     [WebApiTracker]
//    [RESTAuthorizeAttribute]
//    public class ProductsController : ApiController
//    {
//        //在控制器中调用new ProductRepository()不是最好的设计
//        //因为它把控制器绑定到了IProductRepository的一个特定实现上了
//        //更好的办法参见“使用Web API依赖性解析器”。
//        static readonly IProductRepository repository = new ProductRepository();
       
//        public IEnumerable<Product> GetAllProducts()
//        {
//            return repository.GetAll();
//        }

//        public Product GetProduct(int id)
//        {
//            Product item = repository.Get(id);
//            if (item == null)
//            {
//                throw new HttpResponseException(HttpStatusCode.NotFound);
//            }
//            return item;
//        }

//        public IEnumerable<Product> GetProductsByCategory(string category)
//        {
//            return repository.GetAll().Where(
//                p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
//        }

//        public HttpResponseMessage PostProduct(Product item)
//        {
//            //需要添加模型验证
//            item = repository.Add(item);
//            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);
//            string uri = Url.Link("DefaultApi", new { id = item.Id });
//            response.Headers.Location = new Uri(uri);
//            return response;
//        }

//        public void PutProduct(int id, Product product)
//        {
//            product.Id = id;
//            if (!repository.Update(product))
//            {
//                throw new HttpResponseException(HttpStatusCode.NotFound);
//            }
//        }
       
//        public HttpResponseMessage DeleteProduct(int id)
//        {
//            repository.Remove(id);
//            return new HttpResponseMessage(HttpStatusCode.NoContent);
//        }


//    }
//}
