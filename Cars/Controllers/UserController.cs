using Cars.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Cars.Controllers
{
    public class UserController : Controller
    {
        Car_Rent_SystemEntities2 db = new Car_Rent_SystemEntities2();
        // GET: User
        public ActionResult Index(int? page)
        {
            int pagesize = 8, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Vehicles.ToList().OrderBy(x => x.Model);
            IPagedList<Vehicle> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User mvc)
        {
            User ad = db.Users.Where(x => x.Email== mvc.Email && x.Password== mvc.Password).SingleOrDefault();
            if (ad!=null)
            {
                Session["ID"] = ad.ID.ToString();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Invalid username or password";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User cvm, HttpPostedFileBase imgfile)
        {
            if (imgfile == null || imgfile.ContentLength == 0)
            {
                ModelState.AddModelError("imgfile", "Image is required");
            }
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }
            string path = Uploading(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded...";
                return View(cvm);
            }

            User user = new User
            {
                Name = cvm.Name,
                Email = cvm.Email,
                Password = cvm.Password,
                Image = path,
                Contact = cvm.Contact
            };

            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
        public string Uploading(HttpPostedFileBase file)
        {
            string path = "-1";
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".png") || extension.ToLower().Equals(".jpeg"))
                {
                    try
                    {
                        string fileName = $"{Guid.NewGuid()}{extension}";
                        string uploadPath = Path.Combine(Server.MapPath("~/Upload"), fileName);
                        Directory.CreateDirectory(Server.MapPath("~/Upload"));
                        file.SaveAs(uploadPath);
                        path = $"/Upload/{fileName}";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg, and png formats are accepted...');</script>");
                }
            }
            return path;
        }
        public ActionResult SignOut()
        {
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Login");
        }
        public ActionResult AvailableVehicles(int? page)
        {
            int pagesize = 8, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Vehicles.Where(x => x.Status == 1).OrderByDescending(x => x.ID).ToList();
            IPagedList<Vehicle> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }
        public ActionResult Order(int? id, int? page)
        {
            int pagesize = 8, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Vehicles.Where(x => x.ID == 1).OrderByDescending(x => x.ID).ToList();
            IPagedList<Vehicle> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }
        
        public ActionResult ViewOrder(int? id)
        {
            ViewCarOrderModel _Order = new ViewCarOrderModel();
            Vehicle vehicle = db.Vehicles.Where(x => x.ID == id).SingleOrDefault();
            _Order.ID = vehicle.ID;
            _Order.Model = vehicle.Model;
            _Order.DailyPrice = vehicle.Daily_hired_price;
            _Order.ImagePath = vehicle.ImagePath;
            _Order.Des = vehicle.Des;

            User user = db.Users.Where(x=>x.ID == vehicle.ID).SingleOrDefault();
            _Order.Name = user.Name;
            _Order.Image= user.Image;
            _Order.Email= user.Email;
            _Order.Contact= user.Contact;
            _Order.UserID = user.ID;
            return View(_Order);
        }
        [HttpPost]
        public ActionResult Order(int? id, int? page, string search)
        {
            int pagesize = 8, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Vehicles.Where(x => x.ID == 1).OrderByDescending(x => x.ID).ToList();
            IPagedList<Vehicle> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }
        public ActionResult DeletedCar(int? id)
        {
            //Vehicle V = db.Vehicles.Where(x => x.ID == id).SingleOrDefault();
            //db.Tb_Order.Remove(V);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult CreateOrder(int? id)
        {
            List<Vehicle> li = db.Vehicles.ToList();
            ViewBag.VehicleList = new SelectList(li, "ID", "Model");
            return View();
        }

        [HttpPost]
        public ActionResult CreateOrder(Vehicle cvm, HttpPostedFileBase imgfile)
        {
            if (imgfile == null || imgfile.ContentLength == 0)
            {
                ModelState.AddModelError("imgfile", "Image is required");
            }
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }
            string path = Uploading(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded...";
                return View(cvm);
            }

            Vehicle _Order = new Vehicle
            {
                Model = cvm.Model,
                Registration = cvm.Registration,
                Color = cvm.Color,
                Capacity = cvm.Capacity,
                Des = cvm.Des,
                Status = 1,
                Fuel_type = cvm.Fuel_type,
                Transmission = cvm.Transmission,
                Daily_hired_price = cvm.Daily_hired_price,
                Condition = cvm.Condition,
                ImagePath = path,
                Date_added = DateTime.Now
            };

            db.Vehicles.Add(_Order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}