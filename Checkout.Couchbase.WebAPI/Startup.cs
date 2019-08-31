using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Core;
using Couchbase;
using Couchbase.Authentication;
using Couchbase.Configuration.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Checkout.Couchbase.WebAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            ClusterHelper.Initialize(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(ConfigurationDefaults.COUCHBASE_URI) }
            }, new PasswordAuthenticator(ConfigurationDefaults.COUCHBASE_USERNAME, ConfigurationDefaults.COUCHBASE_PASSWORD));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            applicationLifetime.ApplicationStopped.Register(() => 
            {
                ClusterHelper.Close();
            });
        }
    }
}
