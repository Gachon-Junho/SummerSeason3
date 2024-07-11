using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [SerializeField] 
    private MazeGenerator mazeGenerator;

    [SerializeField] 
    private GameObject player;
    
    [SerializeField] 
    private GameObject destination;
    
    // Start is called before the first frame update
    void Start()
    {
        mazeGenerator.Build(51, 51);
        Instantiate(player, new Vector3(mazeGenerator.TopLeft.x, mazeGenerator.TopLeft.y, 0), Quaternion.identity);
        Instantiate(destination, new Vector3(mazeGenerator.BottomRight.x, mazeGenerator.BottomRight.y, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
