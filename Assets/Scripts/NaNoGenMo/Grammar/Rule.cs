using UnityEngine;

[CreateAssetMenu(menuName = "Grammar/Rule")]
public class Rule : ScriptableObject
{
    // TODO: turn into a getter
    public Nonterminal lhs;
    // TODO: turn into a getter
    public GrammarSymbol[] rhs;
}
