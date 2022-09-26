using Microsoft.AspNetCore.Mvc.Filters;

namespace MoviesAPI.Filters
{
    public class MyExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<MyExceptionFilter> logger;
        public MyExceptionFilter(ILogger<MyExceptionFilter> logger)
        {
            this.logger = logger;
        }

        //Bu metod sadece Exception ortaya ciktiginda calisacak
        //Ayrica burda bilmemiz gereken baska birseyde bizim bu method icerisinde
        //http-request e erismiimiz var
        public override void OnException(ExceptionContext context)
        {
            //Ortaya cikacak exception lari loggluyoruz burda
            logger.LogError(context.Exception,context.Exception.Message);
            base.OnException(context);
        }

    }
}

/*
fail: MoviesAPI.Filters.MyExceptionFilter[0]
      Error in the application.
      System.ApplicationException: Error in the application.
Herhangi bir yerde hata firlatilirsa once hata yakalanacak sonra da, buraya dusecek, exceptionlar tum api uygulamasindaki
exceptionlar buryaa dusecek

https://localhost:7046/api/genres/getGenres/3

fail: MoviesAPI.Filters.MyExceptionFilter[0]
      Error in the application.
      System.ApplicationException: Error in the application.
Eger bizim logger lari kaydettigimib bir database imiz var ise biz bu excepiton lari database e kaydedebiliriz
Ozellikle production a ciktiktan sonra bu cok faydalidir

*/