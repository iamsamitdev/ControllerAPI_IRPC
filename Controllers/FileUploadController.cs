using Microsoft.AspNetCore.Mvc;
namespace ControllerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController: ControllerBase
{
    // กำหนดตัวแปรเก็บที่เก็บไฟล์อัพโหลด
    private readonly string _uploadFolder;

    public FileUploadController()
    {
        // กำหนดที่เก็บไฟล์อัพโหลด
        _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        // ถ้าไม่มีโฟลเดอร์ uploads ให้สร้างโฟลเดอร์
        if (!Directory.Exists(_uploadFolder))
        {
            Directory.CreateDirectory(_uploadFolder); // สร้างโฟลเดอร์ uploads
        }
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        // ถ้าไม่มีไฟล์อัพโหลด
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        // กำหนดชื่อไฟล์ที่จะเก็บ
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        // กำหนดที่เก็บไฟล์ที่จะเก็บ
        var filePath = Path.Combine(_uploadFolder, fileName);

        // อัพโหลดไฟล์
        // เช็คประเภทไฟล์
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var ext = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(ext))
        {
            return BadRequest("Invalid file type.");
        }

        // เช็คขนาดไฟล์ไม่เกิน 6MB
        if (file.Length > 6 * 1024 * 1024) // 6MB
        {
            return BadRequest("File size cannot exceed 6MB.");
        }

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // ส่งชื่อไฟล์กลับไป
        return Ok(fileName);
    }
}