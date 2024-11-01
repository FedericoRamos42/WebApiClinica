using Domain.Entities;
using Domain.InterFaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UserRepository : IUserRepository

    {
       private readonly ApplicationContext _context;
       public UserRepository(ApplicationContext context) 
       {
            _context = context; 
       }
        public IEnumerable<User> GetAll() 
        {
            var list = _context.Set<User>().ToList();

            return list;
        }
        public User? GetById(int id)
        {
            var entity = _context.Set<User>().FirstOrDefault(u => u.Id == id);
            return entity;
        }
        public User? Authenticate(string email, string password)
        {
            User? userToAuthenticate = _context.Set<User>().FirstOrDefault(u => u.Email == email && u.Password == password);
            return userToAuthenticate;
        }
        public User? ValidateEmail (string email) {
            User? validateUserEmail = _context.Set<User>().FirstOrDefault(u=>u.Email == email); 
            return validateUserEmail;
    }
        public User? UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
            return user;
        }
        public IEnumerable<User> GetByStatus (bool? state)
        {
            var query = _context.Set<User>().AsQueryable();

            if (state.HasValue)
            {
                query = query.Where(u => u.IsAvailable == state.Value);
            }

            return query.ToList();
        }
            
}
}
