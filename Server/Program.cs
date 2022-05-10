using System;
using EmbedIO;
using EmbedIO.WebApi;

namespace Server
{
    internal class Program
    {
        public static void Main(string[] args)
        {       
            using (var server = CreateWebServer())
            {
                // Once we've registered our modules and configured them, we call the RunAsync() method.
                server.RunAsync();

                // Wait for any key to be pressed before disposing of our web server.
                // In a service, we'd manage the lifecycle of our web server using
                // something like a BackgroundWorker or a ManualResetEvent.
                Console.ReadKey(true);
            }
        }

        private static WebServer CreateWebServer()
        {
            Console.Out.WriteLine("Starting server on 8888");
            WebServer AppSyncServer = new WebServer(o => o
                        .WithUrlPrefix("http://*:8888")
                        .WithMode(HttpListenerMode.EmbedIO))
                    .WithLocalSessionManager()
                    .WithWebApi("/api", m => m
                        .WithController<TestController>())
                ;

            AppSyncServer.StateChanged += SyncServerStateChangedEventHandler;
            return AppSyncServer;
        }

        private static void SyncServerStateChangedEventHandler(object sender, WebServerStateChangedEventArgs e)
        {
            Console.Out.WriteLine($"App sync web server New State - {e.NewState}");
        }
    }
}