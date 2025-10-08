using System.ComponentModel.DataAnnotations;

namespace TestCase.Entities
{
    public class User : BaseEntity
    {
        /// <summary>
        /// 工号
        /// </summary>
        [Required]
        public string EmployeeId { get; set; } = string.Empty;

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 电子邮件
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}