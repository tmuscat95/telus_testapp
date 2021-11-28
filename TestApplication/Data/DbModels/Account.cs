using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestApplication.Data.DbModels
{
    public class Account

    {   
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        [Key] public int UserrId { get; set; }

        public Account()
        {
            
        }
    }
}
