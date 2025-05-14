using Microsoft.EntityFrameworkCore;
using todo.Models;

namespace todo.Datas;

public class DataTesterContext : DbContext
{
    public DbSet<ToDoModel> TasksTester { get; set; }
    
    public DataTesterContext(DbContextOptions<DataTesterContext> options) : base(options)
    {
        // Database.EnsureCreated();
    }
}