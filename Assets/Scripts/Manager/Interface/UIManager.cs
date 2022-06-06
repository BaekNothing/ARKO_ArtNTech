using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTone<UIManager>
{
    public GameObject UICanvas;
    Button resetButton;
    
    Button showInputBoxButton;
    public void SetShowInputButtonInteractable(bool interactableFlag) => showInputBoxButton.interactable = interactableFlag;
    RectTransform inputPanel;

    protected override void Init()
    {
        base.Init();
        UICanvas = GameObject.Find("UICanvas").gameObject;
        resetButton = UIObject.GetT<Button>(UICanvas.transform, nameof(resetButton));
       
        showInputBoxButton = UIObject.GetT<Button>(UICanvas.transform, nameof(showInputBoxButton));
        inputPanel = UIObject.GetT<RectTransform>(UICanvas.transform, nameof(inputPanel));
        UIObject.SetAction(showInputBoxButton, () => { inputPanel.gameObject.SetActive(true); });
        UIObject.SetAction(UIObject.GetT<Button>(inputPanel.transform, "sendButton"), 
            () => { showInputBoxButton.interactable = false; SocketManager.instance.Send(UIObject.GetT<InputField>(inputPanel, "inputField").text); inputPanel.gameObject.SetActive(false); });
    }
}
