﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonBallAPI.Application.DTOs
{
    public class CharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ki { get; set; }
        public string Race { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public string Affiliation { get; set; }
        public List<TransformationDto> Transformations { get; set; } = new List<TransformationDto>();
    }
}
