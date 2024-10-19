using Elastic.Clients.Elasticsearch;
using ElasticDemo.dtos;
using ElasticDemo.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

ElasticsearchClientSettings settings = new(new Uri("http://localhost:9200"));
settings.DefaultIndex("products");

ElasticsearchClient client = new(settings);

client.IndexAsync("products").GetAwaiter().GetResult();

var pingResponse = await client.PingAsync();
if (!pingResponse.IsValidResponse)
{
    throw new Exception("Elasticsearch sunucusuna bağlanılamıyor.");
}

var indexExistsResponse = await client.Indices.ExistsAsync("products");
if (!indexExistsResponse.Exists)
{
    var createIndexResponse = await client.Indices.CreateAsync("products");
    if (!createIndexResponse.IsValidResponse)
    {
        throw new Exception("Elasticsearch indeks oluşturulurken hata oluştu.");
    }
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/products/create", async (CreateProductDto request, CancellationToken cancellationToken) =>
{
    Product product = new()
    {
        Name = request.Name,
        Price = request.Price,
        Stock = request.Stock,
        Description = request.Description
    };

    CreateRequest<Product> createRequest = new(product.Id.ToString())
    {
        Document = product,
    };

    CreateResponse createResponse = await client.CreateAsync(createRequest, cancellationToken);

    return Results.Ok(createResponse.Id);
});

app.MapGet("/products/getAll", async (CancellationToken cancellationToken) =>
{
    var response = await client.SearchAsync<Product>("products", cancellationToken);

    if (response == null || !response.IsValidResponse)
    {
        return Results.NotFound("Arama işlemi başarısız oldu veya geçerli bir yanıt alınamadı.");
    }

    return Results.Ok(response.Documents ?? new List<Product>());
});


app.Run();