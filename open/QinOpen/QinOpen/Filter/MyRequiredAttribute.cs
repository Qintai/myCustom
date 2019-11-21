using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace QinOpen
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRequiredAttribute : RequiredAttribute
    {
        public override bool RequiresValidationContext => true;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult result = new ValidationResult("");
            return result;
            //return base.IsValid(value, validationContext);
        }
        public override bool IsValid(object value)
        {
            return true;
        }
        public override string FormatErrorMessage(string name)
        {
            return "fsdfsd";
        }
    }


    public class Ae : ObjectResult
    {
        public Ae(object o) : base(o) { }

        public override void ExecuteResult(ActionContext context)
        {
            context.HttpContext.Response.WriteAsync("1111111111");
        }
        public override Task ExecuteResultAsync(ActionContext context)
        {
            return context.HttpContext.Response.WriteAsync("1111111111");
        }
        public override void OnFormatting(ActionContext context)
        {
            
        }
    }


   // public class Be : Attribute, IStatusCodeActionResult
   // {

       // public int? StatusCode => 200;



        //public  void ExecuteResult(ActionContext context)
        //{
        //    context.HttpContext.Response.WriteAsync("1111111111");
        //}
        //public override Task ExecuteResultAsync(ActionContext context)
        //{
        //    return context.HttpContext.Response.WriteAsync("1111111111");
        //}
        //public override void OnFormatting(ActionContext context)
        //{ }
   // }
}
