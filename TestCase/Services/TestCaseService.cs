using TestCase.Entities;
using TestCase.Repository;
using System.Linq.Expressions;

namespace TestCase.Services
{
    public class TestCaseService
    {
        private readonly IRepository<ProjectTestCase> _testCaseRepository;
        private readonly IRepository<TestCaseTag> _testCaseTagRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<TestCaseAttachment> _testCaseAttachmentRepository;
        private readonly IRepository<Attachment> _attachmentRepository;

        public TestCaseService(
            IRepository<ProjectTestCase> testCaseRepository,
            IRepository<TestCaseTag> testCaseTagRepository,
            IRepository<Tag> tagRepository,
            IRepository<TestCaseAttachment> testCaseAttachmentRepository,
            IRepository<Attachment> attachmentRepository)
        {
            _testCaseRepository = testCaseRepository;
            _testCaseTagRepository = testCaseTagRepository;
            _tagRepository = tagRepository;
            _testCaseAttachmentRepository = testCaseAttachmentRepository;
            _attachmentRepository = attachmentRepository;
        }

        public IEnumerable<ProjectTestCase> GetTestCasesByModuleId(int moduleId)
        {
            return _testCaseRepository.Find(tc => tc.ModuleId == moduleId);
        }

        public ProjectTestCase? GetTestCaseById(int id)
        {
            return _testCaseRepository.GetById(id);
        }

        public void CreateTestCase(ProjectTestCase testCase)
        {
            testCase.CreatedUser = "system";
            testCase.UpdateUser = "system";
            _testCaseRepository.Add(testCase);
        }

        public void UpdateTestCase(ProjectTestCase testCase)
        {
            testCase.UpdateUser = "system";
            _testCaseRepository.Update(testCase);
        }

        public void DeleteTestCase(int id)
        {
            var testCase = _testCaseRepository.GetById(id);
            if (testCase != null)
            {
                _testCaseRepository.Remove(testCase);
            }
        }

        public IEnumerable<Tag> GetTagsByTestCaseId(int testCaseId)
        {
            var testCaseTags = _testCaseTagRepository.Find(tct => tct.TestCaseId == testCaseId);
            var tagIds = testCaseTags.Select(tct => tct.TagId);
            return _tagRepository.Find(t => tagIds.Contains(t.Id));
        }

        public void AddTagToTestCase(int testCaseId, int tagId)
        {
            var testCaseTag = new TestCaseTag
            {
                TestCaseId = testCaseId,
                TagId = tagId,
                CreatedUser = "system",
                UpdateUser = "system"
            };
            _testCaseTagRepository.Add(testCaseTag);
        }

        public void RemoveTagFromTestCase(int testCaseId, int tagId)
        {
            var testCaseTag = _testCaseTagRepository.Find(tct => tct.TestCaseId == testCaseId && tct.TagId == tagId).FirstOrDefault();
            if (testCaseTag != null)
            {
                _testCaseTagRepository.Remove(testCaseTag);
            }
        }
        
        public IEnumerable<Attachment> GetAttachmentsByTestCaseId(int testCaseId)
        {
            var testCaseAttachments = _testCaseAttachmentRepository.Find(tca => tca.TestCaseId == testCaseId);
            var attachmentIds = testCaseAttachments.Select(tca => tca.AttachmentId).ToList();
            return _attachmentRepository.Find(a => attachmentIds.Contains(a.Id));
        }
        
        public void AddAttachmentToTestCase(int testCaseId, int attachmentId)
        {
            var testCaseAttachment = new TestCaseAttachment
            {
                TestCaseId = testCaseId,
                AttachmentId = attachmentId,
                CreatedUser = "system",
                UpdateUser = "system"
            };
            _testCaseAttachmentRepository.Add(testCaseAttachment);
        }
        
        public void RemoveAttachmentFromTestCase(int testCaseId, int attachmentId)
        {
            var testCaseAttachment = _testCaseAttachmentRepository.Find(tca => tca.TestCaseId == testCaseId && tca.AttachmentId == attachmentId).FirstOrDefault();
            if (testCaseAttachment != null)
            {
                _testCaseAttachmentRepository.Remove(testCaseAttachment);
            }
        }
    }
}