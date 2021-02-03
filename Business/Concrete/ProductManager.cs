using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _IProduct;

        public ProductManager(IProductDal ıProduct)
        {
            _IProduct = ıProduct;
        }


        public List<Product> GetAll()
        {
            //iş kodları
            return _IProduct.GetAll();
        }

        public List<Product> GetAllProductId(int id)
        {
            return _IProduct.GetAll(p => p.CategoryId == id);
        }

        public List<Product> GetAllUnitPrice(decimal min, decimal max)
        {
            return _IProduct.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max);
        }
    }
}
