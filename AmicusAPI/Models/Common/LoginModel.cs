using AmicusAPI.Validators.Common;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmicusAPI.Models.Common
{
    [Validator(typeof(LoginValidator))]
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
