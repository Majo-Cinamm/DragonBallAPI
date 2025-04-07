using DragonBallAPI.Core.Entities;

namespace DragonBallAPI.Core.Interfaces
{
    public interface ICharacterRepository
    {
        Task<IEnumerable<Character>> GetAllAsync();
        Task<Character> GetByIdAsync(int id);
        Task<IEnumerable<Character>> GetByNameAsync(string name);
        Task<IEnumerable<Character>> GetByAffiliationAsync(string affiliation);
        Task<bool> AddAsync(Character character);
        Task<bool> IsEmptyAsync();
    }
}
