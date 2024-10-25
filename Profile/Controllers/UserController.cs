using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profile.Models;
using Profile.ViewModels;
namespace Profile.Controllers
{
    public class UserController : Controller
    {
        private ProfileContext context = new ProfileContext();
        public IActionResult Index()
        {
            List<User> users = context.Users.ToList();
            ViewBag.TempDataMessage = TempData["Message"] ?? string.Empty;
            string? userName = HttpContext.Session.GetString("UserName");
            string ?userEmail = HttpContext.Session.GetString("UserEmail");
          
            ViewBag.UserName = userName;
            ViewBag.UserEmail = userEmail;
            return View(users);
        }
        
        public IActionResult Show(int id)
        {
            if (id <= 0)
            {
                return View("NotFound");
            }
            User? user = context.Users.Where(user => user.Id == id).SingleOrDefault();
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return View("NotFound");

            }
        }
        public IActionResult Create()
        {
            ViewBag.roles = context.Roles.ToList();
            return View();
        }
        public IActionResult Store(User addedUser, int role_id)
        {
            if (role_id <= 0)
            {
                ModelState.AddModelError("Role_Id", "Please select a role.");
                ViewBag.roles = context.Roles.ToList(); 
                return View("Create", addedUser); 
            }

            context.Users.Add(addedUser);
            context.SaveChanges();

            var userRole = new UserRole
            {
                User_Id = addedUser.Id,
                Role_Id = role_id
            };

            context.UsersRoles.Add(userRole);
            context.SaveChanges();

            TempData["Message"] = "User created successfully!";
            return RedirectToAction("Index");
        }




        public IActionResult Edit(int id)
        {
            User? user = context.Users.Where(user => user.Id == id).SingleOrDefault();
            ViewBag.UserRoles = context.UsersRoles.ToList(); 
            ViewBag.roles = context.Roles.ToList(); 

            return View(user);
            
         
            
        }
        public IActionResult Update(User updatedUser, int role_id)
        {
            var user = context.Users.Find(updatedUser.Id); 
            if (user == null)
            {
                return NotFound();
            }
            user.Name = updatedUser.Name;
            user.Age = updatedUser.Age;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password; 

            context.SaveChanges();
            TempData["Message"] = "User updated successfully!";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var user = context.Users.Find(id); 

            if (user == null)
            {
                return NotFound(); 
            }

            var userRoles = context.UsersRoles.Where(ur => ur.User_Id == id).ToList();
            context.UsersRoles.RemoveRange(userRoles); 

            context.Users.Remove(user);
            context.SaveChanges();
            TempData["Message"] = "User deleted successfully!";

            return RedirectToAction("Index");
        }

        //public IActionResult CreateCookie()
        //{
        //    return View();
        //}
        //public IActionResult SetCookie(string CookieName1, string CookieValue1, string CookieName2, string CookieValue2)
        //{
        //    if (!string.IsNullOrEmpty(CookieName1) && !string.IsNullOrEmpty(CookieValue1))
        //    {
        //        CookieOptions option1 = new CookieOptions
        //        {
        //            Expires = DateTime.Now.AddDays(7) 
        //        };
        //        Response.Cookies.Append(CookieName1, CookieValue1, option1);
        //    }

        //    if (!string.IsNullOrEmpty(CookieName2) && !string.IsNullOrEmpty(CookieValue2))
        //    {
        //        CookieOptions option2 = new CookieOptions
        //        {
        //            Expires = DateTime.Now.AddDays(7) 
        //        };
        //        Response.Cookies.Append(CookieName2, CookieValue2, option2);
        //    }

        //    TempData["Message"] = "Cookies have been set successfully!";

        //    return RedirectToAction("Index"); 
        //}

        //public IActionResult GetCookie()
        //{
        //    var cookies = new List<KeyValuePair<string, string>>();

        //    foreach (var cookie in Request.Cookies)
        //    {
        //        cookies.Add(new KeyValuePair<string, string>(cookie.Key, cookie.Value));
        //    }
        //    return View(cookies);

        //}


        //public IActionResult DeleteCookie(string cookieName)
        //{
        //    if (!string.IsNullOrEmpty(cookieName))
        //    {
        //        Response.Cookies.Delete(cookieName);
        //        TempData["Message"] = $"{cookieName} has been deleted successfully!";
        //    }
        //    else
        //    {
        //        TempData["ErrorMessage"] = "Cookie name cannot be empty.";
        //    }

        //    return RedirectToAction("Index");
        //}
        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }
  
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = context.Users.SingleOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    HttpContext.Session.SetString("UserName", user.Name);
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetInt32("UserId", user.Id); 


                    return RedirectToAction("Index", "User");
                }
                else
                {
                    TempData["Error"] = "Invalid email or password!";
                    return View(model);
                }
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "User");
        }


    }
}
