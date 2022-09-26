
namespace MoviesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

}


        /*
        .ConfigureWebHostDefaults(webBuilder =>
        Logger configuration...
            {
                webBuilder.ConfigureLogging(loggingBuilder=>{
                    loggingBuilder.AddProvider();//Burda istegimiz kadar provider ekleyebilirz
                    Hangi class ile loglairmizi isleme alacaksak bu provider icine ekleriz 
                    Ve bu provider lar ile mesajlarimizi yonetiriz
                });

        */