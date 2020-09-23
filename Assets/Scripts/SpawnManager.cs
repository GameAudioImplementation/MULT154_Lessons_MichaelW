﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//did we create tempLilyPad and lilyPad here or in Unity editor 
public class SpawnManager : NetworkBehaviour
{
    public GameObject [] lilyPadObjs= null;
    // Start is called before the first frame update
    public override void OnStartServer()
    {
        int a = 1;
        InvokeRepeating("SpawnLilyPad", 2.0f, 5.0f);  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnLilyPad()
    {
        foreach(GameObject lilyPad in lilyPadObjs)
        {
            GameObject tempLilyPad = Instantiate(lilyPad);
            NetworkServer.Spawn(tempLilyPad);
        }
    }
}