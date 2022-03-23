namespace BackgroudWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly JokeService _jokeService;

    public Worker(ILogger<Worker> logger, JokeService jokeService)
    {
        _logger = logger;
        _jokeService = jokeService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var joke = _jokeService.GetJoke();
            _logger.LogWarning(joke);
            await Task.Delay(1000, stoppingToken);
        }
    }
}