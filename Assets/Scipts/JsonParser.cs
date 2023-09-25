using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonParser : MonoBehaviour
{
    [SerializeField] private TextAsset jsonFile;

    public List<Path> Paths { get; private set; }

    void Start()
    {
        Paths = GetPaths(jsonFile.ToString());
    }

    private List<Path> GetPaths(string json)
    {
        var jArray = JArray.Parse(json);

        var list = new List<Path>();
        foreach (var item in jArray)
        {
            var path = new Path()
            {
                side = GetSide((JObject)item["sides"]),
                x = (int)item["x"],
                y = (int)item["y"]
            };

            list.Add(path);
        }

        return list;
    }

    private List<Side> GetSide(JObject json)
    {
        var sides = new List<Side>();

        foreach (var element in json)
        {
            var side = new Side();

            side.direction = GetDirection(element.Key);

            var values = (JObject)element.Value;

            foreach (var item in values)
            {
                switch (item.Key)
                {
                    case "forward":
                        side.forward = (string)item.Value;
                        break;
                    case "left":
                        side.left = (string)item.Value;
                        break;
                    case "right":
                        side.right = (string)item.Value;
                        break;
                }
            }

            sides.Add(side);
        }

        return sides;
    }

    CompassDirection GetDirection(object val) => val switch
    {
        "e" => CompassDirection.East,
        "n" => CompassDirection.North,
        "s" => CompassDirection.South,
        "w" => CompassDirection.West,
        _ => throw new ArgumentOutOfRangeException(nameof(val), val, null)
    };
}