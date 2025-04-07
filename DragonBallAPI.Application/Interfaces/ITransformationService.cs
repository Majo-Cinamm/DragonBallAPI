using DragonBallAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonBallAPI.Application.Interfaces
{
    public interface ITransformationService
    { 
        Task<IEnumerable<TransformationDto>> GetAllAsync();
    }
}
