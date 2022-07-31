using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Security;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public GameObject gameOverObject;

    public GameObject scoreObject;

    public GameObject livesObject;

    public GameObject playerPrefab;

    public GameObject explosionObject;

    public AudioSource shootSound;

    public AudioSource boomSound;

    public AudioSource levelUpSound;

    private const int NUM_LIVES = 3;

    private const int FUEL = 100;

    private const int STRENGTH = 100;

    public enum PlayState
    {
        WAITING_TO_START,
        PLAYING
    }

    private ScoreKeeper scoreKeeper;

    private int score = 0;

    public PlayState state = PlayState.WAITING_TO_START;

    public int lives = NUM_LIVES; // how many re-spawns left

    public int fuel = FUEL; // how many shots are left

    public int strength = STRENGTH; // how many hits we can survive

    private int strikeCounter = 0; // how many times we connected with a hit
    private int level = 1;

    private int spaceBar = 0;
    private int kills = 0;
    private int enemyBar = 0;
    private int enemySlipped = 0;
    // Because of using RuntimeInitializeOnLoadMethod attribute to find/create and
    // initialize the instance, this property is accessible and
    // usable even in Awake() methods.
    private SpawnManager spawnManager;

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
        scoreKeeper.setScore (score);
        scoreKeeper.setLives (lives);
        spawnManager = gameObject.GetComponent<SpawnManager>(); // sibling
    }

    public void StartGame()
    {
        Debug.Log("GameDirector.StartGame()");
        state = PlayState.PLAYING;
        lives = NUM_LIVES; // how many respawns left
        fuel = FUEL; // how many shots are left
        strength = STRENGTH; // how many hits we can survive
        score = 0;
        level = 1;
        spaceBar = 0;
        kills = 0;
        enemyBar = 0;
        enemySlipped = 0;

        scoreKeeper.setScore (score);
        scoreKeeper.setLives (lives);
        scoreKeeper.setShield(strength);
        scoreKeeper.setLevel(level);
        gameOverObject.active = false;
        spawnManager.startSpawning();
        doSpawnHero(0.0f);
    }

    public void EndGame()
    {
        gameOverObject.active = true;
        spawnManager.stopSpawning();
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

    public void setShield(float shield){
        scoreKeeper.setShield((int) shield);
    }

    public bool takeHit(int unitsPerHit)
    {
        strength -= unitsPerHit;
        if (strength < 0)
        {
            strength = 0;
        }
        scoreKeeper.setShield(strength);

        return strength > 0;
    }

    public GameObject getExplosion()
    {
        return explosionObject;
    }

    public void doSpawnHero(float time)
    {
        StartCoroutine(spawnHero(time, playerPrefab));
    }

    public IEnumerator spawnHero(float respawnTime, GameObject playerPrefab)
    {
        strikeCounter = 0;
        Debug.Log("The wait begins waiting " + respawnTime);
        yield return new WaitForSeconds(respawnTime);
        strength = STRENGTH;
        scoreKeeper.setShield(strength);
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

    public void playLevelUpSound()
    {
        levelUpSound.Play();
    }

    public void addPoints(int amt)
    {
        bumpStrikeCounter();
        int prev = score;
        score += amt;
        Debug.Log("addPoints score:  " + score);
        scoreKeeper.setScore (score);
        if ( 
            (prev < 100000 && crossBoundary(prev/100000, score/100000)) || // first 100,000 points first free 
            (prev >=100000 && crossBoundary(prev/1000000, score/1000000))  // and at every 10,000,000 after that 
        )
        {
            int prevL = lives;
            addLives(1);
            if(prevL != lives){
                playLevelUpSound();
            }
        }
    }

/**
control how often they get power ups
*/
    private void bumpStrikeCounter()
    {
        int prev = strikeCounter;
        strikeCounter++;
        if ( // prev < 1000 || // lots of levels ups
            (prev < 25 && crossBoundary(prev/25, strikeCounter/25)) || 
            (prev >= 25 && crossBoundary(prev/100, strikeCounter/100))
            ){
            level++;
            scoreKeeper.setLevel(level);
            spawnManager.levelUp(level);

        }
    }

    /**
    decide whether they's hit enough enemies to improve your gun configuration
    */
    private bool crossBoundary(int prev, int next){
        return prev != next;
    }

    public bool addLives(int amt)
    {
        lives += amt;
        if (lives < 0) lives = 0;
        if (lives > 10) lives = 10;
        Debug.Log("addLives lives:  " + lives);
        scoreKeeper.setLives (lives);

        return lives > 0;
    }

    public void pressSpaceBar(int spaces){
//        spaceBar += spaces;
//        updateHitRatio();
    }

    public void addKill(int kills){
        this.kills += kills;
        updateHitRatio();
    }
    public void enemySpaceBar(int spaces){
        enemyBar += spaces;
        updateHitRatio();
    }

    public void enemySlippedBy(int enemies){
        enemySlipped += enemies;
        updateHitRatio();
//        enemyBar += enemies;
//        updateHitRatio();
    }
    public void updateHitRatio(){
        float ratio = (enemySlipped == 0 ) ? 0 : kills *100/enemySlipped;
        scoreKeeper.setHitRatio(ratio);
    }
}
