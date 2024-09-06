using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerCamera : MonoSingleton<MainPlayerCamera>
{
    public new Camera camera;

    public Transform viewPoint;
    public GameObject character;
    protected override void OnStart()
    {
        
    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (character == null)
            return;

        viewPoint.transform.position = character.transform.position;
        viewPoint.transform.rotation = character.transform.rotation;
    }
}
