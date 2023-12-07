using System.Collections;
using System.Text.Json;

public class Chain
{

  public Chain(ILogger<Chain> logger, IDateTimeProvider dateTimeProvider)
  {
    _logger = logger;
    _dateTimeProvider = dateTimeProvider;
    AddBlock("Genesis Block");
  }

  private List<Block> _chain = new List<Block>();
  private ILogger<Chain> _logger;
    private IDateTimeProvider _dateTimeProvider;

    public void AddBlock(string data)
  {
    Block block;
    if ((_chain.Any()) && (_chain.Last() != null) && (_chain.Last().BlockHash != null))
    {
      var last = _chain.Last();
      block = new Block(data, last.BlockHash ?? new byte[0], last.Timestamp, _dateTimeProvider.Now );
    }
    else
    {
      block = new Block(data, _dateTimeProvider.Now);
    }
    _chain.Add(block);
    _logger.LogInformation($"Added: {JsonSerializer.Serialize(block)}");
    _logger.LogInformation($"Chain length: {_chain.Count}");
  }

  public Block[] TheChain
  {
    get
    {
      return _chain.ToArray();
    }
  }
}