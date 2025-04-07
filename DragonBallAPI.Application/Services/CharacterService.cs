using DragonBallAPI.Application.DTOs;
using DragonBallAPI.Application.Interfaces;
using DragonBallAPI.Core.Interfaces;

namespace DragonBallAPI.Application.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _characterRepository;

        public CharacterService(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public async Task<IEnumerable<CharacterDto>> GetAllAsync()
        {
            var characters = await _characterRepository.GetAllAsync();
            return characters.Select(c => new CharacterDto
            {
                Id = c.Id,
                Name = c.Name,
                Ki = c.Ki,
                Race = c.Race,
                Gender = c.Gender,
                Description = c.Description,
                Affiliation = c.Affiliation,
                Transformations = c.Transformations.Select(t => new TransformationDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Ki = t.Ki,
                    CharacterId = t.CharacterId
                }).ToList()
            });
        }

        public async Task<CharacterDto> GetByIdAsync(int id)
        {
            var character = await _characterRepository.GetByIdAsync(id);
            if (character == null) return null;

            return new CharacterDto
            {
                Id = character.Id,
                Name = character.Name,
                Ki = character.Ki,
                Race = character.Race,
                Gender = character.Gender,
                Description = character.Description,
                Affiliation = character.Affiliation,
                Transformations = character.Transformations.Select(t => new TransformationDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Ki = t.Ki,
                    CharacterId = t.CharacterId
                }).ToList()
            };
        }

        public async Task<IEnumerable<CharacterDto>> GetByNameAsync(string name)
        {
            var characters = await _characterRepository.GetByNameAsync(name);
            return characters.Select(c => new CharacterDto
            {
                Id = c.Id,
                Name = c.Name,
                Ki = c.Ki,
                Race = c.Race,
                Gender = c.Gender,
                Description = c.Description,
                Affiliation = c.Affiliation,
                Transformations = c.Transformations.Select(t => new TransformationDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Ki = t.Ki,
                    CharacterId = t.CharacterId
                }).ToList()
            });
        }

        public async Task<IEnumerable<CharacterDto>> GetByAffiliationAsync(string affiliation)
        {
            var characters = await _characterRepository.GetByAffiliationAsync(affiliation);
            return characters.Select(c => new CharacterDto
            {
                Id = c.Id,
                Name = c.Name,
                Ki = c.Ki,
                Race = c.Race,
                Gender = c.Gender,
                Description = c.Description,
                Affiliation = c.Affiliation,
                Transformations = c.Transformations.Select(t => new TransformationDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Ki = t.Ki,
                    CharacterId = t.CharacterId
                }).ToList()
            });
        }
    }
}
