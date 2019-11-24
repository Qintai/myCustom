using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QinOpen.Filter
{
    /// <summary>
    /// 验证错误的类型
    /// </summary>
    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string Message { get; }

        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }
    }

    public class ValidationResultModel
    {
        public string Message { get; }

        public List<ValidationError> Errors { get; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            #region MyRegion
            //string errmsg = "";
            //if (!modelState.IsValid)
            //{
            //    foreach (var item in modelState.Values)
            //    {
            //        foreach (var error in item.Errors)
            //        {
            //            errmsg += error.ErrorMessage + "|";
            //        }
            //    }
            //}
            #endregion
            Message = "Validation Failed";
            Errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
        }
    }

    public class ValidationFailedResult : ObjectResult
    {
        //public ValidationFailedResult(ModelStateDictionary modelState)
        //       : base(new ValidationResultModel(modelState))
        //{
        //    StatusCode = StatusCodes.Status422UnprocessableEntity;
        //}

        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(new MessageModel(modelState))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }



}
