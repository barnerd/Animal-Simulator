using System;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Skills
{
    [CreateAssetMenu(fileName = "New Technique", menuName = "Skill/Technique")]
    public class TechniqueData : ScriptableObject
    {
        public enum Proficiency // journeyman, apprentice 
        {
            novice = 1,
            proficient = 10,
            adept = 100,
            expert = 1000,
            master = 10000
        }

        public new string name;

        public KnowledgeData[] prerequisiteKnowledge;

        public Dictionary<SkillType, int> skillRequirements;

        public Dictionary<AttributeType, int> attributeRequirements;

        public static Proficiency ExpToProficiency(int _i)
        {
            var values = (Proficiency[])Enum.GetValues(typeof(Proficiency));
            for (int i = 0; i < values.Length; i++)
            {
                if (_i < (int)values[i])
                    return values[i];
            }
            return Proficiency.master;
        }
    }
}
