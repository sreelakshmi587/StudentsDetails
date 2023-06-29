using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsDetails.Model
{
    [Table("UserLogin", Schema = "dbo")]
    public class UserLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
