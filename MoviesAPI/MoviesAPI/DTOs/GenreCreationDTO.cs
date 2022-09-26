


using System.ComponentModel.DataAnnotations;
using MoviesAPI.Validations;

namespace MoviesAPI.DTOs {
    public class GenreCreationDTO 
    { 

        [Required(ErrorMessage="The field with name {0} is required")] 
        [StringLength(50)]
        [FirstLetterUppercase]
        public string? Name { get; set; }
    }
}
/*
Bursi da bizim Commands yani, CreateGenre islemi icin front-end den kullanici back-end de
 data gonderdigi durumlarinda kullanacagimz DTO dur
 Yani biz disardan bize dogurdan entity datamizin gelmesin istemeyiz...
Ondan dolayi da disardan gelen datayi da bir validation dan gecirmemiz
cok normaldir
 Dolayisi ile biz disardan bize data gonderilirken id almak istemeyiz cunku, id yi ya biz
 auto increment ile, dogrudan veritabaninda verilir ya da Guid ile her instance olsturuldugunda 
 otomatik olarak constructor da olustururuz...
Obur turlu Entityframeworkcore hata firlatabilir
*/