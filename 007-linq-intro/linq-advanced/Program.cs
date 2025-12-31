IEnumerable<Student> students = [
 new("Alice", "4AHIF", 1.7D),
 new("Ben", "4AHIF", 1.82D),
 new("Grace", "4AHIF", 1.55D),
 new("Cara", "4BHIF", 1.6D),
 new("Diego", "4BHIF", 1.75D),
 new("Hiro", "4BHIF", 1.72D),
 new("Eve", "4CHIF", 1.68D),
 new("Frank", "4CHIF", 1.8D)
];

IEnumerable<StudentWithHobbies> studentsWithHobbies = [
 new("Alice", "4AHIF", 1.7D, []),
 new("Ben", "4AHIF", 1.82D, [new Hobby(1, "Reading")]),
 new("Grace", "4AHIF", 1.55D, [new Hobby(2,"Soccer"), new Hobby(3,"Skiing")]),
 new("Cara", "4BHIF", 1.6D, []),
 new("Diego", "4BHIF", 1.75D, []),
 new("Hiro", "4BHIF", 1.72D, []),
 new("Eve", "4CHIF", 1.68D, []),
 new("Frank", "4CHIF", 1.8D, [])
];

var namesOnly = students
                .Where(s => s.Height > 1.75D)
                //.Select(s => s.Name)
                .OrderByDescending(s => s.Class)
                .ThenBy(s => s.Name.Length);
var namesOnlyMaterialized = namesOnly.ToList();
Console.WriteLine();

var maxHeight = students
            .MaxBy(s => s.Height);
Console.WriteLine(maxHeight);

var averageHeight = students.Average(s => s.Height);
Console.WriteLine(averageHeight);

var diegoExists = students.Any(s => s.Name.StartsWith('D'));

var maxAhifHobbyId = studentsWithHobbies
                    .Where(s => s.Class == "4AHIF")
                    .Select(s => s.Hobbies.Select(h => h.Id).DefaultIfEmpty(0).Max())
                    .ToList();

var studentsByClass = students
                        .GroupBy(s => s.Class)
                        .Where(g => g.Count() > 2)
                        .ToDictionary(g => g.Key, 
                                      g => g.OrderBy(s => s.Height).ToList());


internal sealed record StudentWithHobbies(string Name, string Class, double Height, Hobby[] Hobbies);
internal sealed record Student(string Name, string Class, double Height);

sealed record Hobby(int Id, string Name);