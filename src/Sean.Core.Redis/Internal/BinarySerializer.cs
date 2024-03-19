using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sean.Core.Redis;

/// <summary>
/// 二进制序列化\反序列化
/// </summary>
internal static class BinarySerializer
{
    private static readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();

    public static byte[] Serialize(object item)
    {
        using (var ms = new MemoryStream())
        {
            _binaryFormatter.Serialize(ms, item);
            return ms.ToArray();
        }
    }

    public static object Deserialize(byte[] serializedObject)
    {
        using (var ms = new MemoryStream(serializedObject))
        {
            return _binaryFormatter.Deserialize(ms);
        }
    }

    public static T Deserialize<T>(byte[] serializedObject)
    {
        return (T)Deserialize(serializedObject);
    }
}