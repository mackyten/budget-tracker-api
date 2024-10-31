using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BT.SERVICES.AccountsService
{
    public class AccountsOptions : BaseValidatableOptions
    {
        private const string REQUIRED_MESSAGE = "Accounts.{0} is required";
        [Required(ErrorMessage = REQUIRED_MESSAGE), Url]
        public string ApiAuthorityUrl { get; set; }
        public bool RequiresHttpsMeta { get; set; } = false;
        public string HRPApiName { get; set; } = "bt.api";
    }
}