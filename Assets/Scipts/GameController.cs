using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private int X = 0;
    [SerializeField] private int Y = 0;
    [SerializeField] private CompassDirection direction;

    private JsonParser _jsonParser;
    private TextController _text;
    private bool[,] _paths;
    private int _maxX;
    private int _maxY;

    private void Start()
    {
        _jsonParser = GetComponent<JsonParser>();
        _text = GetComponent<TextController>();
        _maxX = _jsonParser.Paths.Max(x => x.x);
        _maxY = _jsonParser.Paths.Max(x => x.y);
        _paths = new bool[_maxX, _maxY];
        //GetMatrix(_jsonParser.Paths);
        _text.GetText(_jsonParser.Paths.FirstOrDefault(p => p.x == X && p.y == Y));
    }

    private void GetMatrix(List<Path> pathsData)
    {
        foreach (var path in pathsData)
        {
            _paths[path.x - 1, path.y - 1] = true;
        }
    }

    public void Move(int number)
    {
        var path = _jsonParser.Paths.FirstOrDefault(p => p.x == X && p.y == Y);
        if (path is null)
            throw new NullReferenceException();

        var side = path.side.FirstOrDefault(s => s.direction == direction);
        if (side is null)
            throw new NullReferenceException();

        switch (number)
        {
            case 1:
                if (IsFreeWay(side.forward))
                    MoveForward();
                
                break;
            case 2:
                if (IsFreeWay(side.right))
                    MoveRigth();
                break;
            case 3:
                if (IsFreeWay(side.left))
                    MoveLeft();
                break;
        }
        Debug.Log(number);

        _text.GetText(_jsonParser.Paths.FirstOrDefault(p => p.x == X && p.y == Y));
    }

    private void MoveForward()
    {
        _ = direction switch
        {
            CompassDirection.East => Y++,
            CompassDirection.North => X--,
            CompassDirection.South => X++,
            CompassDirection.West => Y--
        };
    }

    private void MoveRigth()
    {
        _ = direction switch
        {
            CompassDirection.East => X++,
            CompassDirection.North => Y++,
            CompassDirection.South => Y--,
            CompassDirection.West => X--
        };

        direction = direction switch
        {
            CompassDirection.East => CompassDirection.South,
            CompassDirection.North => CompassDirection.East,
            CompassDirection.South => CompassDirection.West,
            CompassDirection.West => CompassDirection.North
        };
    }

    private void MoveLeft()
    {
        _ = direction switch
        {
            CompassDirection.East => X--,
            CompassDirection.North => Y--,
            CompassDirection.South => Y++,
            CompassDirection.West => X++
        };

        direction = direction switch
        {
            CompassDirection.East => CompassDirection.North,
            CompassDirection.North => CompassDirection.West,
            CompassDirection.South => CompassDirection.East,
            CompassDirection.West => CompassDirection.South
        };
    }

    string GetWayData(string way) => way.Remove(0, way.IndexOf(' ') + 1);

    bool IsFreeWay(string val) => GetWayData(val) switch
    {
        "есть проход" => true,
        "стена" => false
    };
}