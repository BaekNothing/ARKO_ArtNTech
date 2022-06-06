using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : BaseCharacter
{
    int jumpPower = 100;

    GameObject hand;
    public int JumpCount = 0;
    // Start is called before the first frame update
    void Awake()
    {
        init();
    }

    protected override void init()
    {
        base.init();
        setEnterAction(JumpCheck);
        hand = this.transform.Find("hand").gameObject;
    }

    public void attack(){
    }

    void JumpToEnemy()
    {
    }

    void GrabEnemy()
    {
    }

    void Update()
    {
        FindEnemy();
        GrabEnemy();
    }
    void FindEnemy()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 4f);
        foreach(Collider2D c in col)
        {
        }
    }

    void JumpCheck(Collision2D othrer)
    {
        if(othrer.gameObject.tag == "Ground")
            JumpCount = 0;
    }

    public void Jump(Vector2 jumpDir)
    {
        Rigidbody2D playerRigidbody = GetComponent<Rigidbody2D>();
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.angularVelocity = 0f;
        playerRigidbody.AddForce(jumpDir * jumpPower, ForceMode2D.Impulse);
        JumpCount++;
    }
}
