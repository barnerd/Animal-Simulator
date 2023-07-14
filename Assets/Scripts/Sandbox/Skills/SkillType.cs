using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Skills
{
    [CreateAssetMenu(fileName = "New Skill Type", menuName = "Skill/Type")]
    public class SkillType : ScriptableObject
    {
        public new string name;

        public Dictionary<AttributeType, int> attributeRequirements;

        public static int ExpToLevel(int _exp)
        {
            return Mathf.FloorToInt(Mathf.Sqrt(_exp));
        }
    }
}
