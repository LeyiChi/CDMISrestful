using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Query.Validators;
using Microsoft.OData.Core;
using Microsoft.OData.Core.UriParser;



namespace CDMISrestful.CommonLibrary
{
    public class QueryValidation
    {
    }

    //demo
    public class MyOrderByValidator : OrderByQueryValidator
    {
        // Disallow the 'desc' parameter for $orderby option.
        public override void Validate(OrderByQueryOption orderByOption,
                                        ODataValidationSettings validationSettings)
        {
            if (orderByOption.OrderByNodes.Any(
                    node => node.Direction == OrderByDirection.Descending))
            {
                throw new ODataException("The 'desc' option is not supported.");
            }
            base.Validate(orderByOption, validationSettings);
        }
    }

    //demo
    public class MyQueryableAttribute : EnableQueryAttribute
    {
        public override void ValidateQuery(HttpRequestMessage request,
            ODataQueryOptions queryOptions)
        {
            if (queryOptions.OrderBy != null)
            {
                queryOptions.OrderBy.Validator = new MyOrderByValidator();
            }
            base.ValidateQuery(request, queryOptions);
        }
    }

    #region example
    //    // Globally:
    //config.EnableQuerySupport(new MyQueryableAttribute());

    //// Per controller:
    //public class ValuesController : ApiController
    //{
    //    [MyQueryable]
    //    public IQueryable<Product> Get()
    //    {
    //        return products.AsQueryable();
    //    }
    //}
    #endregion

}