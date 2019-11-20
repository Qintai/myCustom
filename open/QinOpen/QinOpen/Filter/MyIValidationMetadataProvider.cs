using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Localization;
using QinCommon.HttpContextUser;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QinOpen.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class MyIValidationMetadataProvider : IValidationMetadataProvider
    {
        //IHttpContextAccessor _accessor;
        //public MyIValidationMetadataProvider(IHttpContextAccessor accessor)
        //{
        //    _accessor = accessor;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            HttpContextHelper.hc.Response.WriteAsync("432", encoding: Encoding.UTF8);
 
        }
    }

    public class eae : IValidationAttributeAdapterProvider
    {
        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            throw new NotImplementedException();
        }
    }
}
