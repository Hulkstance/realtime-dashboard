using System.ComponentModel.DataAnnotations;

namespace Dashboard.Blazor.Data;

public class TornConfiguration
{
    [Required]
    public required string Uid { get; init; }

    [Required]
    public required string Secret { get; init; }
}
