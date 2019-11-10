using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Core;

namespace crud_web.Custom.Filter
{
    public class CostomProducesAttribute : Attribute, IResultFilter, IFilterMetadata, IOrderedFilter,
        IApiResponseMetadataProvider
    {
        #region MyRegion

        /*public CostomProducesAttribute(Type type, string ContextType, params string[] additionalContentTypes)
       : base(type)
   {
       if (ContextType == null)
           throw new ArgumentNullException("contentType");

       MediaTypeHeaderValue.Parse(StringSegment.op_Implicit(ContextType));
       for (int i = 0; i < additionalContentTypes.Length; i++)
       {
           MediaTypeHeaderValue.Parse(StringSegment.op_Implicit(additionalContentTypes[i]));
       }
       base.ContentTypes = base.GetContentTypes(ContextType, additionalContentTypes);
   }

   public override void OnResultExecuting(ResultExecutingContext context)
   {

       base.OnResultExecuting(context);
   }*/

        #endregion

        public MediaTypeCollection ContentTypes { get; set; }

        public int Order { get; set; }

        public Type Type { get; set; }

        public int StatusCode => 200;

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            ObjectResult objectResult;
            if ((objectResult = (context.Result as ObjectResult)) == null) return;
            for (int i = 0; i < context.Filters.Count; i++)
            {
                if ((context.Filters[i] as IFormatFilter)?.GetFormat(context) != null) return;
            }

            MediaTypeCollection mm = new MediaTypeCollection();
            mm.Add("application/javascript");
            objectResult.ContentTypes = mm;

            SetContentTypes(objectResult.ContentTypes);
        }

        public void SetContentTypes(MediaTypeCollection contentTypes)
        {
            contentTypes.Clear();
            foreach (string contentType in ContentTypes) contentTypes.Add(contentType);
        }
    }
}