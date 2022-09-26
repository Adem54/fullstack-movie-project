
using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Helpers {
    public static class HttpContextExtensions {
        //Biz bu exthension method ile, HttpContext type ina bir extension method eklmemis oluyoruz ki artik uygulama genelinde
        //bu isleve nerde ihtiyacim olursa orda kullanabileyim diye..cunku ortak ve genel type lar uzerinden gidilerek, sonucta type lari biz
        //istedigmiz yerde kullanabiliyoruz...
        //Parametredeki IQueryable araciligi ile, biz veritabanimizdaki datamiza direk veritabani uzerinde filtreleme veya sorgu yapip sonra bellege cagirma
        //islemini gerceklestirmis olacagiz bu da IEnumerable ile yapmaya gore bize cok daha yuksek performans elde etmemizi saglar, cuku tum datayi bellege
        //alip sonra bellekte filtreleme islemi yaparsak bu bellegi cok daha fazla yorar ve daha maliyetlidir
        //Neden HttpContext type ina ekliyoruz bu mehtodu cunku, HttpContext ile biz front-ende gonderilen resonse, Headers type larimizi barindiriyor da ondan
        public async static Task InsertParametersPaginationInHeader<T>(this HttpContext httpContext,IQueryable<T> queryable){

            if(httpContext is null){//Eger context bos olursa kullanicidan gelen icerik bos olursa o zaman hata dondurelim.
                throw new ArgumentNullException(nameof(httpContext));
            }

            double count=await queryable.CountAsync();
            //Veritabaninda ki islemden gecirilen data nin sayisi, ve bu sayiyi client, front-end den erisilsin diye 
            //httpContext sinifitmiz in altindaki Response altinda ki bir Dictionay type olan Header s a eklyerek font-end kullanicsinin
            //bu degeri kullanabilmnesine imkan tanimis olyoruz.. aslindsa
            //using Microsoft.EntityFrameworkCore; bu kutuphane gelmez ise CountAsync i alamayiz
            httpContext.Response.Headers.Add("totalAmountOfRecords",count.ToString());
            //http response Header inia total data sayisini koyuyourz,tablomuzdan daha dogrusu veritabani tablomuzdan
            //Bu yolla da client bu dataya erisme imkanini elde ediyor, tabi bunu clientin yapabilmesi icin bizim Startup daki Cors konfigurasyonunda
            //Header dan da datayi okuyabilmesine imkan tanimamiz gerekiyor
        }
    }
}

/*
Extension Method
Extension method eklyecegimz icin static yapiyoruz
Extension methodlari biz cok iyi ogrenelim cunku ne zaman bir global, merkezilestirme
sentralization, ve uygulamayi tek yerden yoetme ve bize dotnet den gelen hazir siniflara
bizim extension method ekleyip o sinif uzerinden cok onemli noktalarda extension methodu kullanarak
ciddi mana da efektif isler basariyoruz..Kullanim ornekleriin cogu bu sekildedir 
Dolayisi ile de artik bu extension mehtodlarin kullanim mantigini ve
bizi nerelerde cok ciddi kurtardigini bilerek, bu ozelikten faydalanmaliyiz
*/