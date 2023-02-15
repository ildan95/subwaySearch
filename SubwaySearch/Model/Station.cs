namespace SubwaySearch.Model;

internal class Station : IReadableStation
{
    public string Id { get; }
    public string BranchId { get; }

    public IReadableStation? Next => _next;
    public IReadableStation? Prev => _prev;


    private readonly HashSet<Station> _transitionsTo = new();
    private Station? _prev;
    private Station? _next;

    public Station(string id, string branchId)
    {
        Id = id;
        BranchId = branchId;
    }

    private Station(string id, string branchId, Station? next, Station? prev) : this(id, branchId)
    {
        _next = next;
        _prev = prev;
    }

    public IEnumerable<IReadableStation> GetTransitions() => _transitionsTo;
    public bool HasTransitions => _transitionsTo.Count > 0;

    public Station AddStationBefore(string stationId)
    {
        var station = new Station(stationId, BranchId, this, _prev);

        if (_prev != null)
        {
            _prev._next = station;
        }

        _prev = station;
        return station;
    }

    public Station AddStationAfter(string stationId)
    {
        var station = new Station(stationId, BranchId, _next, this);

        if (_next != null)
        {
            _next._prev = station;
        }

        _next = station;
        return station;
    }

    public void AddTransition(Station station)
    {
        if (BranchId == station.BranchId)
        {
            Console.WriteLine("Can't add transition for stations on same branch");
            return;
        }

        _transitionsTo.Add(station);
    }

    public void LoopTo(Station station)
    {
        if (BranchId != station.BranchId)
        {
            Console.WriteLine("Can't connect stations on different branches");
            return;
        }

        if (!((_next == null && station._prev == null) || (_prev == null && station._next == null)))
        {
            Console.WriteLine("Can't connect not end stations");
            return;
        }

        if (_next == null)
        {
            _next = station;
            station._prev = this;
        }
        else
        {
            _prev = station;
            station._next = this;
        }
    }
}