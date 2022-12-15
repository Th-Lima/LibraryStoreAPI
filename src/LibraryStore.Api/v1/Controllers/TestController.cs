using LibraryStore.Api.Controllers;
using LibraryStore.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryStore.Api.v1.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/test")]
    public class TestController : MainController
    {
        public TestController(INotifier notifier, IUser appUser) : base(notifier, appUser)
        {
        }

        [HttpGet]
        public string Value()
        {
            return "Sou a V1";
        }
    }
}
