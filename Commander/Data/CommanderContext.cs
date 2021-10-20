using Commander.Models;
using Microsoft.EntityFrameworkCore;

namespace Commander.Data
{
    public class CommanderContext : DbContext
    {
        // call constructor from DbContext
        public CommanderContext(DbContextOptions<CommanderContext> opt ):base(opt)
        {

        }

        // create representation on Command in db as a DBSet
        public DbSet<Command> Commands { get; set; }
    }
}
