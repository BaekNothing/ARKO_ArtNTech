using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPC : BaseCharacter
{
    SpriteRenderer spriteRenderer;
    public int textCode = 1;
    public bool interactable = false; 

    void Awake()
    {
        init();
    }

    float checkDelayTime = 0f;
    private void Update()
    {
        checkDelayTime += Time.deltaTime;
        if(checkDelayTime >= 1f)
        {
            int counter = 0;
            for (int i = 0; i < transform.parent.childCount; i++)
                if (transform.parent.GetChild(i).gameObject.tag == "Enemy")
                    counter++;
            interactable = counter == 0;
            checkDelayTime = 0;
        }
    }

    protected override void init()
    {
        base.init();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        setEnterAction(PlayerEnterCheck);
        setExitAction(PlayerExitCheck);
    }

    void PlayerEnterCheck(Collider2D other)
    {
        if (interactable && other.gameObject.tag == "Player")
        {
            spriteRenderer.color = Color.red;
            DialogManager.instance.focusedNPC = this;
        }
    }

    void PlayerExitCheck(Collider2D other)
    {
        if (interactable && other.gameObject.tag == "player" || other.gameObject.tag == "Player")
        {
            spriteRenderer.color = Color.white;
            if (DialogManager.instance.focusedNPC == this)
                DialogManager.instance.focusedNPC = null;
        }
    }
}
