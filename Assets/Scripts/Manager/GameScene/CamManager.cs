using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : SingleTone<CamManager>
{
    private Camera mainCam;

    public Camera GetCamera()
    {
        if(!mainCam)
            mainCam = Camera.main;
        return mainCam;
    }

    protected override void Init()
    {
        base.Init();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CamFollowPlayer();
    }

    void CamFollowPlayer() => mainCam.transform.position = 
        new Vector3(PlayerManager.instance.GetPlayer().transform.position.x, 0, -10f);

}
