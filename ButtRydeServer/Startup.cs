using Microsoft.Owin;
using Owin;
using System.Web.Http;
using AASC.Partner.API.ActionFilters;
using Autofac;
using System.Reflection;
using Autofac.Integration.WebApi;
using AASC.FW.DataContext;
using AASC.FW.EF6;
using AASC.FW.Repositories;
using AASC.FW.UnitOfWork;
using AASC.Partner.API.Models;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using System.Linq;
using AASC.Partner.API.Services;

[assembly: OwinStartup(typeof(AASC.Partner.API.Startup))]

namespace AASC.Partner.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Get your HttpConfiguration. In Owin, you will create one rather than using GlobalConfiguration.Configuration
            HttpConfiguration httpConfig = new HttpConfiguration();

            // Register Action Filter (LoggingFilterAttribute)
            httpConfig.Filters.Add(new LoggingFilterAttribute());

            ConfigureAuth(app);

            ConfigAutofacIoC(httpConfig);

            ConfigureWebApi(httpConfig);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseAutofacWebApi(httpConfig);

            app.UseWebApi(httpConfig);
        }

        private void ConfigAutofacIoC(HttpConfiguration httpConfig)
        {
            var builder = new ContainerBuilder();

            //Register your web API controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Run other optional steps, like register filters
            // pre-controller-type services, etc., then set the dependency resolver
            // to be Autofac.
            builder.RegisterType<ApplicationDbContext>().As<IDataContextAsync>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<FileUpload>>().As<IRepository<FileUpload>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<IoTGRoadmap>>().As<IRepository<IoTGRoadmap>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Company>>().As<IRepositoryAsync<Company>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Department>>().As<IRepositoryAsync<Department>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Employee>>().As<IRepositoryAsync<Employee>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<CLAForm>>().As<IRepositoryAsync<CLAForm>>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWorkAsync>().InstancePerLifetimeScope();

            builder.RegisterType<CompanyDataService>().As<ICompanyDataService>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyBizService>().As<ICompanyBizService>().InstancePerLifetimeScope();

            builder.RegisterType<EmployeeDataService>().As<IEmployeeDataService>().InstancePerLifetimeScope();
            builder.RegisterType<EmployeeBizService>().As<IEmployeeBizService>().InstancePerLifetimeScope();
            
            builder.RegisterType<DepartmentDataService>().As<IDepartmentDataService>().InstancePerLifetimeScope();
            builder.RegisterType<DepartmentBizService>().As<IDepartmentBizService>().InstancePerLifetimeScope();

            builder.RegisterType<CLADataService>().As<ICLADataService>().InstancePerLifetimeScope();
            builder.RegisterType<CLABizService>().As<ICLABizService>().InstancePerLifetimeScope();

            var container = builder.Build();

            var webApiResolver = new AutofacWebApiDependencyResolver(container);

            httpConfig.DependencyResolver = webApiResolver;
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {

            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            jsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }
    }
}
