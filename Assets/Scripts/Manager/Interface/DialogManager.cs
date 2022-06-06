using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : SingleTone<DialogManager>
{
    public NPC focusedNPC;

    GameObject dialogCanvas;
    public GameObject GetDialogCanvas()
    {
        if (!dialogCanvas)
        { 
            dialogCanvas = GameObject.Find("DialogCanvas");
            for (int i = 0; i < dialogCanvas.transform.childCount; i++)
                Destroy(dialogCanvas.transform.GetChild(i).gameObject);
        }
        return dialogCanvas;
    }    
    GameObject dialogPanel;
    public void SetDialogPanel()
    {
        dialogPanel = Instantiate(DataManager.instance.PrefabLoad<GameObject>
            (DataManager.ResourceCategories.Interface, "DialogPanel"), GetDialogCanvas().transform);
        dialogObjs = new DialogPanelInnerObjects(dialogPanel);

        UIObject.SetAction(dialogObjs.btnSkip, OnClickClose);
        UIObject.SetAction(dialogObjs.btnNext, OnClickNext);
    }

    DialogPanelInnerObjects dialogObjs;
    struct DialogPanelInnerObjects
    {
        public int logIndex;
        public List<DialogInfoStruct> logInfos;
        public RectTransform pnlBg;
        public Image imgChar_left;
        public Image imgChar_right;
        public Button btnSkip;
        public RectTransform pnlTextField;
        public Text lblText;
        public Text lblName;
        public Button btnNext;

        public DialogPanelInnerObjects(GameObject dialogPanel)
        {
            logIndex = 0;
            logInfos = null;
            Transform pnlTf = dialogPanel.transform;
            this.pnlBg = UIObject.GetT<RectTransform>(pnlTf, nameof(pnlBg));
            this.imgChar_left = UIObject.GetT<Image>(pnlTf, nameof(imgChar_left));
            this.imgChar_right = UIObject.GetT<Image>(pnlTf, nameof(imgChar_right));
            this.btnSkip = UIObject.GetT<Button>(pnlTf, nameof(btnSkip));
            this.pnlTextField = UIObject.GetT<RectTransform>(pnlTf, nameof(pnlTextField));
            this.lblName = UIObject.GetT<Text>(pnlTextField, nameof(lblName));
            this.lblText = UIObject.GetT<Text>(pnlTextField, nameof(lblText));
            this.btnNext = UIObject.GetT<Button>(pnlTf, nameof(btnNext));
        }
    }

    protected override void Init()
    {
        base.Init();
        GetDialogCanvas();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            ShowPanel(new List<DialogInfoStruct> { new DialogInfoStruct("temp") });
    }

    public struct DialogInfoStruct
    {
        public string mainText;
        public string name;
        public string left;
        public string right;
        public int code;

        public DialogInfoStruct(string mainText, int code = 0, string name = "", string left = "left_default", string right = "right_default")
        {
            this.mainText = mainText;
            this.name = name;
            this.left = left;
            this.right = right;
            this.code = code; 
        }
    }

    public void ShowPanel(List<DialogInfoStruct> dialogInfos)
    {
        if (dialogInfos.Count <= 0)
            return;
        if (!dialogPanel)
            SetDialogPanel();
        dialogPanel.gameObject.SetActive(true);
        dialogObjs.logIndex = 0;
        dialogObjs.logInfos = dialogInfos;
        dialogObjs.lblName.text = dialogObjs.logInfos[0].name;
        StartCoroutine(showText(dialogObjs.logInfos[0].mainText));
    }

    bool isShowing = false;
    bool isSkip = false;
    IEnumerator showText(string str)
    {
        isShowing = true;
        int index = 0;
        string outStr = "";
        float delay = 0.2f;
        float time = delay;

        while (!isSkip && index < str.Length)
        {
            time += Time.deltaTime;
            if (time >= delay)
            {
                outStr += str.Substring(index++, 1);
                dialogObjs.lblText.text = outStr;
                time = 0;
            }
            yield return new WaitForEndOfFrame();
        }
        dialogObjs.lblText.text = str;
        isShowing = false;
        isSkip = false;
    }

    void OnClickNext()
    {
        if (isShowing)
        {
            isSkip = true;
            return;
        }
        dialogObjs.logIndex++;
        if (dialogObjs.logIndex < dialogObjs.logInfos.Count)
        {
            dialogObjs.lblName.text = dialogObjs.logInfos[dialogObjs.logIndex].name;
            StartCoroutine(showText(dialogObjs.logInfos[dialogObjs.logIndex].mainText));
        }
        else
            OnClickClose();
    }

    public void OnClickClose()
    {
        ShowPanel(new List<DialogInfoStruct> { new DialogInfoStruct("") });
        dialogPanel.gameObject.SetActive(false);
    }

    public List<DialogInfoStruct> LoadDialogInfo(int code)
    {
        return DataManager.instance.DialogLoad(code);
    }
}
