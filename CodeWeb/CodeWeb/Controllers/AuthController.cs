using CodeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeWeb.Controllers
{
    [HandleError]
    public class AuthController : Controller
    {
        //GET: NguoiDung
       VaccineDataContext db = new VaccineDataContext();
        //public ActionResult Index()
        //{
        //    return View();
        //}
        //[HttpGet]
        //public ActionResult DangKy()
        //{

        //    return View();
        //}
        //[HttpPost]
        //public ActionResult DangKy(KhachHang kh, TaiKhoan ac, FormCollection f)
        //{
        //    //gan cac gia tri nguoi dung tu form f cho cac bien
        //    var hoten = f["HotenKH"];
        //    var tendn = f["TenDN"];
        //    var matkhau = f["MatKhau"];
        //    var dienthoai = f["Dienthoai"];
        //    var rematkhau = f["ReMatkhau"];
        //    var email = f["Email"];
        //    var gioitinh = f["Gioitinh"];
        //    var diachi = f["Diachi"];
        //    var ngaysinh = f["ngaysinh"];
        //    if (string.IsNullOrEmpty(hoten))
        //    {
        //        ViewData["Loi1"] = "Họ tên không được bỏ trống!";
        //    }
        //    if (string.IsNullOrEmpty(tendn))
        //    {
        //        ViewData["Loi2"] = "Tên đăng nhập không được bỏ trống!";
        //    }
        //    if (string.IsNullOrEmpty(matkhau))
        //    {
        //        ViewData["Loi3"] = "Vui lòng nhập lại mật khẩu!";
        //    }
        //    if (string.IsNullOrEmpty(rematkhau))
        //    {
        //        ViewData["Loi4"] = "Vui lòng nhập lại mật khẩu!";
        //    }
        //    if (string.IsNullOrEmpty(dienthoai))
        //    {
        //        ViewData["Loi5"] = "Vui lòng nhập số điện thoại(Yêu cầu gồm 10 số)!";
        //    }
        //    if (string.IsNullOrEmpty(ngaysinh))
        //    {
        //        ViewData["Loi6"] = "Vui lòng nhập ngày sinh";
        //    }
        //    if (string.IsNullOrEmpty(email))
        //    {
        //        ViewData["Loi8"] = "Vui lòng nhập email!";
        //    }
        //    if (!string.IsNullOrEmpty(hoten) && !string.IsNullOrEmpty(tendn) && !string.IsNullOrEmpty(matkhau) && !string.IsNullOrEmpty(rematkhau) && !string.IsNullOrEmpty(dienthoai))
        //    {
        //        //gan gia tri cho doi tuong kh
        //        kh.HoTenKH = hoten;
        //        kh.TenTK = tendn;
        //        ac.TenTK = tendn;
        //        ac.MatKhau = matkhau;
        //        kh.SoDienThoaiKH = dienthoai;
        //        kh.NgaySinhKH = ngaysinh;
        //        kh.EmailKH = email;
        //        //ghi du lieu xuong csdl
        //        var KQ = db.TaiKhoans.SingleOrDefault(t => t.TenTK == kh.TenTK);
        //        var KQ1 = db.KhachHangs.SingleOrDefault(t => t.EmailKH == kh.EmailKH);
        //        var KQ2 = db.KhachHangs.SingleOrDefault(t => t.SoDienThoaiKH == kh.SoDienThoaiKH);
        //        var KQ3 = db.KhachHangs.SingleOrDefault(t => t.NgaySinhKH == kh.NgaySinhKH);
        //        if (KQ != null || KQ1 != null || KQ2 != null || KQ3 != null)
        //        {

        //            if (KQ != null) { ViewData["Loi2"] = "Tên đăng nhập đã tồn tại vui lòng nhập tên khác!"; }
        //            if (KQ1 != null) { ViewData["Loi8"] = "Mail đã có!"; }
        //            if (KQ2 != null) { ViewData["Loi5"] = "Số điện thoại đã có!"; }
        //            if (KQ3 != null) { ViewData["Loi6"] = "CMND/CCCD  đã có!"; }
        //            return View();

        //        }
        //        else
        //        {
        //            if (dienthoai.Length == 10 && soCMND.Length == 12 && soCMND != null && dienthoai != null && tendn != null && matkhau != null && rematkhau != null && email != null)
        //            {
        //                db.TaiKhoans.InsertOnSubmit(ac);
        //                db.KhachHangs.InsertOnSubmit(kh);

        //                db.SubmitChanges();
        //                //ViewBag.TB = "Đăng ký thành công!";
        //                return RedirectToAction("DangNhap", "Auth");
        //            }
        //            else
        //            {
        //                ViewData["Loi2"] = "Tên đăng nhập đã tồn tại vui lòng nhập tên khác!";
        //                ViewData["Loi5"] = "Yêu cầu nhập lại số điện thoại gồm 10 số!";
        //                ViewData["Loi6"] = "Yêu cầu nhập lại số CMND/CCCD vào gồm 12 số!";
        //                ViewData["Loi5"] = "Yêu cầu nhập lại email!";
        //                return View();
        //            }
        //        }
        //    }
        //    return View();
        //}

        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            //khai bao cac bien nhan gia tri tu form f
            var tendn = f["TenDN"];
            var matkhau = f["MatKhau"];

            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được bỏ trống!";
            }
            if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được bỏ trống!";
            }

            if (!string.IsNullOrEmpty(tendn) && !string.IsNullOrEmpty(matkhau))
            {
                TaiKhoan ac = db.TaiKhoans.SingleOrDefault(t => t.TenTK == tendn && t.MatKhau == matkhau);
                KhachHang kh = db.KhachHangs.SingleOrDefault(t => t.TenTK == tendn);
                NhanVien nv = db.NhanViens.SingleOrDefault(t => t.TenTK == tendn);
                if (ac != null)
                {
                    if (nv != null && ac.TenTK == nv.TenTK)
                    {
                        ViewBag.TB = "Đăng nhập thành công!";
                        Session["NV"] = nv;
                        return RedirectToAction("DangNhap", "Admin");
                    }

                    if (kh != null && ac.TenTK == kh.TenTK)
                    {
                        ViewBag.TB = "Đăng nhập thành công!";
                        Session["KH"] = kh;
                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["Loi1"] = "Tên đăng nhập hoặc mật khẩu sai vui lòng nhập lại!";
                }
            }
            return View();
        }
        
    }
}