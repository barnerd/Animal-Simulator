using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCreation : MonoBehaviour
{
    public int numTrees;

    public Ground ground;

    public GameObject treePrefab;

    [Header("Trees")]
    public TreeData[] trees;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numTrees; i++)
        {
            // set random position
            float x = Random.Range(0f, ground.size);
            float z = Random.Range(0f, ground.size);
            Vector3 position = new Vector3(x, ground.GetHeightAtXZ(x, z), z);

            GameObject tree = Tree.Create(treePrefab, trees[Random.Range(0, trees.Length)], position, transform);

            tree.transform.parent = this.transform;
        }
    }
}
