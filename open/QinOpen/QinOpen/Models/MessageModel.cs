using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QinOpen
{
    /// <summary>
    /// 通用返回信息类
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool success { get; set; } = false;
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; } = "服务器异常";

        /// <summary>
        /// 返回状态码
        /// </summary>
        public string code { get; set; } = "200";

        /// <summary>
        /// 返回数据
        /// </summary>
        public object data { get; set; }

    }
}
