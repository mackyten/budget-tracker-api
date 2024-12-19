using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Project.DOMAIN.Enums.Accounts
{
    public enum UserRoles
    {
        [Description("SuperAdmin")]
        SuperAdmin,
        [Description("Admin")]
        Admin,
    }

    public static class Roles
    {
        public static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}