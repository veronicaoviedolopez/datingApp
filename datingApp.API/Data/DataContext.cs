using datingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace datingApp.API.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options){}

    public DbSet<Value> Values {get; set;}
    public DbSet<User> Users {get; set;}

    public DbSet<Photo> Photos { get; set; }

    public DbSet<Like> Likes { get; set; }

    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
      modelBuilder.Entity<Like>()
        .HasKey(y => new { y.LikerId, y.LikeeId });

      modelBuilder.Entity<Like>()
        .HasOne(u => u.Likee)
        .WithMany(u => u.likers)
        .HasForeignKey(u => u.LikeeId)
        .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Like>()
        .HasOne(u => u.Liker)
        .WithMany(u => u.likees)
        .HasForeignKey(u => u.LikerId)
        .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Message>()
        .HasOne(u => u.sender)
        .WithMany(m => m.messagesSent)
        .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Message>()
        .HasOne(u => u.recipient)
        .WithMany(m => m.messagesReceived)
        .OnDelete(DeleteBehavior.Restrict);
      
    }
  }
}