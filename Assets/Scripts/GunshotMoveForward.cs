using System.Security.Authentication;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class GunshotMoveForward : MoveForward
{
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ufo"))
        {
            GameDirector.Instance.addPoints (points);
            MoveForward enemy = other.gameObject.GetComponent<EnemyMoveForward>();
            enemy.hits -= hits;
            if(enemy.hits <= 0){
                enemy.fieryDeath();
            }
        }
        Destroy (gameObject);
    }

}
