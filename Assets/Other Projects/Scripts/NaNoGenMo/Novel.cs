using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Novel : MonoBehaviour
{
    public Text title;
    public Text display;
    private Grammar novel;
    public Rule first;

    // Start is called before the first frame update
    void Start()
    {
        novel = new Grammar();
        novel.rule = first;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GenerateNovel()
    {
        display.text = novel.Evaluate();
    }
}
