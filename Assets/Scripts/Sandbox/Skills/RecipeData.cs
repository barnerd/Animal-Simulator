using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Skills
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "Skill/Recipe")]
    public class RecipeData : ScriptableObject
    {
        public new string name;

        public KnowledgeData[] prerequisiteKnowledge;

        public Dictionary<SkillType, int> skillRequirements;

        public Dictionary<AttributeType, int> attributeRequirements;

        public Dictionary<ItemData, int> ingredients;
        public ItemData yield;

        // required Tools
    }
}
