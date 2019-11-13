using System;

public class Grammar
{
    public Rule rule;

    public string Evaluate()
    {
        string production = "";

        // evaluate rule
        GrammarSymbol[] rhs = rule.rhs;

        for (int i = 0; i < rhs.Length; i++)
        {
            rhs[i].EvaluateSymbol();
            production += String.Join(String.Empty, rhs[i].values);
        }

        return production;
    }
}
