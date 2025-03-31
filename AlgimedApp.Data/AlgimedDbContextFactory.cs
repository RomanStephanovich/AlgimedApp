using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgimedApp.Data
{
    public class AlgimedDbContextFactory : IDesignTimeDbContextFactory<AlgimedDbContext>
    {
        public AlgimedDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AlgimedDbContext>();
            optionsBuilder.UseSqlite("Data Source=algimed.db");

            return new AlgimedDbContext(optionsBuilder.Options);
        }
    }
}