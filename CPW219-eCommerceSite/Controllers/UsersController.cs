using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CPW219_eCommerceSite.Data;
using CPW219_eCommerceSite.Models;
using CPW219_eCommerceSite.Repository;
using Microsoft.Data.SqlClient;

namespace CPW219_eCommerceSite.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersContext _context;        

        public UsersController(UsersContext context)
        {
            _context = context;            
        }

        // Login Page
        public ActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_UserName")))
            {
                return RedirectToAction("Index", "/");
            }
            return View();
        }


        // Clears the session and redirects to home page
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "/");

        }

        // user home page, in future will be used as dashboard for users
        public ActionResult Index()
        {
            return View();
        }



        // POST: Users/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.        
        // Authenticates username and password - if successful sets session variables and redirects to home page
        // if authentication fails then a error message is displayed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            ILogin _loginUser = new AuthenticateLogin(_context);

            var issuccess = _loginUser.AuthenticateUser(username, password);

            // if user is not found it returns null otherwise it returns the user
            if (issuccess.Result != null)
            {

                HttpContext.Session.SetString("_UserName", username);
                HttpContext.Session.SetString("_IsLoggedIn", "true");

                return RedirectToAction("Index", "/");
            }
            else
            {
                ViewBag.Message = string.Format("Login Failed ", username);
                return View();
            }
        }        

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsersID,UserName,Password,Email")] Users users)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!ValidatePassword(users.Password))
                    {
                        ViewBag.Message = string.Format("Password must be between 10 and 25 characters and contain at least 1 uppercase, 1 lowercase and 1 numeric character");
                        return View();
                    }

                    _context.Add(users);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Login));
                }                
            }
            catch (DbUpdateException ex)
            {
                ViewBag.Message = string.Format("UserName already exists");
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format(ex.Message);
                return View();
            }
            return View();
        }

        #region not being used functions 

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }        

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsersID,UserName,Password,Email")] Users users)
        {
            if (id != users.UsersID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UsersID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UsersID == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'UsersContext.Users'  is null.");
            }
            var users = await _context.Users.FindAsync(id);
            if (users != null)
            {
                _context.Users.Remove(users);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
          return (_context.Users?.Any(e => e.UsersID == id)).GetValueOrDefault();
        }

        static bool ValidatePassword(string password)
        {
            const int MIN_LENGTH = 10;
            const int MAX_LENGTH = 25;

            if (password == null) throw new ArgumentNullException();

            bool meetsLengthRequirements = password.Length >= MIN_LENGTH && password.Length <= MAX_LENGTH;
            bool hasUpperCaseLetter = false;
            bool hasLowerCaseLetter = false;
            bool hasDecimalDigit = false;

            if (meetsLengthRequirements)
            {
                foreach (char c in password)
                {
                    if (char.IsUpper(c)) hasUpperCaseLetter = true;
                    else if (char.IsLower(c)) hasLowerCaseLetter = true;
                    else if (char.IsDigit(c)) hasDecimalDigit = true;
                }
            }

            bool isValid = meetsLengthRequirements
                        && hasUpperCaseLetter
                        && hasLowerCaseLetter
                        && hasDecimalDigit
                        ;
            return isValid;

        }

        #endregion
    }
}
