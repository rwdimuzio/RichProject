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
    public AudioSource shoot;

    public AudioSource boom;

    public GameObject pewPrefab;

    public GameObject boomPrefab;

    public GameObject[] pews;

    public GameObject playerPrefab;

    private float hits = 1;

    private float minX = -22.5f;

    private float maxX = 23f;

    private float minZ = -5f;

    private float maxZ = 11f;

    private int power = 1;

    bool gameOver = false;

    Rigidbody playerRb;

    //private float thrusterSpeed=1f;
    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        power = 1;
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

            //shoot.Play();
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
                spawnShot(pews[0], 0, 0);
                return 1;
                break;
            case 2:
                spawnShot(pews[1], 0, 0);
                return 2;
                break;
            case 3:
                spawnShot(pews[1], -0.75f, 0.0f);
                spawnShot(pews[1], +0.75f, 0.0f);
                return 4;
                break;
            case 4:
                spawnShot(pews[1], -0.0f, 0.5f);
                spawnShot(pews[1], -1.25f, 0.0f);
                spawnShot(pews[1], 1.25f, 0.0f);
                return 8;
                break;
            case 5:
                spawnShot(pews[1], -1.50f, 0.0f);
                spawnShot(pews[1], +0.75f, 0.0f);
                spawnShot(pews[1], -0.75f, 0.0f);
                spawnShot(pews[1], +1.50f, 0.0f);
                return 12;
                break;
            default:
                spawnShot(pews[1], -2.00f, 0.0f);
                spawnShot(pews[1], -1.25f, 0.0f);
                spawnShot(pews[1], -0.0f, 0.5f);
                spawnShot(pews[1], 1.25f, 0.0f);
                spawnShot(pews[1], 2.00f, 0.0f);
                return 16;
                break;
        }
    }

    private void spawnShot(GameObject pewPrefab, float dx, float dy)
    {
        Instantiate(pewPrefab,
        new Vector3(gameObject.transform.position.x + dx,
            gameObject.transform.position.y + dy,
            gameObject.transform.position.z + 2),
        //pewPrefab.transform.rotation
        Quaternion.Euler(0, 0, 180));
    }

    private void OnTriggerEnter(Collider other)
    {
        MoveForward m = other.gameObject.GetComponent<MoveForward>();
        if (m == null) return;

        // kill me now Destroy(gameObject);
        switch (m.unitType)
        {
            case MoveForward.UnitType.Ufo1:
                Debug.Log("Hit Ufo1");
                spawnExplosion(m.gameObject);
                Destroy(m.gameObject);
                addHits(-m.hits);
                break;
            case MoveForward.UnitType.Ufo2:
                Debug.Log("Hit Ufo2");
                spawnExplosion(m.gameObject);
                Destroy(m.gameObject);
                addHits(-m.hits);
                break;
            case MoveForward.UnitType.Ufo3:
                Debug.Log("Hit Ufo3");
                spawnExplosion(m.gameObject);
                Destroy(m.gameObject);
                addHits(-m.hits);
                break;
            case MoveForward.UnitType.Powerup1:
                Debug.Log("Hit Powerup1");
                Destroy(m.gameObject);
                addHits(m.hits);
                break;
            case MoveForward.UnitType.Powerup2:
                Debug.Log("Hit Powerup 2");
                Destroy(m.gameObject);
                power++;
                addHits(m.hits);
                break;
            case MoveForward.UnitType.Pew1:
                Debug.Log("Hit Pew1");
                Destroy(m.gameObject);
                addHits(m.hits);
                break;
            case MoveForward.UnitType.Pew2:
                Debug.Log("Hit Pew2");
                Destroy(m.gameObject);
                addHits(m.hits);
                break;
            default:
                Debug.Log("Trigger Collide " + m.unitType.ToString());
                break;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        MoveForward m = other.gameObject.GetComponent<MoveForward>();

        Debug.Log("Collide " + m.unitType.ToString());

        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Pew"))
        {
            Debug.Log("Game Pew" + m.hits);

            //            explosionParticle.Play();
            //            playerAudio.PlayOneShot(explodeSound, 1.0f);
            //            gameOver = true;
            //            Debug.Log("Game Over!");
            //            Destroy(other.gameObject);
        }
        // if(other.gameObject.CompareTag("Ufo")){
        //     spawnExplosion(gameObject);
        //     spawnExplosion(other.gameObject);

        // }
    }

    void addHits(float num)
    {
        float h = hits + num;
        if (h > 10) h = 10;
        if (h < 0) h = 0;
        hits = h;
        Debug.Log("Hits -> " + hits);
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
        gameOver = true;
    }

    private void spawnExplosion(GameObject src)
    {
        Vector3 pos =
            new Vector3(src.transform.position.x,
                src.transform.position.y,
                src.transform.position.z);

        // TODO add score
        GameObject g = Instantiate(boomPrefab, pos, Quaternion.identity);
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

        // TODO add score
        GameObject g = Instantiate(boomPrefab, pos, Quaternion.identity);
        UnityEngine.Object.Destroy(g, 1.5f);
    }

    IEnumerator awaitSpawnHero(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);
        Debug.Log("The wait is over!");
        spawnHero();
    }

    public void spawnHero()
    {
        Vector3 pos = new Vector3(0.06f, 1, -4.35f);
        Instantiate(playerPrefab, pos, playerPrefab.transform.rotation);
    }
}
