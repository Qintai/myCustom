using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace QinOpen
{
    /// <summary>
    /// 通用返回信息类
    /// </summary>
    public class MessageModel   /*: ClientErrorData*/
    {
        /// <summary>
        /// 返回错误信息
        /// </summary>
        /// <param name="err"></param>
        /// <returns></returns>
        public static MessageModel Fail(ref MessageModel _msg ,string err) 
        {
            _msg.Msg = err;
            _msg.Success = false;
            return _msg;
        }

        /// <summary>
        /// 返回成功信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static MessageModel Ok(ref MessageModel _msg, string str)
        {
            _msg.Msg = str;
            _msg.Success = true;
            return _msg;
        }


        public MessageModel() { }

        /// <summary>
        /// 返回模型绑定的错误
        /// </summary>
        /// <param name="modelState"></param>
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
