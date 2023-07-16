using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using BarNerdGames.Skills;

[Serializable]
public class StringStringDictionary : SerializableDictionary<string, string> {}

[Serializable]
public class ObjectColorDictionary : SerializableDictionary<UnityEngine.Object, Color> {}

[Serializable]
public class ColorArrayStorage : SerializableDictionary.Storage<Color[]> {}

[Serializable]
public class StringColorArrayDictionary : SerializableDictionary<string, Color[], ColorArrayStorage> {}

[Serializable]
public class MyClass
{
    public int i;
    public string str;
}

[Serializable]
public class QuaternionMyClassDictionary : SerializableDictionary<Quaternion, MyClass> {}


// Below added by Verage
[Serializable]
public class AttributeTypeIntDictionary : SerializableDictionary<AttributeType, int> { }

[Serializable]
public class TechniqueDataProficiencyDictionary : SerializableDictionary<TechniqueData, TechniqueData.Proficiency> { }

[Serializable]
public class SkillTypeIntIntDictionary : SerializableDictionary<SkillType, Vector2Int> { }
