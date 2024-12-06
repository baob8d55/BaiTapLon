using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaiTapLon.Models;
using BaiTapLon.API;
using BaiTapLon.Models.DM;

namespace BaiTapLon.Controllers.CSVC
{
    public class PhongHocGiangDuongHoiTruongController : Controller
    {
        private readonly ApiServices ApiServices_;
        // Lấy từ HemisContext 
        public PhongHocGiangDuongHoiTruongController(ApiServices services)
        {
            ApiServices_ = services;
        }

        // GET: PhongHocGiangDuongHoiTruong
        // Lấy danh sách PHGĐHT từ database, trả về view Index.

        private async Task<List<TbPhongHocGiangDuongHoiTruong>> TbPhongHocGiangDuongHoiTruongs()
        {
            List<TbPhongHocGiangDuongHoiTruong> tbPhongHocGiangDuongHoiTruongs = await ApiServices_.GetAll<TbPhongHocGiangDuongHoiTruong>("/api/csvc/PhongHocGiangDuongHoiTruong");
            List<DmHinhThucSoHuu> dmhinhThucSoHuus = await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu");
            List<DmLoaiDeAnChuongTrinh> dmloaiDeAnChuongTrinhs = await ApiServices_.GetAll<DmLoaiDeAnChuongTrinh>("/api/dm/LoaiDeAnChuongTrinh");
            List<DmLoaiPhongHoc> dmloaiPhongHocs = await ApiServices_.GetAll<DmLoaiPhongHoc>("/api/dm/LoaiPhongHoc");
            List<DmPhanLoaiCsvc> dmphanLoaiCsvcs = await ApiServices_.GetAll<DmPhanLoaiCsvc>("/api/dm/PhanLoaiCsvc");
            List<DmTinhTrangCoSoVatChat> dmtinhTrangCoSoVatChats = await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat");
            tbPhongHocGiangDuongHoiTruongs.ForEach(item => {
                item.IdHinhThucSoHuuNavigation = dmhinhThucSoHuus.FirstOrDefault(x => x.IdHinhThucSoHuu == item.IdHinhThucSoHuu);
                item.IdLoaiDeAnNavigation = dmloaiDeAnChuongTrinhs.FirstOrDefault(x => x.IdLoaiDeAnChuongTrinh == item.IdLoaiDeAn);
                item.IdLoaiPhongHocNavigation = dmloaiPhongHocs.FirstOrDefault(x => x.IdLoaiPhongHoc == item.IdLoaiPhongHoc);
                item.IdPhanLoaiCsvcNavigation = dmphanLoaiCsvcs.FirstOrDefault(x => x.IdPhanLoaiCsvc == item.IdPhanLoaiCsvc);
                item.IdTinhTrangCoSoVatChatNavigation = dmtinhTrangCoSoVatChats.FirstOrDefault(x => x.IdTinhTrangCoSoVatChat == item.IdTinhTrangCoSoVatChat);
            });
            return tbPhongHocGiangDuongHoiTruongs;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbPhongHocGiangDuongHoiTruong> getall = await TbPhongHocGiangDuongHoiTruongs();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // Lấy chi tiết 1 bản ghi dựa theo ID tương ứng đã truyền vào (IdPhongHocGiangDuongHoiTruong)
        // Hiển thị bản ghi đó ở view Details
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbPhongHocGiangDuongHoiTruongs = await TbPhongHocGiangDuongHoiTruongs();
                var tbPhongHocGiangDuongHoiTruong = tbPhongHocGiangDuongHoiTruongs.FirstOrDefault(m => m.IdPhongHocGiangDuongHoiTruong == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbPhongHocGiangDuongHoiTruong == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết PHGĐHT thành công
                return View(tbPhongHocGiangDuongHoiTruong);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: PhongHocGiangDuongHoiTruong/Create
        // Hiển thị view Create để tạo một bản ghi PHGĐHT mới
        // Truyền data từ các table khác hiển thị tại view Create (khóa ngoài)
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu");
                ViewData["IdLoaiDeAn"] = new SelectList(await ApiServices_.GetAll<DmLoaiDeAnChuongTrinh>("/api/dm/LoaiDeAnChuongTrinh"), "IdLoaiDeAnChuongTrinh", "LoaiDeAnChuongTrinh");
                ViewData["IdLoaiPhongHoc"] = new SelectList(await ApiServices_.GetAll<DmLoaiPhongHoc>("/api/dm/LoaiPhongHoc"), "IdLoaiPhongHoc", "LoaiPhongHoc");
                ViewData["IdPhanLoaiCsvc"] = new SelectList(await ApiServices_.GetAll<DmPhanLoaiCsvc>("/api/dm/PhanLoaiCsvc"), "IdPhanLoaiCsvc", "PhanLoaiCsvc");
                ViewData["IdTinhTrangCoSoVatChat"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: PhongHocGiangDuongHoiTruong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // Thêm một PHGĐHT mới vào Database nếu IdPhongHocGiangDuongHoiTruong truyền vào không trùng với Id đã có trong Database
        // Trong trường hợp nhập trùng IdPhongHocGiangDuongHoiTruong sẽ bắt lỗi
        // Bắt lỗi ngoại lệ sao cho người nhập BẮT BUỘC phải nhập khác IdPhongHocGiangDuongHoiTruong đã có
        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Create([Bind("IdPhongHocGiangDuongHoiTruong,MaPhongHocGiangDuongHoiTruong,IdNganhDaoTao,TenPhongHocGiangDuongHoiTruong,DienTich,IdHinhThucSoHuu,QuyMoChoNgoi,IdTinhTrangCoSoVatChat,IdPhanLoaiCsvc,IdLoaiPhongHoc,IdLoaiDeAn,NamDuaVaoSuDung")] TbPhongHocGiangDuongHoiTruong tbPhongHocGiangDuongHoiTruong)
        {
            try
            {
                // Nếu trùng IDPhongHocGiangDuongHoiTruong sẽ báo lỗi
                if (await TbPhongHocGiangDuongHoiTruongExists(tbPhongHocGiangDuongHoiTruong.IdPhongHocGiangDuongHoiTruong)) ModelState.AddModelError("IdPhongHocGiangDuongHoiTruong", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbPhongHocGiangDuongHoiTruong>("/api/csvc/PhongHocGiangDuongHoiTruong", tbPhongHocGiangDuongHoiTruong);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu", tbPhongHocGiangDuongHoiTruong.IdHinhThucSoHuu);
                ViewData["IdLoaiDeAn"] = new SelectList(await ApiServices_.GetAll<DmLoaiDeAnChuongTrinh>("/api/dm/LoaiDeAnChuongTrinh"), "IdLoaiDeAnChuongTrinh", "LoaiDeAnChuongTrinh", tbPhongHocGiangDuongHoiTruong.IdLoaiDeAn);
                ViewData["IdLoaiPhongHoc"] = new SelectList(await ApiServices_.GetAll<DmLoaiPhongHoc>("/api/dm/LoaiPhongHoc"), "IdLoaiPhongHoc", "LoaiPhongHoc", tbPhongHocGiangDuongHoiTruong.IdLoaiPhongHoc);
                ViewData["IdPhanLoaiCsvc"] = new SelectList(await ApiServices_.GetAll<DmPhanLoaiCsvc>("/api/dm/PhanLoaiCsvc"), "IdPhanLoaiCsvc", "PhanLoaiCsvc", tbPhongHocGiangDuongHoiTruong.IdPhanLoaiCsvc);
                ViewData["IdTinhTrangCoSoVatChat"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat", tbPhongHocGiangDuongHoiTruong.IdTinhTrangCoSoVatChat);
                return View(tbPhongHocGiangDuongHoiTruong);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: PhongHocGiangDuongHoiTruong/Edit
        // Lấy data từ Database với Id đã có, sau đó hiển thị ở view Edit
        // Nếu không tìm thấy Id tương ứng sẽ báo lỗi NotFound
        // Phương thức này gần giống Create, nhưng nó nhập dữ liệu vào Id đã có trong database
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbPhongHocGiangDuongHoiTruong = await ApiServices_.GetId<TbPhongHocGiangDuongHoiTruong>("/api/csvc/PhongHocGiangDuongHoiTruong", id ?? 0);
                if (tbPhongHocGiangDuongHoiTruong == null)
                {
                    return NotFound();
                }
                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu", tbPhongHocGiangDuongHoiTruong.IdHinhThucSoHuu);
                ViewData["IdLoaiDeAn"] = new SelectList(await ApiServices_.GetAll<DmLoaiDeAnChuongTrinh>("/api/dm/LoaiDeAnChuongTrinh"), "IdLoaiDeAnChuongTrinh", "LoaiDeAnChuongTrinh", tbPhongHocGiangDuongHoiTruong.IdLoaiDeAn);
                ViewData["IdLoaiPhongHoc"] = new SelectList(await ApiServices_.GetAll<DmLoaiPhongHoc>("/api/dm/LoaiPhongHoc"), "IdLoaiPhongHoc", "LoaiPhongHoc", tbPhongHocGiangDuongHoiTruong.IdLoaiPhongHoc);
                ViewData["IdPhanLoaiCsvc"] = new SelectList(await ApiServices_.GetAll<DmPhanLoaiCsvc>("/api/dm/PhanLoaiCsvc"), "IdPhanLoaiCsvc", "PhanLoaiCsvc", tbPhongHocGiangDuongHoiTruong.IdPhanLoaiCsvc);
                ViewData["IdTinhTrangCoSoVatChat"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat", tbPhongHocGiangDuongHoiTruong.IdTinhTrangCoSoVatChat);
                return View(tbPhongHocGiangDuongHoiTruong);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: PhongHocGiangDuongHoiTruong/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // Lưu data mới (ghi đè) vào các trường Data đã có thuộc IdPhongHocGiangDuongHoiTruong cần chỉnh sửa
        // Nó chỉ cập nhật khi ModelState hợp lệ
        // Nếu không hợp lệ sẽ báo lỗi, vì vậy cần có bắt lỗi.

        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Edit(int id, [Bind("IdPhongHocGiangDuongHoiTruong,MaPhongHocGiangDuongHoiTruong,IdNganhDaoTao,TenPhongHocGiangDuongHoiTruong,DienTich,IdHinhThucSoHuu,QuyMoChoNgoi,IdTinhTrangCoSoVatChat,IdPhanLoaiCsvc,IdLoaiPhongHoc,IdLoaiDeAn,NamDuaVaoSuDung")] TbPhongHocGiangDuongHoiTruong tbPhongHocGiangDuongHoiTruong)
        {
            try
            {
                if (id != tbPhongHocGiangDuongHoiTruong.IdPhongHocGiangDuongHoiTruong)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbPhongHocGiangDuongHoiTruong>("/api/csvc/PhongHocGiangDuongHoiTruong", id, tbPhongHocGiangDuongHoiTruong);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbPhongHocGiangDuongHoiTruongExists(tbPhongHocGiangDuongHoiTruong.IdPhongHocGiangDuongHoiTruong) == false)
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu", tbPhongHocGiangDuongHoiTruong.IdHinhThucSoHuu);
                ViewData["IdLoaiDeAn"] = new SelectList(await ApiServices_.GetAll<DmLoaiDeAnChuongTrinh>("/api/dm/LoaiDeAnChuongTrinh"), "IdLoaiDeAnChuongTrinh", "LoaiDeAnChuongTrinh", tbPhongHocGiangDuongHoiTruong.IdLoaiDeAn);
                ViewData["IdLoaiPhongHoc"] = new SelectList(await ApiServices_.GetAll<DmLoaiPhongHoc>("/api/dm/LoaiPhongHoc"), "IdLoaiPhongHoc", "LoaiPhongHoc", tbPhongHocGiangDuongHoiTruong.IdLoaiPhongHoc);
                ViewData["IdPhanLoaiCsvc"] = new SelectList(await ApiServices_.GetAll<DmPhanLoaiCsvc>("/api/dm/PhanLoaiCsvc"), "IdPhanLoaiCsvc", "PhanLoaiCsvc", tbPhongHocGiangDuongHoiTruong.IdPhanLoaiCsvc);
                ViewData["IdTinhTrangCoSoVatChat"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat", tbPhongHocGiangDuongHoiTruong.IdTinhTrangCoSoVatChat);
                return View(tbPhongHocGiangDuongHoiTruong);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: PhongHocGiangDuongHoiTruong/Delete
        // Xóa một PHGĐHT khỏi Database
        // Lấy data PHGĐHT từ Database, hiển thị Data tại view Delete
        // Hàm này để hiển thị thông tin cho người dùng trước khi xóa
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbPhongHocGiangDuongHoiTruongs = await ApiServices_.GetAll<TbPhongHocGiangDuongHoiTruong>("/api/csvc/PhongHocGiangDuongHoiTruong");
                var tbPhongHocGiangDuongHoiTruong = tbPhongHocGiangDuongHoiTruongs.FirstOrDefault(m => m.IdPhongHocGiangDuongHoiTruong == id);
                if (tbPhongHocGiangDuongHoiTruong == null)
                {
                    return NotFound();
                }

                return View(tbPhongHocGiangDuongHoiTruong);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: PhongHocGiangDuongHoiTruong/Delete
        // Xóa PHGĐHT khỏi Database sau khi nhấn xác nhận 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Lệnh xác nhận xóa hẳn một PHGĐHT
        {
            try
            {
                await ApiServices_.Delete<TbPhongHocGiangDuongHoiTruong>("/api/csvc/PhongHocGiangDuongHoiTruong", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        private async Task<bool> TbPhongHocGiangDuongHoiTruongExists(int id)
        {
            var tbPhongHocGiangDuongHoiTruongs = await ApiServices_.GetAll<TbPhongHocGiangDuongHoiTruong>("/api/csvc/PhongHocGiangDuongHoiTruong");
            return tbPhongHocGiangDuongHoiTruongs.Any(e => e.IdPhongHocGiangDuongHoiTruong == id);
        }
    }
}