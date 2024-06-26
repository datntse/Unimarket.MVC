﻿namespace Unimarket.MVC.Models.CreateModels
{
    public class ProductCM
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String ProductDetail { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public int Status { get; set; }
        public String CategoryId { get; set; }
    }

    public class ProductDTO
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String ProductDetail { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public int Status { get; set; }
        public String ImageUrl { get; set; }
        public List<String> CategoryId { get; set; }
        public List<String> SubImageUrl { get; set; }
    }
}
