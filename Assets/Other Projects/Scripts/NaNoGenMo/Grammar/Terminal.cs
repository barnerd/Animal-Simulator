using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grammar/Terminal")]
public class Terminal : GrammarSymbol
{
    public string[] vocabulary;

    public override void EvaluateSymbol()
    {
        productions.Clear();
        values.Clear();

        productions.Add(this);
        values.Add(vocabulary[Random.Range(0, vocabulary.Length)]);

        PostProcess();
    }

    public override void PostProcess() { }
}
