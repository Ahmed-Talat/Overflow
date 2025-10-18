var builder = DistributedApplication.CreateBuilder(args);

var keycloak = builder.AddKeycloak("keycloak", 6001)
    .WithDataVolume("keycloak.data");

var postgres = builder.AddPostgres("postgres", port: 5432)
    .WithDataVolume("postgres.data")
    .WithPgAdmin();

var postgresDb = postgres.AddDatabase("questionDb");

var questionService = builder.AddProject<Projects.QuestionService>("question-svc")
    .WithReference(keycloak)
    .WithReference(postgresDb)
    .WaitFor(keycloak)
    .WaitFor(postgresDb);

builder.Build().Run();