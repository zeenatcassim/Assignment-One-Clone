using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringDisplay : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void showScore()
    {
        GameObject newCanvas = Instantiate(canvas);
        newCanvas.transform.position = GameObject.FindWithTag("Player").GetComponent<Transform>().position;
        StartCoroutine(deleteDisplay(newCanvas));
    }

    IEnumerator deleteDisplay(GameObject canvas)
    {
        yield return new WaitForSeconds(2f);
        Destroy(canvas);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
