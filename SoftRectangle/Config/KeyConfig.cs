using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace SoftRectangle.Config;

[DataContract]
public class KeyConfig : BaseConfig<KeyConfig>
{
    public static KeyConfig GetDefaultMelee()
    {
        var config = new KeyConfig();

        var tempMapping = new Dictionary<Keys, Action>();

        tempMapping.Add(Keys.W, Action.LeftStickUp);
        tempMapping.Add(Keys.A, Action.LeftStickLeft);
        tempMapping.Add(Keys.S, Action.LeftStickDown);
        tempMapping.Add(Keys.D, Action.LeftStickRight);
        tempMapping.Add(Keys.LShiftKey, Action.LeftTriggerHard);

        tempMapping.Add(Keys.Alt, Action.ModX);
        tempMapping.Add(Keys.Right, Action.ModX); // @REMOVEME: This is Colin's mapping
        tempMapping.Add(Keys.Space, Action.ModY);

        tempMapping.Add(Keys.J, Action.X);
        tempMapping.Add(Keys.K, Action.B);
        tempMapping.Add(Keys.L, Action.RightShoulder);
        tempMapping.Add(Keys.OemSemicolon, Action.LeftStickUp);
        tempMapping.Add(Keys.U, Action.RightTriggerHard);
        tempMapping.Add(Keys.I, Action.Y);
        tempMapping.Add(Keys.O, Action.RightTriggerLight);
        tempMapping.Add(Keys.P, Action.RightTriggerMid);

        tempMapping.Add(Keys.H, Action.A);
        tempMapping.Add(Keys.N, Action.RightStickLeft);
        tempMapping.Add(Keys.M, Action.RightStickDown);
        tempMapping.Add(Keys.Oemcomma, Action.RightStickRight);
        tempMapping.Add(Keys.RShiftKey, Action.RightStickUp);

        tempMapping.Add(Keys.D5, Action.Start);

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

    [DataMember]
    public uint Version { get; set; } = 1;

    private Dictionary<Keys, Action> _keyButtonMapping;

    // The parsed configuration
    // @TODO: Make this configuration mapping one-way, like the stick config
    [IgnoreDataMember]
    public Dictionary<Keys, Action> KeyButtonMapping
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
                Action actionRes;
                if (Enum.TryParse<Action>(entry.Key, out actionRes))
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

    private class BitPopComparator : IComparer<UInt32>
    {
        public int Compare(UInt32 a, UInt32 b)
        {
            return BitOperations.PopCount(a).CompareTo(BitOperations.PopCount(b));
        }
    }

    private static void UpdateStickConfig(
        Stick stick,
        List<StickConfig> incoming,
        List<StickConfig> existing,
        List<(UInt32, Vector2)> existingActionValues
    )
    {
        existing.Clear();
        existingActionValues.Clear();
        var newActionValues = new List<(UInt32, Vector2)>();

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
                List<Action> parsedActions = new List<Action>();

                foreach (var action in actions)
                {
                    Action actionRes;
                    if (Enum.TryParse<Action>(action, out actionRes))
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

                newActionValues.Add((bitMask, coords));
            }
        }

        // Sort from the most bits in the mask to the least
        // We are intentionally using a stable sort here so in the case of
        // conflicting binds, we end up using the one that comes first.
        // We might want to revisit this, as I'm not sure if this matches
        // rectangle controller behaviour
        existingActionValues.AddRange(
            newActionValues.OrderByDescending((e) => e.Item1, new BitPopComparator())
        );
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
            UpdateStickConfig(Stick.LeftStick, value, _leftStickConfig, _leftStickActionValues);
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
            UpdateStickConfig(Stick.RightStick, value, _rightStickConfig, _rightStickActionValues);
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
        _keyButtonMapping = new Dictionary<Keys, Action>();
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
}

