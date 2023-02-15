namespace SubwaySearch.Model;

public class Subway : IStationSearcher
{
    private readonly Dictionary<string, Branch> _branches = new();

    public IReadableStation? FindStation(string stationId)
    {
        return GetStation(stationId);
    }

    public void AddBranch(string id, IEnumerable<string> stationIds, bool isLoop = false)
    {
        var branch = new Branch(id, stationIds);
        if (isLoop)
        {
            branch.Loop();
        }

        _branches[id] = branch;
    }

    public void LoopBranch(string id)
    {
        if (!_branches.TryGetValue(id, out var branch))
        {
            Console.WriteLine($"Branch '{id}' doesn't exists");
            return;
        }

        branch.Loop();
    }

    private Station? GetStation(string stationId, string branchId)
    {
        if (!_branches.TryGetValue(branchId, out var branch))
        {
            return null;
        }

        return branch.TryGetStation(stationId, out var station) ? station : null;
    }

    private Station? GetStation(string stationId)
    {
        return _branches.Keys.Select(branchId => GetStation(stationId, branchId))
            .FirstOrDefault(station => station != null);
    }

    public void AddTransitions(params string[] stationIds)
    {
        var stations = GetStations(stationIds);

        foreach (var station in stations)
        {
            foreach (var otherStation in stations.Where(otherStation => station != otherStation))
            {
                station.AddTransition(otherStation);
                otherStation.AddTransition(station);
            }
        }
    }

    private List<Station> GetStations(IEnumerable<string> stationIds)
    {
        var stations = new List<Station>();
        foreach (var stationId in stationIds)
        {
            var station = GetStation(stationId);
            if (station == null)
            {
                Console.WriteLine($"Station '{stationId}' not found. Skip");
                continue;
            }

            stations.Add(station);
        }

        return stations;
    }
}