namespace WebApiKalum
{
    public class Startup
    {
        public IConfiguration configuration { get;}
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigreServices(IServiceCollection _services)
        {
            _services.AddControllers();
            //_services.AddDbContext<>
        }
    }
}