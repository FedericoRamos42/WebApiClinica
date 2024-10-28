using Application.Models.Request;
using Application.Models.Response;
using Domain.Entities;
using Domain.InterFaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthenticationService
    {
        User? ValidateUser(CredentialRequest credentialRequest);
        AuthenticationResponse? AuthenticateCredentials(CredentialRequest credentialRequest);
    }
}
