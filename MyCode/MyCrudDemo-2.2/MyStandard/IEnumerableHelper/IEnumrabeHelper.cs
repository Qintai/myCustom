using System;
using System.Collections.Generic;
using System.Text;

namespace MyStandard
{
    static class IEnumerableHelper
    {
        /// <summary>
        /// 填充时间范围内 日期空数据(按照天来计算) 
        /// </summary>
        public static IEnumerable<T> Filldate<T>(this List<T> list, string strTime, string endTime, Func<DateTime, List<T>, List<T>> entity)
        {
            DateTime starttime = Convert.ToDateTime(Convert.ToDateTime(strTime).ToString("yyyy-MM-dd HH:mm:ss"));
            DateTime endtime = Convert.ToDateTime(Convert.ToDateTime(endTime).ToString("yyyy-MM-dd HH:mm:ss"));
            int timecount = 0;
            timecount = (int)(endtime - starttime).TotalDays;
            for (int i = 0; i <= timecount; i++)
            {
                DateTime now = starttime.AddDays(i);
                list = entity(now, list);
            }
            return list;
        }





    }
}
