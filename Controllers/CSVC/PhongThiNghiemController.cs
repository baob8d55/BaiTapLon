﻿using System;
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
    public class PhongThiNghiemController : Controller
    {
        private readonly ApiServices ApiServices_;

        public PhongThiNghiemController(ApiServices services)
        {
            ApiServices_ = services;
        }
        private async Task<List<TbPhongThiNghiem>> TbPhongThiNghiems()
        {
            List<TbPhongThiNghiem> tbPhongThiNghiems = await ApiServices_.GetAll<TbPhongThiNghiem>("/api/csvc/PhongThiNghiem");

            List<TbCongTrinhCoSoVatChat> tbCongTrinhCoSoVatChats = await ApiServices_.GetAll<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat");

            List<DmLinhVucNghienCuu> dmlinhVucNghienCuus = await ApiServices_.GetAll<DmLinhVucNghienCuu>("/api/dm/LinhVucNghienCuu");

            List<DmLoaiPhongThiNghiem> dmloaiPhongThiNghiems = await ApiServices_.GetAll<DmLoaiPhongThiNghiem>("/api/dm/LoaiPhongThiNghiem");


            tbPhongThiNghiems.ForEach(item => {
                item.IdCongTrinhCsvcNavigation = tbCongTrinhCoSoVatChats.FirstOrDefault(x => x.IdCongTrinhCoSoVatChat == item.IdCongTrinhCsvc);

                item.IdLinhVucNavigation = dmlinhVucNghienCuus.FirstOrDefault(x => x.IdLinhVucNghienCuu == item.IdLinhVuc);

                item.IdLoaiPhongThiNghiemNavigation = dmloaiPhongThiNghiems.FirstOrDefault(x => x.IdLoaiPhongThiNghiem == item.IdLoaiPhongThiNghiem);


            });
            return tbPhongThiNghiems;
        }


        // GET: PhongThiNghiem
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbPhongThiNghiem> getall = await TbPhongThiNghiems();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }


        }

        // GET: PhongThiNghiem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbPhongThiNghiems = await TbPhongThiNghiems();
                var tbPhongThiNghiem = tbPhongThiNghiems.FirstOrDefault(m => m.IdPhongThiNghiem == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbPhongThiNghiem == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbPhongThiNghiem);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: PhongThiNghiem/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCongTrinhCsvc"] = new SelectList(await ApiServices_.GetAll<DmTuyChon>("/api/dm/TuyChon"), "IdTuyChon", "IdTuyChon");
                ViewData["IdLinhVuc"] = new SelectList(await ApiServices_.GetAll<DmLinhVucNghienCuu>("/api/dm/LinhVucNghienCuu"), "IdLinhVucNghienCuu", "LinhVucNghienCuu");

                ViewData["IdLoaiPhongThiNghiem"] = new SelectList(await ApiServices_.GetAll<DmLoaiPhongThiNghiem>("/api/dm/LoaiPhongThiNghiem"), "IdLoaiPhongThiNghiem", "LoaiPhongThiNghiem");


                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        //ngay lam xong ngay 1:02 2/12/2024
        // POST: PhongThiNghiem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPhongThiNghiem,IdCongTrinhCsvc,IdLoaiPhongThiNghiem,IdLinhVuc,MucDoDapUngNhuCauNckh,NamDuaVaoSuDung")] TbPhongThiNghiem tbPhongThiNghiem)
        {
            try
            {
                // Nếu trùng IdPhongThiNghiem sẽ báo lỗi
                if (await TbPhongThiNghiemExists(tbPhongThiNghiem.IdPhongThiNghiem)) ModelState.AddModelError("IdPhongThiNghiem", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbPhongThiNghiem>("/api/csvc/PhongThiNghiem", tbPhongThiNghiem);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdCongTrinhCsvc"] = new SelectList(await ApiServices_.GetAll<DmTuyChon>("/api/dm/TuyChon"), "IdTuyChon", "IdTuyChon", tbPhongThiNghiem.IdCongTrinhCsvc);

                ViewData["IdLinhVuc"] = new SelectList(await ApiServices_.GetAll<DmLinhVucNghienCuu>("/api/dm/LinhVucNghienCuu"), "IdLinhVucNghienCuu", "IdLinhVucNghienCuu", tbPhongThiNghiem.IdLinhVuc);

                ViewData["IdLoaiPhongThiNghiem"] = new SelectList(await ApiServices_.GetAll<DmLoaiPhongThiNghiem>("/api/dm/LoaiPhongThiNghiem"), "IdLoaiPhongThiNghiem", "IdLoaiPhongThiNghiem", tbPhongThiNghiem.IdLoaiPhongThiNghiem);

                return View(tbPhongThiNghiem);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: PhongThiNghiem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbPhongThiNghiem = await ApiServices_.GetId<TbPhongThiNghiem>("/api/csvc/PhongThiNghiem", id ?? 0);
                if (tbPhongThiNghiem == null)
                {
                    return NotFound();
                }
                ViewData["IdCongTrinhCsvc"] = new SelectList(await ApiServices_.GetAll<DmTuyChon>("/api/dm/TuyChon"), "IdTuyChon", "IdTuyChon", tbPhongThiNghiem.IdCongTrinhCsvc);


                ViewData["IdLinhVuc"] = new SelectList(await ApiServices_.GetAll<DmLinhVucNghienCuu>("/api/dm/LinhVucNghienCuu"), "IdLinhVucNghienCuu", "LinhVucNghienCuu", tbPhongThiNghiem.IdLinhVuc);


                ViewData["IdLoaiPhongThiNghiem"] = new SelectList(await ApiServices_.GetAll<DmLoaiPhongThiNghiem>("/api/dm/LoaiPhongThiNghiem"), "IdLoaiPhongThiNghiem", "LoaiPhongThiNghiem", tbPhongThiNghiem.IdLoaiPhongThiNghiem);

                return View(tbPhongThiNghiem);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: PhongThiNghiem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPhongThiNghiem,IdCongTrinhCsvc,IdLoaiPhongThiNghiem,IdLinhVuc,MucDoDapUngNhuCauNckh,NamDuaVaoSuDung")] TbPhongThiNghiem tbPhongThiNghiem)
        {
            try
            {
                if (id != tbPhongThiNghiem.IdPhongThiNghiem)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbPhongThiNghiem>("/api/csvc/PhongThiNghiem", id, tbPhongThiNghiem);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbPhongThiNghiemExists(tbPhongThiNghiem.IdPhongThiNghiem) == false)
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
                ViewData["IdCongTrinhCsvc"] = new SelectList(await ApiServices_.GetAll<DmTuyChon>("/api/dm/TuyChon"), "IdTuyChon", "IdTuyChon", tbPhongThiNghiem.IdCongTrinhCsvc);

                ViewData["IdLinhVuc"] = new SelectList(await ApiServices_.GetAll<DmLinhVucNghienCuu>("/api/dm/LinhVucNghienCuu"), "IdLinhVucNghienCuu", "IdLinhVucNghienCuu", tbPhongThiNghiem.IdLinhVuc);

                ViewData["IdLoaiPhongThiNghiem"] = new SelectList(await ApiServices_.GetAll<DmLoaiPhongThiNghiem>("/api/dm/LoaiPhongThiNghiem"), "IdLoaiPhongThiNghiem", "IdLoaiPhongThiNghiem", tbPhongThiNghiem.IdLoaiPhongThiNghiem);

                return View(tbPhongThiNghiem);



            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: PhongThiNghiem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbPhongThiNghiems = await TbPhongThiNghiems();
                var tbPhongThiNghiem = tbPhongThiNghiems.FirstOrDefault(m => m.IdPhongThiNghiem == id);
                if (tbPhongThiNghiem == null)
                {
                    return NotFound();
                }

                return View(tbPhongThiNghiem);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: PhongThiNghiem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbPhongThiNghiem>("/api/csvc/PhongThiNghiem", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        private async Task<bool> TbPhongThiNghiemExists(int id)
        {
            var tbPhongThiNghiems = await ApiServices_.GetAll<TbPhongThiNghiem>("/api/csvc/PhongThiNghiem");
            return tbPhongThiNghiems.Any(e => e.IdPhongThiNghiem == id);
        }
    }
}