namespace Useful.Other
{
  public interface ISendable
  {
    int ByteLength();

    byte Identifier();

    byte[] GetBytes();

    ISendable Deserialize(byte[] serialization);
  }
}
