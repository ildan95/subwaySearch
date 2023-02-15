using SubwaySearch.Model;

namespace SubwaySearch.Search;

public class SearchResult
{
    public readonly int StationsCount;
    public readonly int TransitionsCount;

    public readonly IReadOnlyList<IReadOnlyList<IReadableStation>> Ways;

    public bool IsExists => Ways.Count > 0;

    public SearchResult(IReadOnlyList<IReadOnlyList<IReadableStation>> ways, int transitionsCount)
    {
        StationsCount = ways.Count > 0 ? ways[0].Count : 0;
        TransitionsCount = transitionsCount;
        Ways = ways;
    }
}