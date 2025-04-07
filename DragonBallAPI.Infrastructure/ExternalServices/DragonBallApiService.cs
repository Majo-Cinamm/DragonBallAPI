using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using DragonBallAPI.Core.Entities;
using DragonBallAPI.Core.Interfaces;

namespace DragonBallAPI.Infrastructure.ExternalServices
{
    public class DragonBallApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://dragonball-api.com/api";

        public DragonBallApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Character>> GetAllCharactersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/characters");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiCharactersResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return apiResponse?.Items?.Select(item => new Character
                {
                    Id = item.Id,
                    Name = item.Name,
                    Ki = item.Ki,
                    Race = item.Race,
                    Gender = item.Gender,
                    Description = item.Description,
                    Affiliation = item.Affiliation
                }) ?? new List<Character>();
            }
            catch (Exception ex)
            {
                // Loguear el error
                Console.WriteLine($"Error fetching characters: {ex.Message}");
                return new List<Character>();
            }
        }

        public async Task<IEnumerable<Transformation>> GetCharacterTransformationsAsync(int characterId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/characters/{characterId}/transformations");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var transformations = JsonSerializer.Deserialize<List<ApiTransformation>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return transformations?.Select(t => new Transformation
                {
                    Id = t.Id,
                    Name = t.Name,
                    Ki = t.Ki,
                    CharacterId = characterId
                }) ?? new List<Transformation>();
            }
            catch (Exception ex)
            {
                // Loguear el error
                Console.WriteLine($"Error fetching transformations for character {characterId}: {ex.Message}");
                return new List<Transformation>();
            }
        }

        // Clases para deserialización
        private class ApiCharactersResponse
        {
            public List<ApiCharacter> Items { get; set; }
        }

        private class ApiCharacter
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Ki { get; set; }
            public string Race { get; set; }
            public string Gender { get; set; }
            public string Description { get; set; }
            public string Affiliation { get; set; }
        }

        private class ApiTransformation
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Ki { get; set; }
        }
    }
}
