using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace censudex_inventory_service_api.src.Model
{
    [Table("Product")]
    public class Product : BaseModel
    {
        [PrimaryKey("id",true)]
        public Guid id { get; set; }
        [Column("name")]
        public string name { get; set; }
        [Column("category")]
        public string category { get; set; }
        [Column("stock")]
        public int stock { get; set; }
        [Column("is_active")]
        public bool is_active { get; set; }
        [Column("minimum_stock")]
        public int minimum_stock { get; set; }
    }
}