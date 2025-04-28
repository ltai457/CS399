// CapstoneShowcase.Infrastructure/Data/AppDbContext.cs
using CapstoneShowcase.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CapstoneShowcase.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        public DbSet<Student> Students { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<ProjectBadge> ProjectBadges { get; set; }
        public DbSet<TeamFormationProfile> TeamFormationProfiles { get; set; }
        public DbSet<ProjectBid> ProjectBids { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Student configuration
            modelBuilder.Entity<Student>()
                .HasKey(s => s.UserId);
                
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId);
            
            // TeamMember configuration (composite key)
            modelBuilder.Entity<TeamMember>()
                .HasKey(tm => new { tm.TeamId, tm.UserId });
                
            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.Team)
                .WithMany(t => t.Members)
                .HasForeignKey(tm => tm.TeamId);
                
            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.User)
                .WithMany(u => u.TeamMemberships)
                .HasForeignKey(tm => tm.UserId);
            
            // Like configuration (composite key)
            modelBuilder.Entity<Like>()
                .HasKey(l => new { l.ProjectId, l.UserId });
                
            modelBuilder.Entity<Like>()
                .HasOne(l => l.Project)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.ProjectId);
                
            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId);
                
            // ProjectBadge configuration (composite key)
            modelBuilder.Entity<ProjectBadge>()
                .HasKey(pb => new { pb.ProjectId, pb.BadgeId });
                
            modelBuilder.Entity<ProjectBadge>()
                .HasOne(pb => pb.Project)
                .WithMany(p => p.Badges)
                .HasForeignKey(pb => pb.ProjectId);
                
            modelBuilder.Entity<ProjectBadge>()
                .HasOne(pb => pb.Badge)
                .WithMany(b => b.Projects)
                .HasForeignKey(pb => pb.BadgeId);
                
            modelBuilder.Entity<ProjectBadge>()
                .HasOne(pb => pb.AwardedBy)
                .WithMany(u => u.AwardedBadges)
                .HasForeignKey(pb => pb.AwardedByUserId);
                
            // TeamFormationProfile configuration
            modelBuilder.Entity<TeamFormationProfile>()
                .HasKey(tfp => tfp.StudentId);
                
            modelBuilder.Entity<TeamFormationProfile>()
                .HasOne(tfp => tfp.Student)
                .WithOne(s => s.TeamFormationProfile)
                .HasForeignKey<TeamFormationProfile>(tfp => tfp.StudentId);
                
            // ProjectBid configuration (composite key)
            modelBuilder.Entity<ProjectBid>()
                .HasKey(pb => new { pb.StudentId, pb.ProjectId });
                
            modelBuilder.Entity<ProjectBid>()
                .HasOne(pb => pb.Student)
                .WithMany(s => s.ProjectBids)
                .HasForeignKey(pb => pb.StudentId);
                
            modelBuilder.Entity<ProjectBid>()
                .HasOne(pb => pb.Project)
                .WithMany(p => p.Bids)
                .HasForeignKey(pb => pb.ProjectId);
                
            // Comments configuration
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Project)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.ProjectId);
                
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);
                
            // Project configuration
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Projects)
                .HasForeignKey(p => p.TeamId);
        }
    }
}