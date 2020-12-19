using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Managers
{
    public class UserAccountManager : IUserAccountManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IDomainRepository _domainRepository;
        private readonly IMapper _mapper;

        public UserAccountManager(IUserRepository userRepository, IDomainRepository domainRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _domainRepository = domainRepository;
            _mapper = mapper;
        }

        public async Task<JwtConfigurationDto> CreateAccountAsync(UserDto userDto, string url)
        {
            //TODO fail safe voor colliding Guid
            userDto.Id = Guid.NewGuid();

            DomainDto domainDto = _mapper.Map<DomainDto>(await _domainRepository.GetDomainFromUrl(url));
            userDto.Domains.Add(domainDto);

            UserSchemaDto schemaDto = _domainRepository.GetUserSchemaFromTenantIdAsync(domainDto.TenantId);
            userDto.UserModel = new UserModelDto()
            {
                UserId = userDto.Id,
                SchemaTenantId = domainDto.TenantId,
                UserDataModel = userDto.DataModel,
                User = userDto,
                Schema = schemaDto
            };

            UserEntity userEntity = _mapper.Map<UserEntity>(userDto);
            await _userRepository.Insert(userEntity);

            return await GetUserJwtConfigurationAsync(userDto);
        }

        public async Task<JwtConfigurationDto> GetUserJwtConfigurationAsync(UserDto userDto)
        {
            JwtConfigurationDto jwtConfigurationDto = _mapper.Map<JwtConfigurationDto>(await _userRepository.GetUserJwtConfiguration(userDto.Id));

            jwtConfigurationDto.Claims = userDto.Claims;

            return jwtConfigurationDto;
        }
    }
}
