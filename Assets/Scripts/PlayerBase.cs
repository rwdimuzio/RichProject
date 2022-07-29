using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public int _punch = 5; // how hard I hit
    public int _hits = 5; // how many hits I can take
    private int _hitsUsed;

    public virtual bool takeHit(int num){
        _hitsUsed += num;

        return (_hits - _hitsUsed) <= 0;  // died
    }

    public virtual int punch(){
        return _punch;
    }
}
