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
    public class ArtistController : ControllerBase
    {
        #region Properties & constructors

        private readonly IArtistService _artistService;

        private readonly IMapper _mapperService;

        public ArtistController(IArtistService artistService, IMapper mapperService)
        {
            _artistService = artistService;
            _mapperService = mapperService;
        }
        #endregion

        #region Functions
        [HttpGet("GetMusics/{artistId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Music>>> GetAllMusicsByArtistId(int artistId)
        {
            try
            {
                var artist = await _artistService.GetArtistWithMusicsById(artistId);

                if (artist == null)
                {
                    return NotFound("Artist not found");
                }

                var musicResources = _mapperService.Map<IEnumerable<Music>, IEnumerable<MusicResource>>(artist.Musics);

                return Ok(musicResources);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Artists")]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<Artist>>> GetAllArtists()
        {
            try
            {
                var artists = await _artistService.GetAllArtists();

                if (artists == null)
                {
                    return NotFound("Artists not found");
                }

                var artistResources = _mapperService.Map<IEnumerable<Artist>, IEnumerable<ArtistResource>>(artists);

                return Ok(artistResources);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Artist>> GetArtist(int id)
        {
            try
            {
                var artist = await _artistService.GetArtistById(id);

                if (artist == null)
                {
                    return NotFound("Artist not found");
                }

                var artistResource = _mapperService.Map<Artist, ArtistResource>(artist);

                return Ok(artistResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateArtist")]
        //[Authorize]
        public async Task<ActionResult<Artist>> CreateArtist(ArtistToSaveResource artistToSave)
        {
            try
            {
                // Validation of input Data
                var validation = new ArtistToSaveResourceValidator();
                var validationResult = await validation.ValidateAsync(artistToSave);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                // Mapping
                var artist = _mapperService.Map<ArtistToSaveResource, Artist>(artistToSave);

                // Creation
                var artistCreated = await _artistService.CreateArtist(artist);

                // Mapping
                var artistResource = _mapperService.Map<Artist, ArtistResource>(artistCreated);

                return Ok(artistResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateArtist/{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateArtist(int id, ArtistToSaveResource artistToUpdate)
        {
            try
            {

                // Validation of input Data
                var validation = new ArtistToSaveResourceValidator();
                var validationResult = await validation.ValidateAsync(artistToUpdate);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                // Verify existing artist
                var artistExist = await _artistService.GetArtistById(id);

                if (artistExist == null)
                {
                    return NotFound("Artist not found");
                }

                // Mapping
                var artistUpdate = _mapperService.Map<ArtistToSaveResource, Artist>(artistToUpdate);

                // Update
                await _artistService.UpdateArtist(artistExist, artistUpdate);

                return Ok("The artist is updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteArtist/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteArtist(int id)
        {
            try
            {
                var artistToDelete = await _artistService.GetArtistById(id);

                if (artistToDelete == null)
                {
                    return NotFound("Artist not found");
                }

                await _artistService.DeleteArtist(artistToDelete);

                return Ok("The artist is deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
