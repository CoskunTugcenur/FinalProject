using Core.DataAccess;
using DataAccess.Concrete;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface ICustomerDal:IEntityRepository<Customer>
    {
        
    }
}
