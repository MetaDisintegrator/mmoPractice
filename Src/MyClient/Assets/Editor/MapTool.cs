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
            EditorUtility.DisplayDialog("警告", "请先保存当前场景", "确定");
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
                    EditorUtility.DisplayDialog("错误", string.Format("地图：[{0}:{1}] 中的传送点：[{2}] 配置不存在", mapDef.ID, mapDef.Name, teleporter.Id), "确定");
                }
                else
                { 
                    TeleporterDefine teleporterDef = DataManager.Instance.Teleporters[teleporter.Id];
                    if (teleporterDef.MapID != mapDef.ID)
                    {
                        EditorUtility.DisplayDialog("错误", string.Format("地图：[{0}:{1}] 中的传送点：[{2}] 与所配置的地图不符", mapDef.ID, mapDef.Name, teleporter.Id), "确定");
                        continue;
                    }
                    teleporterDef.Position = GameObjectTool.WorldToLogicN(teleporter.transform.position);
                    teleporterDef.Direction = GameObjectTool.WorldToLogicN(teleporter.transform.forward);
                }
            }
        }
        DataManager.Instance.SaveTeleporters();
        EditorSceneManager.OpenScene("Assets/Levels/" + currentName + ".unity");
        EditorUtility.DisplayDialog("提示", "传送点导出完成", "确定");
    }
}
