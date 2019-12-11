using System.ComponentModel;

namespace crud_entity
{
    public  enum Permissions
    {
        [DescriptionAttribute("超级管理员")]
        SuperAdmin=0,

        [DescriptionAttribute("普通管理员")]
        Admin = 1,

        [DescriptionAttribute("停用")]
        Stop = 1,

    }
}
