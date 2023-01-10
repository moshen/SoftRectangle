﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml;

namespace SoftRectangle.Config;

[DataContract]
public class KeyConfig
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

    public static KeyConfig Deserialize(string configXmlStr)
    {
        var serializer = new DataContractSerializer(typeof(KeyConfig));
        KeyConfig? config = (KeyConfig?)serializer.ReadObject(GenerateStreamFromString(configXmlStr));

        if (config == null)
        {
            // @TODO: Add an actual configuration error and error handling
            throw new Exception("Configuration error");
        }

        return config;
    }

    public static KeyConfig GetDefaultMelee()
    {
        var config = new KeyConfig();

        var tempMapping = new Dictionary<Keys, KeyState.Action>();

        tempMapping.Add(Keys.W, KeyState.Action.LeftStickUp);
        tempMapping.Add(Keys.A, KeyState.Action.LeftStickLeft);
        tempMapping.Add(Keys.S, KeyState.Action.LeftStickDown);
        tempMapping.Add(Keys.D, KeyState.Action.LeftStickRight);
        tempMapping.Add(Keys.LShiftKey, KeyState.Action.LeftTriggerHard);

        tempMapping.Add(Keys.Alt, KeyState.Action.ModX);
        tempMapping.Add(Keys.Right, KeyState.Action.ModX); // @REMOVEME: This is Colin's mapping
        tempMapping.Add(Keys.Space, KeyState.Action.ModY);

        tempMapping.Add(Keys.J, KeyState.Action.X);
        tempMapping.Add(Keys.K, KeyState.Action.B);
        tempMapping.Add(Keys.L, KeyState.Action.RightShoulder);
        tempMapping.Add(Keys.OemSemicolon, KeyState.Action.LeftStickUp);
        tempMapping.Add(Keys.U, KeyState.Action.RightTriggerHard);
        tempMapping.Add(Keys.I, KeyState.Action.Y);
        tempMapping.Add(Keys.O, KeyState.Action.RightTriggerLight);
        tempMapping.Add(Keys.P, KeyState.Action.RightTriggerMid);

        tempMapping.Add(Keys.H, KeyState.Action.A);
        tempMapping.Add(Keys.N, KeyState.Action.RightStickLeft);
        tempMapping.Add(Keys.M, KeyState.Action.RightStickDown);
        tempMapping.Add(Keys.Oemcomma, KeyState.Action.RightStickRight);
        tempMapping.Add(Keys.RShiftKey, KeyState.Action.RightStickUp);

        tempMapping.Add(Keys.D5, KeyState.Action.Start);

        config.KeyButtonMapping = tempMapping;

        var leftStickConfig = new List<StickConfig>();

        // Shield configs
        config.RightTriggerLight = .35f;
        config.RightTriggerMid = .67143f;

        // ModX
        StickConfig modXCardinals = new StickConfig();
        modXCardinals.Actions = new List<string> { "ModX" };
        modXCardinals.Cardinals = new List<string> { "Up", "Down", "Left", "Right" };
        modXCardinals.Coordinates = new Vector2(.6625f, .5375f);
        leftStickConfig.Add(modXCardinals);

        StickConfig modXQuadrants = new StickConfig();
        modXQuadrants.Actions = new List<string> { "ModX" };
        modXQuadrants.Quadrants = new List<string> { "UpLeft", "UpRight", "DownLeft", "DownRight" };
        modXQuadrants.Coordinates = new Vector2(.7375f, .3125f);
        leftStickConfig.Add(modXQuadrants);

        // ModY
        StickConfig modYCardinals = new StickConfig();
        modYCardinals.Actions = new List<string> { "ModY" };
        modYCardinals.Cardinals = new List<string> { "Up", "Down", "Left", "Right" };
        modYCardinals.Coordinates = new Vector2(.3375f, .7375f);
        leftStickConfig.Add(modYCardinals);

        StickConfig modYQuadrants = new StickConfig();
        modYQuadrants.Actions = new List<string> { "ModY" };
        modYQuadrants.Quadrants = new List<string> { "UpLeft", "UpRight", "DownLeft", "DownRight" };
        modYQuadrants.Coordinates = new Vector2(.3125f, .7375f);
        leftStickConfig.Add(modYQuadrants);

        // L/R Angles
        var triggers = new List<string>
        {
            "LeftTriggerHard",
            "RightTriggerHard",
        };
        foreach (var trigger in triggers)
        {
            StickConfig triggerModXQuadrants = new StickConfig();
            triggerModXQuadrants.Actions = new List<string> { trigger, "ModX" };
            triggerModXQuadrants.Quadrants = new List<string> { "UpLeft", "UpRight", "DownLeft", "DownRight" };
            triggerModXQuadrants.Coordinates = new Vector2(.6375f, .3750f);
            leftStickConfig.Add(triggerModXQuadrants);

            StickConfig triggerModYQuadrants1 = new StickConfig();
            triggerModYQuadrants1.Actions = new List<string> { trigger, "ModY" };
            triggerModYQuadrants1.Quadrants = new List<string> { "UpLeft", "UpRight" };
            triggerModYQuadrants1.Coordinates = new Vector2(.4750f, .8750f);
            leftStickConfig.Add(triggerModYQuadrants1);

            StickConfig triggerModYQuadrants2 = new StickConfig();
            triggerModYQuadrants2.Actions = new List<string> { trigger, "ModY" };
            triggerModYQuadrants2.Quadrants = new List<string> { "DownLeft", "DownRight" };
            triggerModYQuadrants2.Coordinates = new Vector2(.5000f, .8500f);
            leftStickConfig.Add(triggerModYQuadrants2);
        }

        var coordsFirefoxModXCDown = new Vector2(0.7f, 0.3625f);    // ~27 deg
        var coordsFirefoxModXCLeft = new Vector2(0.7875f, 0.4875f); // ~32 deg
        var coordsFirefoxModXCUp = new Vector2(0.7f, 0.5125f);      // ~36 deg
        var coordsFirefoxModXCRight = new Vector2(0.6125f, 0.525f); // ~41 deg
        var coordsFirefoxModYCRight = new Vector2(0.6375f, 0.7625f);// ~50 deg
        var coordsFirefoxModYCUp = new Vector2(0.5125f, 0.7f);      // ~54 deg
        var coordsFirefoxModYCLeft = new Vector2(0.4875f, 0.7875f); // ~58 deg
        var coordsFirefoxModYCDown = new Vector2(0.3625f, 0.7f);    // ~63 deg

        var coordsExtendedFirefoxModX = new Vector2(0.9125f, 0.3875f);      // ~23 deg
        var coordsExtendedFirefoxModXCDown = new Vector2(0.875f, 0.45f);    // ~27 deg
        var coordsExtendedFirefoxModXCLeft = new Vector2(0.85f, 0.525f);    // ~32 deg
        var coordsExtendedFirefoxModXCUp = new Vector2(0.7375f, 0.5375f);   // ~36 deg
        var coordsExtendedFirefoxModXCRight = new Vector2(0.6375f, 0.5375f);// ~40 deg
        var coordsExtendedFirefoxModYCRight = new Vector2(0.5875f, 0.7125f);// ~50 deg
        var coordsExtendedFirefoxModYCUp = new Vector2(0.5875f, 0.8f);      // ~54 deg
        var coordsExtendedFirefoxModYCLeft = new Vector2(0.525f, 0.85f);    // ~58 deg
        var coordsExtendedFirefoxModYCDown = new Vector2(0.45f, 0.875f);    // ~63 deg
        var coordsExtendedFirefoxModY = new Vector2(0.3875f, 0.9125f);      // ~67 deg

        var coordsFirefox = new List<(List<string>, Vector2)>
        {
            (new List<string> {"RightStickDown", "ModX"}, coordsFirefoxModXCDown),
            (new List<string> {"RightStickLeft", "ModX"}, coordsFirefoxModXCLeft),
            (new List<string> {"RightStickUp", "ModX"}, coordsFirefoxModXCUp),
            (new List<string> {"RightStickRight", "ModX"}, coordsFirefoxModXCRight),
            (new List<string> {"RightStickRight", "ModY"}, coordsFirefoxModYCRight),
            (new List<string> {"RightStickUp", "ModY"}, coordsFirefoxModYCUp),
            (new List<string> {"RightStickLeft", "ModY"}, coordsFirefoxModYCLeft),
            (new List<string> {"RightStickDown", "ModY"}, coordsFirefoxModYCDown),

            (new List<string> {"X", "ModX"}, coordsExtendedFirefoxModX),
            (new List<string> {"X", "ModX", "RightStickDown"}, coordsExtendedFirefoxModXCDown),
            (new List<string> {"X", "ModX", "RightStickLeft"}, coordsExtendedFirefoxModXCLeft),
            (new List<string> {"X", "ModX", "RightStickUp"}, coordsExtendedFirefoxModXCUp),
            (new List<string> {"X", "ModX", "RightStickRight"}, coordsExtendedFirefoxModXCRight),
            (new List<string> {"X", "ModY", "RightStickRight"}, coordsExtendedFirefoxModYCRight),
            (new List<string> {"X", "ModY", "RightStickUp"}, coordsExtendedFirefoxModYCUp),
            (new List<string> {"X", "ModY", "RightStickLeft"}, coordsExtendedFirefoxModYCLeft),
            (new List<string> {"X", "ModY", "RightStickDown"}, coordsExtendedFirefoxModYCDown),
            (new List<string> {"X", "ModY"}, coordsExtendedFirefoxModY),
        };

        foreach (var coord in coordsFirefox)
        {
            StickConfig quadrants = new StickConfig();
            quadrants.Actions = coord.Item1;
            quadrants.Quadrants = new List<string> { "UpLeft", "UpRight", "DownLeft", "DownRight" };
            quadrants.Coordinates = coord.Item2;
            leftStickConfig.Add(quadrants);
        }

        config.LeftStickConfig = leftStickConfig;

        return config;
    }

    private Dictionary<Keys, KeyState.Action> _keyButtonMapping;

    // The parsed configuration
    [IgnoreDataMember]
    public Dictionary<Keys, KeyState.Action> KeyButtonMapping
    {
        get { return _keyButtonMapping; }
        set
        {
            _configMapping.Clear();
            _keyButtonMapping.Clear();

            foreach (var entry in value)
            {
                _keyButtonMapping[entry.Key] = entry.Value;

                if (_configMapping.ContainsKey(entry.Value.ToString()))
                {
                    _configMapping[entry.Value.ToString()].Add(entry.Key.ToString());
                } else
                {
                    var newVal = new List<string>();
                    newVal.Add(entry.Key.ToString());
                    _configMapping.Add(entry.Value.ToString(), newVal);
                }
            }
        }
    }

    private Dictionary<string, List<string>> _configMapping;

    // Textual mapping from Action to Key(s)
    [DataMember(Name = "ActionMapping")]
    public Dictionary<string, List<string>> ConfigMapping
    {
        get { return _configMapping; }
        set
        {
            _configMapping.Clear();
            _keyButtonMapping.Clear();

            foreach (var entry in value)
            {
                KeyState.Action actionRes;
                if (Enum.TryParse<KeyState.Action>(entry.Key, out actionRes))
                {
                    var newVal = new List<string>();

                    foreach (string listEntry in entry.Value)
                    {
                        Keys keyRes;
                        if (Enum.TryParse<Keys>(listEntry, out keyRes))
                        {
                            newVal.Add(listEntry);
                            _keyButtonMapping[keyRes] = actionRes;
                        }
                    }

                    _configMapping[entry.Key] = newVal;
                }
            }
        }
    }

    private struct Stick
    {
        public string Name;
        public UInt32 Up;
        public UInt32 Down;
        public UInt32 Left;
        public UInt32 Right;
        public UInt32 LeftOrRight;
        public UInt32 UpOrDown;
    }

    private static Stick LeftStick = new Stick
    {
        Name = "LeftStick",
        Up = (UInt32)KeyState.Action.LeftStickUp,
        Down = (UInt32)KeyState.Action.LeftStickDown,
        Left = (UInt32)KeyState.Action.LeftStickLeft,
        Right = (UInt32)KeyState.Action.LeftStickRight,
        LeftOrRight = (UInt32)KeyState.Action.LeftStickLeft | (UInt32)KeyState.Action.LeftStickRight,
        UpOrDown = (UInt32)KeyState.Action.LeftStickUp | (UInt32)KeyState.Action.LeftStickDown,
    };
    private static Stick RightStick = new Stick
    {
        Name = "RightStick",
        Up = (UInt32)KeyState.Action.RightStickUp,
        Down = (UInt32)KeyState.Action.RightStickDown,
        Left = (UInt32)KeyState.Action.RightStickLeft,
        Right = (UInt32)KeyState.Action.RightStickRight,
        LeftOrRight = (UInt32)KeyState.Action.RightStickLeft | (UInt32)KeyState.Action.RightStickRight,
        UpOrDown = (UInt32)KeyState.Action.RightStickUp | (UInt32)KeyState.Action.RightStickDown,
    };

    private static void UpdateStickConfig(
        Stick stick,
        List<StickConfig> incoming,
        List<StickConfig> existing,
        List<(UInt32, Vector2)> existingActionValues
    )
    {
        existing.Clear();
        existingActionValues.Clear();

        foreach (var cfg in incoming)
        {
            existing.Add(cfg);

            UInt32 partialBitMask = 0;

            foreach (var action in cfg.ParsedActions)
            {
                partialBitMask |= (UInt32)action;
            }

            List<List<string>> actionNames = new List<List<string>>();

            if (cfg.Cardinals.Count > 0)
            {
                foreach (var cardinal in cfg.Cardinals)
                {
                    actionNames.Add(new List<string> { stick.Name + cardinal });
                }
            }
            else if (cfg.Quadrants.Count > 0)
            {
                foreach (var quadrant in cfg.Quadrants)
                {
                    switch (quadrant)
                    {
                        case "UpLeft":
                            actionNames.Add(new List<string> { stick.Name + "Up", stick.Name + "Left" });
                            break;
                        case "UpRight":
                            actionNames.Add(new List<string> { stick.Name + "Up", stick.Name + "Right" });
                            break;
                        case "DownLeft":
                            actionNames.Add(new List<string> { stick.Name + "Down", stick.Name + "Left" });
                            break;
                        case "DownRight":
                            actionNames.Add(new List<string> { stick.Name + "Down", stick.Name + "Right" });
                            break;
                    }
                }
            }

            foreach (var actions in actionNames)
            {
                List<KeyState.Action> parsedActions = new List<KeyState.Action>();

                foreach (var action in actions)
                {
                    KeyState.Action actionRes;
                    if (Enum.TryParse<KeyState.Action>(action, out actionRes))
                    {
                        parsedActions.Add(actionRes);
                    }
                }

                UInt32 bitMask = partialBitMask;
                foreach (var action in parsedActions)
                {
                    bitMask |= (UInt32)action;
                }

                Vector2 coords = cfg.Coordinates;

                if (cfg.Cardinals.Count > 0)
                {
                    if ((bitMask & stick.LeftOrRight) != 0)
                    {
                        coords.Y = 0f;
                    }
                    if ((bitMask & stick.UpOrDown) != 0)
                    {
                        coords.X = 0f;
                    }
                }

                if ((bitMask & stick.Left) != 0)
                {
                    coords = Vector2.Multiply(coords, new Vector2(-1f, 1f));
                }
                if ((bitMask & stick.Down) != 0)
                {
                    coords = Vector2.Multiply(coords, new Vector2(1f, -1f));
                }

                existingActionValues.Add((bitMask, coords));
            }
        }

        // Sort from the most bits in the mask to the least
        existingActionValues.Sort((a, b) => BitOperations.PopCount(b.Item1).CompareTo(BitOperations.PopCount(a.Item1)));
    }

    private List<(UInt32, Vector2)> _leftStickActionValues;
    [IgnoreDataMember]
    public List<(UInt32, Vector2)> LeftStickActionValues
    {
        get { return _leftStickActionValues; }
    }

    private List<StickConfig> _leftStickConfig;
    [DataMember]
    public List<StickConfig> LeftStickConfig
    {
        get { return _leftStickConfig; }
        set {
            UpdateStickConfig(LeftStick, value, _leftStickConfig, _leftStickActionValues);
        }
    }

    private List<(UInt32, Vector2)> _rightStickActionValues;
    [IgnoreDataMember]
    public List<(UInt32, Vector2)> RightStickActionValues
    {
        get { return _rightStickActionValues; }
    }

    private List<StickConfig> _rightStickConfig;
    [DataMember]
    public List<StickConfig> RightStickConfig
    {
        get { return _rightStickConfig; }
        set {
            UpdateStickConfig(RightStick, value, _rightStickConfig, _rightStickActionValues);
        }
    }

    // Map trigger values, 0 - 1
    private float _leftTriggerLight = .1f;
    [DataMember]
    public float LeftTriggerLight
    {
        get { return _leftTriggerLight; }
        set
        {
            _leftTriggerLight = Math.Clamp(value, 0.0f, 1.0f);
        }
    }
    private float _leftTriggerMid = .5f;
    [DataMember]
    public float LeftTriggerMid
    {
        get { return _leftTriggerMid; }
        set
        {
            _leftTriggerMid = Math.Clamp(value, 0.0f, 1.0f);
        }
    }
    private float _leftTriggerHard = 1f;
    [DataMember]
    public float LeftTriggerHard
    {
        get { return _leftTriggerHard; }
        set
        {
            _leftTriggerHard = Math.Clamp(value, 0.0f, 1.0f);
        }
    }
    private float _rightTriggerLight = .1f;
    [DataMember]
    public float RightTriggerLight
    {
        get { return _rightTriggerLight; }
        set
        {
            _rightTriggerLight = Math.Clamp(value, 0.0f, 1.0f);
        }
    }
    private float _rightTriggerMid = .5f;
    [DataMember]
    public float RightTriggerMid
    {
        get { return _rightTriggerMid; }
        set
        {
            _rightTriggerMid = Math.Clamp(value, 0.0f, 1.0f);
        }
    }
    private float _rightTriggerHard = 1f;
    [DataMember]
    public float RightTriggerHard
    {
        get { return _rightTriggerHard; }
        set
        {
            _rightTriggerHard = Math.Clamp(value, 0.0f, 1.0f);
        }
    }

    public KeyConfig()
    {
        _keyButtonMapping = new Dictionary<Keys, KeyState.Action>();
        _configMapping = new Dictionary<string, List<string>>();
        _leftStickConfig = new List<StickConfig>();
        _leftStickActionValues = new List<(UInt32, Vector2)>();
        _rightStickConfig = new List<StickConfig>();
        _rightStickActionValues = new List<(UInt32, Vector2)>();
    }

    // Call the empty constructor on deserialization
    [OnDeserializing]
    public void OnDeserializing(StreamingContext context)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        GetType().GetConstructor(Array.Empty<Type>()).Invoke(this, null);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    public string Serialize()
    {
        var serializer = new DataContractSerializer(this.GetType());
        string xmlString;
        using (var sw = new StringWriter())
        {
            using (var writer = new XmlTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented; // indent the Xml so it's human readable
                serializer.WriteObject(writer, this);
                writer.Flush();
                xmlString = sw.ToString();
            }
        }
        return xmlString;
    }
}

