using SubwaySearch.Model;

namespace SubwaySearch.Search;

public class RouteSearcher
{
    private readonly IStationSearcher _subway;
    private readonly PointsStorage _storage = new();

    public RouteSearcher(IStationSearcher subway)
    {
        _subway = subway;
    }

    private void PrepareForSearch(IReadableStation startStation)
    {
        _storage.Clear();
        UpdatePoint(startStation, 0);
    }

    private void UpdatePoint(IReadableStation station, int currentPoint)
    {
        if (!TryUpdatePointsStorage(station, currentPoint))
        {
            return;
        }

        var nextPoint = currentPoint + 1;
        if (station.HasTransitions)
        {
            foreach (var transition in station.GetTransitions())
            {
                UpdateNeighborPoints(transition, nextPoint);
            }
        }

        UpdateNeighborPoints(station, nextPoint);
    }

    private void UpdateNeighborPoints(IReadableStation station, int currentPoint)
    {
        if (station.Next != null)
        {
            UpdatePoint(station.Next, currentPoint);
        }

        if (station.Prev != null)
        {
            UpdatePoint(station.Prev, currentPoint);
        }
    }

    private bool TryUpdatePointsStorage(IReadableStation station, int currentPoint)
    {
        if (IsHasLowestPointAmongTransitions(station, currentPoint))
        {
            return false;
        }

        _storage.Set(station.Id, currentPoint);
        station.GetTransitions().ToList().ForEach(st => _storage.Set(st.Id, currentPoint));
        return true;
    }

    private bool IsHasLowestPointAmongTransitions(IReadableStation station, int currentPoint)
    {
        return TryGetLowestPointAmongTransitions(station, out var lowestCount) && lowestCount <= currentPoint;
    }

    private bool TryGetLowestPointAmongTransitions(IReadableStation station,
        out int lowestPoint)
    {
        if (!_storage.TryGet(station.Id, out lowestPoint))
        {
            lowestPoint = int.MaxValue;
        }

        foreach (var transition in station.GetTransitions())
        {
            if (!_storage.TryGet(transition.Id, out var point))
            {
                continue;
            }

            if (lowestPoint >= point)
            {
                lowestPoint = point;
            }
        }

        return lowestPoint != int.MaxValue;
    }

    public SearchResult Search(string startStationId, string endStationId)
    {
        Console.WriteLine($"Searching for route: {startStationId} -> {endStationId}");

        var startStation = _subway.FindStation(startStationId);
        var endStation = _subway.FindStation(endStationId);

        if (startStation == null || endStation == null)
        {
            Console.WriteLine("Stations not found");
            return default;
        }

        PrepareForSearch(startStation);

        if (!_storage.Has(startStationId) || !_storage.Has(endStationId))
        {
            Console.WriteLine("points for station is absent");
            return default;
        }

        var wayChooser = new WaysChooser(_storage, startStation, endStation);
        return wayChooser.GetWays();
    }
}