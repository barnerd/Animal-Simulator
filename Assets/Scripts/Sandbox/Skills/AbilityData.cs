using System;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Skills
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "Skill/Ability")]
    public class AbilityData : ScriptableObject
    {
        public new string name;

        public KnowledgeData[] prerequisiteKnowledge;

        public Dictionary<TechniqueData, TechniqueData.Proficiency> techniqueRequirements;

        public Dictionary<SkillType, (int min, int max)> skillRequirements;

        public Dictionary<AttributeType, int> attributeRequirements;

        // add rewards for higher technique
        // for example, when cooking, add +5 hunger per technique level above requirement
    }
}
