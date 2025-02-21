var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.LylinkBackend_ManagementAPI>("management-api");
builder.AddProject<Projects.LylinkBackend_API>("website");

builder.Build().Run();
