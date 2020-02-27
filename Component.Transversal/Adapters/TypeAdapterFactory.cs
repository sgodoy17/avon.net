using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Transversal.Adapters
{

    public static class TypeAdapterFactory
    {
        #region Members

        static Func<ITypeAdapterFactory> _currentTypeAdapterFactoryResolver = null;
        static ITypeAdapterFactory _typeAdapterFactory = null;
        static bool _update;

        public static bool HaveCurrent
        {
            get
            {
                return _currentTypeAdapterFactoryResolver != null;
            }
        }

        #endregion

        #region Public Static Methods

        public static void SetCurrent(ITypeAdapterFactory adapterFactory)
        {
            _currentTypeAdapterFactoryResolver = delegate()
            {
                return adapterFactory;
            };

            _update = true;
        }

        public static void SetCurrent(Func<ITypeAdapterFactory> adapterFactoryResolver)
        {
            _currentTypeAdapterFactoryResolver = adapterFactoryResolver;

            _update = true;
        }

        public static ITypeAdapter CreateAdapter()
        {
            if (_typeAdapterFactory == null || _update)
            {
                _typeAdapterFactory = _currentTypeAdapterFactoryResolver();
                _update = false;
            }

            return _typeAdapterFactory.Create();
        }

        #endregion
    }

}
