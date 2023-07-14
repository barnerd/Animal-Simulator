using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Plants
{
    [CreateAssetMenu(fileName = "New Tree", menuName = "Plants/Tree Data")]
    public class TreeData : ScriptableObject
    {
        new public string name = "New Tree";

        public GameObject[] meshes;
        public GameObject[] deadMeshes; // just doesn't have leaves. index must match with meshes

        public Material[] barkMaterials;
        public Material[] barkDeadMaterials;

        public Material[] springLeaves;
        public Material[] summerLeaves;
        public Material[] fallLeaves;
        public Material[] winterLeaves;
        public Material[] deadLeaves;
    }
}
