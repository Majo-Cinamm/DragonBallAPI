using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragonBallAPI.Application.DTOs;
using DragonBallAPI.Application.Interfaces;
using DragonBallAPI.Core.Interfaces;
using DragonBallAPI.Infrastructure.Data.Repositories;

namespace DragonBallAPI.Application.Services
{
    public class TransformationService : ITransformationService
    {
        private readonly ITransformationRepository _transformationRepository;

        public TransformationService(ITransformationRepository transformationRepository)
        {
            _transformationRepository = transformationRepository;
        }

        public async Task<IEnumerable<TransformationDto>> GetAllAsync()
        {
            var transformations = await _transformationRepository.GetAllAsync();
            return transformations.Select(t => new TransformationDto
            {
                Id = t.Id,
                Name = t.Name,
                Ki = t.Ki,
                CharacterId = t.CharacterId
            });
        }
    }
}
