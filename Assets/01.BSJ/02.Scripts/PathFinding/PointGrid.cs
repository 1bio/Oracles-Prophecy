using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointGrid : MonoBehaviour
{
    public float NodeRadius { get => _nodeRadius; }
    public PointNode[,,] Grid
    {
        get => _grid;
        set => _grid = value;
    }


    [SerializeField] private Vector3 _gridWorldSize; // (x, y, z)
    [SerializeField] private float _nodeRadius;
    private PointNode[,,] _grid;

    private float _nodeDiameter;
    private int _gridSizeX, _gridSizeY, _gridSizeZ;

    private void Awake()
    {
        _nodeDiameter = _nodeRadius * 2;
        _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
        _gridSizeY = 1;
        _gridSizeZ = Mathf.RoundToInt(_gridWorldSize.z / _nodeDiameter);

        CreateGrid();
    }

    void CreateGrid()
    {
        _grid = new PointNode[_gridSizeX, _gridSizeY, _gridSizeZ];

        Vector3 worldBottomLeft = this.transform.position - Vector3.right * _gridWorldSize.x / 2
                                                          - Vector3.forward * _gridWorldSize.z / 2;


        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                for (int z = 0; z < _gridSizeZ; z++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius)
                                                    + Vector3.forward * (z * _nodeDiameter + _nodeRadius);

                    bool isObstacle = false;
                    bool isGround = false;

                    int obstacleLayer = LayerMask.NameToLayer(GameLayers.Obstacle.ToString());
                    int groundLayer = LayerMask.NameToLayer(GameLayers.Ground.ToString());


                    int layerMask = (1 << LayerMask.NameToLayer(GameLayers.Obstacle.ToString())) | (1 << LayerMask.NameToLayer(GameLayers.Ground.ToString()));
                    
                    Ray ray = new Ray(worldPoint + Vector3.up * 10, Vector3.down);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                    {
                        if (hit.collider.gameObject.layer == obstacleLayer)
                        {
                            isObstacle = true;
                        }

                        if (hit.collider.gameObject.layer == groundLayer)
                        {
                            isGround = true;
                        }
                    }

                    _grid[x, y, z] = new PointNode(worldPoint, isObstacle, isGround);
                }
            }
        }
    }

    public List<PointNode> GetNeighborNodes(PointNode node)
    {
        if (node == null)
        {
            Debug.LogError($"GetNighborNodes의 매개변수 node : {node} 가 null");
            return null;
        }

        List<PointNode> neighbors = new List<PointNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0)
                    continue;

                int checkX = Mathf.FloorToInt(node.Position.x + x);
                int checkZ = Mathf.FloorToInt(node.Position.z + z);

                if (checkX >= 0 && checkX < _gridSizeX && checkZ >= 0 && checkZ < _gridSizeZ
                    && _grid[checkX, 0, checkZ].IsGround && !_grid[checkX, 0, checkZ].IsObstacle)
                {
                    neighbors.Add(_grid[checkX, 0, checkZ]);
                }
            }
        }

        return neighbors;
    }

    public void InitializeNodeValues()
    {
        foreach (PointNode node in _grid)
        {
            node.Initialize();
        }
    }

    public PointNode GetPointNodeFromGridByPosition(Vector3 position)
    {
        foreach (PointNode node in _grid)
        {
            if (node.Position.x - _nodeRadius <= position.x && node.Position.x + _nodeRadius > position.x
                && node.Position.z - _nodeRadius <= position.z && node.Position.z + _nodeRadius > position.z)
                return node;
        }

        return null;
    }

    // 만든 grid 큐브로 시각화
    /*private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_gridWorldSize.x, _gridWorldSize.y, _gridWorldSize.z));

        if (_grid != null)
        {
            foreach (PointNode node in _grid)
            {
                if (node.IsObstacle || !node.IsGround)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.white;
                }
                Gizmos.DrawCube(node.Position, Vector3.one * (_nodeDiameter - .1f));
            }
        }
    }*/
}
