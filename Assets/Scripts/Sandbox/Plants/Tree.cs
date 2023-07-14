using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Plants
{
    public class Tree : MonoBehaviour
    {
        [SerializeField]
        private GameObject GFX;
        [SerializeField]
        private BoxCollider boxCollider;

        public TreeData treeData;
        private int meshIndex;
        private int barkIndex;
        private int leafIndex;

        private bool isDead;

        public Weather weather;
        private Weather.Season currentSeason;

        // Start is called before the first frame update
        void Start()
        {
            //if (GFX == null) GFX = GetComponentInChildren<MeshRenderer>().gameObject;
            if (boxCollider == null) boxCollider = GetComponentInChildren<BoxCollider>();

            currentSeason = weather.GetWeather();
            UpdateLeaves();
        }

        // Update is called once per frame
        void Update()
        {
            Weather.Season newSeason = weather.GetWeather();

            if (newSeason != currentSeason)
            {
                currentSeason = newSeason;

                UpdateLeaves();
            }
        }

        private void UpdateLeaves()
        {
            MeshRenderer renderer = GFX.GetComponentInChildren<MeshRenderer>();

            //Debug.Log("Updating leaves on " + this + " to season: " + currentSeason);

            switch (currentSeason)
            {
                case Weather.Season.spring:
                    SetLeaves(renderer, treeData.springLeaves);
                    break;
                case Weather.Season.summer:
                    SetLeaves(renderer, treeData.summerLeaves);
                    break;
                case Weather.Season.autumn:
                    SetLeaves(renderer, treeData.fallLeaves);
                    break;
                case Weather.Season.winter:
                    SetLeaves(renderer, treeData.winterLeaves);
                    break;
                default:
                    break;
            }
        }

        private void SetLeaves(MeshRenderer _renderer, Material[] _leavesMats)
        {
            int _index;

            if (_leavesMats.Length == 0)
            {
                _renderer.GetComponent<MeshFilter>().sharedMesh = treeData.deadMeshes[meshIndex].GetComponent<MeshFilter>().sharedMesh;
                _renderer.sharedMaterials = new Material[] { treeData.barkMaterials[barkIndex] };
            }
            else
            {
                _renderer.GetComponent<MeshFilter>().sharedMesh = treeData.meshes[meshIndex].GetComponent<MeshFilter>().sharedMesh;

                _index = (leafIndex >= _leavesMats.Length) ? 0 : leafIndex;

                _renderer.sharedMaterials = new Material[] { treeData.barkMaterials[barkIndex], _leavesMats[_index] };
            }
        }

        public static GameObject Create(GameObject _prefab, TreeData _treeData, Vector3 _position, Transform _parent = null)
        {
            GameObject treeGameObject = Instantiate(_prefab, _position, Quaternion.identity, _parent);
            treeGameObject.name = _treeData.name;
            Tree tree = treeGameObject.GetComponent<Tree>();

            tree.treeData = _treeData;

            tree.meshIndex = Random.Range(0, _treeData.meshes.Length);
            GameObject treeGFX = Instantiate(_treeData.meshes[tree.meshIndex], tree.GFX.transform);

            tree.barkIndex = Random.Range(0, _treeData.barkMaterials.Length);
            tree.leafIndex = Random.Range(0, _treeData.fallLeaves.Length);

            Material[] _mats = treeGFX.GetComponentInChildren<MeshRenderer>().sharedMaterials;

            // index 0 hardcorded for bark
            _mats[0] = _treeData.barkMaterials[tree.barkIndex];
            int springIndex = (_treeData.fallLeaves.Length > _treeData.springLeaves.Length) ? 0 : tree.leafIndex;
            // index 1 hardcorded for leaves
            _mats[1] = _treeData.springLeaves[springIndex];

            tree.isDead = false;

            return treeGameObject;
        }
    }
}
