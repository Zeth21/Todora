using Application.CQRS.Commands.RepositoryCommands;
using Application.CQRS.Commands.UserCommands;
using Application.CQRS.Results.RepositoryResults;
using Application.CQRS.Results.RoleResults;
using Application.CQRS.Results.UserResults;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserCreateCommand, User>()
                .ForMember(x => x.UserName, o => o.MapFrom(s => s.UserName))
                .ForMember(x => x.Email, o => o.MapFrom(s => s.Email))
                .ForMember(x => x.Name, o => o.MapFrom(s => s.Name))
                .ForMember(x => x.Surname, o => o.MapFrom(s => s.Surname))
                .ForMember(x => x.UserPhotoPath, o => o.MapFrom(s => s.ProfilePhotoPath));

            CreateMap<RepositoryCreateCommand, Repository>()
                .ForMember(x => x.RepositoryUserId, o => o.MapFrom(s => s.UserId));
            CreateMap<Repository, RepositoryCreateCommandResult>();

            CreateMap<Repository, RepositoryGetUserOwningsQueryResult>();
            CreateMap<Repository, RepositoryGetUserWorkingsQueryResult>();

            CreateMap<User, UserFindByUserNameQueryResult>();

            CreateMap<Role, RoleGetAllQueryResult>()
                .ForMember(x => x.Id, o => o.MapFrom(s => s.RoleId))
                .ForMember(x => x.RoleName, o => o.MapFrom(s => s.RoleName));
        }
    }
}
