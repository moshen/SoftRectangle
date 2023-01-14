using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Serialization;

namespace SoftRectangle.Config;

/// <summary>
/// This represents a single stick configuration mapping <see
/// cref="KeyConfig.UpdateStickConfig(SoftRectangle.Stick,
/// List{SoftRectangle.Config.StickConfig},
/// List{SoftRectangle.Config.StickConfig}, List{(uint, Vector2)})"/>
/// </summary>
[DataContract]
public class StickConfig
{
    private List<Action> _parsedActions = new();
    [IgnoreDataMember]
    public List<Action> ParsedActions
    {
        get { return _parsedActions; }
    }

    private List<string> _actions = new();
    [DataMember]
    public List<string> Actions
    {
        get { return _actions; }
        set
        {
            _parsedActions = new List<Action>();
            _actions = new List<string>();

            foreach (var entry in value)
            {
                if (Enum.TryParse<Action>(entry, out Action actionRes))
                {
                    _actions.Add(entry);
                    _parsedActions.Add(actionRes);
                }
            }
        }
    }

    private List<string> _cardinals = new();
    [DataMember]
    public List<string> Cardinals
    {
        get { return _cardinals; }
        set
        {
            _cardinals = new List<string>();

            foreach (var cardinal in value)
            {
                switch (cardinal)
                {
                    case "Up":
                    case "Down":
                    case "Left":
                    case "Right":
                        _cardinals.Add(cardinal);
                        break;
                }
            }
        }
    }

    private List<string> _quadrants = new();
    [DataMember]
    public List<string> Quadrants
    {
        get { return _quadrants; }
        set
        {
            _quadrants = new List<string>();

            foreach (var quadrant in value)
            {
                switch (quadrant)
                {
                    case "UpLeft":
                    case "UpRight":
                    case "DownLeft":
                    case "DownRight":
                        _quadrants.Add(quadrant);
                        break;
                }
            }
        }
    }

    private Vector2 _coordinates = new();
    [DataMember]
    public Vector2 Coordinates
    {
        get { return _coordinates; }
        set
        {
            _coordinates = Vector2.Clamp(value, Vector2.Zero, Vector2.One);
        } 
    }
}

