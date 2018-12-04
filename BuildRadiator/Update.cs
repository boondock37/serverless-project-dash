using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Xml;

namespace BuildRadiator
{
    public static class Update
    {
        [FunctionName("Update")]
        public static void Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "BuildRadiator/Update")] HttpRequest req,
            [Blob("project-dashboard/build-radiator.json", FileAccess.Write)] Stream statusFile,
            ILogger log)
        {
            log.LogInformation("Received - Request to update build status.");
            var json = FormatPostContent(new StreamReader(req.Body).ReadToEnd());

            var result = Encoding.ASCII.GetBytes(json);
            statusFile.Write(result);

            log.LogInformation("Completed - Request to update build status.");
        }

        private static string FormatPostContent(string body)
        {
            body = body.Substring(5);
            body = body
                    .Replace("%3C", "<")
                    .Replace("%3D", "=")
                    .Replace("%3E", ">")
                    .Replace("%3F", "?")
                    .Replace("+", " ")
                    .Replace("%22", "\"")
                    .Replace("%2C", ",")
                    .Replace("%2F", "/")
                    .Replace("#amp;", "&");
            var doc = new XmlDocument();
            doc.LoadXml(body);

            var json = JsonConvert.SerializeXmlNode(doc);
            json = json.Replace("\"@", "\"").Replace("?xml", "xml");

            return json;
        }
    }
}
