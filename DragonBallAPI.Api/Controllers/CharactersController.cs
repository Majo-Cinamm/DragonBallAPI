using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DragonBallAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using DragonBallAPI.Application.Services;

namespace DragonBallAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] // Descomentar cuando se implemente la autenticación
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterService _characterService;
        private readonly ISyncService _syncService;

        public CharactersController(ICharacterService characterService, ISyncService syncService)
        {
            _characterService = characterService;
            _syncService = syncService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var characters = await _characterService.GetAllAsync();
            return Ok(characters);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var character = await _characterService.GetByIdAsync(id);
            if (character == null)
                return NotFound($"Personaje con ID {id} no encontrado");

            return Ok(character);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var characters = await _characterService.GetByNameAsync(name);
            return Ok(characters);
        }

        [HttpGet("affiliation/{affiliation}")]
        public async Task<IActionResult> GetByAffiliation(string affiliation)
        {
            var characters = await _characterService.GetByAffiliationAsync(affiliation);
            return Ok(characters);
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncCharacters()
        {
            var result = await _syncService.SyncCharactersAsync();
            return Ok(result);
        }
    }
}
