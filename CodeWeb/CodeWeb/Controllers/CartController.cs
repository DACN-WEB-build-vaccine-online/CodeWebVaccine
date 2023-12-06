using CodeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CodeWeb.Controllers
{
    [HandleError]
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
        private int TongThanhTien()
        {
            int ttt = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                ttt += lstGioHang.Sum(t => (int)t.TongThanhTien);
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
            List<GioHang> carts = Session["ToPay"] as List<GioHang>;
            Session["PayList"] = carts;
            Session["Vaccines"] = db.Vaccines.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult DangkyTiem(FormCollection fc)
        {
            string addressDetails = fc["address"];
            string name, phone;

            name = fc["name"];
            phone = fc["phone"];
            if (string.IsNullOrEmpty(addressDetails) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || !(new Regex(@"^([0-9]{10,11})$").IsMatch(phone)))
            {
                ViewData["Error"] = "Lỗi";
                return View();
            }
            List<GioHang> GHs = Session["PayList"] as List<GioHang>;
            var mqh = db.MoiQuanHes.ToList();


            int idBill = 0;
            int idKH = GHs.FirstOrDefault().MaKH;

            foreach (var item in GHs)
            {
                if (idBill == 0)
                {
                    try
                    {
                        HoaDon Donhang = new HoaDon();
                        Donhang.NgayLap = DateTime.Now;
                        //Donhang.ThoiGianTiem = date;
                        Donhang.MaNTC = idKH;
                        Donhang.TongTien = (int)TongThanhTien();

                        db.HoaDons.InsertOnSubmit(Donhang);
                        db.SubmitChanges();

                    }
                    catch
                    {
                        idBill = 0;
                    }
                }
                //    if (idBill > 0)
                //    {
                //        try
                //        {
                //            CHITIETDH ct = new CHITIETDH();
                //            ct.MADH = idBill;
                //            ct.MASACH = item.MASACH;
                //            ct.SOLUONG = item.SOLUONG;
                //            db.CHITIETDHs.InsertOnSubmit(ct);
                //            db.SubmitChanges();
                //            db.GIOHANGs.DeleteOnSubmit(db.GIOHANGs.SingleOrDefault(x => x.MASACH == item.MASACH && item.MAKH == idKH));
                //            db.SubmitChanges();
                //        }
                //        catch
                //        {
                //            idBill = 0;
                //        }
                //    }
                
               
            }
            return RedirectToAction("Index", "Home");
        }


        public ActionResult PaymentList(int idKH)
        {
            List<GioHang> carts = db.GioHangs.Where(x => x.MaKH == idKH).ToList();
            int countRemove = carts.RemoveAll(x => x.Vaccine.SoLuongVC < x.SoLuong);
            if (carts.Count <= 0)
                return RedirectToAction("Index", "Home");
            Session["ToPay"] = carts;
            if (countRemove > 0)
                Session["Remove"] = countRemove;
            return RedirectToAction("DangkyTiem", "Cart", new { idKH = idKH });
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