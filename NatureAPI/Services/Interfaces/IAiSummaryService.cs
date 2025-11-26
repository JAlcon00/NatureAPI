namespace NatureAPI.Services.Interfaces;

public interface IAiSummaryService
{
    Task<string> GeneratePlaceSummaryAsync(int placeId, CancellationToken cancellationToken = default);
}
