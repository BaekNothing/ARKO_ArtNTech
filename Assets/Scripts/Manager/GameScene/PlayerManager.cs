using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Consts;

public class PlayerManager : SingleTone<PlayerManager>
{
    public CoreData.charStatus playerStatus;
    public Player player;
    public Player GetPlayer()
    {
        if(!player)
            player = GameObject.Find("Player").GetComponent<Player>();
        return player;
    }

    //manage char status
    protected override void Init()
    {
        base.Init();
        playerStatus = DataManager.instance.LoadUserData();
        SetSpd(8);
    }

    public void SetSpd(int inputSpd) => playerStatus.spd = inputSpd;
    public void SetHp(int inputHp) => playerStatus.hp = inputHp;
    public void SetAtk(int inputAtk) => playerStatus.atk = inputAtk;
    public void SetExp(int inputExp) => playerStatus.exp = inputExp;
    public void SetLevel(int inputLevel) => playerStatus.level = inputLevel;
    public void SetSkillLevel(int inputSkillLevel) => playerStatus.skillLevel = inputSkillLevel;
    public void SetSkillPoint(int inputSkillPoint) => playerStatus.skillPoint = inputSkillPoint;
}
