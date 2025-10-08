using TestCase.Entities;
using TestCase.Repository;
using System.Linq.Expressions;

namespace TestCase.Services
{
    public class ImageService
    {
        private readonly IRepository<Image> _imageRepository;

        public ImageService(IRepository<Image> imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public Image? GetImageById(int id)
        {
            return _imageRepository.GetById(id);
        }

        public IEnumerable<Image> GetAllImages()
        {
            return _imageRepository.GetAll();
        }

        public IEnumerable<Image> FindImages(Expression<Func<Image, bool>> predicate)
        {
            return _imageRepository.Find(predicate);
        }

        public void CreateImage(Image image)
        {
            image.CreatedUser = "system";
            image.UpdateUser = "system";
            image.CreatedTime = DateTime.Now;
            image.UpdatedTime = DateTime.Now;
            _imageRepository.Add(image);
        }

        public void UpdateImage(Image image)
        {
            image.UpdateUser = "system";
            image.UpdatedTime = DateTime.Now;
            _imageRepository.Update(image);
        }

        public void DeleteImage(Image image)
        {
            _imageRepository.Remove(image);
        }
    }
}