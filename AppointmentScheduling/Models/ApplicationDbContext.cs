using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduling.Models
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //public DbSet<Item> Items { get; set; }
        //public DbSet<Expense> Expenses { get; set; }
        ////ExpenseCategories
        //public DbSet<ExpenseCategory> ExpenseCategories { get; set; }

    }
}
