using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTone <T> : MonoBehaviour where T : SingleTone<T>
{
    protected static T _instance = null;
    protected static GameObject _instanceObject = null;

    public static T instance
    {
        get
        {
            if (_instance == null)
            {
            	// instance가 없을 경우, GamaObject를 만들고 컴포넌트를 붙임         
                _instanceObject = new GameObject();
                _instanceObject.transform.position = Vector3.zero;
                _instanceObject.AddComponent<T>();
                _instance = _instanceObject.GetComponent<T>();
                _instanceObject.name = _instance.GetType().ToString();
                
                _instance.Init();
                DontDestroyOnLoad(_instanceObject);
            }
            return _instance;
        }
    }

    public virtual void Standby() { }
    protected virtual void Init() { }
}