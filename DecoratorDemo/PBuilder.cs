using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorDemo
{
    public interface IPostProductManufacturerBuilder
    {
        IPostProductManufacturerBuilder Costs(double cost);
        Product Priced(double price);
    }

    public class Product
    {
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public double Price { get; set; }
    }
    public interface IPostProductNameBuilder
    {
        IPostProductManufacturerBuilder ManufacturedBy(string manufacturer);
    }
    public interface IPreProductNameBuilder
    {
        IPostProductNameBuilder Named(string name);
    }
    public class ProductBuilder : IPreProductNameBuilder, IPostProductNameBuilder, IPostProductManufacturerBuilder
    {
        private string _manufacturer { get; set; }
        private string _name { get; set; }
        private double _cost { get; set; }
        private double _price { get; set; }

        public static IPreProductNameBuilder CreateProduct()
        {
            return new ProductBuilder();
        }

        public IPostProductNameBuilder Named(string name)
        {
            _name = name;
            return this;
        }

        public IPostProductManufacturerBuilder ManufacturedBy(string manufacturer)
        {
            _manufacturer = manufacturer;
            return this;
        }

        public IPostProductManufacturerBuilder Costs(double cost)
        {
            _cost = cost;
            return this;
        }

        public Product Priced(double price)
        {
            _price = price;
            return this;
        }

        public static implicit operator Product(ProductBuilder builder)
        {
            return builder.Build();
        }

        private Product Build()
        {
            return new Product { Name = _name, Manufacturer = _manufacturer, Cost = _cost, Price = _price };
        }
    }
}
