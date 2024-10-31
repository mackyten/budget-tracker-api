using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity; 

namespace BT.DOMAIN.Entities.BudgetTracker
{
    public class UserAccount : IdentityUser
    {
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Photo { get; set; }
        // public string GetStringRole(UserRole role)
        // {
        //     return role switch
        //     {
        //         UserRole.Administrator => "Administrator",
        //         UserRole.CoAdministrator => "Co-administrator",
        //         UserRole.DepartmentAdministrator => "Department Administrator",
        //         UserRole.DepartmentCoAdministrator => "Department Co-administrator",
        //         UserRole.Member => "Member",
        //         _ => string.Empty,
        //     };
        // }
        // public UserRole GetEnumRole(string role)
        // {
        //     return role switch
        //     {
        //         "Administrator" => UserRole.Administrator,
        //         "Co-administrator" => UserRole.CoAdministrator,
        //         "Department Administrator" => UserRole.DepartmentAdministrator,
        //         "Department Co-administrator" => UserRole.DepartmentCoAdministrator,
        //         "Member" => UserRole.Member,
        //         _ => UserRole.None,
        //     };
        // }
    }
}