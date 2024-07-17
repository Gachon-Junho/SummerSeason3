using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [SerializeField] 
    private MazeGenerator mazeGenerator;
    
    [SerializeField] 
    private ItemGenerator itemGenerator;

    [SerializeField] 
    private GameObject playerPrefab;
    
    [SerializeField] 
    private GameObject destinationPrefab;

    [SerializeField] 
    private Camera mainCamera;

    [SerializeField] private TMP_Text time;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text ready;
    [SerializeField] private GameObject dimmedPanel;
    [SerializeField] private GameObject gameResultOverlay;
    [SerializeField] private AudioSource gameLoop;

    private GameObject player;
    private GameObject destination;
    private int score;
    
    private bool isPlaying;
    
    public float UsedTime;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            scoreText.text = $"{value:000000}";
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        gameLoop.volume = GameSettingsCache.MusicVolume;
        
        mazeGenerator.Build(GameSettingsCache.SizeX, GameSettingsCache.SizeY);
        itemGenerator.GenerateItems();
        
        player = Instantiate(playerPrefab, new Vector3(mazeGenerator.TopLeft.x, mazeGenerator.TopLeft.y, 0), Quaternion.identity);
        destination = Instantiate(destinationPrefab, new Vector3(mazeGenerator.BottomRight.x, mazeGenerator.BottomRight.y, 0), Quaternion.identity);

        time.text = GameSettingsCache.TimeAttack ? $"Remaining time: {GameSettingsCache.RemainingTime:F2}" : "Time: 0.00";
        
        StartCoroutine(showPreview());
    }

    IEnumerator showPreview()
    {
        mainCamera.transform.position = destination.transform.position;
        
        while (Vector3.Distance(mainCamera.transform.position, player.transform.position) > 10)
        {
            var forward = Vector3.MoveTowards(mainCamera.transform.position, player.transform.position, 0.02f);
            mainCamera.transform.position = new Vector3(forward.x, forward.y, -10);
            
            yield return null;
        }

        isPlaying = true;
        player.GetComponent<Player>().Movable = true;
        
        dimmedPanel.SetActive(false);
        ready.gameObject.SetActive(false);
        mainCamera.transform.SetParent(player.transform);
        gameLoop.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            UsedTime += Time.deltaTime;
            time.text = GameSettingsCache.TimeAttack
                ? $"Remaining time: {GameSettingsCache.RemainingTime - UsedTime:F2}"
                : $"Time: {UsedTime:F2}";

            if (GameSettingsCache.TimeAttack && GameSettingsCache.RemainingTime - UsedTime <= 0)
                GameOver(false);
        }
    }

    public void GameOver(bool success)
    {
        isPlaying = false;
        player.GetComponent<Player>().Movable = false;
        gameLoop.Stop();
        
        dimmedPanel.SetActive(true);
        gameResultOverlay.SetActive(true);

        if (success)
            GameSettingsCache.SubmitScore(score, UsedTime);
    }
}
