using System;
using System.Collections.Generic;

#nullable disable

namespace FileCreateWorkerService.Models
{
    public partial class Food
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public string FoodDescription { get; set; }
        public double FoodPrice { get; set; }
        public string ImageUrl { get; set; }
        public int FoodStock { get; set; }
        public int CategoryId { get; set; }
    }
}
