using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    private void Start()
    {
        CamManager.instance.Standby();
        InputManager.instance.Standby();
        UIManager.instance.Standby();
        DialogManager.instance.Standby();
        SocketManager.instance.Standby();
    }

    void OnApplicationQuit() 
    {
        System.GC.Collect();
    }
}
