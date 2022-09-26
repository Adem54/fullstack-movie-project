using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MoviesAPI.Entities;
using MoviesAPI.Filters;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Helpers;

namespace MoviesAPI.Controllers
{
    [Route("api/genres")]

    [ApiController]

    public class GenresController : ControllerBase
    {
        private readonly ILogger<GenresController> _logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public GenresController(ILogger<GenresController> logger, ApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]//https://localhost:7046/api/genres
        //COOOK ONEMLI...BUNU YENI GORDUM...
//Query uzerinden data gonderilmesini soyluyor back-endimiz, front enden datayi gonderecek olan developer
//get ile axios uzerinde istek gonderir ilk parametre url olur 2. parametre ise obje icerisinde params properties i ile
//value olarak da paginationDTO nun bekledigi datalari iceren bir obje olmasi bekleniyor..
        public async Task<ActionResult<List<GenreDTO>>> GetGenres([FromQuery] PaginationDTO paginationDTO)
        {
            // _logger.LogInformation("Getting all the genres");
            IQueryable<Genre>? queryable = context.Genres.AsQueryable();
            //Biz genres datamizi ne olarak aliyorduk liste ye ceviriyorduk hatirlayalim simdi ise queryable a
            //cevirmis olduk, ve bu sayede yapacagimz sorgulari direk veritabnainda yapip sorgu sonucunu sadece 
            //memory ye almis olacagiz, tum datayi memory ye getirip de sorgu yapmak yerine bu sekilde daha perromanslidir
            //Yani kisacasi bizim genres datamiz artik queryable referans tutucu degiskenindedir


            //HttpContext ControllerBase base class i icerisnde bulunan bir type dir 
            //public HttpContext HttpContext { get; }
            //HttpContext ise icerisinde Request,Response, UseClaims gibi request-response arasinda ihtiyacimiz 
            //cok olabilecek propertiesleri barindiran ve cok iyi bilmemiz gereken bir type dir...
            await HttpContext.InsertParametersPaginationInHeader(queryable);
            //Bu yaptigmmz extension method sayesinde front-end de Header icerisinde queryable da kac adet dat var onu gonderiyorz
            //Bu pagination islemini biz uygulama genelinde kullanacaigmz icin bu operasyonu global
            //hale getiririz, merkezilestirip tek yerde tutup ihiyacimiz olan her yerde kullanacagiz..

            /*Queryable bize adim adim, gittigmiz yeri insa etmemizi sagliyor*/
            List<GenreDTO> genreDtos = new List<GenreDTO>();
            List<Genre> genres = await queryable.OrderBy(x=>x.Name).Paginate(paginationDTO).ToListAsync();
            //IQueryable<T> extension sayesinde, Pagination(paginationDTO) yu queryable List<Genre> den cagirip kullanabilyouruz
            genreDtos = mapper.Map<List<GenreDTO>>(genres);
            return genreDtos;
            //Biz pagination i uyguladik burda ama eksigimiz parmaetreye beklddigmz paginationDto yu kullanamadik onu IQueryable da kullanaiblmek
            //icinde bir extension method yazmamiz gerekiyor
        }


        [HttpGet("{Id:int}")]
        public async Task<ActionResult<GenreDTO>> Get(int Id)
        {
            //async await kullaniyorsak, Linq methodlarini ve diger methodlarinda Async versiyonlari ile kullanmaliyiz
          Genre genre=await context.Genres.FirstOrDefaultAsync(x=>x.Id==Id);
          if(genre is null){
            return NotFound();
          }
            GenreDTO genreDTO=new GenreDTO();
            return mapper.Map<GenreDTO>(genre);

        }

        [HttpPost]
        public async Task<IActionResult> AddGenre([FromBody] GenreCreationDTO genreCreationDTO)
        {
            //    List<Genre> genres=await context.Genres.ToListAsync();
            //  genres.Add(genre);
            Genre genre = new Genre();
            genre = mapper.Map<Genre>(genreCreationDTO);
            //Burayi Map liyoruz cunku bizim dbcontext imiz genreCreationDTO yu tanimiyor, ama Genre yi taniyor
            context.Add(genre);//Buraya dikkat edersek direk context.Add(genre) bu sekilde direk context uzerinden
                               //Add genre diyoruz ve Entityframework bunu Genres e ekleyecegini bilebiliyor...Bestpractise..
            await context.SaveChangesAsync();//UnitofWork Pattern
                                             //    return Ok();
            return NoContent();
        }



