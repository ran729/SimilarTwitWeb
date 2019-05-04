using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimilarTwitWeb.Core.DAL;
using Microsoft.EntityFrameworkCore;
using SimilarTwitWeb.Core.Interfaces;
using SimilarTwitWeb.Core.BL;

namespace SimilarTwitWeb
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("SimilarTwitWeb"))
            );

            services.AddTransient<IUserRepository, UserEFRepository>();
            services.AddTransient<IMessageRepository, MessageEFRepository>();
            services.AddTransient<IFollowerRepository, FollowerEFRepository>();
            services.AddTransient<IFeedManager, FeedManager>();
            services.AddTransient<IFollowersManager, FollowersManager>();
            services.AddSingleton<InMemoryStorage>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseMvc();

        }
    }
}
