using TestCase.Entities;
using TestCase.Repository;

namespace TestCase.Services
{
    public class ModuleService
    {
        private readonly IRepository<Module> _moduleRepository;

        public ModuleService(IRepository<Module> moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        public IEnumerable<Module> GetModulesByProjectId(int projectId)
        {
            return _moduleRepository.Find(m => m.ProjectId == projectId);
        }

        public Module? GetModuleById(int id)
        {
            return _moduleRepository.GetById(id);
        }

        public void CreateModule(Module module)
        {
            module.CreatedUser = "system";
            module.UpdateUser = "system";
            _moduleRepository.Add(module);
        }

        public void UpdateModule(Module module)
        {
            module.UpdateUser = "system";
            _moduleRepository.Update(module);
        }

        public void DeleteModule(int id)
        {
            var module = _moduleRepository.GetById(id);
            if (module != null)
            {
                _moduleRepository.Remove(module);
            }
        }
    }
}