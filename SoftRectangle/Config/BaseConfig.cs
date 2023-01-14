using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SoftRectangle.Config;

[DataContract]
public abstract class BaseConfig<T>
{
    private static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    public static T Deserialize(string configJsonStr)
    {
        return Deserialize(GenerateStreamFromString(configJsonStr));
    }

    public static T Deserialize(Stream jsonConfigStream)
    {
        var serializer = new DataContractJsonSerializer(typeof(T));
        T? config = (T?)serializer.ReadObject(jsonConfigStream);

        if (config == null)
        {
            // @TODO: Add an actual configuration error and error handling
            throw new Exception("Configuration error");
        }

        return config;
    }

    public string Serialize()
    {
        var serializer = new DataContractJsonSerializer(typeof(T));
        string jsonString;

        var ms = new MemoryStream();
        using (var writer = JsonReaderWriterFactory.CreateJsonWriter(ms, Encoding.UTF8, true, true, "  "))
        {
            serializer.WriteObject(writer, this);
            writer.Flush();
            jsonString = Encoding.UTF8.GetString(ms.ToArray());
        }

        return jsonString;
    }
}

