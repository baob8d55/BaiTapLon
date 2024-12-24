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
    public class PhongThucHanhController : Controller
    {
        private readonly ApiServices ApiServices_;
        public PhongThucHanhController(ApiServices services)
        {
            ApiServices_ = services;
        }
        private async Task<List<TbPhongThucHanh>> TbPhongThucHanhs()
        {
            List<TbPhongThucHanh> tbPhongThucHanhs = await ApiServices_.GetAll<TbPhongThucHanh>("/api/csvc/PhongThucHanh");
            List<TbCongTrinhCoSoVatChat> tbCongTrinhCoSoVatChats = await ApiServices_.GetAll<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat");
            List<DmLinhVucNghienCuu> dmlinhVucNghienCuus = await ApiServices_.GetAll<DmLinhVucNghienCuu>("/api/dm/LinhVucNghienCuu");
            tbPhongThucHanhs.ForEach(item => {
                item.IdCongTrinhCsvcNavigation = tbCongTrinhCoSoVatChats.FirstOrDefault(x => x.IdCongTrinhCoSoVatChat == item.IdCongTrinhCsvc);
                item.IdLinhVucNavigation = dmlinhVucNghienCuus.FirstOrDefault(x => x.IdLinhVucNghienCuu == item.IdLinhVuc);
            });
            return tbPhongThucHanhs;
        }


        // GET: PhongThucHanh
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbPhongThucHanh> getall = await TbPhongThucHanhs();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        public async Task<IActionResult> Chart()
        {
            try
            {
                List<TbPhongThucHanh> getall = await TbPhongThucHanhs();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // Lấy chi tiết 1 bản ghi dựa theo ID tương ứng đã truyền vào (IdChuongTrinhDaoTao)
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
                var tbPhongThucHanhs = await TbPhongThucHanhs();
                var tbPhongThucHanh = tbPhongThucHanhs.FirstOrDefault(m => m.IdPhongThucHanh == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbPhongThucHanh == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbPhongThucHanh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        // GET: PhongThucHanh/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCongTrinhCsvc"] = new SelectList(await ApiServices_.GetAll<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat"), "IdCongTrinhCoSoVatChat", "TenCongTrinh");
                ViewData["IdLinhVuc"] = new SelectList(await ApiServices_.GetAll<DmLinhVucNghienCuu>("/api/dm/LinhVucNghienCuu"), "IdLinhVucNghienCuu", "LinhVucNghienCuu");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        // POST: PhongThucHanh/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPhongThucHanh,IdCongTrinhCsvc,IdLinhVuc,MucDoDapUngNhuCauNckh,NamDuaVaoSuDung")] TbPhongThucHanh tbPhongThucHanh)
        {
            try
            {
                // Nếu trùng IDChuongTrinhDaoTao sẽ báo lỗi
                if (await TbPhongThucHanhExists(tbPhongThucHanh.IdPhongThucHanh)) ModelState.AddModelError("IdPhongThucHanh", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbPhongThucHanh>("/api/csvc/PhongThucHanh", tbPhongThucHanh);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdCongTrinhCsvc"] = new SelectList(await ApiServices_.GetAll<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat"), "IdCongTrinhCoSoVatChat", "TenCongTrinh", tbPhongThucHanh.IdCongTrinhCsvc);
                ViewData["IdLinhVuc"] = new SelectList(await ApiServices_.GetAll<DmLinhVucNghienCuu>("/api/dm/LinhVucNghienCuu"), "IdLinhVucNghienCuu", "LinhVucNghienCuu", tbPhongThucHanh.IdLinhVuc);

                return View(tbPhongThucHanh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: PhongThucHanh/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbPhongThucHanh = await ApiServices_.GetId<TbPhongThucHanh>("/api/csvc/PhongThucHanh", id ?? 0);
                if (tbPhongThucHanh == null)
                {
                    return NotFound();
                }
                ViewData["IdCongTrinhCsvc"] = new SelectList(await ApiServices_.GetAll<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat"), "IdCongTrinhCoSoVatChat", "TenCongTrinh", tbPhongThucHanh.IdCongTrinhCsvc);
                ViewData["IdLinhVuc"] = new SelectList(await ApiServices_.GetAll<DmLinhVucNghienCuu>("/api/dm/LinhVucNghienCuu"), "IdLinhVucNghienCuu", "LinhVucNghienCuu", tbPhongThucHanh.IdLinhVuc);

                return View(tbPhongThucHanh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: PhongThucHanh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPhongThucHanh,IdCongTrinhCsvc,IdLinhVuc,MucDoDapUngNhuCauNckh,NamDuaVaoSuDung")] TbPhongThucHanh tbPhongThucHanh)
        {
            try
            {
                if (id != tbPhongThucHanh.IdPhongThucHanh)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbPhongThucHanh>("/api/csvc/PhongThucHanh", id, tbPhongThucHanh);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbPhongThucHanhExists(tbPhongThucHanh.IdPhongThucHanh) == false)
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
                ViewData["IdCongTrinhCsvc"] = new SelectList(await ApiServices_.GetAll<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat"), "IdCongTrinhCoSoVatChat", "TenCongTrinh", tbPhongThucHanh.IdCongTrinhCsvc);
                ViewData["IdLinhVuc"] = new SelectList(await ApiServices_.GetAll<DmLinhVucNghienCuu>("/api/dm/LinhVucNghienCuu"), "IdLinhVucNghienCuu", "LinhVucNghienCuu", tbPhongThucHanh.IdLinhVuc);

                return View(tbPhongThucHanh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: PhongThucHanh/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbPhongThucHanhs = await TbPhongThucHanhs();
                var tbPhongThucHanh = tbPhongThucHanhs.FirstOrDefault(m => m.IdPhongThucHanh == id);
                if (tbPhongThucHanh == null)
                {
                    return NotFound();
                }

                return View(tbPhongThucHanh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: ChuongTrinhDaoTao/Delete
        // Xóa CTĐT khỏi Database sau khi nhấn xác nhận 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Lệnh xác nhận xóa hẳn một CTĐT
        {
            try
            {
                await ApiServices_.Delete<TbPhongThucHanh>("/api/csvc/PhongThucHanh", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        private async Task<bool> TbPhongThucHanhExists(int id)
        {
            var tbPhongThucHanhs = await ApiServices_.GetAll<TbPhongThucHanh>("/api/csvc/PhongThucHanh");
            return tbPhongThucHanhs.Any(e => e.IdPhongThucHanh == id);
        }
    }
}
