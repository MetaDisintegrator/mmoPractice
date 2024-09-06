using UnityEngine;


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool global = true;
    static bool inited = false;
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance =(T)FindObjectOfType<T>();
            }
            return instance;
        }

    }

    void Start()
    {
        if (global) 
        {
            if (!inited)
            {
                DontDestroyOnLoad(this.gameObject);
                inited = true;
            }
            else
                Destroy(gameObject);
        }
        this.OnStart();
    }

    protected virtual void OnStart()
    {

    }
}