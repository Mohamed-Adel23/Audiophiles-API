using Audiophiles_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Audiophiles_API.Data
{
    public class AudioDbContext: IdentityDbContext<AudioUser>
    {
        public AudioDbContext(DbContextOptions<AudioDbContext> options) : base(options) { }

        // DbSets
        public DbSet<UserContact> UserContacts { get; set; }
        public DbSet<AdminRespond> AdminResponds { get; set; }
    }
}
