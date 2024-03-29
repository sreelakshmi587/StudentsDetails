﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsDetails.Model
{
    [Table("UserModel", Schema = "dbo")]
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        [NotMapped]
        public string[] RolesArray
        {
            get { return Roles?.Split(','); }
            set { Roles = string.Join(",", value); }
        }
        public string Salt { get; set; }
       
        
    }
}
