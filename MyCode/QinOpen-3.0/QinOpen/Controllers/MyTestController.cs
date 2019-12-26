using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using QinEntity;

namespace QinOpen.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class MyTestController : ControllerBase
    //{
    //    // GET: api/MyTest
    //    [HttpGet]
    //    public IEnumerable<string> Get()
    //    {
    //        return new string[] { "value1", "value2" };
    //    }

    //    // GET: api/MyTest/5
    //    [HttpGet("{id}")]
    //    public string Get(int id)
    //    {
    //        return "value";
    //    }

    //    // POST: api/MyTest
    //    [HttpPost]
    //    public void Post([FromBody] string value)
    //    {
    //    }

    //    // PUT: api/MyTest/5
    //    [HttpPut("{id}")]
    //    public void Put(int id, [FromBody] string value)
    //    {
    //    }

    //    // DELETE: api/ApiWithActions/5
    //    [HttpDelete("{id}")]
    //    public void Delete(int id)
    //    {
    //    }

    //    /// <summary>
    //    /// 测试json配置文件是否可以读取及时数据
    //    /// </summary>
    //    /// <returns></returns>
    //    //[HttpGet]
    //    [HttpPost]
    //    public string pp(string type)
    //    {
    //        return QinCommon.Common.Appsettings
    //            .app(new string[] { "a" });
    //    }


    //    [HttpPost( "")]
    //    public void TsetA(int id, string Jsonstr)
    //    {
    //        // Json字符串转换实体
    //        JObject jObject = JObject.Parse(Jsonstr);
    //        zCustomUser zCustom = new zCustomUser();
    //        var props = zCustom.GetType().GetProperties();
    //        foreach (var item in jObject)
    //        {
    //            foreach (var prop in props)
    //            {
    //                if (prop.Name.Equals(item.Key))
    //                {
    //                    var s = Convert.ChangeType(item.Value, prop.PropertyType); // s需要做数据库关键词过滤
    //                    prop.SetValue(zCustom, s, null);
    //                }
    //            }
    //        }

    //    }

    //}
}
