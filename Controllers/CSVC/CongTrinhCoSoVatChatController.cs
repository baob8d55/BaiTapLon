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
using Newtonsoft.Json;

namespace BaiTapLon.Controllers.CSVC
{
    public class CongTrinhCoSoVatChatController : Controller
    {
        private readonly ApiServices ApiServices_;
        // Lấy từ HemisContext 
        public CongTrinhCoSoVatChatController(ApiServices services)
        {
            ApiServices_ = services;
        }
        // GET: CongTrinhCoSoVatChat
        // Lấy danh sách Congtrinhcosovatchat từ database, trả về view Index.

        private async Task<List<TbCongTrinhCoSoVatChat>> TbCongTrinhCoSoVatChats()
        {
            List<TbCongTrinhCoSoVatChat> tbCongTrinhCoSoVatChats = await ApiServices_.GetAll<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat");
            List<DmTuyChon> dmtuyChons = await ApiServices_.GetAll<DmTuyChon>("/api/dm/TuyChon");

            List<DmHinhThucSoHuu> dmhinhThucSoHuus = await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu");

            List<DmLoaiCongTrinhCoSoVatChat> dmloaiCongTrinhCoSoVatChats = await ApiServices_.GetAll<DmLoaiCongTrinhCoSoVatChat>("/api/dm/LoaiCongTrinhCoSoVatChat");

            List<DmMucDichSuDungCsvc> dmmucDichSuDungCsvcs = await ApiServices_.GetAll<DmMucDichSuDungCsvc>("/api/dm/MucDichSuDungCsvc");

            List<DmTinhTrangCoSoVatChat> dmtinhTrangCoSoVatChats = await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat");

            tbCongTrinhCoSoVatChats.ForEach(item => {
                item.CongTrinhCsvctrongNhaNavigation = dmtuyChons.FirstOrDefault(x => x.IdTuyChon == item.CongTrinhCsvctrongNha);

                item.IdHinhThucSoHuuNavigation = dmhinhThucSoHuus.FirstOrDefault(x => x.IdHinhThucSoHuu == item.IdHinhThucSoHuu);

                item.IdLoaiCongTrinhNavigation = dmloaiCongTrinhCoSoVatChats.FirstOrDefault(x => x.IdLoaiCongTrinhCoSoVatChat == item.IdLoaiCongTrinh);

                item.IdMucDichSuDungNavigation = dmmucDichSuDungCsvcs.FirstOrDefault(x => x.IdMucDichSuDungCsvc == item.IdMucDichSuDung);

                item.IdTinhTrangCsvcNavigation = dmtinhTrangCoSoVatChats.FirstOrDefault(x => x.IdTinhTrangCoSoVatChat == item.IdTinhTrangCsvc);


            });
            return tbCongTrinhCoSoVatChats;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbCongTrinhCoSoVatChat> getall = await TbCongTrinhCoSoVatChats();
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
                List<TbCongTrinhCoSoVatChat> getall = await TbCongTrinhCoSoVatChats();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }


        // GET: CongTrinhCoSoVatChat/Details/5
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
                var tbCongTrinhCoSoVatChats = await TbCongTrinhCoSoVatChats();
                var tbCongTrinhCoSoVatChat = tbCongTrinhCoSoVatChats.FirstOrDefault(m => m.IdCongTrinhCoSoVatChat == id);

                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbCongTrinhCoSoVatChat == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbCongTrinhCoSoVatChat);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        // GET: CongTrinhCoSoVatChat/Create
      
        // Hiển thị view Create để tạo một bản ghi CTCSVC mới
        // Truyền data từ các table khác hiển thị tại view Create (khóa ngoài)
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["CongTrinhCsvctrongNha"] = new SelectList(await ApiServices_.GetAll<DmTuyChon>("/api/dm/TuyChon"), "IdTuyChon", "TuyChon");


                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu");

