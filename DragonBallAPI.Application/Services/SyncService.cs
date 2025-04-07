using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragonBallAPI.Application.Interfaces;
using DragonBallAPI.Core.Entities;
using DragonBallAPI.Core.Interfaces;
using DragonBallAPI.Infrastructure.Data.Repositories;


namespace DragonBallAPI.Application.Services
{
    public class SyncService : ISyncService
    {
        private readonly IExternalApiService _externalApiService;
        private readonly ICharacterRepository _characterRepository;
        private readonly ITransformationRepository _transformationRepository;

        public SyncService(
            IExternalApiService externalApiService,
        ICharacterRepository characterRepository,
            ITransformationRepository transformationRepository)
        {
            _externalApiService = externalApiService;
            _characterRepository = characterRepository;
            _transformationRepository = transformationRepository;
        }

        public async Task<string> SyncCharactersAsync()
        {
            // Verificar si las tablas están vacías según las reglas de negocio
            bool charactersEmpty = await _characterRepository.IsEmptyAsync();
            bool transformationsEmpty = await _transformationRepository.IsEmptyAsync();

            if (!charactersEmpty || !transformationsEmpty)
            {
                return "No se puede sincronizar. Las tablas deben estar vacías primero.";
            }

            // Obtener personajes de la API externa
            var characters = await _externalApiService.GetAllCharactersAsync();

            // Filtrar solo personajes Saiyan según las reglas de negocio
            var saiyanCharacters = characters.Where(c => c.Race == "Saiyan").ToList();

            int charactersAdded = 0;
            int transformationsAdded = 0;

            foreach (var character in saiyanCharacters)
            {
                // Guardar el personaje
                await _characterRepository.AddAsync(character);
                charactersAdded++;

                // Solo guardar transformaciones de personajes "Z Fighter"
                if (character.Affiliation == "Z Fighter")
                {
                    // Obtener transformaciones del personaje
                    var transformations = await _externalApiService.GetCharacterTransformationsAsync(character.Id);

                    foreach (var transformation in transformations)
                    {
                        // Asignar el ID del personaje a la transformación
                        transformation.CharacterId = character.Id;

                        // Guardar la transformación
                        await _transformationRepository.AddAsync(transformation);
                        transformationsAdded++;
                    }
                }
            }

            return $"Sincronización completada. Se agregaron {charactersAdded} personajes Saiyan y {transformationsAdded} transformaciones de personajes 'Z Fighter'.";
        }
    }
}
