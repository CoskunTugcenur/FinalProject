using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
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

        public IResult Add(Product product)
        {
            if (product.ProductName.Length<2)
            {
                return new ErrorResult(Messages.ProductNameInvalid);
            }
            _IProduct.Add(product);

            return new SuccessResult(Messages.ProductAddedMessage);
        }

        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour==1)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            //iş kodları
            return new SuccessDataResult<List<Product>>(_IProduct.GetAll(),Messages.ProductListedMessage);
        }

        public IDataResult<List<Product>> GetAllProductId(int id)
        {
            return new SuccessDataResult<List<Product>>(_IProduct.GetAll(p => p.CategoryId == id),Messages.ProductListedMessage);
        }

        public IDataResult<List<Product>> GetAllUnitPrice(decimal min, decimal max)
        {
            if (DateTime.Now.Hour ==22)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new  SuccessDataResult<List<Product>>(
                _IProduct.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max)
                , Messages.ProductListedMessage);
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new DataResult<Product>(_IProduct.Get(p => p.ProductId == productId),true,Messages.ProductListedMessage);
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_IProduct.GetProductDetails(),Messages.ProductListedMessage);
        }
    }
}
