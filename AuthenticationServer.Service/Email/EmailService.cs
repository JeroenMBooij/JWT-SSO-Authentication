﻿using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.DTOs;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Service.Email
{
    public class EmailService : IEmailService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EmailService(IUserRepository userRepository, ITenantRepository tenantRepository, 
            IMapper mapper, ILogger logger)
        {
            _userRepository = userRepository;
            _tenantRepository = tenantRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task VerifyTenantEmail(string code)
        {
            TenantDto tenantDto;
            try
            {
                tenantDto = _mapper.Map<TenantDto>(await _tenantRepository.Get(Guid.Parse(code)));
            }
            catch (Exception)
            {
                _logger.LogError("Failed to find Tenant {TenantID}", code);
                throw new Exception("Bad Request contact your system administrator.");
            }

            await _tenantRepository.SetVerified(tenantDto.Id);
        }

        public Task VerifyUserEmail(string code)
        {
            throw new NotImplementedException();
        }
    }
}
