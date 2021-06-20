using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using VisioForge.Shared.MediaFoundation.OPM;

namespace PetShop.Models
{
    public class CreateAnimal
    {
         
        public string Name { get; set; } 
        [Range(1, 300)]
        public int Age { get; set; }
        public IFormFile Picture { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}

