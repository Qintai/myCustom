/// <summary>
///  2019年6月1日写
/// </summary>
namespace MyStandard
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;
    
    public class QinVisitor<TS> : ExpressionVisitor
    {
        StringBuilder sql = new StringBuilder();
        static Dictionary<string, object> par = new Dictionary<string, object>(); //搞成静态，不管实例化几次，驻在

        public QinVisitor(Expression<Func<TS, bool>> ex) => base.Visit(ex);

        public QinVisitor(Expression  ex) => base.Visit(ex);

        //获得最终结果
        public (string, Dictionary<string, object>) GetResult()
        {
            return (sql.ToString(), par);
        }

        public string GetColumnName(string name)
        {
            int i = 1;
            while (par.ContainsKey(name))
            {
                name += i;
                i++;
            }
            return name;
        }

        /// <summary>
        /// 解析二元
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            //比如说这种的 a.Id>9
            if (node.Left is MemberExpression && node.NodeType != ExpressionType.OrElse || node.NodeType != ExpressionType.AndAlso)
            {
                string column = (node.Left as MemberExpression).Member.Name;
                sql.Append($"{column}  {GetComparisonSymbols(node.NodeType)}  @{column}");//开始拼装Sql 左 计算符号 右;
                par.Add($"@{GetColumnName(column)}", GetConstant(node.Right));//参数化
            }
            //比如  a.Id>9 && a.Age<20
            else if (node.NodeType == ExpressionType.OrElse || node.NodeType == ExpressionType.AndAlso)
            {
                sql.Append(GetSublevel(node.Left));
                sql.Append($"     {GetComparisonSymbols(node.NodeType)}    ");//中部
                sql.Append(GetSublevel(node.Right));
            }
            return node;//这里一定要返回 node，都执行完成之后，不要再继续访问树节点了
        }

        /// <summary>
        /// 二次解析
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private string GetSublevel(Expression node)
        {
            QinVisitor<TS> z_ = new QinVisitor<TS>(node);
            return z_.GetResult().Item1.ToString();
        }

        private object GetConstant(Expression node)
        {
            ConstantExpression constant = node as ConstantExpression;
            return constant.Value;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            string advanced;
            switch (node.Method.Name)
            {
                case "Equals":
                    //advanced = "= {0}";
                    advanced = "= {0}";
                    break;
                case "StartsWith":
                    //advanced = "like '{0}%'";
                    advanced = "LIKE   CONCAT({0},'%')";
                    break;
                case "EndsWith":
                    //advanced = "like '%{0}'";
                    advanced = "LIKE   CONCAT('%',{0})";
                    break;
                case "Contains":
                    //advanced = "like '%{0}%'";
                    advanced = "LIKE   CONCAT('%',{0},'%')";
                    break;
                default:
                    throw new NotSupportedException($"Not support method name:{node.Method.Name}");
            }
            if (node.Object is MemberExpression m)
            {
                //  node.Arguments[0]== "Foo"
                //  node.Method.Name="Contains"
                //  node.Object=u.Name
                string column = m.Member.Name;
                string sae = string.Format(advanced, $"@{column}");//  like '%@Gender%'

                sql.Append($"{column}  {sae}");//开始拼装Sql 左 计算符号 右;
                par.Add($"@{GetColumnName(column)}", GetConstant(node.Arguments[0]));//参数化
            }
            return node;
        }

        #region 暂不用上
        /// <summary>
        /// 解析Lambda表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        // protected override Expression VisitLambda<T>(Expression<T> node)
        // {
        //     return base.VisitLambda(node);
        // }
        //
        // /// <summary>
        // /// 解析变量，字段
        // /// </summary>
        // /// <param name="node"></param>
        // /// <returns></returns>
        // protected override Expression VisitMember(MemberExpression node)
        // {
        //     return base.VisitMember(node);
        // }
        //
        // protected override MemberBinding VisitMemberBinding(MemberBinding node)
        // {
        //     return base.VisitMemberBinding(node);
        // }
        //
        // /// <summary>
        // /// 解析的常量
        // /// </summary>
        // /// <param name="node"></param>
        // /// <returns></returns>
        // protected override Expression VisitConstant(ConstantExpression node)
        // {
        //     return base.VisitConstant(node);
        // }
        #endregion

        #region 获取比较符号
        /// <summary>
        /// 获取比较符号
        /// </summary>
        /// <param name="expressionType"></param>
        /// <returns></returns>
        protected string GetComparisonSymbols(ExpressionType expressionType)
        {

            switch (expressionType)
            {
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.AndAlso:
                    return "AND";
                case ExpressionType.OrElse:
                    return "OR";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.NotEqual:
                    return "<>";
                default:
                    throw new NotSupportedException($"Unknown ExpressionType {expressionType}");
            }
        }
        #endregion

    }
}
