using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    private const int NUM_LIVES = 10;
    private const int FUEL = 100;
    private const int STRENGTH = 100;
    public enum PlayState
    {
        WAITING_TO_START,
        PLAYING
    };

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
    }

    public void start()
    {
        state = PlayState.PLAYING;
        lives = NUM_LIVES; // how many respawns left
        fuel = FUEL; // how many shots are left
        strength = STRENGTH; // how many hits we can survive
    }

    public bool shoot(int unitsPerShot){
        fuel -= unitsPerShot;
        if(fuel < 0){
            fuel = 0;
        }
        return fuel > 0 ;

    }

    public bool takeHit(int unitsPerHit){
        strength -= unitsPerHit;
        if(strength < 0){
            strength = 0;
        }
        return strength > 0 ;
    }
}
