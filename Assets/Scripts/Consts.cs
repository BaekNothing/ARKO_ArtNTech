using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Consts
{
    public static class CoreData
    {
        public struct charStatus
        {
            public int hp;
            public int atk;
            public int spd;
            public int skillLevel;
            public int skillPoint;
            public int exp;
            public int level;

            public charStatus(int hp = 0, int atk = 0, int spd = 0, int skillLevel = 0, int skillPoint = 0, int exp = 0, int level = 0)
            {
                this.hp = hp;
                this.atk = atk;
                this.spd = spd;
                this.skillLevel = skillLevel;
                this.skillPoint = skillPoint;
                this.exp = exp;
                this.level = level;
            }
        }
    }

    public static class Utility
    {
        public static UnityEngine.Object LogAndNull(string log)
        {
#if UNITY_EDITOR
            Debug.Log(log);
#endif
            return null;
        }
    }
}
