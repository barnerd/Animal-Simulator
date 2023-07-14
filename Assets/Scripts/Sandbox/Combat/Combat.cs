using System.Collections;
using System.Collections.Generic;

namespace BarNerdGames.Combat
{
    public class Combat
    {
        public List<ICombatant>[] Sides { get; private set; }
        // each creature knows which side they're on

        // each creature will have a primary target

        // who started the combat
        public Creature aggressor;

        public Combat(int _sides = 2)
        {
            Sides = new List<ICombatant>[_sides];

            for (int i = 0; i < Sides.Length; i++)
            {
                Sides[i] = new List<ICombatant>();
            }
        }

        public void AddCombatant(ICombatant _combatant, int _side)
        {
            if (Sides.Length > _side)
            {
                if (!Sides[_side].Contains(_combatant))
                {
                    Sides[_side].Add(_combatant);
                }
            }
        }
    }
}
