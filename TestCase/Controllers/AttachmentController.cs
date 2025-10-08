using Microsoft.AspNetCore.Mvc;
using TestCase.Entities;
using TestCase.Services;

namespace TestCase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttachmentController : ControllerBase
    {
        private readonly AttachmentService _attachmentService;

        public AttachmentController(AttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [HttpGet("{id}")]
        public IActionResult DownloadAttachment(int id)
        {
            try
            {
                // 获取附件信息
                var attachment = _attachmentService.GetAttachmentById(id.ToString());
                
                if (attachment == null)
                {
                    return NotFound("附件未找到");
                }

                // 检查文件是否存在
                if (!System.IO.File.Exists(attachment.FilePath))
                {
                    return NotFound("文件未找到");
                }

                // 获取文件扩展名以确定内容类型
                var fileExtension = System.IO.Path.GetExtension(attachment.FilePath)?.ToLowerInvariant();
                var contentType = GetContentType(fileExtension);

                // 返回文件
                var fileBytes = System.IO.File.ReadAllBytes(attachment.FilePath);
                return File(fileBytes, contentType, attachment.FileName);
            }
            catch (Exception ex)
            {
                // 记录异常日志（如果配置了日志系统）
                return StatusCode(500, "下载文件时发生错误: " + ex.Message);
            }
        }

        private string GetContentType(string fileExtension)
        {
            return fileExtension switch
            {
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _ => "application/octet-stream"
            };
        }
    }
}