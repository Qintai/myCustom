using crud_base;
using crud_entity;

namespace crud_server.connector
{
    /// <summary>
    ///     zUser 独有的业务逻辑,也具备公共的逻辑
    /// </summary>
    public interface IzUserSer : IBaseMysql<zUser>
    {
        /// <summary>
        ///     独有业务逻辑
        /// </summary>
        /// <returns></returns>
        int AgeGreater18();
    }
}