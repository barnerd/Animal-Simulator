using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BarNerdGames.Skills;

namespace BarNerdGames.Combat
{
    public class Combat
    {
        public List<CombatActivityReport> results;

        public List<ICombatant>[] Sides { get; private set; }
        // each creature knows which side they're on

        // each creature will have a primary target

        // who started the combat
        public ICombatant aggressor;

        public Combat(int _sides = 2)
        {
            results = new List<CombatActivityReport>();
            InitilizeSides(_sides);
        }

        public Combat(ICombatant _attacker, ICombatant _defender)
        {
            results = new List<CombatActivityReport>();
            InitilizeSides(2);
            InitializeCombatants(_attacker, _defender);
        }

        public void AddCombatant(ICombatant _combatant, int _side)
        {
            if (Sides.Length > _side)
            {
                if (!Sides[_side].Contains(_combatant))
                {
                    Sides[_side].Add(_combatant);
                    _combatant.CurrentCombat = this;
                }
            }
        }

        private void InitilizeSides(int _sides = 2)
        {
            Sides = new List<ICombatant>[_sides];

            for (int i = 0; i < Sides.Length; i++)
            {
                Sides[i] = new List<ICombatant>();
            }
        }

        private void InitializeCombatants(ICombatant _attacker, ICombatant _defender)
        {
            if (_attacker.InCombat && _defender.InCombat)
            {
                // TODO: figure out what to do if both are already in combat
            }
            else
            {
                if (_attacker.InCombat)
                {
                    // TODO: have _defender joined attacker's combat
                }
                if (_defender.InCombat)
                {
                    // TODO: have _attacker joined defender's combat
                }
            }

            aggressor = _attacker;

            AddCombatant(_attacker, 0);
            AddCombatant(_defender, 1);
        }

        public void ExecuteAttackAbility(Creature _attacker, AttackAbilityData _ability)
        {
            // get the attacker's stats
            CreatureSkills attackerSkills = _attacker.GetComponent<CreatureSkills>();
            CreatureAttributes attackerAttributes = _attacker.GetComponent<CreatureAttributes>();
            // get the attacker's ability's stats
            AttackAbilityData attackerAbilityStats = _ability;
            // get the defender's stats
            Creature _defender = _attacker.GetComponent<CreatureCombat>().Target.Transform.GetComponent<Creature>();
            CreatureSkills defenderSkills = _defender.GetComponent<CreatureSkills>();
            CreatureAttributes defenderAttributes = _defender.GetComponent<CreatureAttributes>();

            // calculate combat results
            CombatActivityReport result = new CombatActivityReport();
            result.timeStamp = Time.time;

            result.attacker = _attacker;
            result.defender = _defender;

            // TODO: check ability bonuses for higher skill and higher techniques

            result.toHit = attackerAttributes.GetAttribute(attackerAttributes.DexterityType) / 3 + attackerAbilityStats.toHit;
            result.toDodge = defenderAttributes.GetAttribute(defenderAttributes.DexterityType) / 3;
            result.hitSuccess = Random.Range(1, 21) + result.toHit > result.toDodge;

            result.damageType = _ability.damageType;

            result.damageAttempted = attackerAttributes.GetAttribute(attackerAttributes.StrengthType) / 4 + attackerAbilityStats.damage;
            result.armor = defenderAttributes.GetAttribute(defenderAttributes.ConstitutionType) / 5 + defenderAttributes.GetArmor(result.damageType);
            result.damageDone = result.damageAttempted - result.armor;

            // check for death
            if (defenderAttributes.GetHealthValue() < result.damageDone)
            {
                result.killShot = true;
            }
            Debug.Log("Battle Results: " + result);

            results.Add(result);

            // dish out damage and statuses and effects
            defenderAttributes.TakeDamage(result.damageDone);

            // level up ability
            _attacker.GetComponent<CreatureSkills>().LevelAbility(_ability);
        }
    }
}
