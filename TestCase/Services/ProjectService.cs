using TestCase.Entities;
using TestCase.Repository;

namespace TestCase.Services
{
    public class ProjectService
    {
        private readonly IRepository<Project> _projectRepository;

        public ProjectService(IRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return _projectRepository.GetAll();
        }

        public Project? GetProjectById(int id)
        {
            return _projectRepository.GetById(id);
        }

        public void CreateProject(Project project)
        {
            project.CreatedUser = "system";
            project.UpdateUser = "system";
            _projectRepository.Add(project);
        }

        public void UpdateProject(Project project)
        {
            project.UpdateUser = "system";
            _projectRepository.Update(project);
        }

        public void DeleteProject(int id)
        {
            var project = _projectRepository.GetById(id);
            if (project != null)
            {
                _projectRepository.Remove(project);
            }
        }
    }
}