using SoftRectangle.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SoftRectangle;

public class StickActions
{
    private class BitPopComparator : IComparer<UInt32>
    {
        public int Compare(UInt32 a, UInt32 b)
        {
            return BitOperations.PopCount(a).CompareTo(BitOperations.PopCount(b));
        }
    }

    private readonly UInt32[] bitMasks;
    private readonly Vector<UInt32>[] bitMaskVectors;
    private readonly uint remaining;
    private readonly Vector2[] coordinates;
    private readonly uint count;

    public StickActions()
    {
        count = 0;
        bitMasks = Array.Empty<UInt32>();
        bitMaskVectors = Array.Empty<Vector<UInt32>>();
        remaining = 0;
        coordinates= Array.Empty<Vector2>();
    }

    public StickActions(Stick stick, List<StickConfig> stickConfigs)
    { 
        var newActionValues = new List<(UInt32, Vector2)>();

        foreach (var cfg in stickConfigs)
        {
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
                    if (Enum.TryParse<Action>(action, out Action actionRes))
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
        newActionValues = newActionValues.OrderByDescending(
            (e) => e.Item1,
            new BitPopComparator()
        ).ToList();

        count = (uint)newActionValues.Count;
        bitMasks = new UInt32[count];
        coordinates = new Vector2[count];

        for (int i = 0; i < count; i++)
        {
            var v = newActionValues[i];
            bitMasks[i] = v.Item1;
            coordinates[i] = v.Item2;
        }

        if (Vector.IsHardwareAccelerated)
        {
            remaining = (uint)(count % Vector<UInt32>.Count);
            int vectorCount = (int)(count / Vector<UInt32>.Count);
            bitMaskVectors = new Vector<UInt32>[vectorCount];

            int j = 0;
            for (int i = 0; i < count - remaining; i += (int)Vector<UInt32>.Count)
            {
                bitMaskVectors[j++] = new Vector<UInt32>(bitMasks, i);
            }
        }
        else
        {
            bitMaskVectors = Array.Empty<Vector<UInt32>>();
        }
    }

    public (UInt32, Vector2) GetStickAction(UInt32 actionState)
    {
        uint i = 0;

        if (Vector.IsHardwareAccelerated)
        {
            i = count - remaining;
            Vector<UInt32> actionStateVector = new Vector<UInt32>(actionState);
            for (int j = 0; j < bitMaskVectors.Length; j++)
            {
                // Vector contains a matching bitmask
                if (Vector.EqualsAny<UInt32>((bitMaskVectors[j] & actionStateVector), bitMaskVectors[j]))
                {
                    // Finding the exact bitmask
                    var vec = bitMaskVectors[j];
                    for (int k = 0; k<Vector<UInt32>.Count; k++)
                    {
                        if ((vec[k] & actionState) == vec[k])
                        {
                            // Return bitmask and matching coordinates
                            return (vec[k], coordinates[(j * Vector<UInt32>.Count) + k]);
                        }
                    }
                }
            }

            if (remaining == 0)
            {
                return (0, Vector2.Zero);
            }
        }

        for (; i<count; i++)
        {
            if ((bitMasks[i] & actionState) == bitMasks[i])
            {
                return (bitMasks[i], coordinates[i]);
            }
        }

        return (0, Vector2.Zero);
    }
}

