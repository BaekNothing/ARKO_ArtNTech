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
            // ���̽� ȯ�� ����

            psi.StartInfo.Arguments = "D:/KangZin-gu/PyCharmProjects/20190503_DeepLearningFromScratch/tensorflow-pix2pix-master/testResult_eng.py";
            // ������ ���̽� ����
            
            psi.StartInfo.CreateNoWindow = true;
            // ��â���� ���� �� ���� �δµ�

            psi.StartInfo.UseShellExecute = false;
            // ���μ����� �����Ҷ� �ü�� ���� ������� �̰͵� �� ���� �δµ�
            psi.Start();

            UnityEngine.Debug.Log("[�˸�] .py file ����");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("[�˸�] �����߻�: " + e.Message);
        }
    }
}
//[��ó] Unity3D ����Ƽ���� ���̽����� �����Ű��(Run .py in Unity)| �ۼ��� kanrhaehfdl1

