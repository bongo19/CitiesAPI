using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstApi.Models
{
    public class PointOfInterestCreationDto
    {
        [Required(ErrorMessage ="You need to provide a city name")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
