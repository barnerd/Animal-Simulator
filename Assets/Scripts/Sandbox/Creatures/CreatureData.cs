using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BarNerdGames.Skills;

[CreateAssetMenu(fileName = "New Creature", menuName = "Creature/Creature Data")]
public class CreatureData : ScriptableObject
{
    public enum CreatureSize
    {
        small,
        medium,
        large
    }

    public enum CreatureType 
    {
        beasts,
        elementals,
        constructs,
        dragons,
        fey,
        humanoid,
        ooze,
        plant,
        undead
    }

    public enum CreatureDiet // blood for vampires
    {
        herbivore,
        omnivore,
        carnivore
    }

    new public string name = "New Creature";

    public CreatureGraphicsData maleGraphcisData;
    public CreatureGraphicsData femaleGraphcisData;

    [Header("Tags")]
    public CreatureSize size;
    public CreatureType type;
    public CreatureDiet diet;

    [Header("Radii")]
    [Tooltip("Sight distance, in m")]
    public float sightRadius;
    [Tooltip("Distance to eat/drink, in m")]
    public float consumingRange;

    [Header("Hunger/Thirst Thresholds")]
    [Tooltip("how fast this creature eats")]
    public float eatRate;
    [Tooltip("how fast this creature drinks")]
    public float drinkRate;
    [Tooltip("how fast this creature requires food/water")]
    public float metabolismRate;

    public List<ScriptableObject> food;

    public List<AbilityData> startingAbilities;

    public float healthBase;
    public float hungerBase;
    public float thirstBase;

    [Header("Attributes")]
    public float speedBase;
    public float strengthBase;
    public float dexterityBase;
    public float constitutionBase;

    [Header("Armors")]
    public float slashArmorBase;
    public float pierceArmorBase;
    public float bludgeoningArmorBase;

    [Header("Damages")]
    public float slashDamageBase;
    public float pierceDamageBase;
    public float bludgeoningDamageBase;

    [Header("Combat AI Chances")]
    [Range(0, 1)]
    public float chanceToAttack;
    [Range(0, 1)]
    public float chanceToDefend;
    [Range(0, 1)]
    public float chanceToSupport;
}