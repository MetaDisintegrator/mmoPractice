using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneManager : MonoSingleton<SceneManager>
{
    UnityAction<float> onProgress = null;
    public event UnityAction onComplete = null;

    public bool loadCompleted = true;
    object lockObj = new object();

    // Use this for initialization
    protected override void OnStart()
    {
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void LoadScene(string name)
    {
        StartCoroutine(LoadLevel(name));
    }

    IEnumerator LoadLevel(string name)
    {
        Debug.LogFormat("LoadLevel: {0}", name);
        loadCompleted = false;
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
        async.allowSceneActivation = true;
        async.completed += LevelLoadCompleted;
        while (!async.isDone)
        {
            if (onProgress != null)
                onProgress(async.progress);
            yield return null;
        }
    }

    private void LevelLoadCompleted(AsyncOperation obj)
    {
        if (onProgress != null)
            onProgress(1f);
        Debug.Log("LevelLoadCompleted:" + obj.progress);
        onComplete?.Invoke();
        onComplete = null;
        lock (lockObj)
        {
            loadCompleted = true;
        }
    }

    public void SubscribeLoadDone(UnityAction action)
    {
        lock (lockObj)
        {
            if (!loadCompleted)
            {
                onComplete += action;
                return;
            }
        }
        action?.Invoke();
    }
}
