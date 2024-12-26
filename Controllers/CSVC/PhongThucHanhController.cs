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
using Newtonsoft.Json;

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

        // Lấy chi tiết 1 bản ghi dựa theo ID tương ứng đã truyền vào (IdCPhongThuchanh)
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
                // Hiển thị thông thi chi tiết PhongThucHanh thành công
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
                // Nếu trùng IDPhongThucHanh sẽ báo lỗi
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

        // POST: PhongThucHanh/Delete
        // Xóa PhongThucHanh khỏi Database sau khi nhấn xác nhận 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Lệnh xác nhận xóa hẳn một PhongThucHanh
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

                // Danh sách lưu các đối tượng TbPhongThucHanh
                List<TbPhongThucHanh> lst = new List<TbPhongThucHanh>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();

                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    if (item.Count < 4) // Kiểm tra nếu dòng dữ liệu không đủ số cột
                    {
                        return BadRequest(Json(new { msg = "Dữ liệu không đầy đủ." }));
                    }

                    TbPhongThucHanh model = new TbPhongThucHanh();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (await TbPhongThucHanhExists(id)); // Kiểm tra id có tồn tại không

                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdPhongThucHanh = id;// gán id
                    model.MucDoDapUngNhuCauNckh = item[0];// Gán mức độ đáp ứng nhu cầu NCKH cho cột đầu tiên
                    model.NamDuaVaoSuDung = item[1];// Gán năm đưa vào sử dụng (chuyển đổi từ string sang int)
                    model.IdCongTrinhCsvc = ParseInt(item[2]);// Gán id công trình csvc (chuyển đổi từ string sang int)
                    model.IdLinhVuc = ParseInt(item[3]);// Gán lĩnh vực (chuyển đổi từ string sang int)

                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu
                foreach (var item in lst)
                {
                    await CreateTbPhongThucHanh(item);
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbPhongThucHanh(TbPhongThucHanh item)
        {
            await ApiServices_.Create<TbPhongThucHanh>("/api/csvc/PhongThucHanh", item);
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
   
