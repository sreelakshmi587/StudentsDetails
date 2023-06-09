﻿using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsDetails.Model
{
    [Table("UserModel", Schema = "dbo")]
    public class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
