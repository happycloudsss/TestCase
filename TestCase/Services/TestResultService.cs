using TestCase.Entities;
using TestCase.Repository;
using System.Linq.Expressions;

namespace TestCase.Services
{
    public class TestResultService
    {
        private readonly IRepository<TestResult> _testResultRepository;
        private readonly IRepository<TestResultImage> _testResultImageRepository;
        private readonly IRepository<Image> _imageRepository;
        private readonly IRepository<TestResultAttachment> _testResultAttachmentRepository;
        private readonly AttachmentService _attachmentService;


        public TestResultService(
            IRepository<TestResult> testResultRepository,
            IRepository<TestResultImage> testResultImageRepository,
            IRepository<Image> imageRepository,
            IRepository<TestResultAttachment> testResultAttachmentRepository,
            AttachmentService attachmentService)
        {
            _testResultRepository = testResultRepository;
            _testResultImageRepository = testResultImageRepository;
            _imageRepository = imageRepository;
            _testResultAttachmentRepository = testResultAttachmentRepository;
            _attachmentService = attachmentService;
        }

        public IEnumerable<TestResult> GetTestResultsByTestCaseId(int testCaseId)
        {
            return _testResultRepository.Find(tr => tr.TestCaseId == testCaseId)
                .OrderByDescending(tr => tr.CreatedTime);
        }

        public IEnumerable<TestResult> GetTestResultsByTestCaseIdAndTestRound(int testCaseId, int testRound)
        {
            return _testResultRepository.Find(tr => tr.TestCaseId == testCaseId && tr.TestRound == testRound)
                .OrderByDescending(tr => tr.CreatedTime);
        }

        public TestResult? GetTestResultById(int id)
        {
            return _testResultRepository.GetById(id);
        }

        public void CreateTestResult(TestResult testResult)
        {
            testResult.CreatedUser = "system";
            testResult.UpdateUser = "system";
            _testResultRepository.Add(testResult);
        }

        public void UpdateTestResult(TestResult testResult)
        {
            testResult.UpdateUser = "system";
            _testResultRepository.Update(testResult);
        }

        public void DeleteTestResult(int id)
        {
            var testResult = _testResultRepository.GetById(id);
            if (testResult != null)
            {
                _testResultRepository.Remove(testResult);
            }
        }

        public IEnumerable<Image> GetImagesByTestResultId(int testResultId)
        {
            var testResultImages = _testResultImageRepository.Find(tri => tri.TestResultId == testResultId);
            var imageIds = testResultImages.Select(tri => tri.ImageId);
            return _imageRepository.Find(i => imageIds.Contains(i.Id));
        }

        public void AddImageToTestResult(int testResultId, int imageId)
        {
            var testResultImage = new TestResultImage
            {
                TestResultId = testResultId,
                ImageId = imageId,
                CreatedUser = "system",
                UpdateUser = "system"
            };
            _testResultImageRepository.Add(testResultImage);
        }

        public void RemoveImageFromTestResult(int testResultId, int imageId)
        {
            var testResultImages = _testResultImageRepository.Find(tri => tri.TestResultId == testResultId && tri.ImageId == imageId);
            _testResultImageRepository.RemoveRange(testResultImages);
        }

        public IEnumerable<Attachment> GetAttachmentsByTestResultId(int testResultId)
        {
            var testResultAttachments = _testResultAttachmentRepository.Find(tra => tra.TestResultId == testResultId);
            var attachmentIds = testResultAttachments.Select(tra => tra.AttachmentId);
            return attachmentIds.Select(id => _attachmentService.GetAttachmentById(id.ToString())).Where(a => a != null)!;
        }

        public void AddAttachmentToTestResult(int testResultId, int attachmentId)
        {
            var existingRelation = _testResultAttachmentRepository.Find(tra => tra.TestResultId == testResultId && tra.AttachmentId == attachmentId).Any();

            if (!existingRelation)
            {
                var testResultAttachment = new TestResultAttachment
                {
                    TestResultId = testResultId,
                    AttachmentId = attachmentId,
                    CreatedUser = "system",
                    UpdateUser = "system"
                };
                _testResultAttachmentRepository.Add(testResultAttachment);
            }
        }

        public void RemoveAttachmentFromTestResult(int testResultId, int attachmentId)
        {
            var testResultAttachments = _testResultAttachmentRepository.Find(tra => tra.TestResultId == testResultId && tra.AttachmentId == attachmentId);
            _testResultAttachmentRepository.RemoveRange(testResultAttachments);
        }
    }
}