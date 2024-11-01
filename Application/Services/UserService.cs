﻿using Application.Interfaces;
using Application.Models.Response;
using Domain.InterFaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<UserResponse> GetAll() 
        {
            var list = _repository.GetAll();
            return UserResponse.CreatelistDto(list);
        }
    }
}