﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BarNerdGames.Combat;

public class Sight : MonoBehaviour
{
    public Creature creature;

    [SerializeField]
    private SphereCollider sightTrigger;

    void Start()
    {
        sightTrigger.radius = creature.creatureData.sightRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Creature>(out Creature c))
        {
            Debug.Log("Sight: " + creature.name + " (" + creature.creatureData.name + ", " + creature.creatureData.size + ", " + creature.creatureData.diet + ")" + " can now see " + c + " (" + c.creatureData.name + ", " + c.creatureData.size + ", " + c.creatureData.diet + ")" + ", a creature.");

            if (creature.creatureData.type == CreatureData.CreatureType.beasts)
            {
                switch (creature.creatureData.diet)
                {
                    case CreatureData.CreatureDiet.herbivore:
                        if (c.creatureData.type == CreatureData.CreatureType.beasts)
                        {
                            if (c.creatureData.diet == CreatureData.CreatureDiet.omnivore && creature.creatureData.size < c.creatureData.size)
                            {
                                // flee
                                creature.logicSM.ChangeState(creature, CreatureLogicSM.fleeingState);
                            }
                            if (c.creatureData.diet == CreatureData.CreatureDiet.carnivore && creature.creatureData.size <= c.creatureData.size)
                            {
                                // flee
                                creature.logicSM.ChangeState(creature, CreatureLogicSM.fleeingState);
                            }
                        }
                        break;
                    case CreatureData.CreatureDiet.omnivore:
                        if (c.creatureData.type == CreatureData.CreatureType.beasts)
                        {
                            if (creature.creatureData.size > c.creatureData.size)
                            {
                                // attack
                                creature.logicSM.ChangeState(creature, CreatureLogicSM.combatState);
                                Combat combat = new Combat();
                                combat.aggressor = creature;
                                combat.AddCombatant(creature, 0);
                                combat.AddCombatant(c, 1);
                                creature.combat = combat;
                                c.combat = combat;
                            }
                            if (creature.creatureData.size < c.creatureData.size)
                            {
                                // flee
                                creature.logicSM.ChangeState(creature, CreatureLogicSM.fleeingState);
                            }
                        }
                        break;
                    case CreatureData.CreatureDiet.carnivore:
                        if (c.creatureData.type == CreatureData.CreatureType.beasts)
                        {
                            if (c.creatureData.diet == CreatureData.CreatureDiet.omnivore && creature.creatureData.size > c.creatureData.size)
                            {
                                // attack
                                creature.logicSM.ChangeState(creature, CreatureLogicSM.combatState);
                            }
                            if (c.creatureData.diet == CreatureData.CreatureDiet.herbivore && creature.creatureData.size >= c.creatureData.size)
                            {
                                // attack
                                creature.logicSM.ChangeState(creature, CreatureLogicSM.combatState);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        else if (other.TryGetComponent<ItemPickup>(out ItemPickup i))
        {
            //Debug.Log("Sight: " + creature.name + " can now see " + i.itemData.name + ", an item.");
        }
        else if (other.TryGetComponent<Plant>(out Plant g))
        {
            //Debug.Log("Sight: " + creature.name + " can now see " + g.plantData.name + ", a plant.");
            if (creature.creatureData.food.Contains(g.plantData) && g.RemainingFood > 0)
            {
                //Debug.Log("Sight: " + creature.name + " likes to eat " + g.plantData.name + ", a plant.");
                creature.AddNearbyFood(g);
            }
        }
        else if (other.TryGetComponent<WaterBody>(out WaterBody _wb))
        {
            //Debug.Log("Sight: " + creature.name + " can now see " + _wb.name + ", a water body.");
            // get the spot of the water, above ground, closest to the creature
            Vector3 waterDrinkingPosition = _wb.GetClosestPointAboveGround(creature.transform.position);
            // add waterDrinkingPosition to creature.NearbyWaterSource();
        }
        else
        {
            //Debug.Log("Sight: " + creature.name + " can now see " + other + ", which I don't know what it is");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Creature>(out Creature c))
        {
            //Debug.Log("Sight: " + creature.name + " can no longer see " + c + ", a creature.");
        }
        else if (other.TryGetComponent<ItemPickup>(out ItemPickup i))
        {
            //Debug.Log("Sight: " + creature.name + " can no longer see " + i.itemData.name + ", an item.");
        }
        else
        {
            //Debug.Log("Sight: " + creature.name + " can no longer see " + other + ", which I don't know what it is");
        }
    }

    //private void OnTriggerStay(Collider other) { }
}
