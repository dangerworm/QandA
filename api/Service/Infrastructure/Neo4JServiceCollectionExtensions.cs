using Neo4j.Driver;
using Persistence.Neo4j;
using Persistence.Neo4j.Repositories;
using Persistence.Neo4j.Workflows;

namespace Service.Infrastructure

{
    public static class Neo4JServiceCollectionExtensions
    {
        public static IServiceCollection AddNeo4JDriver(this IServiceCollection services)
        {
            var boltUri = "bolt://localhost:7687"; // Environment.GetEnvironmentVariable("NEO4J_URI");
            var user = "neo4j"; // Environment.GetEnvironmentVariable("NEO4J_USER");
            var password = "secret"; // Environment.GetEnvironmentVariable("NEO4J_PASSWORD");

            services.AddSingleton(GraphDatabase.Driver(boltUri, AuthTokens.Basic(user, password)));
            services.AddScoped<INeo4jDataAccess, Neo4jDataAccess>();

            return services;
        }

        public static IServiceCollection AddNeo4JWorkflows(this IServiceCollection services)
        {
            return services
                .AddTransient<QuestionWorkflow>()
                .AddTransient<UserWorkflow>();
        }

        public static IServiceCollection AddNeo4JRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient<IQuestionRepository, QuestionRepository>()
                .AddTransient<IUserRepository, UserRepository>();
        }
    }
}
