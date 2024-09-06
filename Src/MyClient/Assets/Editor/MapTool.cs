using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTool
{
    [MenuItem("MapTool/Export Teleporters")]
    public static void ExportTeleporters()
    {
        DataManager.Instance.Load();

        Scene currentScene =  EditorSceneManager.GetActiveScene();
        string currentName = currentScene.name;
        if (currentScene.isDirty)
        {
            EditorUtility.DisplayDialog("����", "���ȱ��浱ǰ����", "ȷ��");
            return;
        }

        foreach (var mapDef in DataManager.Instance.Maps.Values)
        {
            string mapRes = "Assets/Levels/" + mapDef.Resource + ".unity";
            if (!System.IO.File.Exists(mapRes))
            {
                Debug.LogErrorFormat("Scene [{0}] do not exist", mapDef.Resource);
                continue;
            }
            EditorSceneManager.OpenScene(mapRes);
            TeleporterObject[] teleporters = GameObject.FindObjectsOfType<TeleporterObject>();
            foreach (var teleporter in teleporters)
            {
                if (!DataManager.Instance.Teleporters.ContainsKey(teleporter.Id))
                {
                    EditorUtility.DisplayDialog("����", string.Format("��ͼ��[{0}:{1}] �еĴ��͵㣺[{2}] ���ò�����", mapDef.ID, mapDef.Name, teleporter.Id), "ȷ��");
                }
                else
                { 
                    TeleporterDefine teleporterDef = DataManager.Instance.Teleporters[teleporter.Id];
                    if (teleporterDef.MapID != mapDef.ID)
                    {
                        EditorUtility.DisplayDialog("����", string.Format("��ͼ��[{0}:{1}] �еĴ��͵㣺[{2}] �������õĵ�ͼ����", mapDef.ID, mapDef.Name, teleporter.Id), "ȷ��");
                        continue;
                    }
                    teleporterDef.Position = GameObjectTool.WorldToLogicN(teleporter.transform.position);
                    teleporterDef.Direction = GameObjectTool.WorldToLogicN(teleporter.transform.forward);
                }
            }
        }
        DataManager.Instance.SaveTeleporters();
        EditorSceneManager.OpenScene("Assets/Levels/" + currentName + ".unity");
        EditorUtility.DisplayDialog("��ʾ", "���͵㵼�����", "ȷ��");
    }
}
