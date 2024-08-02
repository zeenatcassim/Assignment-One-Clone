using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoringDisplay : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void showScore(int Score)
    {
        GameObject newCanvas = Instantiate(canvas);
        newCanvas.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "+" + Score.ToString() + "pts";
        newCanvas.transform.position = GameObject.FindWithTag("Player").GetComponent<Transform>().position;
        StartCoroutine(deleteDisplay(newCanvas));
    }

    IEnumerator deleteDisplay(GameObject canvas)
    {
        yield return new WaitForSeconds(1f);
        Destroy(canvas);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
