using AuthVideo.Domain.Entities;
using AuthVideo.Domain.InfrastructureInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthVideo.Infrastructure.Repositories
{
    public class ContentRepository : IRepository<Content>
    {
        private readonly ApplicationContext _context;

        public ContentRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void Add(Content entity)
        {
            _context.Contents.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Content entity)
        {
            _context.Contents.Remove(entity);
            _context.SaveChanges();
        }

        public Content? Get(string identicName)
        {
            return _context.Contents.FirstOrDefault(c => c.VideoURL == identicName);
        }

        public IEnumerable<Content> GetAll()
        {
            try
            {
                return _context.Contents.Where(c => c != null).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Content> GetAllByName(string name)
        {
            try
            {
                return _context.Contents.Where(c => c.Title == name);
            }
            catch
            {
                throw;
            }
        }

        public Content? GetById(long id)
        {
            return _context.Contents.Find(id);
        }

        public void UpdateEntity(Content entity)
        {
            _context.Contents.Update(entity);
            _context.SaveChanges();
        }
    }
}
