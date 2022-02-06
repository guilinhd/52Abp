using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MockSchoolManagement.DataRepositories;
using Microsoft.EntityFrameworkCore;
using MockSchoolManagement.Infrastructure;

namespace MockSchoolManagement
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
            //services.AddControllersWithViews(a => a.EnableEndpointRouting = false).AddXmlSerializerFormatters();

            services.AddDbContextPool<AppDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("MockStudentDBConnection"));
                });
            services.AddControllersWithViews();
            //services.AddSingleton<IStudentRepository, MockStudentRepository>();
            services.AddTransient<IStudentRepository, SqlStudentRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsStaging() || env.IsProduction() || env.IsEnvironment("UAT"))
            {
                //app.UseStatusCodePages();
                app.UseExceptionHandler("/Error");
                //app.UseStatusCodePagesWithRedirects("/Error/{0}");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseMvcWithDefaultRoute();

            //app.UseMvc(routes => {
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}"
            //    );
            //});
        }
    }
}
