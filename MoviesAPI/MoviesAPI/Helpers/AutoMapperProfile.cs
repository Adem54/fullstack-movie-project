
using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Helpers {
    public class AutoMapperProfile:Profile 
    {
          public AutoMapperProfile(){
            CreateMap<Genre,GenreDTO>().ReverseMap();//Bu sekilde normalde kaynak Genre, destination GenreDTO dur 
            //ama ReverseMap diyerek iki yonlu donusumu de saglayabilmis ouruz
            CreateMap<GenreCreationDTO,Genre>();

            CreateMap<Actor,ActorDTO>().ReverseMap();
            CreateMap<ActorCreationDTO,Actor>().ForMember(x=>x.Picture,options=>options.Ignore());
            //front-end den ActorCreationDTO type inda bir data gonderilecek ve picture file seklinde gonderilecek ama
            //Actor da picture string olarak tutulacak ondan dolayi biz ActorCreationDTO dan Actor e cevirirken gelen
            //picture formatini ihmal et, onu ceivrme diyoruz..burda
          
          }  
    }
}