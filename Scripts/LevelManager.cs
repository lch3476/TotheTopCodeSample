using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private LevelManager instance;

    public LevelManager Instance { get { return instance; }} 
    
    void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    public void DelayedLoadScene(string _sceneName, float _delay)
    {
        StartCoroutine(WaitAndLoad(_sceneName, _delay));
    }

    IEnumerator WaitAndLoad(string _sceneName, float _delay)
    {
        yield return new WaitForSecondsRealtime(_delay);
        SceneManager.LoadScene(_sceneName);
    }
}
