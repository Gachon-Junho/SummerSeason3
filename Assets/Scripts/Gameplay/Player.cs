using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public readonly float InitialSpeed = 5;
    
    public float CurrentSpeed;
    public bool Movable;
    
    // Start is called before the first frame update
    void Start()
    {
        CurrentSpeed = InitialSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        
        if (Movable)
            gameObject.transform.Translate(horizontal * Time.deltaTime * CurrentSpeed, vertical * Time.deltaTime * CurrentSpeed, 0);
    }
}
