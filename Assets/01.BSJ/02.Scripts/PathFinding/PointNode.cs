using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class PointNode
{
    public Vector3 Position { get => _position; }
    public PointNode Parent { get => _parent; set => _parent = value; }
    public float GCost { get => _gCost; set => _gCost = value; }
    public float HCost { get => _hCost; set => _hCost = value; }
    public float FCost { get => _fCost; set => _fCost = value; }
    public int Congestion { get => _congestion; set => _congestion = value; }
    public bool IsObstacle
    { 
        get => _isObstacle;
        set => _isObstacle = value;
    }
    public bool IsGround { get => _isGround; }


    [SerializeField] private Vector3 _position;
    private PointNode _parent;
    private float _gCost, _hCost;
    private float _fCost;
    private int _congestion; // ¹ÐÁýµµ
    private bool _isObstacle;
    private bool _isGround;

    public PointNode(Vector3 positions, bool isObstacle, bool isGround)
    {
        _position = positions;
        _isObstacle = isObstacle;
        _isGround = isGround;

        _gCost = 0;
        _hCost = 0;
        _fCost = 0;
    }

    public void Initialize()
    {
        _gCost = 0;
        _hCost = 0;
        _fCost = 0;

        _parent = null;
    }
}