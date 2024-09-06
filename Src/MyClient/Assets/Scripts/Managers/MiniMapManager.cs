using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers 
{
    public class MiniMapManager : Singleton<MiniMapManager>
    {
        private UIMiniMap miniMap = null;
        private BoxCollider _miniMapBoundingBox;
        public BoxCollider MiniMapBoundingBox => _miniMapBoundingBox;

        public void SetMiniMap(UIMiniMap miniMap) 
        {
            if(this.miniMap == null)
                this.miniMap = miniMap; 
        }

        public void UpdateMiniMap(BoxCollider miniMapBoundingBox)
        { 
            _miniMapBoundingBox = miniMapBoundingBox;
            miniMap.UpadateMiniMap();
        }
        public Sprite LoadCurrentMiniMap()
        {
            return Resloader.Load<Sprite>("UI/MiniMap/" + User.Instance.CurrentMapData.MiniMap);
        }
    }
}

