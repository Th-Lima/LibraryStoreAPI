using LibraryStore.Api.Controllers;
using LibraryStore.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryStore.Api.v2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/test")]
    public class TestController : MainController
    {
        private readonly ILogger _logger;

        public TestController(INotifier notifier, IUser appUser, ILogger<TestController> logger) : base(notifier, appUser)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Value()
        {

            throw new Exception("Error");
            //try
            //{
            //    var i = 0;
            //    var result = 42 / i;
            //}
            //catch (DivideByZeroException e)
            //{
            //    e.Ship(HttpContext);
            //}


            _logger.LogTrace("Log de trace");
            _logger.LogDebug("Log de Debug");
            _logger.LogInformation("Log de Informação");
            _logger.LogWarning("Log de Aviso");
            _logger.LogError("Log de Erro");
            _logger.LogCritical("Log de Critico");

            return "Sou a V2";
        }
    }
}
