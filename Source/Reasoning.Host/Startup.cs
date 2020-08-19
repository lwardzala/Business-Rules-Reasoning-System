using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Abstraction;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Reasoning.Core.Contracts;
using Reasoning.Core.Services;
using Reasoning.Host.Repositories;
using Reasoning.Host.Services;
using Reasoning.MongoDb.Repositories;
using Reasoning.MongoDb.Configuration;

namespace Reasoning.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MongoDatabaseSettings>(Configuration.GetSection(nameof(MongoDatabaseSettings)));

            services.AddSingleton<IReasoningService, ReasoningService>();
            services.AddSingleton<IMongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
            services.AddSingleton<IKnowledgeBaseService, KnowledgeBaseService>();
            services.AddSingleton<IReasoningTaskService, ReasoningTaskService>();
            services.AddSingleton<IReasoningTaskResolver, ReasoningTaskResolver>();
            services.AddSingleton<IReasoningTaskRepository, ReasoningTaskRepository>();
            services.AddSingleton<IKnowledgeBaseRepository, KnowledgeBaseRepository>();
            services.AddSingleton<Initiator>();
            services.AddHostedService<ReasoningHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddHttpClient<IHttpClientService, HttpClientService>();

            MongoDatabaseConfiguration.ConfigureBsonSerializers();
            MongoDatabaseConfiguration.ConfigureConventionRegistry();

            services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                opts.JsonSerializerOptions.IgnoreNullValues = true;
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opts.JsonSerializerOptions.Converters.Add(new JsonAbstractionConverter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(env.IsDevelopment() ? "/error-local" : "/error");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
