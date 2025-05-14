using Microsoft.EntityFrameworkCore;
using todo.Models;

namespace todo.Datas;

public class DataContext: DbContext
{
    public DbSet<ToDoModel> Tasks { get; set; }
    
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        // Database.EnsureCreated();
    }
}