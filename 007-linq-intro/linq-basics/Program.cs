List<int> numbers = [1, 2, 3, 4, 5, 6];
HashSet<double> set = [6, 5, 4, 3, 2, 1];

IEnumerable<int> filteredResults = numbers.Filter(i => i % 2 != 0);
IEnumerable<int> results = numbers.Map(i => i * i);

//Map(set, n => n * 2);
//Map(Filter(numbers, i => i <= 4), n => n * 2);

var r1 = set.Map(n => n * 2)
            .Filter(i => i > 10);
var r2 = numbers.Filter(i => i <= 4)
                .Map(n => n * 3);


int count = 0;
foreach (var result in results)
{
    count++;
    if (count == 3)
    {
        break;
    }
    System.Console.WriteLine(result);
}


static class Extensions
{
    extension<T>(IEnumerable<T> iterable)
    {
        public IEnumerable<T> Map(Func<T, T> mapper)
        {
            foreach (var item in iterable)
            {
                yield return mapper(item);
            }
        }

        public IEnumerable<T> Filter(Func<T, bool> predicate)
        {
            foreach (var number in iterable)
            {
                if (predicate(number))
                {
                    yield return number;
                }
            }
        }

    }

}

// delegate bool Predicate(int number);
// delegate int Mapper(int number);