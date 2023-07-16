using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Combat
{
    public class CombatActivityReport
    {
        public float timeStamp;

        public Creature attacker;
        public Creature defender;

        public float toHit;
        public float toDodge;
        public bool hitSuccess;

        public DamageType damageType;
        public float damageAttempted;
        public float armor;
        public float damageDone;

        public bool killShot;
    }
}
