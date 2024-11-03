using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    private int _layerMask = 0;
    private List<Collider> _raycastHitColliders = new List<Collider>();

    private bool _hasHit = false;

    private void Start()
    {
        _layerMask = (1 << LayerMask.NameToLayer(GameLayers.Obstacle.ToString()));
    }

    private void FixedUpdate()
    {
        Vector3 direction = (_playerTransform.position - transform.position).normalized;
        float maxDistance = Vector3.Distance(transform.position, _playerTransform.position);

        List<Collider> currentFrameHitColliders = new List<Collider>();

        foreach (RaycastHit hit in Physics.RaycastAll(transform.position, direction, maxDistance, _layerMask))
        {
            if (hit.collider != null && hit.collider is MeshCollider)
            {
                currentFrameHitColliders.Add(hit.collider);

                if (!_raycastHitColliders.Contains(hit.collider))
                {
                    _raycastHitColliders.Add(hit.collider);
                }
            }
        }

        foreach (Collider collider in _raycastHitColliders.ToList())
        {
            if (!currentFrameHitColliders.Contains(collider))
            {
                ResetTransparency(collider);
                _raycastHitColliders.Remove(collider);
            }
        }
    }

    private void ResetTransparency(Collider collider)
    {
        Material material = collider.GetComponent<Renderer>().material;
        ChangeWallTransparency(material, false);
    }

    private void LateUpdate()
    {
        if (_raycastHitColliders.Count > 0)
        {
            Material[] materials = new Material[_raycastHitColliders.Count];
            for (int i = 0; i < _raycastHitColliders.Count; i++)
            {
                materials[i] = _raycastHitColliders[i].GetComponent<Renderer>().material;
            }
            StartFadeOut(materials);
        }
    }

    public void StartFadeIn(Material[] materials)
    {
        FadeInOut(materials, 0.5f, 1);
    }

    public void StartFadeOut(Material[] materials)
    {
        FadeInOut(materials, 1, 0.5f);
    }

    private void FadeInOut(Material[] materials, float startAlpha, float endAlpha)
    {
        Color[] colors = new Color[materials.Length];

        for (int i = 0; i < colors.Length; i++)
        {
            ChangeWallTransparency(materials[i], true);

            colors[i] = materials[i].color;
            colors[i].a = startAlpha;
            materials[i].color = colors[i];
        }

        for (int i = 0; i < materials.Length; i++)
        {
            colors[i].a = endAlpha;
            materials[i].color = colors[i];
        }

        for (int i = 0; i < materials.Length; i++)
        {
            colors[i].a = endAlpha;
            materials[i].color = colors[i];

            if (endAlpha >= 1)
            {
                ChangeWallTransparency(materials[i], false);
            }
        }
    }

    public void ChangeWallTransparency(Material wallMaterial, bool transparent)
    {
        if (transparent)
        {
            wallMaterial.SetFloat("_Surface", 1);   // Transparent
            wallMaterial.SetFloat("_Blend", 0);  // 알파 블렌드 모드 설정
        }
        else
        {
            wallMaterial.SetFloat("_Surface", 0); // Opaque
        }

        SetupMaterialBlendMode(wallMaterial, transparent);
    }

    void SetupMaterialBlendMode(Material material, bool transparent)
    {
        if (transparent)
        {
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            material.SetShaderPassEnabled("ShadowCaster", false);
        }
        else
        {
            material.SetOverrideTag("RenderType", "");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;
            material.SetShaderPassEnabled("ShadowCaster", true);
        }
    }

}
