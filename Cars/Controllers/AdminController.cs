using Cars.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Cars.Controllers
{
    public class AdminController : Controller
    {
        Car_Rent_SystemEntities2 db = new Car_Rent_SystemEntities2();

        // GET: Admin//login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //Register
        [HttpPost]
        public ActionResult Index(Admin avm)
        {
            Admin admin = db.Admins.Where(model => model.Username == avm.Username && model.Password == avm.Password).SingleOrDefault();
            if (admin != null)
            {
                Session["ID"] = admin.ID.ToString();
                return RedirectToAction("ViewVehicles");
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
        public ActionResult Register(Admin avm)
        {
            Admin admin = db.Admins.Where(model => model.Username == avm.Username).SingleOrDefault();
            if (admin != null)
            {
                ViewBag.error = "You cannot register with this username";
            }
            else
            {
                Admin ad = new Admin();
                ad.Username = avm.Username;
                ad.Password = avm.Password;
                db.Admins.Add(ad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult ViewVehicles(int? page)
        {
            int pagesize = 8, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Vehicles.Where(x => x.Status == 1).OrderByDescending(x => x.ID).ToList();
            IPagedList<Vehicle> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }

        [HttpGet]
        public ActionResult CreateVehicles()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateVehicles(Vehicle cvm, HttpPostedFileBase imgfile)
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

            Vehicle vehicle = new Vehicle
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

            db.Vehicles.Add(vehicle);
            db.SaveChanges();
            return RedirectToAction("ViewVehicles");
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



    }
}