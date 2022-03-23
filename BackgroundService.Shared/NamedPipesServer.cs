using System;
using System.Threading.Tasks;
using H.Pipes;
using H.Pipes.Args;

namespace BackgroundService.Shared
{
    public class NamedPipesServer : IDisposable
    {
        private const string PIPE_NAME = "coolickypipe";
        private PipeServer<PipeMessage>? _server;

        public async Task InitializeAsync()
        {
            _server = new PipeServer<PipeMessage>(PIPE_NAME);
            _server.ClientConnected += async (o, args) => await OnClientConnectedAsync(args);
            _server.ClientDisconnected += async (o, args) => OnClientDisconnected(args);
            _server.MessageReceived += async (sender, args) => OnMessageReceived(args.Message);
            _server.ExceptionOccurred += async (o, args) => OnExceptionOccurred(args.Exception);

            await _server.StartAsync();
        }

        private async Task OnClientConnectedAsync(ConnectionEventArgs<PipeMessage> args)
        {
            Console.WriteLine($"Client is now connected!");

            await args.Connection.WriteAsync(new PipeMessage
            {
                Action = ActionType.SendText,
                Text = "Hi from server"
            });
        }

        private void OnClientDisconnected(ConnectionEventArgs<PipeMessage> args)
        {
            Console.WriteLine($"Client disconnected");
        }
        
        private void OnMessageReceived(PipeMessage? message)
        {
            switch(message.Action)
            {
                case ActionType.SendText:
                    Console.WriteLine($"Text from client: {message.Text}");
                    break;

                case ActionType.ShowTrayIcon:
                    throw new NotImplementedException();

                case ActionType.HideTrayIcon:
                    throw new NotImplementedException();

                default:
                    Console.WriteLine($"Unknown Action Type: {message.Action}");
                    break;
            }
        }
        
        private void OnExceptionOccurred(Exception ex)
        {
            Console.WriteLine($"Exception occured in pipe: {ex}");
        }

        public void Dispose()
        {
            DisposeAsync().GetAwaiter().GetResult();
        }

        public async Task DisposeAsync()
        {
            if (_server != null)
                await _server.DisposeAsync();
        }
    }
}