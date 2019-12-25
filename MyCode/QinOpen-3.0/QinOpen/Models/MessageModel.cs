using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace QinOpen
{
    /// <summary>
    /// 通用返回信息类
    /// </summary>
    public class MessageModel   /*: ClientErrorData*/
    {

        public MessageModel() { }

        public MessageModel(ModelStateDictionary modelState)
        {
            //IEnumerable<dynamic> dy = modelState.Keys.SelectMany(key => modelState[key].Errors
            //.Select(x => new { x.ErrorMessage }));

            List<string> str = new List<string>();
            foreach (ModelStateEntry entry in modelState.Values)
                foreach (ModelError err in entry.Errors)
                    str.Add(err.ErrorMessage);

            Msg = string.Join(",", str);
        }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// 返回信息
        /// </summary>
        private string _msg;
        public string Msg
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_msg))
                    return "操作成功！";
                return _msg;
            }
            set => _msg = value;
        }

        /// <summary>
        /// 返回状态码
        /// </summary>
        public string Code { get; set; } = "200";

        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }

    }
}
