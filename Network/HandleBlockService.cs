
public class HandleBlockService
{
    private ILogger<HandleBlockService> _logger;
    private Chain _chain;

    public HandleBlockService(ILogger<HandleBlockService> logger, Chain chain)
    {
        _logger = logger;
        _chain = chain;
    }

    public async Task Do(string tmpMsg)
    {
      _logger.LogInformation($"Message received: \"{tmpMsg}\"");
      await Task.Delay(1);
      _chain.AddBlock(tmpMsg);
    }
}