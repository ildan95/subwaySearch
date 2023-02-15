namespace SubwaySearch.Model;

public interface IReadableStation
{
    IReadableStation? Next { get; }
    IReadableStation? Prev { get; }
    string Id { get; }
    string BranchId { get; }
    bool HasTransitions { get; }

    IEnumerable<IReadableStation> GetTransitions();
}