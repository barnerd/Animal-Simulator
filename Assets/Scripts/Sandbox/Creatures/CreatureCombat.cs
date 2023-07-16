using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BarNerdGames.Combat;

public class CreatureCombat : MonoBehaviour, ICombatant
{
    public Transform Transform { get { return transform; } }

    private int side;
    public int Side { get { return side; } }

    private ICombatant target;
    public ICombatant Target { get { return target; } set { target = value; } }

    private Combat currentCombat;
    public Combat CurrentCombat { get { return currentCombat; } set { currentCombat = value; } }

    public bool InCombat { get { return currentCombat == null; } }

    private bool isEngaged;
    public bool Engaged { get { return isEngaged; } }


    void Awake()
    {
        currentCombat = null;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
