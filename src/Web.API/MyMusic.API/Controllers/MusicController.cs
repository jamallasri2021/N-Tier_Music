using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMusic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        #region Properties & Constructor

        private readonly IMusicService _musicService;

        private readonly IMapper _mapperService;

        public MusicController(IMusicService musicService, IMapper mapperService)
        {
            _musicService = musicService;
            _mapperService = mapperService;
        }
        #endregion

        #region Functions
        [HttpGet("Musics")]
        public async Task<ActionResult<IEnumerable<MusicResource>>> GetAllMusic()
        {
            var musics = await _musicService.GetAllWithArtist();
            var musicResources = _mapperService.Map<IEnumerable<Music>, IEnumerable<MusicResource>>(musics);

            return Ok(musicResources); 
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<MusicResource>> GetMusicById(int id)
        {
            try
            {
                var music = await _musicService.GetMusicById(id);

                if (music == null)
                {
                    return NotFound();
                }

                var musicResource = _mapperService.Map<Music, MusicResource>(music);

                return Ok(musicResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateMusic")]
        [Authorize]
        public async Task<ActionResult<MusicResource>> CreateMusic(MusicToSaveResource musicToSave)
        {
            try
            {
                // Get current User
                var userId = User.Identity.Name;

                // Validation of input Data
                var validation = new MusicToSaveResourceValidator();
                var validationResult = await validation.ValidateAsync(musicToSave);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                // Mapping
                var music = _mapperService.Map<MusicToSaveResource, Music>(musicToSave);

                // Create music
                var musicCreated = await _musicService.CreateMusic(music);

                // Mapping
                var musicResult = _mapperService.Map<Music, MusicResource>(musicCreated);

                return Ok(musicResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateMusic/{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateMusic(int id, MusicToSaveResource musicToUpdate)
        {
            try
            {
                // Validation of input Data
                var validation = new MusicToSaveResourceValidator();
                var validationResult = await validation.ValidateAsync(musicToUpdate);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                // Verify existing music
                var musicExist = await _musicService.GetMusicById(id);

                if (musicExist == null)
                {
                    return NotFound();
                }

                // Mapping 
                var musicUpdate = _mapperService.Map<MusicToSaveResource, Music>(musicToUpdate);

                // Update music
                await _musicService.UpdateMusic(musicExist, musicUpdate);

                return Ok("The music is updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteMusic/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteMusic(int id)
        {
            try
            {
                var musicToDelete = await _musicService.GetMusicById(id);

                if (musicToDelete == null)
                {
                    return NotFound();
                }

                await _musicService.DeleteMusic(musicToDelete);

                return Ok("The music is deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("MusicsByArtistId/{artistId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Music>>> GetMusicsByArtistId(int artistId)
        {
            try
            {
                var musics = await _musicService.GetMusicsByArtistId(artistId);

                if (musics == null)
                {
                    return NotFound("Musics not found");
                }

                var musicsResources = _mapperService.Map<IEnumerable<Music>, IEnumerable<MusicResource>>(musics);

                return Ok(musicsResources);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
