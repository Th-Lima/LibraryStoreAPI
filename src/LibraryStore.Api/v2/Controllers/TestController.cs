using LibraryStore.Api.Controllers;
using LibraryStore.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryStore.Api.v2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/test")]
    public class TestController : MainController
    {
        public TestController(INotifier notifier, IUser appUser) : base(notifier, appUser)
        {
        }

        [HttpGet]
        public string Value()
        {
            return "Sou a V2";
        }
    }
}
