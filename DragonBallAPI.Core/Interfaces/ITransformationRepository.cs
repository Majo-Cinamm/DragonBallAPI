using DragonBallAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonBallAPI.Core.Interfaces
{
    public interface ITransformationRepository
    {
        Task<IEnumerable<Transformation>> GetAllAsync();
        Task<bool> AddAsync(Transformation transformation);
        Task<bool> IsEmptyAsync();
    }
}
