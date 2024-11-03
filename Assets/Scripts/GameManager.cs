using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject Water;
    public GameObject SunLight;
    public bool GameEnd;
    private int count;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        GameEnd = false;
    }

    public void CheckPlayerDead(float yValue)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player.transform.position.y<=yValue)
        {
            HandleReloadCurrentLevel();
        }
    }

    public void HandleActiveWater()
    {
        Water.SetActive(true);
    }

    public void HandleActiveSunLight()
    {
        SunLight.SetActive(true);
        count++;
        CheckLevel1Ending();
    }

    public void HandleActiveGem()
    {
        count++;
        CheckLevel1Ending();
    }

    private void CheckLevel1Ending()
    {
        if(count == 2)
        {
            GameEnd = true;
            print("game end");
            GameObject.FindGameObjectWithTag("GAME_END_DOOR").GetComponent<Animator>().SetBool("Open",true);
        }
    }

    public void HandleReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HandleLoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void HandleQuit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                CheckPlayerDead(-20);
                break;
            case 2:
            case 3:
                GameEnd = true;
                break;
            default:
                break;
        }

        if(GameEnd)
        {
            if(Vector3.Distance(GameObject.FindGameObjectWithTag("GAME_END_DOOR").transform.position, GameObject.FindGameObjectWithTag("Player").transform.position)<=1.5f)
            {
                HandleLoadNextLevel();
            }
        }
    }
}
