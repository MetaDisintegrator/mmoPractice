using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    class UIElement
    {
        public string resource;
        public bool cache;
        public GameObject instance;
    }

    public Transform Canvas { private get; set; }
    private Dictionary<Type, UIElement> elements = new Dictionary<Type, UIElement>();

    public UIManager()
    {
        elements.Add(typeof(UITest), new UIElement() { resource = "UI/UITest", cache = true });
    }

    public T Show<T>()
    { 
        Type type = typeof(T);
        if (elements.ContainsKey(type))
        {
            UIElement info = elements[type];
            if (info.instance != null)
            {
                info.instance.gameObject.SetActive(true);
            }
            else
            { 
                GameObject prefab = Resources.Load<GameObject>(info.resource);
                if (prefab == null)
                {
                    Debug.LogErrorFormat("UIManager: Unable to load prefab: [Type:{}, Resource:{1}]", type.Name, info.resource);
                    return default;
                }
                info.instance = UnityEngine.Object.Instantiate(prefab, Canvas);
                info.instance.transform.localScale = Vector3.one;
                info.instance.transform.localPosition = Vector3.zero;
            }
            return info.instance.GetComponent<T>();
        }
        return default;
    }

    public void Close(Type type)
    {
        if (elements.ContainsKey(type))
        {
            UIElement info = elements[type];
            if (info.cache)
            {
                info.instance?.gameObject.SetActive(false);
            }
            else if(info.instance != null)
            { 
                GameObject.Destroy(info.instance);
                info.instance = null;
            }
        }
    }
}
