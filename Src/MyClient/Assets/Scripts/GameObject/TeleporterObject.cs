using Common.Data;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterObject : MonoBehaviour
{
    public int Id;
    Mesh mesh = null;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireMesh(mesh, transform.position, transform.rotation, transform.localScale);
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0,transform.position,transform.rotation,1f,EventType.Repaint);
    }
#endif

    private void OnTriggerEnter(Collider other)
    {
        PlayerInputController controller = other.GetComponent<PlayerInputController>();
        if (controller != null && controller.isActiveAndEnabled)
        {
            DataManager.Instance.Teleporters.TryGetValue(Id, out var def);
            if(def == null)
            {
                Debug.LogErrorFormat("TeleporterObject: Character: [{0}] enter teleporter: [{1}] but teleporterdefine do not exist",controller.character.Name,Id);
                return;
            }
            Debug.LogFormat("TeleporterObject: Character: [{0}] enter teleporter: [{1}:{2}]", controller.character.Name, Id, def.Name);
            if (def.LinkTo > 0)
            {
                if (DataManager.Instance.Teleporters.ContainsKey(Id))
                    MapService.Instance.SendMapTeleport(Id);
                else
                    Debug.LogErrorFormat("TeleporterObject: Teleporter: [{0}] link to teleporter: [{1}] but teleporterdefine do not exist", Id, def.LinkTo);
            }
        }
    }
}
