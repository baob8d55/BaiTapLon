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
    public class ThietBiPTN_THTren500TrController : Controller
    {
        private readonly ApiServices ApiServices_;
        // Lấy từ HemisContext 
        public ThietBiPTN_THTren500TrController(ApiServices services)
        {
            ApiServices_ = services;
        }

        // GET: ThietBiPTN_THTren500Tr
        // Lấy danh sách TBPTN_THT500T từ database, trả về view Index.

        private async Task<List<TbThietBiPtnThtren500Tr>> TbThietBiPtnThtren500Trs()
        {
            List<TbThietBiPtnThtren500Tr> tbThietBiPtnThtren500Trs = await ApiServices_.GetAll<TbThietBiPtnThtren500Tr>("/api/csvc/ThietBiPTN_THTren500Tr");
            List<TbCongTrinhCoSoVatChat> tbcongTrinhCsvcs = await ApiServices_.GetAll<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat");
            List<DmQuocTich> dmquoctiches = await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich");
            tbThietBiPtnThtren500Trs.ForEach(item => {
                item.IdCongTrinhCsvcNavigation = tbcongTrinhCsvcs.FirstOrDefault(x => x.IdCongTrinhCoSoVatChat == item.IdCongTrinhCsvc);
                item.IdQuocGiaXuatXuNavigation = dmquoctiches.FirstOrDefault(x => x.IdQuocTich == item.IdQuocGiaXuatXu);
            });
            return tbThietBiPtnThtren500Trs;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbThietBiPtnThtren500Tr> getall = await TbThietBiPtnThtren500Trs();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // Lấy chi tiết 1 bản ghi dựa theo ID tương ứng đã truyền vào (IdThietBiPtnThtren500Tr)
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
                var tbThietBiPtnThtren500Trs = await TbThietBiPtnThtren500Trs();
                var tbThietBiPtnThtren500Tr = tbThietBiPtnThtren500Trs.FirstOrDefault(m => m.IdThietBiPtnTh == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbThietBiPtnThtren500Tr == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết TBPTN_THT500T thành công
                return View(tbThietBiPtnThtren500Tr);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: ThietBiPtnThtren500Tr/Create
        // Hiển thị view Create để tạo một bản ghi TBPTN_THT500T mới
        // Truyền data từ các table khác hiển thị tại view Create (khóa ngoài)
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCongTrinhCsvc"] = new SelectList(await ApiServices_.GetAll<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat"), "IdCongTrinhCsvc", "CongTrinhCsvc");
                ViewData["IdQuocGiaXuatXu"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocGiaXuatXu", "QuocGiaXuatXu"); 
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: ThietBiPtnThtren500Tr/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // Thêm một TBPTN_THT500T mới vào Database nếu IdThietBiPtnThtren500Tr truyền vào không trùng với Id đã có trong Database
        // Trong trường hợp nhập trùng IdThietBiPtnThtren500Tr sẽ bắt lỗi
        // Bắt lỗi ngoại lệ sao cho người nhập BẮT BUỘC phải nhập khác IdThietBiPtnThtren500Tr đã có
        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Create([Bind("IdThietBiPtnTh,MaThietBi,IdCongTrinhCsvc,TenThietBi,NamSanXuat,NamBatDauTuyenSinh,IdQuocGiaXuatXu,HangSanXuat,SoLuongThietBiCungLoai,NamDuaVaoSuDung")] TbThietBiPtnThtren500Tr tbThietBiPtnThtren500Tr)
        {
            try
            {
                // Nếu trùng IDThietBiPtnThtren500Tr sẽ báo lỗi
                if (await TbThietBiPtnThtren500TrExists(tbThietBiPtnThtren500Tr.IdThietBiPtnTh)) ModelState.AddModelError("IdThietBiPtnThtren500Tr", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbThietBiPtnThtren500Tr>("/api/csvc/ThietBiPtnThtren500Tr", tbThietBiPtnThtren500Tr);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdCongTrinhCsvc"] = new SelectList(await ApiServices_.GetAll<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat"), "IdCongTrinhCsvc", "CongTrinhCsvc", tbThietBiPtnThtren500Tr.IdCongTrinhCsvc);
                ViewData["IdQuocGiaXuatXu"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocGiaXuatXu", "QuocGiaXuatXu", tbThietBiPtnThtren500Tr.IdQuocGiaXuatXu);
                return View(tbThietBiPtnThtren500Tr);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: ThietBiPtnThtren500Tr/Edit
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

                var tbThietBiPtnThtren500Tr = await ApiServices_.GetId<TbThietBiPtnThtren500Tr>("/api/csvc/ThietBiPtnThtren500Tr", id ?? 0);
                if (tbThietBiPtnThtren500Tr == null)
                {
                    return NotFound();
                }
                ViewData["IdCongTrinhCsvc"] = new SelectList(await ApiServices_.GetAll<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat"), "IdCongTrinhCsvc", "CongTrinhCsvc", tbThietBiPtnThtren500Tr.IdCongTrinhCsvc);
                ViewData["IdQuocGiaXuatXu"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocGiaXuatXu", "QuocGiaXuatXu", tbThietBiPtnThtren500Tr.IdQuocGiaXuatXu);
                return View(tbThietBiPtnThtren500Tr);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: ThietBiPtnThtren500Tr/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // Lưu data mới (ghi đè) vào các trường Data đã có thuộc IdThietBiPtnThtren500Tr cần chỉnh sửa
        // Nó chỉ cập nhật khi ModelState hợp lệ
        // Nếu không hợp lệ sẽ báo lỗi, vì vậy cần có bắt lỗi.

        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Edit(int id, [Bind("IdThietBiPtnTh,MaThietBi,IdCongTrinhCsvc,TenThietBi,NamSanXuat,NamBatDauTuyenSinh,IdQuocGiaXuatXu,HangSanXuat,SoLuongThietBiCungLoai,NamDuaVaoSuDung")] TbThietBiPtnThtren500Tr tbThietBiPtnThtren500Tr)
        {
            try
            {
                if (id != tbThietBiPtnThtren500Tr.IdThietBiPtnTh)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbThietBiPtnThtren500Tr>("/api/csvc/ThietBiPtnThtren500Tr", id, tbThietBiPtnThtren500Tr);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbThietBiPtnThtren500TrExists(tbThietBiPtnThtren500Tr.IdThietBiPtnTh) == false)
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
                ViewData["IdCongTrinhCsvc"] = new SelectList(await ApiServices_.GetAll<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat"), "IdCongTrinhCsvc", "CongTrinhCsvc", tbThietBiPtnThtren500Tr.IdCongTrinhCsvc);
                ViewData["IdQuocGiaXuatXu"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocGiaXuatXu", "QuocGiaXuatXu", tbThietBiPtnThtren500Tr.IdQuocGiaXuatXu);
                return View(tbThietBiPtnThtren500Tr);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: ThietBiPtnThtren500Tr/Delete
        // Xóa một TBPTN_THT500T khỏi Database
        // Lấy data TBPTN_THT500T từ Database, hiển thị Data tại view Delete
        // Hàm này để hiển thị thông tin cho người dùng trước khi xóa
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbThietBiPtnThtren500Trs = await TbThietBiPtnThtren500Trs();
                var tbThietBiPtnThtren500Tr = tbThietBiPtnThtren500Trs.FirstOrDefault(m => m.IdThietBiPtnTh == id);
                if (tbThietBiPtnThtren500Tr == null)
                {
                    return NotFound();
                }

                return View(tbThietBiPtnThtren500Tr);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: ThietBiPtnThtren500Tr/Delete
        // Xóa TBPTN_THT500T khỏi Database sau khi nhấn xác nhận 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Lệnh xác nhận xóa hẳn một TBPTN_THT500T
        {
            try
            {
                await ApiServices_.Delete<TbThietBiPtnThtren500Tr>("/api/csvc/ThietBiPtnThtren500Tr", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        private async Task<bool> TbThietBiPtnThtren500TrExists(int id)
        {
            var tbThietBiPtnThtren500Trs = await ApiServices_.GetAll<TbThietBiPtnThtren500Tr>("/api/csvc/ThietBiPtnThtren500Tr");
            return tbThietBiPtnThtren500Trs.Any(e => e.IdThietBiPtnTh == id);
        }
    }
}