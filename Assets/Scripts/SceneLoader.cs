using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName) => this.LoadSceneAsync(sceneName);
}
