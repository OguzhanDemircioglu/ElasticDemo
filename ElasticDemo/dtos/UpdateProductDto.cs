namespace ElasticDemo.dtos;

public record UpdateProductDto(
    Guid Id,
    string Name,
    decimal Price,
    int Stock,
    string Description);