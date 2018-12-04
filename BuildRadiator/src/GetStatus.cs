using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace BuildRadiator
{
    public static class GetStatus
    {
        [FunctionName("GetStatus")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "BuildRadiator/GetStatus")] HttpRequest req,
            [Blob("project-dashboard/build-radiator.json", FileAccess.Read)] Stream statusFile,
            ILogger log)
        {
            log.LogInformation("Received - request for radiator status");

            string result = await new StreamReader(statusFile).ReadToEndAsync();

            log.LogInformation("Complete - request for radiator status");

            return (ActionResult)new OkObjectResult(result);
        }
    }
}