using DragonBallAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonBallAPI.Application.Interfaces
{
    public interface ICharacterService
    {
        Task<IEnumerable<CharacterDto>> GetAllAsync();
        Task<CharacterDto> GetByIdAsync(int id);
        Task<IEnumerable<CharacterDto>> GetByNameAsync(string name);
        Task<IEnumerable<CharacterDto>> GetByAffiliationAsync(string affiliation);
    }
}
