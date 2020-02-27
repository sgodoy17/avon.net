using AutoMapper;
using System;
using System.Linq;

namespace Component.Transversal.Adapters
{
    public class AutomapperTypeAdapter
       : ITypeAdapter
    {
        #region ITypeAdapter Members

        public TTarget Adapt<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class, new()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TTarget>();
            });

            IMapper mapper = config.CreateMapper();

            try
            {
                return mapper.Map<TSource, TTarget>(source);
            }
            catch (Exception)
            {
                try
                {
                    this.Init();

                    return mapper.Map<TSource, TTarget>(source);
                }
                catch (Exception)
                {
                    try
                    {
                        SetMap<TSource, TTarget>();

                        return mapper.Map<TSource, TTarget>(source);
                    }
                    catch
                    {
                        return default(TTarget);
                    }
                }
            }
        }

        public void SetMap<TSource, TTarget>()
            where TSource : class
            where TTarget : class, new()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TTarget>();
            });

            IMapper mapper = config.CreateMapper();
        }

        #endregion

        void Init()
        {
            var domain = AppDomain.CurrentDomain;
            var filterAss = domain.GetAssemblies().Where(a => a.DefinedTypes.Any(dt => dt.Name == "DTOMapReference"));
            var dtoMapReference = filterAss.SelectMany(s => s.GetTypes()).FirstOrDefault(t => t.Name == "DTOMapReference");
            if (dtoMapReference != null)
                dtoMapReference.GetMethod("GetProfiles").Invoke(null, null);

        }
    }
}
