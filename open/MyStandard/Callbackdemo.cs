/*/*
*  创建人：喔是传奇 
*  Date: 2019-10-14 14:17:30
*  C#回调函数 
#1#

using System;

namespace crud_web
{
    internal class Program1 //用户层，执行输入等操作
    {
        private static void Main1(string[] args)
        {
            var cc = new CalculateClass();
            var fc = new FunctionClass();

            var result1 = cc.PrintAndCalculate(2, 3, fc.GetSum);
            var result3 = cc.PrintAndCalculate(2, 3, (a, b) => { return a + b; });
            Console.WriteLine("调用了开发人员的加法函数，处理后返回结果：" + result1);

            var result2 = cc.PrintAndCalculate(2, 3, fc.GetMulti);
            Console.WriteLine("调用了开发人员的乘法函数，处理后返回结果：" + result2);

            Console.ReadKey();
        }
    }

    internal class FunctionClass //开发层处理，开发人员编写具体的计算方法
    {
        public int GetSum(int a, int b)
        {
            return a + b;
        }

        public int GetMulti(int a, int b)
        {
            return a * b;
        }
    }

    #region 实际开发中，下面这个类会封装起来，只提供函数接口。相当于系统底层

    internal class CalculateClass
    {
        public delegate int SomeCalculateWay(int num1, int num2);

        //将传入参数在系统底层进行某种处理，具体计算方法由开发者开发，函数仅提供执行计算方法后的返回值
        public int PrintAndCalculate(int num1, int num2, SomeCalculateWay cal)
        {
            Console.WriteLine("系统底层处理：" + num1);
            Console.WriteLine("系统底层处理：" + num2);
            return cal(num1, num2); //调用传入函数的一个引用
        }

        //可以封装更多的业务逻辑方法
    }

    #endregion
}*/