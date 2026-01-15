using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Npgsql;
using EfDemo;

Console.WriteLine("*** EfDemo ***");

var context = new DemoContextFactory().CreateDbContext([]); 

context.Students.AddRange(
                          new Student
                          {
                              FirstName = "Max",
                              LastName = "Mustermann",
                              EarlyWarnings = ["Late submission of homework", "Low attendance"],
                              Hobbies =
                              [
                                  new Hobby { Title = "Football", MonthlyCost = 30.0m },
                                  new Hobby { Title = "Chess", MonthlyCost = 10.0m }
                              ],
                              TodoItems =
                              [
                                  new TodoItem { Text = "Learn EF Core", IsDone = false },
                                  new TodoItem { Text = "Write demo app", IsDone = false },
                                  new TodoItem { Text = "Test app", IsDone = false }
                              ]
                          },
                          new Student
                          {
                              FirstName = "Erika",
                              LastName = "Musterfrau",
                              EarlyWarnings = ["Missed project deadline", "Incomplete assignments"],
                              Hobbies =
                              [
                                  new Hobby { Title = "Painting", MonthlyCost = 25.0m },
                                  new Hobby { Title = "Cycling", MonthlyCost = 15.0m },
                                  new Hobby { Title = "Very very expensive hobby", MonthlyCost = 9999.99m }
                              ],
                              TodoItems =
                              [
                                  new TodoItem { Text = "Read EF Core docs", IsDone = true },
                                  new TodoItem { Text = "Create sample data", IsDone = false }
                              ]
                          },
                          new Student
                          {
                              FirstName = "John",
                              LastName = "Doe",
                              EarlyWarnings = [],
                              Hobbies = [],
                              TodoItems =
                              [
                                  new TodoItem { Text = "Set up database", IsDone = true },
                                  new TodoItem { Text = "Implement features", IsDone = false },
                                  new TodoItem { Text = "Fix bugs", IsDone = false },
                                  new TodoItem { Text = "Deploy application", IsDone = false }
                              ]
                          }
                         );

await context.SaveChangesAsync();

var olles = await context.Students.Include(s => s.TodoItems).ToListAsync();

var s = olles.Last();
s.EarlyWarnings = [];
s.TodoItems[0].IsDone = true;
s.TodoItems.Remove(s.TodoItems[1]);
s.TodoItems.Add(new TodoItem()
{
    Text = "Schlafen"
});

await context.SaveChangesAsync();

var avgCost = await context.Students
                     .Where(st => st.FirstName == "Erika")
                     .SelectMany(st => st.Hobbies.Select(h => h.MonthlyCost))
                     .AverageAsync();
var henry = await context.Students
                         .Select(st => new
                         {
                             Student = st,
                             CountNotDone = st.TodoItems.Count(td => !td.IsDone)
                         })
                         .GroupBy(a => a.CountNotDone)
                         .SelectMany(g => g.Select(a => a.Student.EarlyWarnings))
                         .ToListAsync();
try
{
    List<TodoItem> allTodoItems = await context.TodoItems.ToListAsync();
    Console.WriteLine($"Todo Count: {allTodoItems.Count}");
}
catch (PostgresException ex) when (ex.SqlState == "42P01")
{
    Console.WriteLine("Fails initially, because tables have not yet been created");
}

public sealed class DemoContextFactory : IDesignTimeDbContextFactory<DemoContext>
{
    public DemoContext CreateDbContext(string[] _)
    {
        DbContextOptionsBuilder<DemoContext> optionsBuilder = new DbContextOptionsBuilder<DemoContext>()
            .UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=postgres", 
            options => options.UseNodaTime()) 
            .ConfigureWarnings(warnings =>
                warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning)) 
            .EnableSensitiveDataLogging() 
            .EnableDetailedErrors() 
            .LogTo(Console.WriteLine, LogLevel.Information); 

        return new DemoContext(optionsBuilder.Options); 
    }
}