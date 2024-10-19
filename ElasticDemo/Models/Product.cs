namespace ElasticDemo.Models;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public decimal Price  { get; set; }
    public int Stock { get; set; }
    public string Description { get; set; } = String.Empty;
}