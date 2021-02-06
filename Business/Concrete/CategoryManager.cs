using Business.Abstract;
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

        public List<Category> GetAll()
        {
            return _ICategoryDal.GetAll();
        }

        public Category GetById(int categoryId)
        {
            return _ICategoryDal.Get(c => c.CategoryId==categoryId);
        }
    }
}
