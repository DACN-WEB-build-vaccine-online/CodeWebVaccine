using CodeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeWeb.Controllers
{
    public class SanphamController : Controller
    {
        VaccineDataContext db = new VaccineDataContext();
        // GET: Sanpham
        public ActionResult ShowVaccine(int page = 1, string SortColumn = "MaHH", string IconClass = "fa-sort-asc")
        {
            List<Vaccine> vaccines = db.Vaccines.ToList();

            //sort
            ViewBag.SortColumn = SortColumn;
            ViewBag.IconClass = IconClass;
            if (SortColumn == "GiaBan")
            {
                if (IconClass == "fa-sort-asc")
                {
                    vaccines = vaccines.OrderBy(row => row.GiaBan).ToList();
                }
                else
                {
                    vaccines = vaccines.OrderByDescending(row => row.GiaBan).ToList();
                }
            }
            else if (SortColumn == "MaVaccin")
            {
                if (IconClass == "fa-sort-asc")
                {
                    vaccines = vaccines.OrderBy(row => row.MaVaccin).ToList();
                }
                else
                {
                    vaccines = vaccines.OrderByDescending(row => row.MaVaccin).ToList();
                }
            }

            //paging
            int NoOfRecordPerPage = 9;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(vaccines.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPage = NoOfPages;
            vaccines = vaccines.Skip(NoOfRecordSkip).Take(NoOfRecordPerPage).ToList();

            return View(vaccines);

        }
        public ActionResult Details(int id)
        {
            var Vaccine = db.Vaccines.Single(t => t.MaVaccin == id);
            if (Vaccine == null)
            {
                return HttpNotFound();
            }
            return View(Vaccine);
        }


        public ActionResult LoaiVaccine(int Maloai)
        {
            var list = db.Vaccines.Where(s => s.MaLoai == Maloai).ToList();

            if (list.Count == 0)
            {
                ViewBag.TB = "Khong tim thay";
            }
            else
            {
                string ten = db.LoaiVaccines.Where(s => s.MaLoai == Maloai).ToString();
                ViewBag.Loai = ten;
            }
            return View(list);
        }

        public ActionResult NhomVaccine(int Manhom)
        {
            var nhomvc = db.NhomVaccines.Where(s => s.MaNhom == Manhom).ToList();

            if (nhomvc.Count == 0)
            {
                ViewBag.TB = "Khong tim thay";
            }
            else
            {
                string ten = db.NhomVaccines.Where(s => s.MaNhom == Manhom).ToString();
                ViewBag.Loai = ten;
            }


            return View(nhomvc);
        }

    }
}