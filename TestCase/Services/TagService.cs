using TestCase.Entities;
using TestCase.Repository;
using System.Linq.Expressions;

namespace TestCase.Services
{
    public class TagService
    {
        private readonly IRepository<Tag> _tagRepository;

        public TagService(IRepository<Tag> tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return _tagRepository.GetAll();
        }

        public Tag? GetTagById(int id)
        {
            return _tagRepository.GetById(id);
        }

        public void CreateTag(Tag tag)
        {
            tag.CreatedUser = "system";
            tag.UpdateUser = "system";
            _tagRepository.Add(tag);
        }

        public void UpdateTag(Tag tag)
        {
            tag.UpdateUser = "system";
            _tagRepository.Update(tag);
        }

        public void DeleteTag(int id)
        {
            var tag = _tagRepository.GetById(id);
            if (tag != null)
            {
                _tagRepository.Remove(tag);
            }
        }
    }
}