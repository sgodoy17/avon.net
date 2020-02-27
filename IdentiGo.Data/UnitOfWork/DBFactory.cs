using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentiGo.Data.UnitOfWork
{
    public class DbFactory : Disposable, IDbFactory
    {
        public DbFactory()
        {
            Database.SetInitializer<MainContext>(null);
        }

        private MainContext _dataContext;

        public MainContext Get()
        {
            return _dataContext ?? (_dataContext = new MainContext());
        }

        protected override void DisposeCore()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }
    }
}
