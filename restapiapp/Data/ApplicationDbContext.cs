using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using restapiapp.Models;

namespace restapiapp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public virtual DbSet<RegisterModel> Registers { get; set; }
    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Module> Modules { get; set; }
    public virtual DbSet<UserCourse> UserCourses { get; set; }

}
