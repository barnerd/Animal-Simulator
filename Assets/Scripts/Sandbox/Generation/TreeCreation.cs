using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BarNerdGames.Plants;

public class TreeCreation : MonoBehaviour
{
    public int numTrees;

    public Ground ground;
    public Weather weather;

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
            x = Random.Range(0, ground.size);
            z = Random.Range(0, ground.size);
            Vector3 position = new Vector3(x, ground.GetHeightAtXZ(x, z), z);

            chance = Mathf.PerlinNoise(x, z);

            if (chance > .3f)
            {
                TreeData treeData = trees[Random.Range(0, trees.Length)];

                GameObject tree = BarNerdGames.Plants.Tree.Create(treePrefab, treeData, position, transform);

                tree.GetComponent<BarNerdGames.Plants.Tree>().weather = weather;

                tree.transform.Rotate(new Vector3(0, Random.Range(-180f, 180f), 0));
                tree.transform.localScale = chance * Vector3.one;

                tree.transform.parent = this.transform;
            }
        }
    }
}
