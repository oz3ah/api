namespace Shortha.Application.Dto.Responses.Package
{
    public class PackageInfoDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
    }
}