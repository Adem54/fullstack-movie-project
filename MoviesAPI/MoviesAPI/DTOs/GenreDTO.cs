

namespace MoviesAPI {
    public class GenreDTO 
    {
         public int? Id { get; set; }
        public string? Name { get; set; }
    }
}
/*
QUERY ISLEMLERI ICIN DONECEGIMZ TYPE GENREDTO OLACAK
GETGENRES-GETGENRE-GETGENREBYID GIBI
Bizim burda herhangi bir validation islemine ihtiyacimz yok cunku bu GenreDTO yu biz sadece query
information,database disinda kullanicinin sorgu islemlerine cevap niteliginde Genres,GenresById
GetGenre gibi spesifik bir id ye ait bir genre veya genres listesinin tamamai ya da herhangi bir kritere
gore filtrelenmis sorgudan gecirilmis genre lerden olusan bir liste olarak biz GenreDTO araciligi ile
yani bunu kullanarak kullanicinin query islemlerinde kullanicya sundgumuz data mahiyetindedir
Yani back-end den front-ende sundgumzu qery data sidir...
Biz bu GenreDTO yu, createGenre endpoint action methoduna parametre olarak almayacagiz
Yani bu bizim front-end den back-end e kullanicinin gonderdigi data degil, back-end den front-ende
kullaniciya giden data olacaktir ondan dolayi validation lik bir islem yok burda
*/