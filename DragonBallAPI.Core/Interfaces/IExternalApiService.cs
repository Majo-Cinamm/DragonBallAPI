using DragonBallAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonBallAPI.Core.Interfaces
{
    public interface IExternalApiService
    {
        Task<IEnumerable<Character>> GetAllCharactersAsync();
        Task<IEnumerable<Transformation>> GetCharacterTransformationsAsync(int characterId);
    }
}
