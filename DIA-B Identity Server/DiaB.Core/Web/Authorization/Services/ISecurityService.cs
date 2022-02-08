// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     ISecurityService.cs
// Created Date:  2018/11/16 3:25 PM
// ------------------------------------------------------------------------

using System.Threading.Tasks;

namespace DiaB.Core.Web.Authorization.Services
{
    public interface ISecurityService
    {
        Task GenerateAntiforegyToken(string cookieName);

        Task<string> RequestClientCredentialsTokenAsync(string authority, string clientId, string clientSecret, string scope);

        Task<string> RequestPasswordTokenAsync(string authority, string clientId, string clientSecret, string username, string password, string scope);
    }
}
