namespace EfDemo;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class DemoContext 
(DbContextOptions<DemoContext> options) 
    : DbContext(options)
{
    public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        var todoItem = modelBuilder.Entity<ToDoItem>();
        todoItem.HasKey(td => td.Id);
        todoItem.Property(td => td.Id).ValueGeneratedOnAdd();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Remove(typeof(TableNameFromDbSetConvention)); 
    }
    
}

public class ToDoItem 
{
    public int Id { get; set; }
    public required string Text { get; set; } 
    public bool IsDone { get; set; }
}