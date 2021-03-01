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
    public class ComposerController : ControllerBase
    {
        #region Properties & constructors

        private readonly IComposerService _composerService;

        private readonly IMapper _mapperService;

        public ComposerController(IComposerService composerService, IMapper mapperService)
        {
            _composerService = composerService;
            _mapperService = mapperService;
        }
        #endregion

        #region Functions
        [HttpGet("Composers")]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<Composer>>> GetAllComposers()
        {
            try
            {
                var composers = await _composerService.GetAllComposer();

                if (composers == null)
                {
                    return NotFound("Composers not found");
                }

                var composerResources = _mapperService.Map<IEnumerable<Composer>, IEnumerable<ComposerResource>>(composers);

                return Ok(composerResources);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateComposer")]
        //[Authorize]
        public async Task<ActionResult<ComposerResource>> CreateComposer(ComposerToSaveResource composerToSave)
        {
            try
            {
                // Validation of input Data
                var validation = new ComposerToSaveResourceValidator();
                var validationResult = await validation.ValidateAsync(composerToSave);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                // Mapping
                var composer = _mapperService.Map<ComposerToSaveResource, Composer>(composerToSave);

                // Creation
                var composerCreated = await _composerService.CreateComposer(composer);

                // Mapping
                var composerResource = _mapperService.Map<Composer, ComposerResource>(composerCreated);

                return Ok(composerResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateComposer/{id}")]
        //[Authorize]
        public async Task<ActionResult> UpdateComposer(string id, ComposerToSaveResource composerToUpdate)
        {
            try
            {
                // Validation of input Data
                var validation = new ComposerToSaveResourceValidator();
                var validationResult = await validation.ValidateAsync(composerToUpdate);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                var composerExist = await _composerService.GetComposerById(id);

                if (composerExist == null)
                {
                    return NotFound("Composer Not Found");
                }

                // Mapping
                var composer = _mapperService.Map<ComposerToSaveResource, Composer>(composerToUpdate);

                // Update
                _composerService.UpdateCompser(id, composer);

                return Ok("Composer Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteComposer/{id}")]
        //[Authorize]
        public async Task<ActionResult> DeleteComposer(string id)
        {
            try
            {
                var composerExist = await _composerService.GetComposerById(id);

                if (composerExist == null)
                {
                    return NotFound("Composer Not Found");
                }

                // Delete
                if(await _composerService.DeleteComposer(id))
                {
                    return Ok("Composer deleted");
                }

                return BadRequest("Delete error");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
