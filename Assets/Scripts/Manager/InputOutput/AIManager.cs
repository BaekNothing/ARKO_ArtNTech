using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Diagnostics;

public class AIManager : SingleTone<AIManager>
{
    protected override void Init()
    {
        base.Init();
    }

    public void StartPy()
    {
        try
        {
            string filePath = Application.dataPath + "runGPT2.py";
            Process psi = new Process();
            psi.StartInfo.FileName = "D:/KangZin-gu/Anaconda3/envs/TensorFlowGPU/python.exe";
            // 파이썬 환경 연결

            psi.StartInfo.Arguments = "D:/KangZin-gu/PyCharmProjects/20190503_DeepLearningFromScratch/tensorflow-pix2pix-master/testResult_eng.py";
            // 실행할 파이썬 파일
            
            psi.StartInfo.CreateNoWindow = true;
            // 새창에서 시작 걍 일케 두는듯

            psi.StartInfo.UseShellExecute = false;
            // 프로세스를 시작할때 운영체제 셸을 사용할지 이것도 걍 일케 두는듯
            psi.Start();

            UnityEngine.Debug.Log("[알림] .py file 실행");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("[알림] 에러발생: " + e.Message);
        }
    }
}
//[출처] Unity3D 유니티에서 파이썬파일 실행시키기(Run .py in Unity)| 작성자 kanrhaehfdl1

