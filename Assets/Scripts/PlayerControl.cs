using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Xml.Schema;
using UnityEngine;

//using MoveForward.UnitType;
public class PlayerControl : MonoBehaviour
{
    public GameObject[] pews;

    public GameObject playerPrefab;
    
    private float minX = -22.5f;

    private float maxX = 23f;

    private float minZ = -5f;

    private float maxZ = 11f;
    private int power = 1;
    private float hits = 10.0f;

    Rigidbody playerRb;

    //private float thrusterSpeed=1f;
    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        // use rigid body kinematics
        //playerRb.AddForce(Vector3.forward * thrusterSpeed * vertInput);
        //playerRb.AddForce(Vector3.right * thrusterSpeed * horizInput);
        horizInput = 0;
        vertInput = 0;
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > minX)
        {
            horizInput = -speed;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < maxX
        )
        {
            horizInput = speed;
        }
        else if (Input.GetKey(KeyCode.UpArrow) && transform.position.z < maxZ)
        {
            vertInput = speed;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && transform.position.z > minZ)
        {
            vertInput = -speed;
        }

        // update if we have gone too far, pull us back in
        if (transform.position.x < minX) horizInput += speed;
        if (transform.position.x > maxX) horizInput -= speed;

        if (transform.position.z > maxZ) vertInput -= speed;
        if (transform.position.z < minZ) vertInput += speed;

        transform.position =
            new Vector3(transform.position.x + horizInput,
                transform.position.y,
                transform.position.z + vertInput);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameDirector.Instance.playShootSound();

            int cost = fire();
            GameDirector.Instance.addFuel(-1 * cost);
        }
    }

    /**
spawn a number of pew object based on the level
return the cost of doing business.
*/
    private int fire()
    {
        switch (power)
        {
            case 1:
                spawnShot(pews[0], 0.55f, 0.0f);
                return 1;
                break;
            case 2:
                spawnShot(pews[0], 0.30f, 0.0f);
                spawnShot(pews[0], 0.80f, 0.0f);
                return 2;
                break;
            case 3:
                spawnShot(pews[1], 0f, 0.0f);
                return 3;
                break;
            case 4:
                spawnShot(pews[1], -0.75f, 0.0f);
                spawnShot(pews[1], +0.75f, 0.0f);
                return 4;
                break;
            case 5:
                spawnShot(pews[1], -1.25f, 0.5f);
                spawnShot(pews[1], 0f, 1.0f);
                spawnShot(pews[1], 1.25f, 0.5f);
                return 8;
                break;
            case 6:
                spawnShot(pews[1], -2.00f, 0.0f);
                spawnShot(pews[1], +0.75f, 0.0f);
                spawnShot(pews[1], 0f, 1.0f);
                spawnShot(pews[1], -0.75f, 0.0f);
                spawnShot(pews[1], +2.00f, 0.0f);
                return 12;
                break;
            case 7:
                spawnShot(pews[1], -2.50f, 0.0f,-5);
                spawnShot(pews[1], -1.25f, 0.0f);
                spawnShot(pews[1], -0.0f, 2.0f);
                spawnShot(pews[1], 1.25f, 0.0f);
                spawnShot(pews[1], 2.50f, 0.0f,5);
                return 16;
            default:
                spawnShot(pews[1], -3.5f, 0.0f,-45);
                spawnShot(pews[1], -2.50f, 1.0f,-30);
                spawnShot(pews[1], -1.25f, 0.0f);
                spawnShot(pews[1], -0.0f, 2.0f);
                spawnShot(pews[1], 1.25f, 0.0f);
                spawnShot(pews[1], 2.50f, 1.0f,30);
                spawnShot(pews[1], 3.5f, 0.0f,45);
                return 16;
                break;
        }
    }

    private void spawnShot(GameObject pewPrefab, float dx, float dy, int skew=0)
    {
        Instantiate(pewPrefab,
            new Vector3(
                gameObject.transform.position.x + dx,
                gameObject.transform.position.y,
                gameObject.transform.position.z + 1.5f + dy
            ),
            //pewPrefab.transform.rotation
            Quaternion.Euler(0, skew, 180) 

        );
    }

/*
something just hit me - how will that affect
*/
    private void OnTriggerEnter(Collider other)
    {
        MoveForward m = other.gameObject.GetComponent<MoveForward>();
        if (m == null) return;

        // kill me now Destroy(gameObject);
        switch (m.unitType)
        {
            case MoveForward.UnitType.EasyUfo:
                //Debug.Log("Hit Ufo1");
                takeHit(m.punch());
                spawnExplosion(m.gameObject);
                Destroy(m.gameObject);
                break;
            case MoveForward.UnitType.MediumUfo:
                //Debug.Log("Hit Ufo2");
                takeHit(m.punch());
                spawnExplosion(m.gameObject);
                Destroy(m.gameObject);
                break;
            case MoveForward.UnitType.HardUfo:
                //Debug.Log("Hit Ufo3");
                takeHit(m.punch());
                spawnExplosion(m.gameObject);
                Destroy(m.gameObject);
                break;
            case MoveForward.UnitType.PowerupGun:
                //Debug.Log("Hit Powerup1");
                Destroy(m.gameObject);
                power++;
                break;
            case MoveForward.UnitType.PowerupLife:
                //Debug.Log("Hit Powerup 2");
                Destroy(m.gameObject);
                power++;
                break;
            case MoveForward.UnitType.LowPowerShot:
                //Debug.Log("Hit Pew1");
                Destroy(m.gameObject);
                break;
            case MoveForward.UnitType.HighPowerShot:
                //Debug.Log("Hit Pew2");
                Destroy(m.gameObject);
                break;
            default:
                Debug.Log("Trigger Collide " + m.unitType.ToString());
                break;
        }
    }

    void takeHit(float punch)
    {
        hits -= punch;
        if (hits <= 0)
        {
            GameDirector.Instance.playBoomSound();
            OnHeroDied();
        } 
    }

    void OnHeroDied()
    {
        Debug.Log("Hero Died");
        bool stillPlaying = GameDirector.Instance.addLives(-1);
        if(stillPlaying){ 
            GameDirector.Instance.doSpawnHero(2f);
        } else {
            OnGameOver();
        }
        Destroy (gameObject);
    }

    void OnGameOver()
    {
        Debug.Log("Game Over!");
        GameDirector.Instance.EndGame();
    }

    private void spawnExplosion(GameObject src)
    {
        Vector3 pos =
            new Vector3(src.transform.position.x,
                src.transform.position.y,
                src.transform.position.z);

        GameObject g = Instantiate(GameDirector.Instance.getExplosion(), pos, Quaternion.identity);
        MoveForward base_c = src.gameObject.GetComponent<MoveForward>();
        MoveForward g_c = g.GetComponent<MoveForward>();
        g_c.speed = -1 * base_c.speed; // they are coming at us, so flip the speed
        UnityEngine.Object.Destroy(g, 1.5f);
    }

    private void spawnPlayerDeath(GameObject src)
    {
        Vector3 pos =
            new Vector3(src.transform.position.x,
                src.transform.position.y,
                src.transform.position.z);

        GameObject g = Instantiate(GameDirector.Instance.getExplosion(), pos, Quaternion.identity);
        UnityEngine.Object.Destroy(g, 1.5f);
    }

}
