using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Combat
{
    public interface ICombatant
    {
        Transform Transform { get; }

        int Side { get; }

        ICombatant Target { get; set; }

        Combat CurrentCombat { get; set; }

        bool InCombat { get; }
        bool Engaged { get; }
    }
}
