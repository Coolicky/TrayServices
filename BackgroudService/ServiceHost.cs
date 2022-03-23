using System.ServiceProcess;
using BackgroundService.Shared;

namespace BackgroundService;

public class ServiceHost : ServiceBase
{
    private static Thread _serviceThread;
    private static bool _stopping;
    private static NamedPipesServer _pipeServer;
    
    public ServiceHost() => ServiceName = "Coolicky Background Service";

    protected override void OnStart(string[] args)
    {
        Run(args);
    }

    protected override void OnStop()
    {
        Abort();
    }

    protected override void OnShutdown()
    {
        Abort();
    }

    public static void Run(string[] args)
    {
        _serviceThread = new Thread(InitializeServiceThread)
        {
            Name = "Coolicky Background Service Thread",
            IsBackground = true
        };
        _serviceThread.Start();
    }

    public static void Abort()
    {
        _stopping = true;
    }

    private static void InitializeServiceThread()
    {
        _pipeServer = new NamedPipesServer();
        _pipeServer.InitializeAsync().GetAwaiter().GetResult();
        while (!_stopping)
        {
            Task.Delay(100).GetAwaiter().GetResult();
        }
    }
}