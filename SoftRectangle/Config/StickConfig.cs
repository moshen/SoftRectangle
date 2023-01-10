using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Serialization;

namespace SoftRectangle.Config;

[DataContract]
public class StickConfig
{
    private List<KeyState.Action> _parsedActions = new();
    [IgnoreDataMember]
    public List<KeyState.Action> ParsedActions
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
            _parsedActions = new List<KeyState.Action>();
            _actions = new List<string>();

            foreach (var entry in value)
            {
                KeyState.Action actionRes;
                if (Enum.TryParse<KeyState.Action>(entry, out actionRes))
                {
                    _actions.Add(entry);
                    _parsedActions.Add(actionRes);
                }
            }
        }
    }

    private List<string> _cardinals = new();
    // Use x,0 for Left/Right and 0,y for Up/Down
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


