using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;

namespace crud.Web.Filter
{
    /// <summary>
    /// 没用上
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CostomProducesAttribute : Attribute, IResultFilter, IFilterMetadata, IOrderedFilter, IApiResponseMetadataProvider
    {
        public CostomProducesAttribute(Type type)
        {
            if (type == (Type)null)
                throw new ArgumentNullException(nameof(type));
            this.Type = type;
            this.ContentTypes = new MediaTypeCollection();
        }

        public CostomProducesAttribute(string contentType, params string[] additionalContentTypes)
        {
            if (contentType == null)
                throw new ArgumentNullException("contentType");
            MediaTypeHeaderValue.Parse(contentType);
            for (int i = 0; i < additionalContentTypes.Length; i++)
            {
                MediaTypeHeaderValue.Parse(additionalContentTypes[i]);
            }
            ContentTypes = GetContentTypes(contentType, additionalContentTypes);
        }

    
        public Type Type { get; set; }

        public MediaTypeCollection ContentTypes { get; set; }

        public int StatusCode
        { get { return 200; } }

        public int Order { get; set; }

        public virtual void OnResultExecuting(ResultExecutingContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (!(context.Result is ObjectResult result))
                return;
            for (int index = 0; index < context.Filters.Count; ++index)
            {
                switch ((context.Filters[index] as IFormatFilter)?.GetFormat((ActionContext)context))
                {
                    case null:
                        continue;
                    default:
                        return;
                }
            }
            this.SetContentTypes(result.ContentTypes);

            if (result.ContentTypes?.First()== "application/javascript")
            {
                if (context.RouteData.Values.Count > 0)
                {
                    string val = context.RouteData.Values["callback"].ToString();
                    var rr = ((ObjectResult)context.Result).Value;
                    rr = $"{val}({rr})";
                    MediaTypeCollection mm = new MediaTypeCollection();
                    mm.Add("application/javascript");
                    ObjectResult objectResult = new ObjectResult(rr);
                    objectResult.ContentTypes = mm;
                    context.Result = objectResult;
                }
            }

        }

        public virtual void OnResultExecuted(ResultExecutedContext context)
        { }

 
        public void SetContentTypes(MediaTypeCollection contentTypes)
        {
            contentTypes.Clear();
            foreach (string contentType in (Collection<string>)this.ContentTypes)
                contentTypes.Add(contentType);
        }

        private MediaTypeCollection GetContentTypes(string firstArg, string[] args)
        {
            List<string> stringList = new List<string>();
            stringList.Add(firstArg);
            stringList.AddRange((IEnumerable<string>)args);
            MediaTypeCollection mediaTypeCollection = new MediaTypeCollection();
            foreach (string mediaType in stringList)
            {
                if (new MediaType(mediaType).HasWildcard)
                {
                    //通配符号错误
                    continue;
                }
                mediaTypeCollection.Add(mediaType);
            }
            return mediaTypeCollection;
        }

    }
}