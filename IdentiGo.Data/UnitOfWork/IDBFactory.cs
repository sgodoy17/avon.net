using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentiGo.Data.UnitOfWork
{
    public interface IDbFactory
    {
        MainContext Get();
    }
}
