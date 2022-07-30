using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
// child
public class EnemyMoveForward : MoveForward
{
// ABSTRACTION
// other details like points and speed have been hidden 

    public override void ObjectExitsArena(){
        GameDirector.Instance.enemySlippedBy(100);
    }
// POLYMORPHISM
    public override void fieryDeath(){
                Debug.Log("Fiery Death For Me");

        Vector3 pos =
            new Vector3(transform.position.x,
                transform.position.y,
                transform.position.z);

        GameObject g =
            Instantiate(GameDirector.Instance.getExplosion(),
            pos,
            Quaternion.identity);

        MoveForward g_c = g.GetComponent<MoveForward>();

        g_c.speed = -1 * speed; // they are coming at us, so flip the speed

        UnityEngine.Object.Destroy(gameObject, 0.10f);
    }

}
