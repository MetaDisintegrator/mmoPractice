using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public GameObject UILogin;
    public GameObject UILoading;
    public GameObject UITips;

    public Slider prograssBar;
    public Text prograssText;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));
        UnityLogger.Init();
        Common.Log.Init("Unity");//LogÊÇlog4net½Ó¿Ú
        Common.Log.Info("LoadingManager start");

        UILogin.SetActive(false);
        UILoading.SetActive(false);
        UITips.SetActive(false);

        UITips.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        UILoading.SetActive(true) ;
        yield return new WaitForSeconds(1.2f);
        UITips.SetActive(false);

        MapService.Instance.Init();
        DataManager.Instance.Load();
        StartCoroutine(DataManager.Instance.LoadData());
        for (float i = 50; i < 100; i += Random.Range(0.1f, 1f))
        {
            prograssBar.value = i;
            prograssText.text = ((int)i).ToString() + "%";
            yield return new WaitForEndOfFrame();
        }

        UILoading.SetActive(false);
        UILogin.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
