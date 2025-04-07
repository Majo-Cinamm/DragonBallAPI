using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragonBallAPI.Core.Entities;
using DragonBallAPI.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DragonBallAPI.Infrastructure.Data.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly AppDbContext _context;

        public CharacterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Character>> GetAllAsync()
        {
            return await _context.Characters
                .Include(c => c.Transformations)
                .ToListAsync();
        }

        public async Task<Character> GetByIdAsync(int id)
        {
            return await _context.Characters
                .Include(c => c.Transformations)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Character>> GetByNameAsync(string name)
        {
            return await _context.Characters
                .Include(c => c.Transformations)
                .Where(c => c.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<IEnumerable<Character>> GetByAffiliationAsync(string affiliation)
        {
            return await _context.Characters
                .Include(c => c.Transformations)
                .Where(c => c.Affiliation == affiliation)
                .ToListAsync();
        }

        public async Task<bool> AddAsync(Character character)
        {
            await _context.Characters.AddAsync(character);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsEmptyAsync()
        {
            return !await _context.Characters.AnyAsync();
        }
    }
}
