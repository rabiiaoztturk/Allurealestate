using Microsoft.AspNetCore.Mvc;
using Allurerealstate.Models;
using Allurerealstate.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Allurerealstate.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }
        private const string AdminCode = ""; 

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Blog(string Title)
        {
            if (Title == AdminCode)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Blog");
            }
            else
            {
                ViewBag.Error = "Invalid Admin Code. Please try again.";
                return View("Index");
            }
        }

        public IActionResult Blog()
        {
            if (HttpContext.Session.GetString("IsAdmin") == "true")
            {
                var Blogs = _context.Blogs.ToList();
                return View(Blogs);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Dashboard(string Title)
        {
            if (Title == AdminCode)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Error = "Invalid Admin Code. Please try again.";
                return View("Index");
            }
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("IsAdmin") == "true")
            {
                var Houses = _context.Properties.ToList();
                return View(Houses);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult AddForm()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");

            if (isAdmin == "true")
            {
                var model = new Property();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public IActionResult AddBlogForm()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");

            if (isAdmin == "true")
            {
                var blog = new Blog();
                return View(blog);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [RequestSizeLimit(100_000_000)]
        [HttpPost]
        public IActionResult AddBlog(Blog model, IFormFileCollection Images)
        {
            model.Images ??= new List<IFormFile>();
            var imageNames = new List<string>();

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            try
            {
                foreach (var image in model.Images)
                {
                    if (image != null && image.Length > 0)
                    {
                        var imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var path = Path.Combine(uploadPath, imageName);

                        using var stream = new FileStream(path, FileMode.Create);
                        image.CopyTo(stream); // Resmi kaydediyoruz

                        imageNames.Add($"/uploads/{imageName}");
                    }
                }

                model.ImageUrls = imageNames;

                _context.Blogs.Add(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while saving images: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while uploading the images.");
                return View(model);
            }

            return RedirectToAction("Blog");
        }

        [RequestSizeLimit(100_000_000)]
        [HttpPost]
        public IActionResult Add(Property model, IFormFileCollection Images)
        {
            model.Images ??= new List<IFormFile>();
            var imageNames = new List<string>();

            var maxId = _context.Properties.Any() ? _context.Properties.Max(p => p.PropertyId) : 0; 
            model.PropertyId = maxId + 1;

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            try
            {
                foreach (var image in model.Images)
                {
                    if (image != null && image.Length > 0)
                    {
                        var imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var path = Path.Combine(uploadPath, imageName);

                        using var stream = new FileStream(path, FileMode.Create);
                        image.CopyTo(stream);

                        imageNames.Add($"/uploads/{imageName}");
                    }
                }

                model.ImageUrls = imageNames;

                _context.Properties.Add(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while saving images: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while uploading the images.");
                return View(model);
            }

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult EditForm(int id)
        {

            var isAdmin = HttpContext.Session.GetString("IsAdmin");

            if (isAdmin == "true")
            {
                var Houses = _context.Properties.SingleOrDefault(l => l.Id == id);
                if (Houses == null)
                {
                    return NotFound();
                }
                return View(Houses);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public IActionResult EditBlogForm(int id)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");

            if (isAdmin == "true")
            {
                var Blogs = _context.Blogs.SingleOrDefault(l => l.Id == id);
                if (Blogs == null)
                {
                    return NotFound();
                }
                return View(Blogs);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [RequestSizeLimit(100_000_000)]
        [HttpPost]
        public IActionResult EditBlog(Blog model)
        {
            var existingBlogs = _context.Blogs.SingleOrDefault(l => l.Id == model.Id);
            if (existingBlogs == null)
            {
                return NotFound();
            }

            existingBlogs.Title = model.Title;
            existingBlogs.Description = model.Description;
            existingBlogs.Images = model.Images;

            if (model.Images != null && model.Images.Any())
            {
                existingBlogs.ImageUrls = new List<string>();

                foreach (var image in model.Images)
                {
                    if (image.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                        var filePath = Path.Combine("wwwroot/uploads", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        }

                        existingBlogs.ImageUrls.Add("/uploads/" + fileName);
                    }
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Blog");
        }

        [RequestSizeLimit(100_000_000)]
        [HttpPost]
        public IActionResult Edit(Property Houses)
        {
            var existingHouses = _context.Properties.SingleOrDefault(l => l.Id == Houses.Id);
            if (existingHouses == null)
            {
                return NotFound();
            }

            existingHouses.Title = Houses.Title;
            existingHouses.Description = Houses.Description;
            existingHouses.Location = Houses.Location;
            existingHouses.Area = Houses.Area;
            existingHouses.Rooms = Houses.Rooms;
            existingHouses.Bathrooms = Houses.Bathrooms;
            existingHouses.Price = Houses.Price;
            existingHouses.Images = Houses.Images;

            if (Houses.Images != null && Houses.Images.Any())
            {
                existingHouses.ImageUrls = new List<string>();

                foreach (var image in Houses.Images)
                {
                    if (image.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                        var filePath = Path.Combine("wwwroot/uploads", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        }

                        existingHouses.ImageUrls.Add("/uploads/" + fileName);
                    }
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _context.Remove(_context.Properties.Single(l => l.Id == id));
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        public IActionResult DeleteBlog(int id)
        {
            _context.Remove(_context.Blogs.Single(l => l.Id == id));
            _context.SaveChanges();
            return RedirectToAction("Blog");
        }

        public IActionResult Favorite(int id)
        {
            var property = _context.Properties.Find(id);
            if (property == null)
            {
                return NotFound();
            }

            property.Favorite = true; 
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult DeleteFavorite(int id)
        {
            var item = _context.Properties.Find(id);
            if (item == null)
            {
                return RedirectToAction("Dashboard");
            }

            item.Favorite = !item.Favorite; 
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        public IActionResult Under(int id)
        {
            var property = _context.Properties.Find(id);
            if (property == null)
            {
                return NotFound();
            }

            property.UnderConstruction = true;
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult DeleteUnder(int id)
        {
            var item = _context.Properties.Find(id);
            if (item == null)
            {
                return RedirectToAction("Dashboard");
            }

            item.UnderConstruction = !item.UnderConstruction;
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }
    }
}
