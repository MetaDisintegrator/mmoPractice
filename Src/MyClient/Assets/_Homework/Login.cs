using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Network.NetClient.Instance.Init("127.0.0.1", 8888);
        Network.NetClient.Instance.Connect();

        SkillBridge.Message.NetMessage msg = new()
        {
            Request = new()
            {
                firstRequest = new()
                {
                    Str = "Hello World!"
                }
            }
        };
        Network.NetClient.Instance.SendMessage(msg);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
