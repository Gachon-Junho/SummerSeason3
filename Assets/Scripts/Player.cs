using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 5;
    public bool Movable;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        
        if (Movable)
            gameObject.transform.Translate(horizontal * Time.deltaTime * Speed, vertical * Time.deltaTime * Speed, 0);
    }
}
