using AuthVideo.Domain.Entities;
using AuthVideo.Domain.InfrastructureInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthVideo.Infrastructure.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void Add(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(User entity)
        {
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public User? Get(string identicName)
        {
            return _context.Users.FirstOrDefault(u => u.Login == identicName);
        }

        public IEnumerable<User> GetAll()
        {
            try
            {
                return _context.Users.Where(u => u != null).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public IEnumerable<User> GetAllByName(string name)
        {
            try
            {
                return _context.Users.Where(u => u.Login == name);
            }
            catch
            {
                throw;
            }
        }

        public User? GetById(long id)
        {
            return _context.Users.Find(id);
        }

        public void UpdateEntity(User entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }
    }
}
