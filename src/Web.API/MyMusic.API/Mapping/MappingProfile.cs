using AutoMapper;
using MyMusic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Domain(BDD) vers Resource
            CreateMap<Music, MusicResource>();
            CreateMap<Artist, ArtistResource>();
            CreateMap<Music, MusicToSaveResource>();
            CreateMap<Artist, ArtistToSaveResource>();
            CreateMap<Composer, ComposerResource>();
            CreateMap<Composer, ComposerToSaveResource>();
            CreateMap<User, UserResource>();
            CreateMap<User, UserToSave>();

            // Resource vers Domain(BDD)
            CreateMap<MusicResource, Music>();
            CreateMap<ArtistResource, Artist>();
            CreateMap<MusicToSaveResource, Music>();
            CreateMap<ArtistToSaveResource, Artist>();
            CreateMap<ComposerResource, Composer>();
            CreateMap<ComposerToSaveResource, Composer>();
            CreateMap<UserResource, User>();
            CreateMap<UserToSave, User>();
        }
    }
}
