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
    public class TransformationRepository : ITransformationRepository
    {
        private readonly AppDbContext _context;

        public TransformationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transformation>> GetAllAsync()
        {
            return await _context.Transformations
                .Include(t => t.Character)
                .ToListAsync();
        }

        public async Task<bool> AddAsync(Transformation transformation)
        {
            await _context.Transformations.AddAsync(transformation);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsEmptyAsync()
        {
            return !await _context.Transformations.AnyAsync();
        }
    }
}
