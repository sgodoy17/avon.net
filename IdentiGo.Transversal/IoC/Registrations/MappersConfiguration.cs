using AutoMapper;
using IdentiGo.Domain.DTO;
using IdentiGo.Domain.DTO.Master;
using IdentiGo.Domain.Security;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using IdentiGo.Domain.Entity.General;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Domain.Entity.Master;

namespace IdentiGo.Transversal.IoC.Registrations
{
    public class MappersConfiguration
        : Profile
    {
        protected override void Configure()
        {
            #region EntityToDTO
            //Mapper.CreateMap<User, UserDto>();
            //Mapper.CreateMap<User, UserDtoList>();
            //Mapper.CreateMap<Role, RoleDto>();
            //Mapper.CreateMap<Company, CompanyDto>();
            //Mapper.CreateMap<Config, ConfigDto>();
            //Mapper.CreateMap<AffiliationType, AffiliationTypeDto>();
            //Mapper.CreateMap<UserAffiliation, UserAffiliationDto>();
            //Mapper.CreateMap<Country, CountryDto>();
            //Mapper.CreateMap<Department, DepartmentDto>();
            //Mapper.CreateMap<City, CityDto>();
            //Mapper.CreateMap<VoteSite, VoteSiteDto>();
            //Mapper.CreateMap<SecretaryTransit, SecretaryTransitDto>();
            //Mapper.CreateMap<Nomination, UserValidationDto>();

            var configEntityToDTO = new MapperConfiguration(cfg => {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<User, UserDtoList>();
                cfg.CreateMap<Role, RoleDto>();
                cfg.CreateMap<Company, CompanyDto>();
                cfg.CreateMap<Config, ConfigDto>();
                cfg.CreateMap<AffiliationType, AffiliationTypeDto>();
                cfg.CreateMap<UserAffiliation, UserAffiliationDto>();
                cfg.CreateMap<Country, CountryDto>();
                cfg.CreateMap<Department, DepartmentDto>();
                cfg.CreateMap<City, CityDto>();
                cfg.CreateMap<VoteSite, VoteSiteDto>();
                cfg.CreateMap<SecretaryTransit, SecretaryTransitDto>();
                cfg.CreateMap<Nomination, UserValidationDto>();
            });
            #endregion

            #region DTOToEntity
            //Mapper.CreateMap<UserDto, User>();
            //Mapper.CreateMap<RoleDto, Role>();
            //Mapper.CreateMap<CompanyDto, Company>();
            //Mapper.CreateMap<ConfigDto, Config>();
            //Mapper.CreateMap<AffiliationTypeDto, AffiliationType>();
            //Mapper.CreateMap<UserAffiliationDto, UserAffiliation>();
            //Mapper.CreateMap<CountryDto, Country>();
            //Mapper.CreateMap<DepartmentDto, Department>();
            //Mapper.CreateMap<CityDto, City>();
            //Mapper.CreateMap<VoteSiteDto, VoteSite>();
            //Mapper.CreateMap<UserValidationDto, Nomination>();

            var configDTOToEntity = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDto, User>();
                cfg.CreateMap<RoleDto, Role>();
                cfg.CreateMap<CompanyDto, Company>();
                cfg.CreateMap<ConfigDto, Config>();
                cfg.CreateMap<AffiliationTypeDto, AffiliationType>();
                cfg.CreateMap<UserAffiliationDto, UserAffiliation>();
                cfg.CreateMap<CountryDto, Country>();
                cfg.CreateMap<DepartmentDto, Department>();
                cfg.CreateMap<CityDto, City>();
                cfg.CreateMap<VoteSiteDto, VoteSite>();
                cfg.CreateMap<UserValidationDto, Nomination>();
            });
            #endregion
        }
    }

    public static class DtoMapReference
    {
        public static IEnumerable<Type> GetProfiles()
        {
            //scan all assemblies finding Automapper Profile
            var profiles = Assembly
                            .GetExecutingAssembly()
                            .GetTypes()
                            .Where(t =>
                                t.BaseType == typeof(Profile));

            return profiles;

        }
    }
}
