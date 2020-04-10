using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebSite.Core.Entities;
using WebSite.DataAcces.Identity;
using System;
using System.Collections.Generic;

namespace WebSite.DataAccess
{
    public static class ApplicationSeed
    {
        private static Guid AuthorId => Guid.Parse("c6248606-ed80-4c2e-89b3-353d183fe284");
        private static Guid blogId => Guid.Parse("22795939-9e17-492e-a890-9234ca8f5a08");
        private static Guid postId => Guid.Parse("f11fe6c8-0d5b-4c6b-9673-035d47d4396e");
        private static Guid adminId => Guid.Parse("37a0a393-bacf-48c3-b947-c45c53d467ee");
        private static Guid adminRoleId => Guid.Parse("7e2a48a9-ffeb-4b9c-bf87-4990ea281ec7");

        public static void ApplySeed(ModelBuilder builder)
        {
            builder.Entity<AuthorEntity>().HasData(Authors());
            builder.Entity<BlogEntity>().HasData(Blogs());
            builder.Entity<PostEntity>().HasData(Posts());
            builder.Entity<ReplyEntity>().HasData(Comments());
            builder.Entity<ApplicationUser>().HasData(ApplicationUsers());
            builder.Entity<IdentityRole>().HasData(Roles());
            builder.Entity<IdentityUserRole<string>>().HasData(UserRoles());
        }

        private static List<AuthorEntity> Authors()
        {
            return new List<AuthorEntity>
            {
                new AuthorEntity
                {
                    Id = AuthorId,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Image = System.IO.File.ReadAllBytes(".//wwwroot//images//EQUIVISION-Background-Logos-Overlay-Blue-to-Red-1500x1500.png"),
                    Name = "Bojan"
                }
            };
        }

        private static List<BlogEntity> Blogs()
        {
            return new List<BlogEntity>
            {
                new BlogEntity
                {
                    Id = blogId,
                    Cover = System.IO.File.ReadAllBytes(".//wwwroot//images//blog-logo-7E0C67F0D1-seeklogo.com.png"),
                    Created = DateTime.Now,
                    Description = "default description",
                    Name = "default",
                    Modified = DateTime.Now
                }
            };
        }

        private static List<PostEntity> Posts()
        {
            return new List<PostEntity>
            {
                new PostEntity
                {
                    Id = postId,
                    AuthorId = AuthorId,
                    BlogId = blogId,
                    Archieved = true,
                    Pinned = true,
                    Content = "Default Text",
                    Title = "Default Title",
                    Cover = System.IO.File.ReadAllBytes(".//wwwroot//images//developer-solid-2-17-512.png"),
                    Views = 120,
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                }
            };
        }

        private static List<ReplyEntity> Comments()
        {
            return new List<ReplyEntity>
            {
                new ReplyEntity
                {
                    Id = Guid.NewGuid(),
                    PostId = postId,
                    AuthorId = AuthorId,
                    Content = "Default Comment",
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                }
            };
        }

        private static List<IdentityRole> Roles()
        {
            return new List<IdentityRole>
            {
                new IdentityRole {
                    Id = adminRoleId.ToString(),
                    Name = ApplicationRoles.Administrator,
                    NormalizedName = ApplicationRoles.Administrator
                }
            };
        }

        private static List<ApplicationUser> ApplicationUsers()
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            return new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = adminId.ToString(),
                    AuthorId = AuthorId,
                    UserName = "Bojan",
                    NormalizedUserName = "bojan",
                    Email = "kovacevskib@gmail.com",
                    NormalizedEmail = "kovacevskib@gmail.com",
                    PasswordHash = hasher.HashPassword(null, "bojan123"),
                    SecurityStamp = string.Empty,
                    EmailConfirmed = true
                }
            };
        }

        private static List<IdentityUserRole<string>> UserRoles()
        {
            return new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    UserId = adminId.ToString(),
                    RoleId = adminRoleId.ToString()
                }
            };
        }
    }
}
