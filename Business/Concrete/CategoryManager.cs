using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _ICategoryDal;

        public CategoryManager(ICategoryDal ıCategoryDal)
        {
            _ICategoryDal = ıCategoryDal;
        }


        public IDataResult<List<Category>> GetAll()
        {

            return new SuccessDataResult<List<Category>>(_ICategoryDal.GetAll(), Messages.CategoryListedMessage);

        }

        public Category GetById(int categoryId)
        {
            return _ICategoryDal.Get(c => c.CategoryId==categoryId);
        }
    }
}
