// See https://aka.ms/new-console-template for more information

using System.ServiceProcess;
using BackgroundService;

Console.WriteLine("Starting Service...");

if (!Environment.UserInteractive)
{
    using var serviceHost = new ServiceHost();
        ServiceBase.Run(serviceHost);
}
else
{
    Console.WriteLine("User interactive mode");
    ServiceHost.Run(args);
    Console.WriteLine("Press ESC to stop...");

    while (Console.ReadKey(true).Key != ConsoleKey.Escape)
    {
    }
    ServiceHost.Abort();
}