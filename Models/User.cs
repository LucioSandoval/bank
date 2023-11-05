using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int user_id {get;set;}
        public string name {get;set;}
        public string email {get;set;}
        public string password {get;set;}

        public User()
        {
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }
    }
}