using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace censudex_inventory_service_api.src.Model
{
    public class Product
    {
        [Key]
        public Guid id { get; set; }
        [Required]  
        public string name { get; set; }
        [Required]
        public string category { get; set; }
        [Required]
        public int stock { get; set; }
        [Required]
        public bool is_active { get; set; }
    }
}