using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TestCase.Entities;
using TestCase.Services;

namespace TestCase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ImageService _imageService;
        private readonly AppSettngs _appSettings;

        public ImageController(ImageService imageService, IOptions<AppSettngs> appSettings)
        {
            _imageService = imageService;
            _appSettings = appSettings.Value;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("文件为空");
                }

                // 检查文件类型是否为图片
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("只允许上传图片文件 (jpg, jpeg, png, gif, bmp, webp)");
                }

                // 确保图片存储目录存在
                var imagesPath = _appSettings.ImagesPath;
                if (string.IsNullOrEmpty(imagesPath))
                {
                    return StatusCode(500, "图片存储路径未配置");
                }

                if (!Directory.Exists(imagesPath))
                {
                    Directory.CreateDirectory(imagesPath);
                }

                // 生成唯一文件名
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(imagesPath, uniqueFileName);

                // 保存文件
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // 保存图片信息到数据库
                var image = new Image
                {
                    Name = file.FileName,
                    FilePath = filePath
                };
                
                _imageService.CreateImage(image);

                // 返回图片URL，供wangEditor使用
                var imageUrl = Url.Action("GetImage", "Image", new { id = image.Id }, Request.Scheme);
                
                return Ok(new { url = imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"上传图片时发生错误: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetImage(int id)
        {
            try
            {
                var image = _imageService.GetImageById(id);
                
                if (image == null)
                {
                    return NotFound("图片未找到");
                }

                if (!System.IO.File.Exists(image.FilePath))
                {
                    return NotFound("图片文件未找到");
                }

                var fileExtension = Path.GetExtension(image.FilePath)?.ToLowerInvariant();
                var contentType = GetContentType(fileExtension);

                var fileBytes = System.IO.File.ReadAllBytes(image.FilePath);
                return File(fileBytes, contentType, image.Name);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"获取图片时发生错误: {ex.Message}");
            }
        }

        private string GetContentType(string fileExtension)
        {
            return fileExtension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                _ => "image/jpeg"
            };
        }
    }
}