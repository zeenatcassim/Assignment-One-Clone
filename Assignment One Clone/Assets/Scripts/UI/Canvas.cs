using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Canvas : MonoBehaviour
{
    [SerializeField] float step = 1f;
    Vector2 target;
    float x, y;
    // Start is called before the first frame update
    void Start()
    {
        x = transform.position.x;
        y = transform.position.y + 3f;
        target = new Vector2(x, y);
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, step * Time.deltaTime);
    }
}
