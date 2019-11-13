using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grammar/Sentence")]
public class Sentence : Nonterminal
{
    public override void PostProcess()
    {
        values[0] = char.ToUpper(values[0][0]) + values[0].Substring(1);
    }
}
