using System.Diagnostics;

namespace PerformanceCounterExplorer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PerformanceCounterCategory[] categories = PerformanceCounterCategory.GetCategories();
            foreach (var category in categories)
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("No argument given!\nArgument must be a category name");
                    break;
                }
                if (category.CategoryName != args[0])
                {
                    continue;
                }
                Console.WriteLine(category.CategoryName);
                var instances = category.GetInstanceNames();
                if (instances.Length > 0)
                {
                    foreach (var intance in instances)
                    {
                        foreach (var counter in category.GetCounters(intance))
                        {
                            Console.WriteLine("\t- " + intance.ToString() + ": " + counter.CounterName);
                        }
                    }
                }
                else
                {
                    foreach (var counter in category.GetCounters())
                    {
                        Console.WriteLine("\t- " + counter.CounterName);
                    }
                }
            }
            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
        }
    }
}