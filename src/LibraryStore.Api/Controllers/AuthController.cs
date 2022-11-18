using LibraryStore.Api.Dtos.AuthUserDtos;
using LibraryStore.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryStore.Api.Controllers
{
    [Route("api")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(INotifier notifier, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) : base(notifier)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("new-account")]
        public async Task<ActionResult> Register(RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var identityUser = new IdentityUser
            {
                UserName = registerUserDto.Email,
                Email = registerUserDto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(identityUser, registerUserDto.Password);
            
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(identityUser, isPersistent: false);

                return CustomResponse(registerUserDto);
            }

            foreach(var error in result.Errors)
            {
                NotificationErro(error.Description);
            }

            return CustomResponse(registerUserDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserDto loginUserDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUserDto.Email, loginUserDto.Password, false, true);

            if (result.Succeeded)
                return CustomResponse(loginUserDto);

            if (result.IsLockedOut)
            {
                NotificationErro("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse(loginUserDto);
            }

            NotificationErro("Usuário e Senha incorretos");
            return CustomResponse(loginUserDto);
        }
    }
}
