using System.Globalization;
using AsyncProductAPI.Data;
using AsyncProductAPI.Dtos;
using AsyncProductAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    options.UseSqlite(connectionString);
});

var app = builder.Build();

app.UseHttpsRedirection();

// Start Endpoint
app.MapPost("/api/v1/products", async (AppDbContext context, ListeningRequest? listeningRequest) =>
{
    if (listeningRequest is null) return Results.BadRequest();

    listeningRequest.RequestStatus = "ACCEPT";
    listeningRequest.EstimatedCompletionTime = DateTime.Now.AddSeconds(5).ToString(CultureInfo.InvariantCulture);
    await context.ListeningRequests.AddAsync(listeningRequest);
    await context.SaveChangesAsync();

    return Results.Accepted($"api/v1/productstatus/{listeningRequest.RequestId}", listeningRequest);
});

// Status endpoint
app.MapGet("api/v1/productstatus/{requestId}", async (AppDbContext context, string requestId) =>
{
    var listingRequest = await context.ListeningRequests.FirstOrDefaultAsync(lr => lr.RequestId == requestId);
    if (listingRequest is null) return Results.NotFound();

    ListingStatus listingStatus = new ListingStatus()
    {
        RequestStatus = listingRequest.RequestStatus,
        ResourceURL = string.Empty
    };

    if (listingRequest.RequestStatus?.ToUpper() == "COMPLETE")
    {
        listingStatus.ResourceURL = $"api/v1/products/{Guid.NewGuid().ToString()}";
        return Results.Redirect("http://localhost:5298/" + listingStatus.ResourceURL);
    }

    listingStatus.EstimatedCompletionTime = DateTime.Now.AddSeconds(5).ToString(CultureInfo.InvariantCulture);
    return Results.Ok(listingStatus);
});

// Final Endpoint
app.MapGet("api/v1/products/{requestId}", (string requestId) => 
    Results.Ok("Final result"));

app.Run();