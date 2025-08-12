using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IGenericService<T>
    {
        IResult TAdd(T entity);
        IResult TDelete(T entity);
        void TUpdate(T entity);
        List<T> TGetList();
        T TGetByID(int id);
    }
}
