using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Plants
{
    [CreateAssetMenu(fileName = "New Tree", menuName = "Plants/Procedural Tree Data")]
    public class ProceduralTreeData : ScriptableObject
    {
        new public string name = "New Tree";

        [Header("Tree")]
        public float branchingAngle;
        public float branchLengthRatio; // ratio of length of branches to child branches
        [Range(2, 3)]
        public float branchThicknessRatio; // ratio of thickness of branches to child branches, expressed as r^n = r1^n + r2^n

        public Vector3 minDimensions;
        public Vector3 maxDimensions;

        public int minNumAttractionPoints;
        public int maxNumAttractionPoints;

        // TODO: constrain RadiusOfConsumption < RadiusOfInfluence
        public float minRadiusOfInfluence;
        public float maxRadiusOfInfluence;

        public float minRadiusOfConsumption;
        public float maxRadiusOfConsumption;
    }
}
