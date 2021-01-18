using System;

[Serializable]
public class IntReference
{
    public bool UseConstant = true;
    public int ConstantValue;
    public IntVariable Variable;

    public IntReference()
    { }

    public IntReference(int value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public IntReference(IntVariable value)
    {
        UseConstant = false;
        Variable = value;
    }

    public int Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
        set { Variable.Value = value; }
    }

    public static implicit operator int(IntReference reference)
    {
        return reference.Value;
    }
}
