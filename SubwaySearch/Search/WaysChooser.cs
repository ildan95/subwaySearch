using SubwaySearch.Model;

namespace SubwaySearch.Search;

internal class WaysChooser
{
    private readonly PointsStorage _storage;
    private readonly IReadableStation _start;
    private readonly IReadableStation _end;
    private int _minTransitionsCount;
    private List<IReadOnlyList<IReadableStation>> _minTransitionWays;

    public WaysChooser(PointsStorage storage, IReadableStation start, IReadableStation end)
    {
        _storage = storage;
        _start = start;
        _end = end;

        _minTransitionsCount = int.MaxValue;
        _minTransitionWays = new List<IReadOnlyList<IReadableStation>>();
    }

    public SearchResult GetWays()
    {
        if (!_storage.TryGet(_end.Id, out var points))
        {
            Console.WriteLine($"no points for end station '{_end.Id}'");
            return default;
        }

        ProcessToStart(new List<IReadableStation> { _end }, points, 0);
        return new SearchResult(_minTransitionWays, _minTransitionsCount);
    }

    private void ProcessToStart(List<IReadableStation> way, int currentPoint, int currentTransitions)
    {
        var last = way.Last();
        if (currentPoint == 0)
        {
            CompleteWay(way, currentTransitions, last);
            return;
        }

        var nextPoint = currentPoint - 1;
        foreach (var relativeStation in GetRelativeStationsWithPoint(last, nextPoint))
        {
            var nextWay = way.Concat(new[] { relativeStation }).ToList();
            var nextTransitionsCount = currentTransitions + (relativeStation.BranchId == last.BranchId ? 0 : 1);
            ProcessToStart(nextWay, nextPoint, nextTransitionsCount);
        }
    }

    private void CompleteWay(List<IReadableStation> way, int currentTransitions, IReadableStation last)
    {
        if (!IsStartStation(last))
        {
            Console.WriteLine("Error! Incorrect way");
            return;
        }

        if (currentTransitions > _minTransitionsCount)
        {
            return;
        }

        if (currentTransitions < _minTransitionsCount)
        {
            _minTransitionsCount = currentTransitions;
            _minTransitionWays = new List<IReadOnlyList<IReadableStation>>();
        }

        way.Reverse();
        _minTransitionWays.Add(way);
    }

    private bool IsStartStation(IReadableStation station)
    {
        return _start == station || _start.GetTransitions().Any(st => st == station);
    }

    private IEnumerable<IReadableStation> GetRelativeStationsWithPoint(IReadableStation station, int point)
    {
        foreach (var readableStation in GetNeighborStationsWithPoint(station, point))
        {
            yield return readableStation;
        }

        foreach (var transition in station.GetTransitions())
        {
            foreach (var readableStation in GetNeighborStationsWithPoint(transition, point))
            {
                yield return readableStation;
            }
        }
    }

    private IEnumerable<IReadableStation> GetNeighborStationsWithPoint(IReadableStation station, int point)
    {
        if (station.Next != null && HasStationWithPoint(station.Next, point))
        {
            yield return station.Next;
        }

        if (station.Prev != null && HasStationWithPoint(station.Prev, point))
        {
            yield return station.Prev;
        }
    }

    private bool HasStationWithPoint(IReadableStation station, int point)
    {
        return _storage.TryGet(station.Id, out var p) && p == point;
    }
}