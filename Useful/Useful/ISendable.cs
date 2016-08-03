namespace Useful
{
  public interface ISendable
  {
    int ByteLength();

    byte Identifier();

    byte[] GetBytes();

    ISendable Deserialize(byte[] serialization);
  }
}
