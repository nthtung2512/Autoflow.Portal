using Autoflow.Portal.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Autoflow.Portal.HttpApi.Controllers
{
    [Route("api/runner-bot")]
    [ApiController]
    public class RunnerBotController(IRunnerBotRepository runnerBotRepository)
    {
        private readonly IRunnerBotRepository _runnerBotRepository = runnerBotRepository;

        //[HttpGet("Sample")]
        //public async Task<string> GetSample()
        //{
        //    return await _runnerBotRepository.GetHelloWorld();
        //}
    }
}
