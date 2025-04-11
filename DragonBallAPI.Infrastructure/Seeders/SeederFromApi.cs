using DragonBallAPI.Core.Entities;
using DragonBallAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonBallAPI.Infrastructure.Seeders
{
    public class SeederFromApi
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public SeederFromApi(AppDbContext context)
        {
            _context = context;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://dragonball-api.com/api/")
            };
        }

        public async Task<string> SeedAsync()
        {
            var hasCharacters = await _context.Characters.AnyAsync();
            var hasTransformations = await _context.Transformations.AnyAsync();

            if (hasCharacters || hasTransformations)
            {
                return "La base de datos ya contiene datos. Limpie las tablas antes de sincronizar.";
            }

            // Traer todos los IDs de las transformaciones
            var transformationListResponse = await _httpClient.GetAsync("transformations");
            if (!transformationListResponse.IsSuccessStatusCode) return "Error al obtener lista de transformaciones.";

            var listContent = await transformationListResponse.Content.ReadAsStringAsync();
            var allTransformations = JsonConvert.DeserializeObject<List<ApiTransformationShort>>(listContent);

            int saved = 0;

            foreach (var shortTrans in allTransformations)
            {
                // Consumir la transformación individual
                var detailResponse = await _httpClient.GetAsync($"transformations/{shortTrans.Id}");
                if (!detailResponse.IsSuccessStatusCode) continue;

                var detailContent = await detailResponse.Content.ReadAsStringAsync();
                var fullTransformation = JsonConvert.DeserializeObject<ApiTransformationDetail>(detailContent);

                var character = fullTransformation.Character;

                // Solo guardar si el personaje es Saiyan y Z Fighter
                if (character.Race != "Saiyan" || character.Affiliation != "Z Fighter") continue;

                // Verificar si ya se guardó el personaje
                var existingCharacter = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Name == character.Name);

                if (existingCharacter == null)
                {
                    existingCharacter = new Character
                    {
                        Name = character.Name,
                        Ki = character.Ki,
                        Race = character.Race,
                        Gender = character.Gender,
                        Description = character.Description,
                        Affiliation = character.Affiliation
                    };

                    _context.Characters.Add(existingCharacter);
                    await _context.SaveChangesAsync();
                }

                // Guardar la transformación relacionada
                var transformationEntity = new Transformation
                {
                    Name = fullTransformation.Name,
                    Ki = fullTransformation.Ki,
                    CharacterId = existingCharacter.Id
                };

                _context.Transformations.Add(transformationEntity);
                await _context.SaveChangesAsync();

                saved++;
            }

            return saved > 0
                ? $"Sincronización completa. Transformaciones guardadas: {saved}"
                : "No se guardaron transformaciones.";
        }

        // Clases auxiliares para deserializar

        public class ApiTransformationShort
        {
            [JsonProperty("id")]
            public int Id { get; set; }
        }

        public class ApiTransformationDetail
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("ki")]
            public string Ki { get; set; }

            [JsonProperty("character")]
            public ApiCharacter Character { get; set; }
        }

        public class ApiCharacter
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("ki")]
            public string Ki { get; set; }

            [JsonProperty("race")]
            public string Race { get; set; }

            [JsonProperty("gender")]
            public string Gender { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("affiliation")]
            public string Affiliation { get; set; }
        }
    }
}
