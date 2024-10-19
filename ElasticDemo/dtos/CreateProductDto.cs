namespace ElasticDemo.dtos;

public record CreateProductDto(
    string Name,
    decimal Price,
    int Stock,
    string Description);