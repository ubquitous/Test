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
        [RegularExpression(val.Name, ErrorMessage = val.ErrorMessage)]
        
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public static class val
    {
        public const string Name = "[a]{1}[0-9a-zA-Z]{1,40}";
        public const string ErrorMessage = "Не верный формат: a*";
    }

    public class Reg
    {
        [Required]
        [MinLength(3)]
        [RegularExpression(val.Name, ErrorMessage = val.ErrorMessage)]
        public string Name { get; set; }
        [Required]
        [MinLength(5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [MinLength(5)]
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

    public class cross
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class InfoCr
    {
        public bool[,] inf { get; set; }
        public int row { get; set; }
        public int col { get; set; }
        
        public void preob(List<string> str)
        {
            if (str.Count > 0 && str[0].Length > 0)
            {
                row = str.Count();
                col = str[0].Length;
                inf = new bool[row, col];
                int k = 0, g = 0;
                foreach (string a in str)
                {
                    g = 0;
                    foreach (char b in a)
                    {
                        if (b == '0')
                        {
                            inf[k, g] = false;
                        }
                        else
                        {
                            inf[k, g] = true;
                        }
                        g++;
                    }
                    k++;
                }
            }
        }
    }
}