using TestCase.Entities;
using TestCase.Repository;
using System.Linq.Expressions;

namespace TestCase.Services
{
    public class AttachmentService
    {
        private readonly IRepository<Attachment> _attachmentRepository;

        public AttachmentService(IRepository<Attachment> attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }
        
        public Attachment? GetAttachmentById(string id)
        {
            return _attachmentRepository.Find(a => a.Id.ToString() == id).FirstOrDefault();
        }
        
        public void CreateAttachment(Attachment attachment)
        {
            attachment.CreatedUser = "system";
            attachment.UpdateUser = "system";
            _attachmentRepository.Add(attachment);
        }

        public void DeleteAttachment(Attachment attachment)
        {
            _attachmentRepository.Remove(attachment);
        }
    }
}