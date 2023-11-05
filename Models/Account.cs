using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class Account : BaseEntity
    {
        [Key]
        public int account_id {get;set;}
        public double balance {get;set;}
        public int user_id {get;set;}
        public User User {get;set;}

        public List<Transaction> Transactions {get;set;}

        public Account()
        {
            Transactions = new List<Transaction>();
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }
    }
}