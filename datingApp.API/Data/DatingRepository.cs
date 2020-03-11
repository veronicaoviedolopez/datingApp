using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using datingApp.API.Helpers;
using datingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace datingApp.API.Data
{
  public class DatingRepository : IDatingRepository
  {
    private DataContext _context;

    public DatingRepository(DataContext context) => _context = context;
    public void Add<T>(T entity) where T : class
    {
      _context.Add(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
      _context.Remove(entity);
    }

    public async Task<User> GetUser(int id)
    {
        var user = await  _context.Users.Include(p => p.photos).FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task<PagedList<User>> GetUsers(UserParams userParams)
    {
      var users =  _context.Users.Include(p => p.photos);
      return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
    }

    public async Task<bool> SaveAll()
    {
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Photo> GetPhoto(int id) {
      return await _context.Photos.FirstOrDefaultAsync(p => p.id == id);
    }

    public async Task<Photo> GetMainPhotoForUser(int userId)
    {
      return await _context.Photos.Where(x => x.userId == userId).FirstOrDefaultAsync(x => x.isMain);
    }
  }
}