using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiniMap : MonoBehaviour
{
    public Text labMapName;
    public Image imgMap;
    public Image imgArrow;

    Transform currentAvatarTrans;
    BoxCollider miniMapBoundingBox;

    private void Awake()
    {
        MiniMapManager.Instance.SetMiniMap(this);
    }

    void Start()
    {
        UpadateMiniMap();
    }

    public void UpadateMiniMap()
    {
        labMapName.text = User.Instance.CurrentMapData.Name;
        imgMap.overrideSprite = MiniMapManager.Instance.LoadCurrentMiniMap();
        imgMap.SetNativeSize();
        imgMap.transform.localPosition = Vector3.zero;

        miniMapBoundingBox = MiniMapManager.Instance.MiniMapBoundingBox;
        currentAvatarTrans = User.Instance.CurrentAvatarObj?.transform;
    }

    void Update()
    {
        if (currentAvatarTrans == null)
                return;
        float realWidth = miniMapBoundingBox.bounds.size.x;
        float realHeight = miniMapBoundingBox.bounds.size.z;
        float relaX = currentAvatarTrans.position.x - miniMapBoundingBox.bounds.min.x;
        float relaY = currentAvatarTrans.position.z - miniMapBoundingBox.bounds.min.z;

        Vector2 relaPos = new Vector2(relaX/realWidth, relaY/realHeight);
        imgMap.rectTransform.pivot = relaPos;
        imgMap.rectTransform.localPosition = Vector2.zero;

        imgArrow.transform.eulerAngles = new Vector3(0, 0, -currentAvatarTrans.eulerAngles.y);
    }
}
