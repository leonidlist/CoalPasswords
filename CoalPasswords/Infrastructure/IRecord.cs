using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoalPasswords
{
    interface IRecord
    {
        string Title { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string Website { get; set; }
    }
}
