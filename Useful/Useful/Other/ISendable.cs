namespace Useful.Other
{
/// <summary>
/// Interface used with Net server and client.
/// </summary>
  public interface ISendable
  {
    /// <summary>
    /// Length of object in bytes.
    /// </summary>
    int ByteLength();
    
    /// <summary>
    /// Identifier used when sending and receiving this object.
    /// </summary>
    byte Identifier();
    
    /// <summary>
    /// Returns byte array to reconstruct an object.
    /// </summary>
    byte[] GetBytes();
    
    /// <summary>
    /// Returns object from byte array.
    /// </summary>
    /// <param name="serialization">Byte array to reconstruct form</param>
    ISendable Deserialize(byte[] serialization);
  }
}