                ViewData["IdLoaiCongTrinh"] = new SelectList(await ApiServices_.GetAll<DmLoaiCongTrinhCoSoVatChat>("/api/dm/LoaiCongTrinhCoSoVatChat"), "IdLoaiCongTrinhCoSoVatChat", "LoaiCongTrinhCoSoVatChat");


                ViewData["IdMucDichSuDung"] = new SelectList(await ApiServices_.GetAll<DmMucDichSuDungCsvc>("/api/dm/MucDichSuDungCsvc"), "IdMucDichSuDungCsvc", "MucDichSuDungCsvc");

                ViewData["IdTinhTrangCsvc"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat");

                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }


        // POST: CongTrinhCoSoVatChat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Thêm một CSVC mới vào Database nếu IdCongTrinhCoSoVatChat truyền vào không trùng với Id đã có trong Database
        // Trong trường hợp nhập trùng IdCongTrinhCoSoVatChatsẽ bắt lỗi
        // Bắt lỗi ngoại lệ sao cho người nhập BẮT BUỘC phải nhập khác IdCongTrinhCoSoVatChato đã có
        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Create([Bind("IdCongTrinhCoSoVatChat,MaCongTrinh,TenCongTrinh,IdLoaiCongTrinh,IdMucDichSuDung,DoiTuongSuDung,DienTichSanXayDung,VonBanDau,VonDauTu,IdTinhTrangCsvc,IdHinhThucSoHuu,CongTrinhCsvctrongNha,SoPhongOcongVu,SoChoOchoCanBoGiangDay,NamDuaVaoSuDung")] TbCongTrinhCoSoVatChat tbCongTrinhCoSoVatChat)
        {
            try
            {
                // Nếu trùng IDChuongTrinhDaoTao sẽ báo lỗi
                if (await TbCongTrinhCoSoVatChatExists(tbCongTrinhCoSoVatChat.IdCongTrinhCoSoVatChat)) ModelState.AddModelError("IdChuongTrinhDaoTao", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat", tbCongTrinhCoSoVatChat);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CongTrinhCsvctrongNha"] = new SelectList(await ApiServices_.GetAll<DmTuyChon>("/api/dm/TuyChon"), "IdTuyChon", "TuyChon", tbCongTrinhCoSoVatChat.CongTrinhCsvctrongNha);

                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu", tbCongTrinhCoSoVatChat.IdHinhThucSoHuu);

                ViewData["IdLoaiCongTrinh"] = new SelectList(await ApiServices_.GetAll<DmLoaiCongTrinhCoSoVatChat>("/api/dm/LoaiCongTrinhCoSoVatChat"), "IdLoaiCongTrinhCoSoVatChat", "LoaiCongTrinhCoSoVatChat", tbCongTrinhCoSoVatChat.IdLoaiCongTrinh);

                ViewData["IdMucDichSuDung"] = new SelectList(await ApiServices_.GetAll<DmMucDichSuDungCsvc>("/api/dm/MucDichSuDungCsvc"), "IdMucDichSuDungCsvc", "MucDichSuDungCsvc", tbCongTrinhCoSoVatChat.IdMucDichSuDung);

                ViewData["IdTinhTrangCsvc"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat", tbCongTrinhCoSoVatChat.IdTinhTrangCsvc);
                return View(tbCongTrinhCoSoVatChat);
                // 3:07 đã làm tới đây (chiều)
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: CongTrinhCoSoVatChat/Edit/5
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

                var tbCongTrinhCoSoVatChat = await ApiServices_.GetId < TbCongTrinhCoSoVatChat> ("/api/csvc/CongTrinhCoSoVatChat", id ?? 0);
                if (tbCongTrinhCoSoVatChat == null)
                {   
                    return NotFound();
                }
                ViewData["CongTrinhCsvctrongNha"] = new SelectList(await ApiServices_.GetAll<DmTuyChon>("/api/dm/TuyChon"), "IdTuyChon", "TuyChon", tbCongTrinhCoSoVatChat.CongTrinhCsvctrongNha);

                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu", tbCongTrinhCoSoVatChat.IdHinhThucSoHuu);

                ViewData["IdLoaiCongTrinh"] = new SelectList(await ApiServices_.GetAll<DmLoaiCongTrinhCoSoVatChat>("/api/dm/LoaiCongTrinhCoSoVatChat"), "IdLoaiCongTrinhCoSoVatChat", "LoaiCongTrinhCoSoVatChat", tbCongTrinhCoSoVatChat.IdLoaiCongTrinh);

                ViewData["IdMucDichSuDung"] = new SelectList(await ApiServices_.GetAll<DmMucDichSuDungCsvc>("/api/dm/MucDichSuDungCsvc"), "IdMucDichSuDungCsvc", "MucDichSuDungCsvc", tbCongTrinhCoSoVatChat.IdMucDichSuDung);

                ViewData["IdTinhTrangCsvc"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat", tbCongTrinhCoSoVatChat.IdTinhTrangCsvc);
                return View(tbCongTrinhCoSoVatChat);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: CongTrinhCoSoVatChat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
               // Lưu data mới (ghi đè) vào các trường Data đã có thuộc IdChuongTrinhDaoTao cần chỉnh sửa
        // Nó chỉ cập nhật khi ModelState hợp lệ
        // Nếu không hợp lệ sẽ báo lỗi, vì vậy cần có bắt lỗi.
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCongTrinhCoSoVatChat,MaCongTrinh,TenCongTrinh,IdLoaiCongTrinh,IdMucDichSuDung,DoiTuongSuDung,DienTichSanXayDung,VonBanDau,VonDauTu,IdTinhTrangCsvc,IdHinhThucSoHuu,CongTrinhCsvctrongNha,SoPhongOcongVu,SoChoOchoCanBoGiangDay,NamDuaVaoSuDung")] TbCongTrinhCoSoVatChat tbCongTrinhCoSoVatChat)
        {
            try
            {
                if (id != tbCongTrinhCoSoVatChat.IdCongTrinhCoSoVatChat)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat", id, tbCongTrinhCoSoVatChat);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbCongTrinhCoSoVatChatExists(tbCongTrinhCoSoVatChat.IdCongTrinhCoSoVatChat) == false)
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
                ViewData["CongTrinhCsvctrongNha"] = new SelectList(await ApiServices_.GetAll<DmTuyChon>("/api/dm/TuyChon"), "IdTuyChon", "TuyChon", tbCongTrinhCoSoVatChat.CongTrinhCsvctrongNha);

                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu", tbCongTrinhCoSoVatChat.IdHinhThucSoHuu);

                ViewData["IdLoaiCongTrinh"] = new SelectList(await ApiServices_.GetAll<DmLoaiCongTrinhCoSoVatChat>("/api/dm/LoaiCongTrinhCoSoVatChat"), "IdLoaiCongTrinhCoSoVatChat", "LoaiCongTrinhCoSoVatChat", tbCongTrinhCoSoVatChat.IdLoaiCongTrinh);

                ViewData["IdMucDichSuDung"] = new SelectList(await ApiServices_.GetAll<DmMucDichSuDungCsvc>("/api/dm/MucDichSuDungCsvc"), "IdMucDichSuDungCsvc", "MucDichSuDungCsvc", tbCongTrinhCoSoVatChat.IdMucDichSuDung);

                ViewData["IdTinhTrangCsvc"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat", tbCongTrinhCoSoVatChat.IdTinhTrangCsvc);
                return View(tbCongTrinhCoSoVatChat);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: CongTrinhCoSoVatChat/Delete/5
        // Xóa một CTCSVC khỏi Database
        // Lấy data CTCSVC từ Database, hiển thị Data tại view Delete
        // Hàm này để hiển thị thông tin cho người dùng trước khi xóa
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbCongTrinhCoSoVatChats = await TbCongTrinhCoSoVatChats();
                var tbCongTrinhCoSoVatChat = tbCongTrinhCoSoVatChats.FirstOrDefault(m => m.IdCongTrinhCoSoVatChat == id);
                if (tbCongTrinhCoSoVatChat == null)
                {
                    return NotFound();
                }

                return View(tbCongTrinhCoSoVatChat);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: CongTrinhCoSoVatChat/Delete/5
        // Xóa CTCSVC khỏi Database sau khi nhấn xác nhận 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Lệnh xác nhận xóa hẳn một CTCSVC
        {
            try
            {
                await ApiServices_.Delete<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        private async Task<bool> TbCongTrinhCoSoVatChatExists(int id)
        {
            var tbCongTrinhCoSoVatChats = await ApiServices_.GetAll<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat");
            return tbCongTrinhCoSoVatChats.Any(e => e.IdCongTrinhCoSoVatChat == id);
        }
        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "No Lỗi";

                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                if (data == null || !data.Any())
                {
                    return BadRequest(Json(new { msg = "Dữ liệu không hợp lệ." }));
                }

                // Danh sách lưu các đối tượng TbCongTrinhCoSoVatChat
                List<TbCongTrinhCoSoVatChat> lst = new List<TbCongTrinhCoSoVatChat>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();

                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    if (item.Count < 8) // Kiểm tra nếu dòng dữ liệu không đủ số cột
                    {
                        return BadRequest(Json(new { msg = "Dữ liệu không đầy đủ." }));
                    }

                    TbCongTrinhCoSoVatChat model = new TbCongTrinhCoSoVatChat();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (await TbCongTrinhCoSoVatChatExists(id)); // Kiểm tra id có tồn tại không

                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdCongTrinhCoSoVatChat = id;// Gán ID
                    model.MaCongTrinh = item[0];// Gán mã công trình từ cột đầu tiên
                    model.TenCongTrinh = item[1];// Gán tên công trình (chuyển đổi từ string sang int)
                    model.DoiTuongSuDung = item[2];// Gán đói tượng sử dụng (chuyển đổi từ string sang int)
                    model.DienTichSanXayDung = ParseInt(item[3]);// Gán diện tích sàn xây dựng (chuyển đổi từ string sang int)
                    model.VonBanDau = ParseInt(item[4]);// Gán vốn ban đầu (chuyển đổi từ string sang int)
                    model.VonDauTu = ParseInt(item[5]);// Gán vốn đầu tư (chuyển đổi từ string sang int)
                    model.SoPhongOcongVu = ParseInt(item[6]);// Gán số phòng ở công vụ (chuyển đổi từ string sang int)
                    model.SoChoOchoCanBoGiangDay = ParseInt(item[7]);// Gán số chỗ ở cho cán bộ giảng dạy (chuyển đổi từ string sang int)
                    model.NamDuaVaoSuDung = item[8];// Gán năm đưa vào sử dụng(chuyển đổi từ string sang int)
                    model.CongTrinhCsvctrongNha = ParseInt(item[9]);// Gán công trình nhà nước csvc (chuyển đổi từ string sang int)
                    model.IdHinhThucSoHuu = ParseInt(item[10]);// Gán hình thức sở hữu (chuyển đổi từ string sang int)
                    model.IdLoaiCongTrinh = ParseInt(item[11]);// Gán loại công trinh (chuyển đổi từ string sang int)
                    model.IdMucDichSuDung = ParseInt(item[12]);// Gán mục đích sử dụng (chuyển đổi từ string sang int)
                    model.IdTinhTrangCsvc = ParseInt(item[13]);// Gán tình trạng cơ sở vật chất (chuyển đổi từ string sang int)

                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu
                foreach (var item in lst)
                {
                    await CreateTbCongTrinhCoSoVatChat(item);
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbCongTrinhCoSoVatChat(TbCongTrinhCoSoVatChat item)
        {
            await ApiServices_.Create<TbCongTrinhCoSoVatChat>("/api/csvc/CongTrinhCoSoVatChat", item);
        }

        private int? ParseInt(string v)
        {
            if (int.TryParse(v, out int result)) // Nếu chuỗi có thể chuyển thành int
            {
                return result; // Trả về giá trị int
            }
            else
            {
                return null; // Nếu không thể chuyển thành int, trả về null
            }
        }
    }
}
   
