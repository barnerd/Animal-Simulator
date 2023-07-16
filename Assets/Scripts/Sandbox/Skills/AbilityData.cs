using System;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Skills
{
    public class AbilityData : ScriptableObject
    {
        public new string name;

        [Header("Prerequisites")]
        public KnowledgeData[] knowledgeRequirements;

        public TechniqueDataProficiencyDictionary techniqueRequirements;

        public SkillTypeIntIntDictionary skillRequirements;
        //public Dictionary<SkillType, (int min, int max)> skillRequirements;

        public AttributeTypeIntDictionary attributeRequirements;

        [Space(20)]
        public float cooldown;

        public bool usableInCombat;
        // add rewards for higher technique
        // for example, when cooking, add +5 hunger per technique level above requirement
    }
}
