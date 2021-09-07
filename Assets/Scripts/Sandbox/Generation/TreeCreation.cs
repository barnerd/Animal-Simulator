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
        float x, z, chance;

        for (int i = 0; i < numTrees; i++)
        {
            // set random position
            x = Random.Range(ground.size * 1 / 4, ground.size * 3 / 4f);
            z = Random.Range(ground.size * 1 / 4, ground.size * 3 / 4f);
            Vector3 position = new Vector3(x, ground.GetHeightAtXZ(x, z), z);

            chance = Mathf.PerlinNoise(x, z);

            if (chance > .3f)
            {
                GameObject tree = Tree.Create(treePrefab, trees[Random.Range(0, trees.Length)], position, transform);

                tree.transform.Rotate(new Vector3(0, Random.Range(-180f, 180f), 0));
                tree.transform.localScale = chance * Vector3.one;

                tree.transform.parent = this.transform;
            }
        }
    }
}
