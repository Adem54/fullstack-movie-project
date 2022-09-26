

namespace MoviesAPI.DTOs {
        public class PaginationDTO{
            public int Page {get; set;}=1;//We can change initial value,or default value
            private int recordsPerPage=10;
            private readonly int maxRecordsPerPage=50;

            public int RecordsPerPage {
                get {
                    return recordsPerPage;
                }
                set {
                    recordsPerPage=(value>maxRecordsPerPage) ? maxRecordsPerPage : value;
                }
                //Encapsulation a harika bir ornek..
                //Disardaki kullanicinin kafasina gore her rakami girebilmesini onluyoruz,
                //RecordsPage degerimze deger atanirken bizim belirledimg kurallara gore deger veriliyor
                //ve biz de aslinda RecordPage properties ini kontrol altinda tutmus oluyoruz onu kapsullyerek,
                //Disardaki kullanici dogrudan bizim field imiza ersemiyor...Bu cook onemli ve cok iyi bilnmesi geren
                //bir ozelliktir...
            }

        }    
}
/*
Burda front-end developer a hangi sayfada olduklarini takip edebilme firsati vermenin yaninda
birde sayfa basi ne kadar data miktari tutulmali konusundda da kullanabilecegi data lar sunaacgiz
default olarak 10 olarak baslayan ve maximum olarak da 50 olarak verilen
*/