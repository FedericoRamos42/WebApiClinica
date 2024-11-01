﻿using Application.Interfaces;
using Application.Models.Response;
using Domain.Entities;
using Domain.Exceptions;
using Domain.InterFaces;
using Microsoft.VisualBasic.FileIO;
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
        public User GetById (int id)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
            {
                throw new NotFoundException("nulo");
            }
            return entity;

        }
        public IEnumerable<UserResponse> GetAll() 
        {
            var list = _repository.GetAll();
            return UserResponse.CreatelistDto(list);
        }
        public string Delete (int id)
        {
            var entity = _repository.GetById(id);
            if(entity == null)
            {
                return "No existe el User";
            }

            entity.IsAvailable = false;
             _repository.UpdateUser(entity);
            return "Baja logica con exito";
        }
        public IEnumerable<User>? GetByState(bool? state)
        {
            return _repository.GetByStatus(state);
        }
    }
}
