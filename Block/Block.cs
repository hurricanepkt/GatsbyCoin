using System.IO.Hashing;
using System.Text;
using System.Text.Json.Serialization;

public class Block {
  [JsonInclude]
  public  long 	    Timestamp; //         int64  // the time when the block was created
  [JsonInclude]
  [JsonConverter(typeof(ConvertToHexStringJsonConverter))]
	public  byte[]?   PreviousBlockHash; // []byte // the hash of the previous block
  [JsonInclude]
  public long PreviousTimestamp;
  [JsonInclude]
  [JsonConverter(typeof(ConvertToHexStringJsonConverter))]
  public  byte[]?   BlockHash;//       []byte // the hash of the current block
  [JsonInclude]
  [JsonConverter(typeof(ConvertToHexStringJsonConverter))]
	public  byte[]?   MessageBytes; //           []byte // the data or transactions (body info)
  public  string    Message =>  Encoding.UTF8.GetString(MessageBytes ?? new byte[0]);



  public Block(DateTime dateTime) {
    BlockHash = new byte[0];
    BlockInternal(new byte[0], new byte[0], 0, dateTime);
  }

  public Block(byte[] messageBytes, byte[] previousBlockHash, long previousTimestamp, DateTime dateTime) {
    BlockHash = new byte[0];
    BlockInternal(messageBytes, previousBlockHash, previousTimestamp, dateTime);
  }

  public Block(string message, byte[] previousBlockHash, long previousTimestamp, DateTime dateTime) {
    BlockHash = new byte[0];
    BlockInternal(Encoding.UTF8.GetBytes(message), previousBlockHash, previousTimestamp, dateTime);
  }

  public Block(byte[] messageBytes, DateTime dateTime) {
    BlockHash = new byte[0];
    BlockInternal(messageBytes, new byte[0], 0, dateTime);
  }

  public Block(string message, DateTime dateTime) {
    BlockHash = new byte[0];
    BlockInternal(Encoding.UTF8.GetBytes(message), new byte[0], 0, dateTime);
  }

  public void BlockInternal(byte[] messageBytes, byte[] previousBlockHash, long previousTimestamp, DateTime dateTime) {
    Timestamp = new DateTimeOffset(dateTime).ToUniversalTime().ToUnixTimeMilliseconds();
    PreviousBlockHash = previousBlockHash;
    PreviousTimestamp = previousTimestamp;
    MessageBytes = messageBytes;
    BlockHash = new byte[0];
    SetHash();
  }

  public void SetHash() {
    var hashAlgorithm = new XxHash3();
    hashAlgorithm.Append(BitConverter.GetBytes(Timestamp));
    hashAlgorithm.Append(PreviousBlockHash ?? new byte[0]);
    hashAlgorithm.Append(MessageBytes ?? new byte[0]);
    BlockHash = hashAlgorithm.GetCurrentHash();
  }

  // public string Print() {
  //   return $"\n{new String('*',80)}\nTimestamp:\t{Timestamp}\nPreviousBlockHash:\t{Convert.ToHexString(PreviousBlockHash)}\nMyBlockHash:\t{Convert.ToHexString(BlockHash)}\nMessage:\t{Message}\n{new String('*',80)}";
  // }
}

