using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CrossDoubleN.ViewModels
{
    public class Log
    {
        public string Role { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        
    }

    public class LoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class Reg
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string Confirm { get; set; }

    }

    public class cr
    {
        public string name { get; set; }
        public List<string> zn { get; set; }
        public int row { get; set; }
        public int col { get; set; }

    }
}