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
using Business.BusinessAspects.Autofac;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;

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

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]

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

        [CacheAspect]
        public IDataResult<List<Product>> GetAll()
        {
            //if (DateTime.Now.Hour == 1)
            //{
            //    return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            //}
            //iş kodları
            return new SuccessDataResult<List<Product>>(_IProduct.GetAll(), Messages.ProductListedMessage);
        }

        public IDataResult<List<Product>> GetByProductId(int id)
        {
            return new SuccessDataResult<List<Product>>(_IProduct.GetAll(p => p.CategoryId == id), Messages.ProductListedMessage);
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            if (DateTime.Now.Hour == 22)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(
                _IProduct.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max)
                , Messages.ProductListedMessage);
        }

        [CacheAspect]
        public IDataResult<Product> GetById(int productId)
        {
            return new DataResult<Product>(_IProduct.Get(p => p.ProductId == productId), true, Messages.ProductListedMessage);
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_IProduct.GetProductDetails(), Messages.ProductListedMessage);
        }

        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {

            var result = _IProduct.GetAll(p => p.CategoryId == product.CategoryId);
            if (result.Count>10)
            {
                return new ErrorResult();
            }

            return new SuccessResult();
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
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }

        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)
        {
            Add(product);
            if (product.UnitPrice<10)
            {
                throw new Exception("");
            }

            Add(product);

            return null;
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int categoryId)
        {
            var result=_IProduct.GetAll(p => p.CategoryId == categoryId);
            return new SuccessDataResult<List<Product>>(result);
        }
    }
}
