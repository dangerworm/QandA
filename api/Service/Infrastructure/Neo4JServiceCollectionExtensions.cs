using Neo4j.Driver;
using Persistence.Neo4j;

namespace Service.Infrastructure

{
    public static class Neo4JServiceCollectionExtensions
    {
        public static IServiceCollection AddNeo4JDriver(this IServiceCollection services)
        {
            var boltUri = Environment.GetEnvironmentVariable("NEO4J_URI");
            var user = Environment.GetEnvironmentVariable("NEO4J_USER");
            var password = Environment.GetEnvironmentVariable("NEO4J_PASSWORD");

            services.AddSingleton(GraphDatabase.Driver(boltUri, AuthTokens.Basic(user, password)));
            services.AddScoped<INeo4jDataAccess, Neo4jDataAccess>();

            return services;
        }

        public static IServiceCollection AddNeo4JRepositories(this IServiceCollection services)
        {
            //return services.AddTransient<IQuestionRepository, QuestionRepository>();
            return services;
        }
    }
}
