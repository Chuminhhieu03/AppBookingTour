public class CityImportDto
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Region { get; set; } = null!;
    public bool IsPopular { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
}