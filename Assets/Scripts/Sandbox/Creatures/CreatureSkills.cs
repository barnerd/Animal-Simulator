using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BarNerdGames.Skills;

public class CreatureSkills : MonoBehaviour
{
    public CreatureAttributes attributes;

    public Dictionary<SkillType, int> SkillLevels { get; private set; }
    public Dictionary<TechniqueData, int> TechniqueLevels { get; private set; }
    public List<AbilityData> Abilities { get; private set; }
    public List<KnowledgeData> KnownKnowledge { get; private set; }

    void Awake()
    {
        SkillLevels = new Dictionary<SkillType, int>();
        TechniqueLevels = new Dictionary<TechniqueData, int>();

        Abilities = new List<AbilityData>();
        KnownKnowledge = new List<KnowledgeData>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LearnTechnique(TechniqueData _technique)
    {
        if (KnowAllReqs(_technique.prerequisiteKnowledge))
        {
            if (!TechniqueLevels.ContainsKey(_technique))
            {
                TechniqueLevels.Add(_technique, 1);
            }
        }
    }

    public void LearnAbility(AbilityData _ability)
    {
        if (KnowAllReqs(_ability.prerequisiteKnowledge))
        {
            if (!Abilities.Contains(_ability))
            {
                Abilities.Add(_ability);
            }
        }
    }

    public void LearnKnowledge(KnowledgeData _knowledge)
    {
        if (KnowAllReqs(_knowledge.prerequisiteKnowledge))
        {
            if (!KnownKnowledge.Contains(_knowledge))
            {
                KnownKnowledge.Add(_knowledge);
            }
        }
    }

    private bool KnowAllReqs(KnowledgeData[] _knowledge)
    {
        foreach (var item in _knowledge)
        {
            if (!KnownKnowledge.Contains(item))
            {
                return false;
            }
        }

        return true;
    }

    public void LevelAbility(AbilityData _ability)
    {
        // level skills
        foreach (var skill in _ability.skillRequirements.Keys)
        {
            if (_ability.skillRequirements[skill].max > GetSkillLevel(skill))
            {
                LevelSkill(skill);
            }
        }

        // Level techniques
        foreach (var technique in _ability.techniqueRequirements.Keys)
        {
            LevelTechnique(technique);
        }
    }

    public void LevelSkill(SkillType _type, int _exp = 1)
    {
        if (SkillLevels.ContainsKey(_type))
        {
            SkillLevels[_type] += _exp;
        }
    }

    public void LevelTechnique(TechniqueData _t, int _exp = 1)
    {
        if (TechniqueLevels.ContainsKey(_t))
        {
            TechniqueLevels[_t] += _exp;
        }
    }

    public TechniqueData.Proficiency GetTechniqueProficiency(TechniqueData _tech)
    {
        return (TechniqueLevels.ContainsKey(_tech)) ? TechniqueData.ExpToProficiency(TechniqueLevels[_tech]) : 0;
    }

    public int GetSkillLevel(SkillType _type)
    {
        return (SkillLevels.ContainsKey(_type)) ? SkillType.ExpToLevel(SkillLevels[_type]) : 0;
    }
}
