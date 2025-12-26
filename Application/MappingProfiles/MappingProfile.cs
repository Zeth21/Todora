using Application.CQRS.Commands.RepositoryCommands;
using Application.CQRS.Commands.RepositoryRoleCommands;
using Application.CQRS.Commands.UserCommands;
using Application.CQRS.Commands.WorkTaskCommands;
using Application.CQRS.Results.RepositoryResults;
using Application.CQRS.Results.RepositoryRoleResults;
using Application.CQRS.Results.RoleResults;
using Application.CQRS.Results.UserResults;
using Application.CQRS.Results.WorkTaskResults;
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

            CreateMap<Repository, RepositoryItem>();

            CreateMap<User, UserFindByUserNameQueryResult>();

            CreateMap<Role, RoleGetAllQueryResult>()
                .ForMember(x => x.Id, o => o.MapFrom(s => s.RoleId))
                .ForMember(x => x.RoleName, o => o.MapFrom(s => s.RoleName));

            CreateMap<RepositoryRoleCreateCommand, RepositoryRole>()
                .ForMember(x => x.RoleId, o => o.MapFrom(s => s.RoleName))
                .ForMember(x => x.RepositoryId, o => o.MapFrom(s => s.RepositoryId))
                .ForMember(x => x.UserId, o => o.MapFrom(s => s.UserId));

            CreateMap<RepositoryRole, RepositoryRoleCreateCommandResult>()
                .ForMember(dest => dest.UserRepositoryRole, opt => opt.MapFrom(src => src.Role.RoleName))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.User.Surname))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedDate));

            CreateMap<RepositoryRole, RepositoryRoleUpdateCommandResult>()
                .ForMember(dest => dest.UserRepositoryRole, opt => opt.MapFrom(src => src.Role.RoleName))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.User.Surname))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedDate));

            CreateMap<WorkTaskCreateCommand, WorkTask>();

            CreateMap<WorkTask, WorkTaskCreateCommandResult>()
                .ForMember(dest => dest.TaskCreatedUserName, opt => opt.MapFrom(src => src.User.UserName));
        }
    }
}
