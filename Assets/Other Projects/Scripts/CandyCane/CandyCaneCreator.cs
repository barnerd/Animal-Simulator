using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class CandyCaneCreator : MonoBehaviour
{
    [Range(1, 36)]
    public int numCandyCanes;
    public Mesh candyCaneMesh;
    public Shader candyCaneShader;

    float gridWidth = .25f;
    float gridHeight = .5f;

    [Range(0, 1)]
    public float majorBandWidth;
    [Range(0, 1)]
    public float minorBandWidth;
    [Range(0, 1)]
    public float chanceOfSecondBand;
    [Range(0, 1)]
    public float chanceOfMajorGreen;
    [Range(0, 1)]
    public float chanceOfMinorGreen;

    // Start is called before the first frame update
    void Start()
    {
        float ran, ran2;
        int gridX, gridY;
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(numCandyCanes));
        float offsetX = -Mathf.RoundToInt(gridSize / 2) * gridWidth;
        float offsetY = Mathf.RoundToInt(gridSize / 2) * gridHeight;

        for (int i = 0; i < numCandyCanes; i++)
        {
            gridX = i % (2 * gridSize);
            gridY = i / (2 * gridSize);
            var go = new GameObject("CandyCane" + i);
            go.transform.parent = this.transform;
            ran = Random.Range(0.8f, 1.1f);
            ran2 = Random.Range(0.8f, 1.1f);
            go.transform.localScale = new Vector3(ran, ran, ran2);
            go.transform.position = new Vector3(gridX * gridWidth + offsetX, gridY * gridHeight + offsetY - ran2 / 2f * gridHeight, 0);
            go.transform.Rotate(Vector3.left, 90f);
            //go.transform.Rotate(Vector3.right, Random.Range(0, 45f));
            var mf = go.AddComponent<MeshFilter>();
            mf.sharedMesh = candyCaneMesh;
            var mr = go.AddComponent<MeshRenderer>();
            mr.sharedMaterial = new Material(candyCaneShader);

            // set base color to a shade of pink or Color(1f, x, halfway from x to 1f)
            ran = Random.Range(0.5f, 1f);
            mr.sharedMaterial.SetColor("_BaseColor", new Color(1f, ran, (ran + 1f) / 2));
            mr.sharedMaterial.SetFloat("_BandAngle", Random.Range(1f, 5f));
            mr.sharedMaterial.SetFloat("_NumBands", Random.Range(1, 4) * -2f * Mathf.PI); // needs to be a multiple of 2 * PI

            /**** set bands *****
             *
             * can be red or green
             * can have 1 to 5 bands
             * each band can have different widths
             *
             * examples:
             * 1 major band
             * 2 major bands
             * 1 major, single/double/triple minor bands
             * 1 major, with single/double minor band on either side
             * 
             */

            // set major band
            SetBandMaterialParameters(mr.sharedMaterial, 1, 0, true);

            // 2nd band? 
            if (Random.Range(0f, 1f) < chanceOfSecondBand)
            {
                switch (Random.Range(0, 5 + 1))
                {
                    // TODO: currently bands can bump up to each other. potentially add spacer?
                    case 0: // 2nd major band
                        SetBandMaterialParameters(mr.sharedMaterial, 2, 0.5f, true);
                        break;
                    case 1: // single minor band
                        SetBandMaterialParameters(mr.sharedMaterial, 2, 0.5f, false);
                        break;
                    case 2: // double minor band
                        SetBandMaterialParameters(mr.sharedMaterial, 2, 0.5f - minorBandWidth, false);
                        SetBandMaterialParameters(mr.sharedMaterial, 3, 0.5f + minorBandWidth, false);
                        break;
                    case 3: // triple minor band
                        SetBandMaterialParameters(mr.sharedMaterial, 2, 0.5f - minorBandWidth * 1.5f, false);
                        SetBandMaterialParameters(mr.sharedMaterial, 3, 0.5f, false);
                        SetBandMaterialParameters(mr.sharedMaterial, 4, 0.5f + minorBandWidth * 1.5f, false);
                        break;
                    case 4: // single minor band on either side
                        SetBandMaterialParameters(mr.sharedMaterial, 2, -(majorBandWidth + minorBandWidth) * 0.5f, false);
                        SetBandMaterialParameters(mr.sharedMaterial, 3, (majorBandWidth + minorBandWidth) * 0.5f, false);
                        break;
                    case 5: // double minor band on either side
                        SetBandMaterialParameters(mr.sharedMaterial, 2, -(majorBandWidth + minorBandWidth * 1.5f) * 0.5f, false);
                        SetBandMaterialParameters(mr.sharedMaterial, 3, -(majorBandWidth + minorBandWidth) * 0.5f, false);
                        SetBandMaterialParameters(mr.sharedMaterial, 4, (majorBandWidth + minorBandWidth) * 0.5f, false);
                        SetBandMaterialParameters(mr.sharedMaterial, 5, (majorBandWidth + minorBandWidth * 1.5f) * 0.5f, false);
                        break;
                    default:
                        break;
                }
            }

            // set rotation
            var br = go.AddComponent<boxRotate>();
            br.angle = Vector3.forward;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetBandMaterialParameters(Material _m, int band, float center, bool major = true)
    {
        float ran;

        _m.SetFloat("_BandCenter" + band, ((center + 1f) % 1f) * Random.Range(0.9f, 1f)); // 90 to 100%
        _m.SetFloat("_BandWidth" + band, ((major) ? majorBandWidth : minorBandWidth) * Random.Range(0.9f, 1f)); // 90 to 100%

        // pick green or red
        if (Random.Range(0f, 1f) < ((major) ? chanceOfMajorGreen : chanceOfMinorGreen))
        {
            ran = Random.Range(.1f, .333f);
            _m.SetColor("_BandColor" + band, new Color(ran, 3 * ran, ran));
        }
        else
        {
            _m.SetColor("_BandColor" + band, new Color(Random.Range(.5f, 1f), 0f, 0f));
        }
    }
}
