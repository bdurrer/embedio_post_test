using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace Server
{
    public class TestController : WebApiController
    {
        [Route(HttpVerbs.Any, "/reading")]
        public async Task<string> SayHello()
        {
            Console.Out.WriteLine("Called /reading");
            // Workaround: Input must be read, or EmbedIO closes the connection before sending data
            await HttpContext.GetRequestBodyAsStringAsync();
            return await Task.FromResult("response1");
        }
        
        [Route(HttpVerbs.Any, "/ignorant")]
        public async Task<string> SayHello2()
        {
            Console.Out.WriteLine("Called /ignorant");
            return await Task.FromResult("response2");
        }
    }
}