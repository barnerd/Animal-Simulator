using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Skills
{
    [CreateAssetMenu(fileName = "New Knowledge", menuName = "Skill/Knowledge")]
    public class KnowledgeData : ScriptableObject
    {
        public new string name;

        public KnowledgeData[] prerequisiteKnowledge;

        public Dictionary<SkillType, int> skillRequirements;

        public Dictionary<AttributeType, int> attributeRequirements;
    }
}
