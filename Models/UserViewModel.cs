using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public abstract class BaseEntity 
    {
        public DateTime created_at {get;set;}
        public DateTime updated_at {get;set;}
    }
    public class RegisterUser : BaseEntity
    {
        

        [Required(ErrorMessage="Last Name is required")]
        [MinLength(2, ErrorMessage="You must have at least 2 names for last name")]
        [Display(Name="Name")]
        public string name {get;set;}

        [EmailAddress]
        [Required(ErrorMessage="Email is required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name="Email")]
        public string email {get;set;}

        [Required(ErrorMessage="Password is required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="You must have at least 8 characters for your password")]
        [Display(Name="Password")]
        public string password {get;set;}

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        public string confirm {get;set;}
    }
        public class LoginUser : BaseEntity
    {
        [EmailAddress]
        [Required(ErrorMessage="Email is required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name="Email")]
        public string email {get;set;}

        [Required(ErrorMessage="Password is required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="You must have at least 8 characters for your password")]
        [Display(Name="Password")]
        public string password {get;set;}

    }
}