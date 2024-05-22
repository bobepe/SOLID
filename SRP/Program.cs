using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRP
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }

    // bad attitude
    public class UserCreatorBAD
    {
        public void CreateUser(string username, string email, string password)
        {
            // Validation logic
            if (!ValidateEmail(email))
            {
                throw new ArgumentException("Invalid email format.");
            }
            // Business rules
            // Database persistence
            SaveUserToDatabase(username, email, password);
        }
        private bool ValidateEmail(string email)
        {
            // Validation logic
            return true;
        }
        private void SaveUserToDatabase(string username, string email, string password)
        {
            // Database persistence logic
        }
    }

    // SRP attitude
    // Každá třída má vlastní odpovědnost
    public class UserValidator
    {
        public bool ValidateEmail(string email)
        {
            // Validation logic
            return true;
        }
    }
    public class UserRepository
    {
        public void SaveUser(string username, string email, string password)
        {
            // Database persistence logic
        }
    }
    public class UserCreator
    {
        private readonly UserValidator _validator;
        private readonly UserRepository _repository;
        public UserCreator(UserValidator validator, UserRepository repository)
        {
            _validator = validator;
            _repository = repository;
        }
        public void CreateUser(string username, string email, string password)
        {
            if (!_validator.ValidateEmail(email))
            {
                throw new ArgumentException("Invalid email format.");
            }
            // Business rules
            _repository.SaveUser(username, email, password);
        }
    }
}
