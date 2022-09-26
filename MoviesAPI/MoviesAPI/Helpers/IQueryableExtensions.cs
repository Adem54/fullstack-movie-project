
using MoviesAPI.DTOs;

namespace MoviesAPI.Helpers {
    public static class IQueryableExtensions {
//IQueryable<T> tipten olusturulan referans tutucu degiskenler icin, method hazirliyoruz cunku pagination islemini yapacagimz yerlerde
//biz tum datayi memory mize getirip de orda filtreleme yapmak yeriine filtrelemeyi direk veritabaninda yapip filtreledimgiz datayi getirmek 
//istiuyoruz ve bunu bircok farkli entity icin yapmak istiyoruz movies,genres.. ondan dolayi da bunu global yapyoruz iste IQueryable<T> ile olusturulmus
//tiplerde kullanmak istiyoruz, this ile birlikte olan type bu extension methodun hazirlandigi tipdir, sagdaki PaginatinDTO ise bu extension methodun
//alacagi parametredir
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO ){

            return queryable
            .Skip((paginationDTO.Page -1)* paginationDTO.RecordsPerPage)
            .Take(paginationDTO.RecordsPerPage);
     //querable data mizdan ornegin sayfa 1 ise, her sayfa da kac tane data olacak ise o kadar datyi getirecek otomatik olarak
     /*
     Sayfa-1 icin Skip(0) hicbir datayi es gecemeyecek ve Take de ilk sayfada olacak data sayisini direk almis gelmis olacak 0 dan baslayip
     Sayfa-2 Skip(1) bu sayede 1.index ten baslar 0.index i zaten almis idi ilk de, ve bu sekilde her bu method kullaniminda belli sayida datalarini 
     getirerek pagination islemini yapabilmis oluyoruz...
     Frontend deki developer a diyoruz ki sen bana, her sayfada kac data istiyorsun onu ver, bir de hangi sayfa oldugunu ver ben sana
     o araliiklari vereyim ornegin page-1 geldi ve recordsPerPage-20 geldi o zaman 0-20 arasi datayi verecegiz
     diyelim ki page-2 20 verdi o zaman da 1-sayfa 0-20 arasi 2.sayfa ise 20*(2-1) den baslayarak 20 sayfa alacak yani 20-40 i alacak
     */           
        }
    }
}