using AutoMapper;
using System;
using System.Collections.Generic;

namespace Component.Transversal.Adapters
{
    public class AutomapperTypeAdapterFactory
        : ITypeAdapterFactory
    {
        #region Constructor

        /// <summary>
        /// Create a new Automapper type adapter factory
        /// </summary>
        public AutomapperTypeAdapterFactory(IEnumerable<Type> profiles)
        {
            //Mapper.Initialize(cfg =>
            //{
            //    foreach (var item in profiles)
            //    {
            //        cfg.AddProfile(Activator.CreateInstance(item) as Profile);
            //    }
            //});

            var config = new MapperConfiguration(cfg =>
            {
                foreach (var item in profiles)
                {
                    cfg.AddProfile(Activator.CreateInstance(item) as Profile);
                }
            });

            IMapper mapper = config.CreateMapper();
        }

        #endregion

        #region ITypeAdapterFactory Members

        public ITypeAdapter Create()
        {
            return new AutomapperTypeAdapter();
        }

        #endregion
    }
}
