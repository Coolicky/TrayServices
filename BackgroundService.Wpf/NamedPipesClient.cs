using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using BackgroundService.Shared;
using H.Pipes;

namespace BackgroundService.Wpf;

public class NamedPipesClient : IDisposable
{
    private const string PIPE_NAME = "coolickypipe";
    private static NamedPipesClient? _instance;
    private PipeClient<PipeMessage>? _client;

    public static NamedPipesClient Instance => _instance ?? new NamedPipesClient();

    public NamedPipesClient()
    {
        _instance = this;
    }

    public async Task InitializeAsync()
    {
        if(_client != null && _client.IsConnected) return;

        _client = new PipeClient<PipeMessage>(PIPE_NAME);
        _client.MessageReceived += (sender, args) => OnMessageReceived(args.Message);
        _client.Disconnected += (o, args) => MessageBox.Show("Disconnected from server");
        _client.Connected += (o, args) => MessageBox.Show("Connected to server");
        _client.ExceptionOccurred += (o, args) => OnExceptionOccurred(args.Exception);

        await _client.ConnectAsync();
        await _client.WriteAsync(new PipeMessage
        {
            Action = ActionType.SendText,
            Text = "Hello World"
        });
    }
    
    private void OnMessageReceived(PipeMessage message)
    {
        switch (message.Action)
        {
            case ActionType.SendText:
                MessageBox.Show(message.Text);
                break;
            default:
                MessageBox.Show($"Method {message.Action} not implemented");
                break;
        }
    }

    private void OnExceptionOccurred(Exception exception)
    {
        MessageBox.Show($"An exception occured: {exception}");
    }

    public void Dispose()
    {
        _client?.DisposeAsync().GetAwaiter().GetResult();
    }
}