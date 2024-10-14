using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Application.Interfaces;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        private readonly IUserContextService _userContextService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUserContextService userContextService) : base(options)
        {
            _userContextService = userContextService;
        }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Membership>()
                .HasOne(m => m.User).WithMany().HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Membership>()
                .HasOne(m => m.Restaurant).WithMany().HasForeignKey(m => m.RestaurantId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.InvitedUser).WithMany().HasForeignKey(i => i.InvitedUserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.InvitingUser).WithMany().HasForeignKey(i => i.InvitingUserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Restaurant).WithMany().HasForeignKey(i => i.RestaurantId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Restaurant).WithOne().HasForeignKey<Chat>(c => c.RestaurantId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChatMessage>()
                .HasOne(m => m.Chat).WithMany(c => c.Messages).HasForeignKey(m => m.ChatId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatMessage>()
                .HasOne(m => m.Sender).WithMany().HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.Restrict);

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is AuditableEntity &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            Guid userId = _userContextService.GetUserId();

            if(userId != Guid.Empty)
            {
                foreach (var entityEntry in entries)
                {
                    var auditableEntity = (AuditableEntity)entityEntry.Entity;


                        if (entityEntry.State == EntityState.Added)
                        {
                            auditableEntity.CreatedAt = DateTime.UtcNow;
                            auditableEntity.CreatedBy = userId;
                        }
                        else
                        {
                            auditableEntity.UpdatedAt = DateTime.UtcNow;
                            auditableEntity.UpdatedBy = userId;
                        }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
