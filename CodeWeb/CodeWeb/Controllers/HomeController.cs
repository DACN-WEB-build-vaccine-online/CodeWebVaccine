using CodeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeWeb.Controllers
{
    public class HomeController : Controller
    {
        VaccineDataContext db = new VaccineDataContext();
        public ActionResult Index()
        {
            return View();
        }

        

        public ActionResult Loai()
        {
            var model = db.LoaiVaccines.OrderByDescending(p => p.TenLoai);
            return PartialView(model);
        }

        public ActionResult NhomVacxin()
        {
            var model2 = db.NhomVaccines.OrderByDescending(p => p.TenNhom);
            return PartialView(model2);
        }

        public ActionResult DangKyTiem()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
    }
}