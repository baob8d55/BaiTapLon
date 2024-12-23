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
using Microsoft.AspNetCore.Http.HttpResults;

namespace BaiTapLon.Controllers.CSVC
{
    public class DatDaiController : Controller
    {/// <summary>
     /// khởi tạo một controller cho phép truy cập và thao tác dữ liệu trong bảng TbDatDai
     /// </summary>
        private readonly ApiServices ApiServices_;

        public DatDaiController(ApiServices services)
        {
            ApiServices_ = services;
        }

        // GET: ChuongTrinhDaoTao
        // Lấy danh sách CTĐT từ database, trả về view Index.

        private async Task<List<TbDatDai>> TbDatDais()
        {
            List<TbDatDai> tbDatDais = await ApiServices_.GetAll<TbDatDai>("/api/csvc/DatDai");
            List < DmHinhThucSoHuu > dmhinhThucSoHuus = await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu");
            tbDatDais.ForEach(item => {
                item.IdHinhThucSoHuuNavigation = dmhinhThucSoHuus.FirstOrDefault(x => x.IdHinhThucSoHuu == item.IdHinhThucSoHuu);
            });
            return tbDatDais;
        }

        // GET: DatDai
        //Truy vấn tất cả dữ liệu từ bảng TbDatDais và đưa vào view
        //lấy dữ liệu từ cơ sở dữ liệu (bao gồm cả các thông tin liên quan) và trả về một view với danh sách dữ liệu đó.
        //Nếu có lỗi xảy ra trong quá trình lấy dữ liệu, nó sẽ trả về mã trạng thái HTTP 400.
        public async Task<IActionResult> Index()
        {
            //try
            {
                List<TbDatDai> getall = await TbDatDais();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            //catch (Exception ex)
            {
                return BadRequest();
            }

        }
        public async Task<IActionResult> chart()
        {
            //try
            {
                List<TbDatDai> getall = await TbDatDais();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            //catch (Exception ex)
            {
                return BadRequest();
            }

        }



        // GET: DatDai/Details/
        //Kiểm tra nếu id là null, nếu có, trả về NotFound.
        //Tìm một bản ghi cụ thể trong bảng TbDatDais dựa trên id và trả về view chi tiết nếu tìm thấy.
        public async Task<IActionResult> Details(int? id)
        {
          //  try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbDatDais = await TbDatDais();
                var tbDatDai = tbDatDais.FirstOrDefault(m => m.IdDatDai == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbDatDai == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbDatDai);
            }
          //  catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: DatDai/Create
        //Tạo view để nhập thông tin cho một bản ghi mới, bao gồm danh sách chọn lựa cho trường IdHinhThucSoHuu.
        public async Task<IActionResult> Create()
        {
          //  try
            {
                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu");
                return View();
            }
            //catch (Exception ex)
            {
                return BadRequest();
            }


        }
        //Xử lý dữ liệu được gửi từ form. Nếu IdDatDai đã tồn tại, thêm lỗi vào ModelState.
        //Nếu ModelState hợp lệ, thêm bản ghi vào cơ sở dữ liệu và chuyển hướng về trang danh sách.
        // POST: DatDai/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?Lin kId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDatDai,MaGiayChungNhanQuyenSoHuu,DienTichDat,IdHinhThucSoHuu,TenDonViSoHuu,MinhChungQuyenSoHuuDatDai,MucDichSuDungDat,NamBatDauSuDungDat,ThoiGianSuDungDat,DienTichDatDaSuDung")] TbDatDai tbDatDai)
        {
            //try
            {
                // Nếu trùng IDChuongTrinhDaoTao sẽ báo lỗi
                if (await TbDatDaiExists(tbDatDai.IdDatDai)) ModelState.AddModelError("IdDatDai", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbDatDai>("/api/csvc/DatDai", tbDatDai);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu", tbDatDai.IdHinhThucSoHuu);

                return View(tbDatDai);
            }
            //catch (Exception ex)
            {
                return BadRequest();
            }


        }

        // GET: DatDai/Edit/5
        //Kiểm tra nếu id là null. Nếu có, trả về NotFound.
        //Lấy bản ghi cần sửa và trả về view chỉnh sửa.
        public async Task<IActionResult> Edit(int? id)
        {
         //   try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbDatDai = await ApiServices_.GetId<TbDatDai>("/api/csvc/DatDai", id ?? 0);
                if (tbDatDai == null)
                {
                    return NotFound();
                }
                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu", tbDatDai.IdHinhThucSoHuu);

                return View(tbDatDai);
            }
          //  catch (Exception ex)
            {
                return BadRequest();
            }
        }
        //Kiểm tra nếu id và tbDatDai.IdDatDai không khớp, trả về NotFound.
        //Nếu ModelState hợp lệ, cập nhật bản ghi và lưu thay đổi.
        // POST: DatDai/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDatDai,MaGiayChungNhanQuyenSoHuu,DienTichDat,IdHinhThucSoHuu,TenDonViSoHuu,MinhChungQuyenSoHuuDatDai,MucDichSuDungDat,NamBatDauSuDungDat,ThoiGianSuDungDat,DienTichDatDaSuDung")] TbDatDai tbDatDai)
        {
       //     try
            {
                if (id != tbDatDai.IdDatDai)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbDatDai>("/api/csvc/DatDai", id, tbDatDai);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbDatDaiExists(tbDatDai.IdDatDai) == false)
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
                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu", tbDatDai.IdHinhThucSoHuu);
  

                return View(tbDatDai);
            }
          //  catch (Exception ex)
            {
                return BadRequest();
            }

        }
        //iểm tra nếu id là null và tìm bản ghi để xóa
        // GET: DatDai/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
         //   try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbDatDais = await TbDatDais();
                var tbDatDai = tbDatDais.FirstOrDefault(m => m.IdDatDai == id);
                if (tbDatDai == null)
                {
                    return NotFound();
                }

                return View(tbDatDai);
            }
          //  catch (Exception ex)
            {
                return BadRequest();
            }

        }
        //Xác nhận việc xóa một bản ghi. Nếu bản ghi tồn tại, xóa và lưu thay đổi.
        // POST: DatDai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
          //  try
            {
                await ApiServices_.Delete<TbDatDai>("/api/csvc/DatDai", id);
                return RedirectToAction(nameof(Index));
            }
            //catch (Exception ex)
            {
                return BadRequest();
            }

        }

        private async Task<bool> TbDatDaiExists(int id)
        {
            var tbDatDais = await ApiServices_.GetAll<TbDatDai>("/api/csvc/DatDai");
            return tbDatDais.Any(e => e.IdDatDai == id);
        }
    }
}
