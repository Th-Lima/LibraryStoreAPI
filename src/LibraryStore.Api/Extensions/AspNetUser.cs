﻿using LibraryStore.Business.Interfaces;
using System.Security.Claims;

namespace LibraryStore.Api.Extensions
{
    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name => _accessor.HttpContext.User.Identity.Name;

        public Guid GetUserId() =>
            IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;

        public string GetUserEmail() =>
            IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : string.Empty;

        public bool IsAuthenticated() =>
            _accessor.HttpContext.User.Identity.IsAuthenticated;

        public bool IsInRole(string role) =>
            _accessor.HttpContext.User.IsInRole(role);

        public IEnumerable<Claim> GetClaimsIdentity() =>
            _accessor.HttpContext.User.Claims;
    }

    public static class ClaimsPrincipalExtension
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if(principal == null)
                throw new ArgumentException(nameof(principal));

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);

            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentException(nameof(principal));

            var claim = principal.FindFirst(ClaimTypes.Email);
            
            return claim?.Value;
        }
    }
}
