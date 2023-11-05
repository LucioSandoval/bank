using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class Transaction : BaseEntity
    {
        [Key]
        public int transaction_id {get;set;}
        public double amount {get;set;}
        public int account_id {get;set;}
        public Account Account {get;set;}

        public DateTime trans_date {get;set;}
        public Transaction()
        {
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }
    }
}