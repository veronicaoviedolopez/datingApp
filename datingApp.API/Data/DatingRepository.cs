using System;
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
      var users =  _context.Users.Include(p => p.photos).OrderByDescending(u => u.lastActive).AsQueryable();
      users = users.Where(u => u.Id != userParams.userId && u.gender == userParams.Gender);


      if (userParams.Likers)
      {
        var userLikers = await GetUserLikes(userParams.userId, userParams.Likers);
        users = users.Where(u => userLikers.Contains(u.Id));
      }
      
      if (userParams.Likees)
      {
        var userLikes = await GetUserLikes(userParams.userId, userParams.Likers);
        users = users.Where(u => userLikes.Contains(u.Id));
      }

      

      if(userParams.MinAge != 18 || userParams.MaxAge != 99){
          var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
          var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
          users = users.Where(u => u.dateOfBirth >= minDob && u.dateOfBirth <= maxDob);
      }

      if(!string.IsNullOrEmpty(userParams.OrderBy)){
        switch (userParams.OrderBy)
        {
            case "created":
              users = users.OrderByDescending(u => u.created);
              break;
            default:
              users = users.OrderByDescending(u => u.lastActive);
              break;
        }
      }

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

    public async Task<Like> GetLike(int userId, int recipientId)
    {
      return await _context.Likes.FirstOrDefaultAsync(x=> x.LikerId == userId && x.LikeeId == recipientId);
    }

    private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
    {
      var user = await _context.Users
        .Include(x=> x.likers)
        .Include(x => x.likees)
        .FirstOrDefaultAsync(x => x.Id == id);

      if (likers)
        return user.likers.Where(x => x.LikeeId == id).Select(i => i.LikerId);
      else
        return user.likees.Where(x => x.LikerId == id).Select(i => i.LikeeId);
    
    }
  }
}