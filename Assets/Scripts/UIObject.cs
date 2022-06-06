using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIObject
{
    public static T GetT<T>(Transform parent, string name) where T : Component
    {
        Transform child = parent.Find(name);
        if (!child)
            return null;
        return child.GetComponent<T>();
    }

    public static T GetEldestT<T>(Transform parent)where T : Component
    {
        foreach(RectTransform child in parent.GetComponentInChildren<RectTransform>())
        {
            T childComponent = child.GetComponent<T>();
            if (childComponent)
                return childComponent;
        }
        return null;
    }

    public static void SetAction(Button btn, System.Action action)
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => action());
    }
}
