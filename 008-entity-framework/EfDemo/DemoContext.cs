namespace EfDemo;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class DemoContext
(DbContextOptions<DemoContext> options)
    : DbContext(options)
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<Student> Students => Set<Student>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureTodo(modelBuilder);
        ConfigureStudent(modelBuilder.Entity<Student>());
    }


    private static void ConfigureStudent(EntityTypeBuilder<Student> student)
    {
        student.HasKey(st => st.Id);
        student.Property(st => st.Id).ValueGeneratedOnAdd();
        student.ComplexCollection<Hobby>(st => st.Hobbies).ToJson();
        student.HasMany(st => st.TodoItems)
        .WithOne(td => td.Student)
        .HasForeignKey(td => td.StudentId)
        .OnDelete(DeleteBehavior.Cascade);
    }
    private static void ConfigureTodo(ModelBuilder modelBuilder)
    {
        var todoItem = modelBuilder.Entity<TodoItem>();
        todoItem.HasKey(td => td.Id);
        todoItem.Property(td => td.Id).ValueGeneratedOnAdd();
        todoItem.Property(td => td.Text).HasMaxLength(200); // for example
    }



    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Remove(typeof(TableNameFromDbSetConvention));
    }

}


public class TodoItem
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public bool IsDone { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
}

public class Student
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public List<string> EarlyWarnings { get; set; } = [];
    public List<Hobby> Hobbies { get; set; } = [];
    public List<TodoItem> TodoItems { get; set; } = [];

}


public class Hobby // no id - owned entity
{
    public required string Title { get; set; }
    public decimal MonthlyCost { get; set; } 
}