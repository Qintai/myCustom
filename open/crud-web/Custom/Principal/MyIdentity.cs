/*
*  创建人：喔是传奇 
*  Date: 2019-10-16 11:01:19
*/

namespace crud_web
{
    /// <summary> 
    /// MyIdentity 的摘要说明。 
    /// </summary> 
    /// 实现IIdentity接口 
    public class MyIdentity : System.Security.Principal.IIdentity
    {
        private string userID;
        private string password;

        public MyIdentity(string currentUserID, string currentPassword)
        {
            // 
            // TODO: 在此处添加构造函数逻辑 
            // 
            userID = currentUserID;
            password = currentPassword;
        }

        /// <summary>
        /// 用户名或者密码是否正确
        /// </summary>
        /// <returns></returns>
        private bool CanPass()
        {
            //这里朋友们可以根据自己的需要改为从数据库中验证用户名和密码， 
            //这里为了方便我直接指定的字符串 
            if (userID == "admin" && password == "123456")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        #region IIdentity 成员 

        public bool IsAuthenticated
        {
            get
            {
                // TODO:   添加 MyIdentity.IsAuthenticated getter 实现 
                return CanPass();
            }
        }

        public string Name
        {
            get
            {
                // TODO:   添加 MyIdentity.Name getter 实现 
                return userID;
            }
        }

        //这个属性我们可以根据自己的需要来灵活使用,在本例中没有用到它 
        public string AuthenticationType
        {
            get
            {
                // TODO:   添加 MyIdentity.AuthenticationType getter 实现 
                return null;
            }
        }

        #endregion
    }
}