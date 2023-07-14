using System;

[Serializable]
public class StringReference
{
    public bool UseConstant = true;
    public string ConstantValue;
    public StringVariable Variable;

    public StringReference()
    { }

    public StringReference(string value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public StringReference(StringVariable value)
    {
        UseConstant = false;
        Variable = value;
    }

    public string Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
        set { Variable.Value = value; }
    }

    public static implicit operator string(StringReference reference)
    {
        return reference.Value;
    }
}
