using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAttackIndicator : Indicator
{
    private LineRenderer _lineRenderer;
    private float _maxDistance = 50f;
    private float _offset = 0.5f;

    private void OnEnable()
    {
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _lineRenderer.positionCount = 2;
    }

    void FixedUpdate()
    {
        Vector3 startPos = transform.position;
        Vector3 direction = transform.forward;

        int layerMask = ~(1 << LayerMask.NameToLayer(GameLayers.Player.ToString())) & (1 << LayerMask.NameToLayer(GameLayers.Obstacle.ToString()));
        if (Physics.Raycast(startPos, direction, out RaycastHit hit, _maxDistance, layerMask))
        {
            Debug.Log($"Raycast hit: {hit.collider.name}");
            _lineRenderer.SetPosition(0, startPos);
            _lineRenderer.SetPosition(1, hit.point + direction * _offset);
        }
        else
        {
            _lineRenderer.SetPosition(0, startPos);
            _lineRenderer.SetPosition(1, startPos + direction * _maxDistance);
        }
    }
}
