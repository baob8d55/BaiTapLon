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
    public class ThuVienTrungTamHocLieuController : Controller
    {
        private readonly ApiServices ApiServices_;
        // Lấy từ HemisContext 
        public ThuVienTrungTamHocLieuController(ApiServices services)
        {
            ApiServices_ = services;
        }

        // GET: ThuVienTrungTamHocLieu
        // Lấy danh sách TVTTHL từ database, trả về view Index.

        private async Task<List<TbThuVienTrungTamHocLieu>> TbThuVienTrungTamHocLieus()
        {
            List<TbThuVienTrungTamHocLieu> tbThuVienTrungTamHocLieus = await ApiServices_.GetAll<TbThuVienTrungTamHocLieu>("/api/csvc/ThuVienTrungTamHocLieu");
            List<DmHinhThucSoHuu> dmhinhThucSoHuus = await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu");
            List<DmTinhTrangCoSoVatChat> dmtinhTrangCoSoVatChats = await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat");
            tbThuVienTrungTamHocLieus.ForEach(item => {
                item.IdHinhThucSoHuuNavigation = dmhinhThucSoHuus.FirstOrDefault(x => x.IdHinhThucSoHuu == item.IdHinhThucSoHuu);
                item.IdTinhTrangCsvcNavigation = dmtinhTrangCoSoVatChats.FirstOrDefault(x => x.IdTinhTrangCoSoVatChat == item.IdTinhTrangCsvc);
            });
            return tbThuVienTrungTamHocLieus;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbThuVienTrungTamHocLieu> getall = await TbThuVienTrungTamHocLieus();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // Lấy chi tiết 1 bản ghi dựa theo ID tương ứng đã truyền vào (IdThuVienTrungTamHocLieu)
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
                var tbThuVienTrungTamHocLieus = await TbThuVienTrungTamHocLieus();
                var tbThuVienTrungTamHocLieu = tbThuVienTrungTamHocLieus.FirstOrDefault(m => m.IdThuVienTrungTamHocLieu == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbThuVienTrungTamHocLieu == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết TVTTHL thành công
                return View(tbThuVienTrungTamHocLieu);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: ThuVienTrungTamHocLieu/Create
        // Hiển thị view Create để tạo một bản ghi TVTTHL mới
        // Truyền data từ các table khác hiển thị tại view Create (khóa ngoài)
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu");
                ViewData["IdTinhTrangCsvc"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: ThuVienTrungTamHocLieu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // Thêm một TVTTHL mới vào Database nếu IdThuVienTrungTamHocLieu truyền vào không trùng với Id đã có trong Database
        // Trong trường hợp nhập trùng IdThuVienTrungTamHocLieu sẽ bắt lỗi
        // Bắt lỗi ngoại lệ sao cho người nhập BẮT BUỘC phải nhập khác IdThuVienTrungTamHocLieu đã có
        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Create([Bind("IdThuVienTrungTamHocLieu,MaThuVienTrungTamHocLieu,TenThuVienTrungTamHocLieu,NamDuaVaoSuDung,DienTich,DienTichPhongDoc,SoPhongDoc,SoLuongMayTinh,SoLuongChoNgoi,SoLuongSach,SoLuongTapChi,SoLuongSachDienTu,SoLuongTapChiDienTu,SoLuongThuVienDienTuLienKetNn,IdTinhTrangCsvc,IdHinhThucSoHuu,SoLuongDauSach,SoLuongDauTapChi,SoLuongDauSachDienTu,SoLuongDauTapChiDienTu")] TbThuVienTrungTamHocLieu tbThuVienTrungTamHocLieu)
        {
            try
            {
                // Nếu trùng IDThuVienTrungTamHocLieu sẽ báo lỗi
                if (await TbThuVienTrungTamHocLieuExists(tbThuVienTrungTamHocLieu.IdThuVienTrungTamHocLieu)) ModelState.AddModelError("IdThuVienTrungTamHocLieu", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbThuVienTrungTamHocLieu>("/api/csvc/ThuVienTrungTamHocLieu", tbThuVienTrungTamHocLieu);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu", tbThuVienTrungTamHocLieu.IdHinhThucSoHuu);
                ViewData["IdTinhTrangCsvc"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat", tbThuVienTrungTamHocLieu.IdTinhTrangCsvc);
                return View(tbThuVienTrungTamHocLieu);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: ThuVienTrungTamHocLieu/Edit
        // Lấy data từ Database với Id đã có, sau đó hiển thị ở view Edit
        // Nếu không tìm thấy Id tương ứng sẽ báo lỗi NotFound
        // Phương thức này gần giống Create, nhưng nó nhập dữ liệu vào Id đã có trong database
        public async Task<IActionResult> Edit(int? id)
        {
            //try
            //{
                if (id == null)
                {
                    return NotFound();
                }

                var tbThuVienTrungTamHocLieu = await ApiServices_.GetId<TbThuVienTrungTamHocLieu>("/api/csvc/ThuVienTrungTamHocLieu", id ?? 0);
                if (tbThuVienTrungTamHocLieu == null)
                {
                    return NotFound();
                }
                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu", tbThuVienTrungTamHocLieu.IdHinhThucSoHuu);
                ViewData["IdTinhTrangCsvc"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat", tbThuVienTrungTamHocLieu.IdTinhTrangCsvc);
                return View(tbThuVienTrungTamHocLieu);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest();
            //}

        }

        // POST: ThuVienTrungTamHocLieu/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // Lưu data mới (ghi đè) vào các trường Data đã có thuộc IdThuVienTrungTamHocLieu cần chỉnh sửa
        // Nó chỉ cập nhật khi ModelState hợp lệ
        // Nếu không hợp lệ sẽ báo lỗi, vì vậy cần có bắt lỗi.

        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Edit(int id, [Bind("IdThuVienTrungTamHocLieu,MaThuVienTrungTamHocLieu,TenThuVienTrungTamHocLieu,NamDuaVaoSuDung,DienTich,DienTichPhongDoc,SoPhongDoc,SoLuongMayTinh,SoLuongChoNgoi,SoLuongSach,SoLuongTapChi,SoLuongSachDienTu,SoLuongTapChiDienTu,SoLuongThuVienDienTuLienKetNn,IdTinhTrangCsvc,IdHinhThucSoHuu,SoLuongDauSach,SoLuongDauTapChi,SoLuongDauSachDienTu,SoLuongDauTapChiDienTu")] TbThuVienTrungTamHocLieu tbThuVienTrungTamHocLieu)
        {
            try
            {
                if (id != tbThuVienTrungTamHocLieu.IdThuVienTrungTamHocLieu)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbThuVienTrungTamHocLieu>("/api/csvc/ThuVienTrungTamHocLieu", id, tbThuVienTrungTamHocLieu);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbThuVienTrungTamHocLieuExists(tbThuVienTrungTamHocLieu.IdThuVienTrungTamHocLieu) == false)
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
                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu", tbThuVienTrungTamHocLieu.IdHinhThucSoHuu);
                ViewData["IdTinhTrangCsvc"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat", tbThuVienTrungTamHocLieu.IdTinhTrangCsvc);
                return View(tbThuVienTrungTamHocLieu);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: ThuVienTrungTamHocLieu/Delete
        // Xóa một TVTTHL khỏi Database
        // Lấy data TVTTHL từ Database, hiển thị Data tại view Delete
        // Hàm này để hiển thị thông tin cho người dùng trước khi xóa
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbThuVienTrungTamHocLieus = await ApiServices_.GetAll<TbThuVienTrungTamHocLieu>("/api/csvc/ThuVienTrungTamHocLieu");
                var tbThuVienTrungTamHocLieu = tbThuVienTrungTamHocLieus.FirstOrDefault(m => m.IdThuVienTrungTamHocLieu == id);
                if (tbThuVienTrungTamHocLieu == null)
                {
                    return NotFound();
                }

                return View(tbThuVienTrungTamHocLieu);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: ThuVienTrungTamHocLieu/Delete
        // Xóa TVTTHL khỏi Database sau khi nhấn xác nhận 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Lệnh xác nhận xóa hẳn một TVTTHL
        {
            try
            {
                await ApiServices_.Delete<TbThuVienTrungTamHocLieu>("/api/csvc/ThuVienTrungTamHocLieu", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        private async Task<bool> TbThuVienTrungTamHocLieuExists(int id)
        {
            var tbThuVienTrungTamHocLieus = await ApiServices_.GetAll<TbThuVienTrungTamHocLieu>("/api/csvc/ThuVienTrungTamHocLieu");
            return tbThuVienTrungTamHocLieus.Any(e => e.IdThuVienTrungTamHocLieu == id);
        }
    }
}