using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profile.Models;

namespace Profile.Controllers
{
    public class PostController : Controller
    {
        private ProfileContext _context = new ProfileContext();

        public IActionResult Index()
        {
            var posts = _context.Posts.Include(p => p.user).ToList(); 
            return View(posts);
        }

        public IActionResult Create()
        {
            ViewBag.Users = _context.Users.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId.HasValue)
                {
                    post.User_Id = userId.Value; 
                    _context.Posts.Add(post);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "User not found in session.");
                }
            }
            ViewBag.Users = _context.Users.ToList();
            return View(post);
        }


        public IActionResult Edit(int id)
        {
            var post = _context.Posts.Include(p => p.user).SingleOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            ViewBag.Users = _context.Users.ToList();
            return View(post);
        }
        [HttpPost]
        public IActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                post.User_Id = (int)HttpContext.Session.GetInt32("UserId");

                _context.Posts.Update(post);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Users = _context.Users.ToList();
            return View(post);
        }


        public IActionResult Delete(int id)
        {
            var post = _context.Posts.Include(p => p.user).SingleOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var post = _context.Posts.Find(id); 
            if (post != null)
            {
                _context.Posts.Remove(post); 
                _context.SaveChanges(); 
                return RedirectToAction(nameof(Index));
            }
            return NotFound(); 
        }

        public IActionResult MyPosts()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "User");
            }

            var posts = _context.Posts
                .Where(p => p.User_Id == userId.Value)
                .Include(p => p.user)
                .ToList();

            return View(posts);
        }


    }

}
