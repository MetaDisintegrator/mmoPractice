using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public BoxCollider miniMapBoundingBox;

    void Start()
    {
        MiniMapManager.Instance.UpdateMiniMap(miniMapBoundingBox);
    }
}
