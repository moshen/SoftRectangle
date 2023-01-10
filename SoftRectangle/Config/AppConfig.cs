using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace SoftRectangle.Config;

[DataContract]
public class AppConfig
{
    private HashSet<Keys> _disableKeys = new();

    [IgnoreDataMember]
    public HashSet<Keys> DisableKeys { get { return _disableKeys; } }

    [DataMember]
    public List<string> DisableKeysConfig
    {
        get { return _disableKeys.Select(i => i.ToString()).ToList(); }
        set
        {
            _disableKeys = new HashSet<Keys>();
            foreach (var key in value)
            {
                Keys keyRes;
                if (Enum.TryParse<Keys>(key, out keyRes))
                {
                    _disableKeys.Add(keyRes);
                }
            }

            // Always want to add a disable key if none are provided
            // Don't want anyone to have to restart their computer like I did
            if (_disableKeys.Count == 0)
            {
                _disableKeys.Add(Keys.Escape);
            }
        }
    }

    private HashSet<Keys> _passthroughKeys = new();

    [IgnoreDataMember]
    public HashSet<Keys> PassthroughKeys { get { return _passthroughKeys; } }

    [DataMember]
    public List<string> PassthroughKeysConfig
    {
        get { return _passthroughKeys.Select(i => i.ToString()).ToList(); }
        set
        {
            _passthroughKeys = new HashSet<Keys>();
            foreach (var key in value)
            {
                Keys keyRes;
                if (Enum.TryParse<Keys>(key, out keyRes))
                {
                    _passthroughKeys.Add(keyRes);
                }
            }
        }
    }

    public AppConfig()
    {
        DisableKeysConfig = new List<string>();
        PassthroughKeysConfig = new List<string>();
    }

    // Call the empty constructor on deserialization
    [OnDeserializing]
    public void OnDeserializing(StreamingContext context)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        GetType().GetConstructor(Array.Empty<Type>()).Invoke(this, null);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}


