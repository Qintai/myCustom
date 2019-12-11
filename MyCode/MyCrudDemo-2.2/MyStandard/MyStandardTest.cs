 
//using System.Collections.Generic;
//using System.Linq;

//namespace MyStandard
//{
//    public class MyStandardTest
//    {
        
//        public void FilldateTest()
//        {
//            List<dynamic> dlist = new List<dynamic>();
//            dlist.Add(new { addtime = "2019-09-19", Id = 2 });
//            dlist.Add(new { addtime = "2019-09-18", Id = 1 });
//            dlist.Add(new { addtime = "2019-09-20", Id = 3 });
//            dlist.Add(new { addtime = "2019-09-21", Id = 4 });
//            dlist.Add(new { addtime = "2019-09-22", Id = 5 });
//            dlist.Add(new { addtime = "2019-09-23", Id = 6 });
//            dlist.Add(new { addtime = "2019-09-24", Id = 7 });
//            dlist.Add(new { addtime = "2019-09-25", Id = 8 });

//            var  rlist = dlist.Filldate("2019-09-15", "2019-09-30", (now, datalist) =>
//             {
//                  if (datalist.Where(m => m.addtime.ToString() == now.ToString("yyyy-MM-dd")).Count() == 0)
//                      datalist.Add(new
//                      {  Id = 0,  addtime = now  });
//                  return datalist;
//             }).OrderBy(m => m.addtime.ToString("yyyy-MM-dd")).ToList();

//        }


//    }
//}
