using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

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
    //public class ae : ValidationException
    //{
    //    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    //    {

    //        base.GetObjectData(info, context);
    //    }
    //}
}
