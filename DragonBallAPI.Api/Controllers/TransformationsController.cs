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
    public class TransformationsController : ControllerBase
    {
        private readonly ITransformationService _transformationService;

        public TransformationsController(ITransformationService transformationService)
        {
            _transformationService = transformationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transformations = await _transformationService.GetAllAsync();
            return Ok(transformations);
        }
    }
}
