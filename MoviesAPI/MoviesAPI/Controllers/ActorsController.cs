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
        private readonly IFileStorageService fileStorageService;

        private readonly string containerName="actors";

        public ActorsController(ApplicationDbContext context, IMapper mapper, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorageService=fileStorageService;
        }

        [HttpGet]

        public async Task<ActionResult<List<ActorDTO>>> GetActors()
        {
            var actors = await context.Actors.ToListAsync();
            return mapper.Map<List<ActorDTO>>(actors);
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var actor = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (actor is null)
            {
                return NotFound();
            }
            return mapper.Map<ActorDTO>(actor);
        }


/*
REsim dosyasina ait tum datalari almis olacagiz burda, fromForm ve IFormFile ile ve
front-end de yaptimgz resim gonderme islemi sayesinde...Cook onemli bir bestpractise..
ContentDisposition [string]:
"form-data; name=\"picture\"; filename=\"1635089560298.jpg\""
ContentType [string]:
"image/jpeg"
FileName [string]:
"1635089560298.jpg"
Headers [IHeaderDictionary]:
{Microsoft.AspNetCore.Http.HeaderDictionary}
Length [long]:
52705
Name [string]:
"picture"
*/
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreationDTO actorCreationDTO)
        {
            //Actor type i ile datayi Azure kaydedecegiz, ama front-end den bize actorCreationDTO type inda geliyor
           var actor=mapper.Map<Actor>(actorCreationDTO); 
           if(actorCreationDTO.Picture is not null){
                actor.Picture=await fileStorageService.SaveFile(containerName,actorCreationDTO.Picture);//Burda container name i gececegiz
           }

           context.Add(actor);
           await context.SaveChangesAsync();
           return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromForm] ActorCreationDTO actorCreationDTO)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete(int id)
        {
            var actor = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (actor is null)
            {
                return NotFound();
            }
            context.Remove(new Actor() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}