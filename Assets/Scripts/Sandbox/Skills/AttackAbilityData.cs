using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Skills
{
    [CreateAssetMenu(fileName = "New Attack", menuName = "Skill/Attack Ability")]
    public class AttackAbilityData : AbilityData
    {
        public enum TargetMode
        {
            Single,
            AreaOfEffect
        }

        [Header("Attack Stats")]
        public float distance;
        public TargetMode targetMode;

        public DamageType damageType;
        public float toHit;
        public float damage;

        [ShowWhen("targetMode", TargetMode.AreaOfEffect)]
        public float aoeRadius;
    }
}
