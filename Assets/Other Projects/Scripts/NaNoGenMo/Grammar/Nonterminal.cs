using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grammar/Nonterminal")]
public class Nonterminal : GrammarSymbol
{
    public List<Rule> rules = new List<Rule>();

    public void AddRule(Rule _r)
    {
        rules.Add(_r);
    }
    
    public override void EvaluateSymbol()
    {
        Rule rule = rules[Random.Range(0, rules.Count)];
        GrammarSymbol[] rhs = rule.rhs;
        Debug.Log(rule);

        productions.Clear();
        values.Clear();

        productions.Add(this);

        for (int i = 0; i < rhs.Length; i++)
        {
            if(rhs[i] != null)
            {
                rhs[i].EvaluateSymbol();
                productions.AddRange(rhs[i].productions);
                values.AddRange(rhs[i].values);
            }
        }

        PostProcess();
    }

    public override void PostProcess() { }
}
