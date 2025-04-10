using DragonBallAPI.Core.Entities;
using DragonBallAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            // ✅ Verifica si hay datos en las tablas
            var hasCharacters = await _context.Characters.AnyAsync();
            var hasTransformations = await _context.Transformations.AnyAsync();

            if (hasCharacters || hasTransformations)
            {
                return "La base de datos ya contiene datos. Limpie las tablas antes de sincronizar.";
            }

            var response = await _httpClient.GetAsync("characters");
            if (!response.IsSuccessStatusCode) return "Error al consumir la API externa.";

            var content = await response.Content.ReadAsStringAsync();
            var apiCharacters = JsonConvert.DeserializeObject<ApiCharacterResponse>(content);

            int savedCharacters = 0;

            foreach (var apiChar in apiCharacters.Items)
            {
                if (apiChar.Race != "Saiyan") continue;

                var character = new Character
                {
                    Name = apiChar.Name,
                    Ki = apiChar.Ki,
                    Race = apiChar.Race,
                    Gender = apiChar.Gender,
                    Description = apiChar.Description,
                    Affiliation = apiChar.Affiliation
                };

                _context.Characters.Add(character);
                await _context.SaveChangesAsync(); // Para obtener el Id del personaje

                // Solo guardar transformaciones si el personaje es Z Fighter
                if (apiChar.Affiliation == "Z Fighter" && apiChar.Transformations != null)
                {
                    foreach (var t in apiChar.Transformations.Where(t => t != null))
                    {
                        var transformation = new Transformation
                        {
                            Name = t.Name,
                            Ki = t.Ki,
                            CharacterId = character.Id
                        };

                        _context.Transformations.Add(transformation);
                    }

                    await _context.SaveChangesAsync(); // Guardar transformations
                }

                savedCharacters++;
            }

            return savedCharacters > 0
                ? "Sincronización exitosa."
                : "No se encontraron personajes válidos para guardar.";
        }
    }

    // Clases auxiliares para deserializar datos de la API

    public class ApiCharacterResponse
    {
        [JsonProperty("items")]
        public List<ApiCharacter> Items { get; set; }
    }

    public class ApiCharacter
    {
        public string Name { get; set; }
        public string Ki { get; set; }
        public string Race { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public string Affiliation { get; set; }
        public List<ApiTransformation> Transformations { get; set; }
    }

    public class ApiTransformation
    {
        public string Name { get; set; }
        public string Ki { get; set; }
    }
}