        [HttpPut("{id:int}")]
        //burda root parametresi icin restriction kullaniyoruz
        public async Task<ActionResult> Put(int id,[FromBody] GenreCreationDTO genreCreationDTO)
        {
            Genre genre=await context.Genres.FirstOrDefaultAsync(x=>x.Id==id);
            
            if(genre is null){
                return NotFound();
            }
            //Bize genreCreationDTO geliyor onu kendi entity miz olan genre ye donusrturuyor
         //   genre=mapper.Map<Genre>(genreCreationDTO);  
            genre=mapper.Map(genreCreationDTO,genre); 
            //gelen genreCreationDTO yu genreye map edip sonra bunu da bizim genre referans tutucu 
            //degiskenimize atama yapiyoruz 
           await context.SaveChangesAsync();
           //UnitofWork mantigi entityframework un kendisi icindedir ondan dolayi hemen Map iselim ile
           //beraber update islemi database de gerceklesmiyor SaveChanges ile brilikte gerceklsiyor
           return NoContent();
           //return Ok de verilebilirdi herhalde
            
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
           var exist=await context.Genres.AnyAsync(x=>x.Id==id);//bool doner
           if(!exist){
                return NotFound();
           }
           //Burda olan sudur aslinda..
           //Biz Genre den bir instance olusturuyoruz id si bize front-end den gelen id ve bu id bizim databaseimzde 
           //var mi onu da kontrol edip var oldugunu anladiktan sonra context.Remove iceriisnde genre den yeni bir instance olusturup
           //id olarak da front-end den gelen degeri veriuyoruz ve ardindan da, context e gidilip SaveChanges de bu su demek
           //biz instance olusturdugumuz kayidi silmek istiyoruz, bunu entityframework bu sekilde anlar ve gider o kaydi siler..
           context.Remove(new Genre(){Id=id});
           await context.SaveChangesAsync();
           return NoContent();
        }
    }
}

/*
  MANUEL MAPLAMA ISLEMI YAPIYORUZ
  public async Task<ActionResult<List<GenreDTO>>> GetGenres()
        {
            _logger.LogInformation("Getting all the genres");
            List<GenreDTO> genreDtos = new List<GenreDTO>();
            List<Genre> genres = await context.Genres.ToListAsync();
            foreach (Genre genre in genres)
            {
                genreDtos.Add(new GenreDTO()
                {
                    Id = genre.Id,
                    Name = genre.Name,
                });
            }
            return genreDtos;

    Manuel maplama olayi iyi bir practise degildir ornegin bizim 20 tane propertyimiz var ise her birisi icin ayni bu sekilde islem yapmak
    bestpractise acisindan cok da tavsiye edilir bir yontem degildir, ve bad practise dir.Mesela 1 tane property yi maplemeyi unutsak bize 
    ciddi sorun yasatacaktir ve farketmmeiz bazen gercekten zor olabilir 
    Ondan dolayi biz Automapper paketini yukleyerek AutoMapper kutuphanesini kullaniriz      
*/


/*

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Helpers;

namespace MoviesAPI.Controllers
{
    [Route("api/actors")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext? context;
        private readonly IMapper? mapper;
        public ActorsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]

        public async Task<ActionResult<List<ActorDTO>>> GetActors()
        {
            var actors = await context.Actors.ToListAsync();
            return mapper.Map<List<ActorDTO>>(actors);
        }

        // [HttpGet("{id:int}")]

        // public async Task<ActionResult<ActorDTO>> Get(int id)
        // {
        //     var actor = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);
        //     if (actor is null)
        //     {
        //         return NotFound();
        //     }
        //     return mapper.Map<ActorDTO>(actor);
        // }

        // [HttpPost]
        // public async Task<ActionResult> Post([FromForm] ActorCreationDTO actorCreationDTO)
        // {
        //     return NoContent();
        //     throw new NotImplementedException();
        // }

        // [HttpPut]
        // public async Task<ActionResult> Put([FromForm] ActorCreationDTO actorCreationDTO)
        // {
        //     throw new NotImplementedException();
        // }

        // [HttpDelete("{id:int")]

        // public async Task<ActionResult> Delete(int id)
        // {
        //     var actor = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);
        //     if (actor is null)
        //     {
        //         return NotFound();
        //     }
        //     context.Remove(new Actor() { Id = id });
        //     await context.SaveChangesAsync();
        //     return NoContent();
        // }
    }
}




*/