namespace ApiServer.Features.Middleware;

public class ModelPointIndex
{
    public int ModelId { get; set; }
    public int CeqTypeId { get; set; }
    public int PointTypeId { get; set; }
    public int? PointIndex { get; set; }
    public int? DynamicIndex { get; set; }
    public string? Name { get; set; }
    public string EName { get; set; } = null!;
}
