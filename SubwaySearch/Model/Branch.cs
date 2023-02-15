namespace SubwaySearch.Model;

internal class Branch
{
    private readonly Dictionary<string, Station> _stations = new();

    public string Id { get; }
    public IEnumerable<Station> Stations => _stations.Values;
    public Station? First { get; }
    public Station? Last { get; }


    public Branch(string id)
    {
        Id = id;
    }

    public Branch(string id, IEnumerable<string> stationIds) : this(id)
    {
        Station? current = null;
        foreach (var stationId in stationIds)
        {
            if (current == null)
            {
                current = new Station(stationId, id);
                First = current;
            }
            else
            {
                current = current.AddStationAfter(stationId);
            }

            _stations.Add(stationId, current);
        }

        Last = current;
    }

    public bool TryGetStation(string id, out Station? station)
    {
        return _stations.TryGetValue(id, out station);
    }

    public void Loop()
    {
        if (First == null || Last == null)
        {
            Console.WriteLine($"branch '{Id}' is empty");
            return;
        }

        First.LoopTo(Last);
    }
}