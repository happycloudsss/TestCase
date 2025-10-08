using TestCase.Entities;
using TestCase.Repository;
using System.Linq.Expressions;

namespace TestCase.Services
{
    public class UserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public User? GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public User? GetUserByEmployeeId(string employeeId)
        {
            return _userRepository.Find(u => u.EmployeeId == employeeId).FirstOrDefault();
        }

        public IEnumerable<User> GetUsersByName(string name)
        {
            return _userRepository.Find(u => u.Name.Contains(name));
        }

        public void CreateUser(User user)
        {
            user.CreatedUser = "system";
            user.UpdateUser = "system";
            _userRepository.Add(user);
        }

        public void UpdateUser(User user)
        {
            user.UpdateUser = "system";
            _userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user != null)
            {
                _userRepository.Remove(user);
            }
        }
    }
}