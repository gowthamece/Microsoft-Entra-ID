var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Microsoft_Entra_ID_ApiService>("apiservice");

builder.AddProject<Projects.Microsoft_Entra_ID_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.AddProject<Projects.fn_entra_id_utility>("fn-entra-id-utility");

builder.Build().Run();
