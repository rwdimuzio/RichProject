using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Authentication;
using UnityEngine;

// INHERITANCE
// child
public class GunshotMoveForward : MoveForward
{
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ufo"))
        {
            GameDirector.Instance.addPoints (points);
            MoveForward enemy = other.gameObject.GetComponent<EnemyMoveForward>();
            // ENCAPSULATION
            if (enemy.takeHit(punch() ) )
            {
                GameDirector.Instance.addKill (1);
                enemy.fieryDeath();
            }
        }
        fieryDeath();
    }

// POLYMORPHISM
    public override void fieryDeath()
    {
        Destroy(gameObject);
    }
}
