namespace SubwaySearch.Search;

internal class PointsStorage
{
    private readonly Dictionary<string, int> _searchPoints = new();

    public void Clear()
    {
        _searchPoints.Clear();
    }

    public bool TryGet(string id, out int points)
    {
        return _searchPoints.TryGetValue(id, out points);
    }

    public bool Has(string id)
    {
        return _searchPoints.ContainsKey(id);
    }

    public void Set(string id, int point)
    {
        _searchPoints[id] = point;
    }
}