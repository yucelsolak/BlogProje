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
        IResult TDelete(T entity);
        List<T> TGetList();
        T TGetByID(int id);
    }
}
