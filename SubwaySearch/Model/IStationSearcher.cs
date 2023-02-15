namespace SubwaySearch.Model;

public interface IStationSearcher
{
    IReadableStation? FindStation(string stationId);
}