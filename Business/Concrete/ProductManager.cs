using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.Utilities.Business;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _IProduct;
        ICategoryService _categoryService;

        public ProductManager(IProductDal ıProduct, ICategoryService categoryService)
        {
            _IProduct = ıProduct;
            _categoryService = categoryService;
        }


        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {

            IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                               CheckIfProductNameExists(product.ProductName),
                               CheckIfCategoryLimitExceded());

            if (result != null)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }

            _IProduct.Add(product);

            return new SuccessResult(Messages.ProductAddedMessage);

        }

        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 1)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            //iş kodları
            return new SuccessDataResult<List<Product>>(_IProduct.GetAll(), Messages.ProductListedMessage);
        }

        public IDataResult<List<Product>> GetAllProductId(int id)
        {
            return new SuccessDataResult<List<Product>>(_IProduct.GetAll(p => p.CategoryId == id), Messages.ProductListedMessage);
        }

        public IDataResult<List<Product>> GetAllUnitPrice(decimal min, decimal max)
        {
            if (DateTime.Now.Hour == 22)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(
                _IProduct.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max)
                , Messages.ProductListedMessage);
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new DataResult<Product>(_IProduct.Get(p => p.ProductId == productId), true, Messages.ProductListedMessage);
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_IProduct.GetProductDetails(), Messages.ProductListedMessage);
        }

        [ValidationAspect(typeof(ProductValidator))]

        public IResult Update(Product product)
        {

            return null;

        }


        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var result = _IProduct.GetAll(p => p.CategoryId == categoryId);
            if (result.Count >= 15)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _IProduct.GetAll(p => p.ProductName == productName).Any();

            if (result)
            {
                return new ErrorResult(Messages.ProductAlreadyExists);
            }

            return new SuccessResult();
        }


        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count() > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }
    }
}
