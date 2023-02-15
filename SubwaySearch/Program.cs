using SubwaySearch.Model;
using SubwaySearch.Search;

namespace SubwaySearch
{
    public static class Program
    {
        public static void Main()
        {
            var subway = new Subway();
            subway.AddBranch("red", new[] { "A", "B1", "C1", "D1", "E1", "F1" });
            subway.AddBranch("black", new[] { "B2", "H", "J1", "F2", "G" });
            subway.AddBranch("blue", new[] { "N", "L1", "D2", "J2", "O" });
            subway.AddBranch("green", new[] { "L2", "K", "C2", "J3", "E2", "M" }, true);
            subway.AddTransitions("B1", "B2");
            subway.AddTransitions("C1", "C2");
            subway.AddTransitions("D1", "D2");
            subway.AddTransitions("E1", "E2");
            subway.AddTransitions("F1", "F2");
            subway.AddTransitions("L1", "L2");
            subway.AddTransitions("J1", "J2", "J3");

            var searcher = new RouteSearcher(subway);

            var result = searcher.Search("M", "A");

            if (!result.IsExists)
            {
                Console.WriteLine("No ways found");
                return;
            }

            Console.WriteLine(
                $"Found {result.Ways.Count} ways with {result.StationsCount} stations and {result.TransitionsCount} transitions:");
            foreach (var way in result.Ways)
            {
                Console.WriteLine(string.Join(" -> ", way.Select(station => station.Id)));
            }
        }
    }
}