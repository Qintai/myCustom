using System;
using SqlSugar;

namespace QinEntity
{
    /// <summary>
    ///  用户和文章关系
    /// </summary>
    public class UserArticlePO  
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 性别
        /// </summary>
        public string  Gender { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 文章名称
        /// </summary>
        public string ArticleName { get; set; }

        /// <summary>
        /// 文章的游览量
        /// </summary>
        public string ArticlePV { get; set; }
    }
}