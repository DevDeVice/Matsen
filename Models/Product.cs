﻿using System.ComponentModel.DataAnnotations;

namespace YourNamespace.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ProductCode { get; set; }
    }
}