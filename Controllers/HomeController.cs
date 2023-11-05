using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BankAccounts.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;
    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

  [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet("")]
        public IActionResult Register()
        {
            return View();
        }
         [HttpPost("registeruser")]
        public IActionResult RegisterUser(RegisterUser newuser)
        {
            User CheckEmail = _context.users
                .Where(u => u.email == newuser.email)
                .SingleOrDefault();

            if(CheckEmail != null)
            {
                ViewBag.errors = "That email already exists";
                return RedirectToAction("Register");
            }
            if(ModelState.IsValid)
            {
                PasswordHasher<RegisterUser> Hasher = new PasswordHasher<RegisterUser>();
                User newUser = new User
                {
                    name = newuser.name,
                    email = newuser.email,
                    password = Hasher.HashPassword(newuser, newuser.password)
                  };
                _context.Add(newUser);
                _context.SaveChanges();
                ViewBag.success = "Successfully registered";
                User UserRegistered = _context.users
                    .Single(u => u.email == newuser.email);
                Account account = new Account
                {
                    balance = 0,
                    user_id = UserRegistered.user_id
                };
                _context.accounts.Add(account);
                _context.SaveChanges();
                Account UserAccount = _context.accounts
                    .Single(a => a.user_id == UserRegistered.user_id);
                HttpContext.Session.SetInt32("user_id", UserRegistered.user_id);
                HttpContext.Session.SetString("name", UserRegistered.name);
                return RedirectToAction("Account", new {id = UserAccount.account_id});
            }
            else
            {
                return View("Register");
            }
        }

        [HttpPost("loginuser")]
        public IActionResult LoginUser(LoginUser loginUser) 
        {
            User CheckEmail = _context.users
                .SingleOrDefault(u => u.email == loginUser.email);
            if(CheckEmail != null)
            {
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(CheckEmail, CheckEmail.password, loginUser.password))
                {
                    Account UserAccount = _context.accounts
                        .Single(a => a.user_id == CheckEmail.user_id);
                    HttpContext.Session.SetInt32("user_id", CheckEmail.user_id);
                    HttpContext.Session.SetString("name", CheckEmail.name);
                    return RedirectToAction("Account", new {id = UserAccount.user_id});
                }
                else
                {
                    ViewBag.errors = "Incorrect Password";
                    return View("Register");
                }
            }
            else
            {
                ViewBag.errors = "Email not registered";
                return View("Register");
            }
        }
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet("account/{id}")]
        public IActionResult Account(int id)
        {
            if(HttpContext.Session.GetInt32("user_id") == null)
            {
                return RedirectToAction("Login");
            }
            Account UserAccount = _context.accounts
                .Include(a => a.Transactions)
                .Single(a => a.user_id == HttpContext.Session.GetInt32("user_id"));
            if(UserAccount.account_id != id)
            {
                return RedirectToAction("Login");
            }
            UserAccount.Transactions = UserAccount.Transactions.OrderByDescending(t => t.trans_date).ToList();
            ViewBag.account = UserAccount;
            ViewBag.user = HttpContext.Session.GetString("name");
            return View();
        }

        [HttpPost("process_transaction")]
        public IActionResult ProcessTransaction(double amount, int accountid, int Balance)
        {
            Account UserAccount = _context.accounts
                .Single(a => a.user_id == HttpContext.Session.GetInt32("user_id"));
            if(UserAccount.balance + amount >= 0)
            {
                UserAccount.balance += amount;
                Transaction trans = new Transaction
                {
                    amount = amount,
                    account_id = accountid,
                    trans_date = DateTime.Now
                };
                _context.transactions.Add(trans);
                _context.SaveChanges();
                return RedirectToAction("Account", new {id = UserAccount.user_id});
            }
            return RedirectToAction("Account", new {id = UserAccount.user_id}); 
        }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
