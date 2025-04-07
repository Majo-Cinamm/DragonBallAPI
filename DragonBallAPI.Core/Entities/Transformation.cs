using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonBallAPI.Core.Entities
{
    public class Transformation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ki { get; set; }

        // Relación con Character
        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
