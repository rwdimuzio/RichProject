using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Security;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public GameObject scoreObject;
    public GameObject livesObject;

    public GameObject playerPrefab;

    public AudioSource shootSound;

    public AudioSource boomSound;

    private const int NUM_LIVES = 10;

    private const int FUEL = 100;

    private const int STRENGTH = 100;

    public enum PlayState
    {
        WAITING_TO_START,
        PLAYING
    }

    private ScoreKeeper scoreKeeper;

    private int score = 0;

    private LivesKeeper livesKeeper;

    public PlayState state = PlayState.WAITING_TO_START;

    public int lives = NUM_LIVES; // how many respawns left

    public int fuel = FUEL; // how many shots are left

    public int strength = STRENGTH; // how many hits we can survive

    // Because of using RuntimeInitializeOnLoadMethod attribute to find/create and
    // initialize the instance, this property is accessible and
    // usable even in Awake() methods.
    public static GameDirector Instance { get; private set; }

    // Thanks to the attribute, this method is executed before any other MonoBehaviour
    // logic in the game.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnRuntimeMethodLoad()
    {
        var instance = FindObjectOfType<GameDirector>();

        if (instance == null)
            instance =
                new GameObject("Game Director").AddComponent<GameDirector>();

        DontDestroyOnLoad (instance);

        Instance = instance;
    }

    // This Awake() will be called immediately after AddComponent() execution
    // in the OnRuntimeMethodLoad(). In other words, before any other MonoBehaviour's
    // in the scene will begin to initialize.
    private void Awake()
    {
        // Initialize non-MonoBehaviour logic, etc.
        Debug.Log("GameDirector.Awake()", this);
        scoreKeeper = scoreObject.GetComponent<ScoreKeeper>();
        livesKeeper = livesObject.GetComponent<LivesKeeper>();
        scoreKeeper.setScore(score);
        livesKeeper.setLives(lives);
    }


    public void startGame()
    {
        state = PlayState.PLAYING;
        lives = NUM_LIVES; // how many respawns left
        fuel = FUEL; // how many shots are left
        strength = STRENGTH; // how many hits we can survive
        score =0;

        scoreKeeper.setScore(score);
        livesKeeper.setLives(lives);
    }

    public bool shoot(int unitsPerShot)
    {
        fuel -= unitsPerShot;
        if (fuel < 0)
        {
            fuel = 0;
        }
        return fuel > 0;
    }

    public bool takeHit(int unitsPerHit)
    {
        strength -= unitsPerHit;
        if (strength < 0)
        {
            strength = 0;
        }
        return strength > 0;
    }

    public void doSpawnHero(float time)
    {
        StartCoroutine(spawnHero(time, playerPrefab));
    }

    public IEnumerator spawnHero(float respawnTime, GameObject playerPrefab)
    {
        Debug.Log("The wait begins waiting " + respawnTime);
        yield return new WaitForSeconds(respawnTime);
        Debug.Log("The wait is over!");
        Vector3 pos = new Vector3(0.06f, 1, -4.35f);
        Instantiate(playerPrefab, pos, playerPrefab.transform.rotation);
    }

    public float addFuel(int amt)
    {
        //            powerMeter.addFuel((float)amt);
        //            return powerMeter.getFuel();
        return 0;
    }

    public void playShootSound()
    {
        shootSound.Play();
    }

    public void playBoomSound()
    {
        boomSound.Play();
    }

    public void addPoints(int amt)
    {
        score += amt;
        Debug.Log("addPoints score:  " + score);
        scoreKeeper.setScore (score);
    }

    public bool  addLives(int amt)
    {
        lives += amt;
        if(lives < 0) lives = 0;
        if(lives > 99) lives = 99;
        Debug.Log("addLives lives:  " + lives);
        livesKeeper.setLives (lives);

        return lives > 0;
    }

}
