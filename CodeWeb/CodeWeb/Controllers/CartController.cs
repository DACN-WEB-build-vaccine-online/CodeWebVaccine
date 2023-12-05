using CodeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeWeb.Controllers
{
    public class CartController : Controller
    {
        VaccineDataContext db = new VaccineDataContext();
        // GET: Cart
        public List<GioHang> LaygioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        //them vao gio hang
        public ActionResult ThemGioHang(int ma, int sl, string strURL)
        {
            KhachHang kh = Session["KH"] as KhachHang;
            if (kh == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { strURL = strURL });
            }
            //lay ra gio hang
            var lstGioHang = db.GioHangs.Where(x => x.MaKH == kh.MaKH).ToList();
            //kiem tra vc nay da co trong session "Gio Hang" hay chua ?
            GioHang vc = lstGioHang.SingleOrDefault(t => t.MaVC == ma);
            if (vc == null)// chua co trong gio hang
            {
                vc = new GioHang();
                vc.MaKH = kh.MaKH;
                vc.MaVC = ma;
                vc.SoLuong = sl;
                db.GioHangs.InsertOnSubmit(vc);
                db.SubmitChanges();
                return Redirect(strURL);
            }
            else
            {
                vc.SoLuong++;
                db.SubmitChanges();
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            KhachHang kh = Session["KH"] as KhachHang;
            if (kh == null)
            {
                return 0;
            }
            var lstGioHang = db.GioHangs.Where(x => x.MaKH == kh.MaKH).ToList();
            if (lstGioHang != null)
            {
                return (int)lstGioHang.Sum(x => x.SoLuong);
            }

            return 0;
        }
        private double TongThanhTien()
        {
            double ttt = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                ttt += lstGioHang.Sum(t => (double)t.TongThanhTien);
            }
            return ttt;
        }
        public ActionResult GioHang(string strURL)
        {
            KhachHang kh = Session["KH"] as KhachHang;
            if (kh == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { strURL = strURL });
            }
            var lstGioHang = db.GioHangs.Where(x => x.MaKH == kh.MaKH).ToList();
            var lstSach = db.Vaccines.ToList();
            ViewBag.lstGioHang = lstGioHang;
            ViewBag.lstSach = lstSach;
            return View();
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView();
        }

        public ActionResult XoaGioHang(int MaSP, int idkh)
        {

            GioHang s = db.GioHangs.SingleOrDefault(t => t.MaVC == MaSP && t.MaKH == idkh);
            if (s != null)
            {
                db.GioHangs.DeleteOnSubmit(s);
                db.SubmitChanges();
                Session["DeleteCart"] = "Xóa thành công.";
                Session["ResultDelete"] = "t";
            }
            else
            {
                Session["DeleteCart"] = "Không tìm thấy!";
                Session["ResultDelete"] = "f";
            }
            return RedirectToAction("GioHang", "Cart");
        }
        public ActionResult XoaAll(int idkh)
        {
            db.GioHangs.DeleteAllOnSubmit(db.GioHangs.Where(x => x.MaKH == idkh));
            db.SubmitChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CapNhatGioHang(int MaSP, int idkh, FormCollection f)
        {
            int amount;
            bool kt = int.TryParse(f["amount"], out amount);
            if (kt)
            {
                GioHang s = db.GioHangs.SingleOrDefault(x => x.MaVC == MaSP && x.MaKH == idkh);
                if (s != null)
                {
                    s.SoLuong = amount;
                    db.SubmitChanges();
                    Session["UpdateCart"] = "Cập nhật thành công.";
                    Session["ResultUpdate"] = "t";
                }
                else
                {
                    Session["UpdateCart"] = "Không tìm thấy!";
                    Session["ResultUpdate"] = "f";
                }
            }
            else
            {
                Session["UpdateCart"] = "Cập nhật thất bại!";
                Session["ResultUpdate"] = "f";
            }
            return RedirectToAction("GioHang", "Cart");
        }

        public ActionResult DangkyTiem(int idKH)
        {
            List<HoaDon> carts = Session["ToPay"] as List<HoaDon>;
            Session["PayList"] = carts;
            Session["Vaccines"] = db.Vaccines.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Payment(FormCollection fc)
        {

            return RedirectToAction("Index", "Home");
        }

            public ActionResult DanhSachDonHang(int idKH)
        {

            ViewBag.TB = null;
            List<HoaDon> donhangs = db.HoaDons.Where(x => x.MaNTC == idKH).ToList();
            if (donhangs.Count <= 0)
            {
                ViewBag.TB = "Bạn chưa mua đơn nào !";
                return PartialView();
            }
            return PartialView(donhangs);
        }

        public ActionResult ChiTietDonHangDonHang(int idHoaDon, int idKH)
        {
            HoaDon donhang = db.HoaDons.FirstOrDefault(x => x.MaHD == idHoaDon);
            if (idKH == donhang.MaNTC)
            {
                List<ChiTietHoaDon> ctdh = db.ChiTietHoaDons.Where(x => x.MaHD == idHoaDon).ToList();

                ViewBag.CTDH = ctdh;
                ViewBag.DonHang = donhang;
                return PartialView();
            }
            return RedirectToAction("PageNotFound", "Error");
        }

    }
}