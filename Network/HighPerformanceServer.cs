using System.Net;
using System.Net.Sockets;
using System.Text;

public class HighPerformanceServer : BackgroundService 
{
    private const int PacketSize = 256; 
    private static readonly IPEndPoint _blankEndpoint = new IPEndPoint(IPAddress.Any, 0);
    private readonly ILogger<HighPerformanceServer> _logger;
    private readonly IServiceProvider _service;
    private readonly Chain _chain;
    private readonly TcpListener _listener;

    public HighPerformanceServer(ILogger<HighPerformanceServer> logger, IServiceProvider service, Chain chain)
    {
        _logger = logger;
        _service = service;
        _chain = chain;
        _listener = new TcpListener(new IPEndPoint(IPAddress.Any,5415 ));
    }    


    private async Task DoReceiveAsync(Socket udpSocket, CancellationToken cancelToken)
    {
        // Taking advantage of pre-pinned memory here using the .NET5 POH (pinned object heap).
        byte[] buffer = GC.AllocateArray<byte>(length: 512, pinned: true);
        Memory<byte> bufferMem = buffer.AsMemory();
        using var scope = _service.CreateScope();
        var handleBlock = scope.ServiceProvider.GetService<HandleBlockService>();

        while (!cancelToken.IsCancellationRequested)
        {
            try
            {

                var result = await udpSocket.ReceiveFromAsync(bufferMem, SocketFlags.None, _blankEndpoint);
                var tmpMsg = Encoding.UTF8.GetString(bufferMem.Slice(0, result.ReceivedBytes).ToArray());
                ArgumentNullException.ThrowIfNull(handleBlock);
                await handleBlock.Do(tmpMsg);
                
                
            }
            catch (SocketException exception)
            {
                _logger.LogError(14214, exception, "Do Receive Socket Exception");
                // Socket exception means we are finished.
            }
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _listener.Start();
        var handler = await _listener.AcceptTcpClientAsync();

        while (!stoppingToken.IsCancellationRequested)
        {
            var stream = handler.GetStream();
            var buffer = new byte[PacketSize];
            var read = await stream.ReadAsync(buffer, 0, PacketSize, stoppingToken);
            var message = Encoding.UTF8.GetString(buffer, 0, read);
            if (!String.IsNullOrWhiteSpace(message)) {
                _logger.LogInformation($"Message received: \"{message}\"");
                _chain.AddBlock(message);
            }
        }
    }


}