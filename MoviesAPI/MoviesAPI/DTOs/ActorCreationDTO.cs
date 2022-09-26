

using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs {
    public class ActorCreationDTO 
    {
        [Required]
        [StringLength(120)]
        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string? Biography {get; set;}

        // public string? Picture {get; set;}
        //Biz front-end den Picture u string olarak almayacagiz, direk file olarak alacagiz 
        //ama veritabaninda tabi url hali ile tutulacak ama, front-end den dogrudan dosya yuklenecek
        public IFormFile? Picture {get; set;}
        //Asp.net core da file dosyalarini bu type ile aliyoruz..
    }
}