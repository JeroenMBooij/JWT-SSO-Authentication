using AuthenticationServer.Common.Exceptions;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels.Applications;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Workers
{
    public class ApplicationManager : IApplicationManager
    {
        private readonly IMapper _mapper;
        private readonly IJwtTokenWorker _jwtManager;
        private readonly IRepository _repository;

        public ApplicationManager(IMapper mapper, IJwtTokenWorker jwtManager, IRepository repository)
        {
            _mapper = mapper;
            _jwtManager = jwtManager;
            _repository = repository;
        }

        public async Task<List<ApplicationWithId>> GetApplications(string token)
        {
            var adminId = _jwtManager.GetUserId(token);

            var applicationDtos = _mapper.Map<List<ApplicationDto>>(await _repository.Application.GetAll(adminId));

            return _mapper.Map<List<ApplicationWithId>>(applicationDtos);
        }

        public async Task<ApplicationWithId> GetApplication(string token, Guid id)
        {
            var adminId = _jwtManager.GetUserId(token);

            var applicationDto = _mapper.Map<ApplicationDto>(await _repository.Application.Get(adminId, id));

            return _mapper.Map<ApplicationWithId>(applicationDto);
        }

        public async Task<Guid> CreateApplication(string token, Application application)
        {
            var applicationDto = _mapper.Map<ApplicationDto>(application);

            //TODO auto increment Guid in EF core
            populateApplicationProperties(applicationDto, token);

            Guid? newApplicationId = null;
            using (_repository.Database)
            {
                _repository.Database.BeginTransaction();

                newApplicationId = await _repository.Application.Insert(_mapper.Map<ApplicationEntity>(applicationDto));

                foreach (DomainNameDto domainNameDto in applicationDto.Domains)
                    await _repository.DomainName.Insert(_mapper.Map<DomainNameEntity>(domainNameDto));

                foreach (JwtTenantConfigDto jwtConfigDto in applicationDto.JwtTenantConfigurations)
                    await _repository.JwtTenantConfig.Insert(_mapper.Map<JwtTenantConfigEntity>(jwtConfigDto));
            }

            return newApplicationId.Value;

        }

        private void populateApplicationProperties(ApplicationDto applicationDto, string token)
        {
            if(applicationDto.Id == Guid.Empty)
                applicationDto.Id = Guid.NewGuid();

            var adminId = _jwtManager.GetUserId(token);

            applicationDto.AdminId = adminId;

            foreach (DomainNameDto domainDto in applicationDto.Domains)
            {
                domainDto.Id = Guid.NewGuid();
                domainDto.Application = applicationDto;
            }

            foreach (JwtTenantConfigDto jwtTenantconfig in applicationDto.JwtTenantConfigurations)
            {
                jwtTenantconfig.Id = Guid.NewGuid();
                jwtTenantconfig.Application = applicationDto;
            }
        }

        public async Task UpdateApplication(string token, Guid id, Application application)
        {
            Guid adminId = _jwtManager.GetUserId(token);

            ApplicationDto applicationDto = _mapper.Map<ApplicationDto>(await _repository.Application.Get(adminId, id));
            if (applicationDto is null)
                throw new AuthenticationApiException("update authentication", "Application not found", 404);


            ApplicationDto mergedApplicationDto = _mapper.Map(application, applicationDto);

            using (_repository.Database)
            {
                _repository.Database.BeginTransaction();

                await _repository.Application.Update(adminId, applicationDto.Id, _mapper.Map<ApplicationEntity>(mergedApplicationDto));

                foreach (DomainNameDto domainDto in mergedApplicationDto.Domains)
                    await _repository.DomainName.Update(adminId, domainDto.Id, _mapper.Map<DomainNameEntity>(domainDto));

                foreach (JwtTenantConfigDto jwtTenantConfigDto in mergedApplicationDto.JwtTenantConfigurations)
                    await _repository.JwtTenantConfig.Update(adminId, jwtTenantConfigDto.Id, _mapper.Map<JwtTenantConfigEntity>(jwtTenantConfigDto));
            }
        }


        public async Task DeleteApplication(string token, Guid id)
        {
            var adminId = _jwtManager.GetUserId(token);

            await _repository.Application.Delete(adminId, id);
        }

        public async Task<Guid> GetApplicationIconUUID(Guid applicationId)
        {
            return await _repository.Application.GetApplicationIconUUID(applicationId);
        }
    }
}
