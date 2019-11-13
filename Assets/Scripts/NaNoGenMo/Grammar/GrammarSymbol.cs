using System.Collections.Generic;
using UnityEngine;

public abstract class GrammarSymbol : ScriptableObject
{
    public string symbolName;
    public List<GrammarSymbol> productions = new List<GrammarSymbol>();
    public List<string> values = new List<string>();

    public abstract void EvaluateSymbol();
    public abstract void PostProcess();
}
