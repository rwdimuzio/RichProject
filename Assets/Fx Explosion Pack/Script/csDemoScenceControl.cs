using System;
using UnityEngine;
using System.Collections;

public class csDemoScenceControl : MonoBehaviour {

    public GameObject[] AllEffect;
    int i;
    public GUIText Text;
    public Transform mg;
    GameObject MakedObject;

    void Start()
    {
        i = 1;
        MakedObject = Instantiate(AllEffect[i - 1], AllEffect[i - 1].transform.position, Quaternion.identity) as GameObject;
        Text.text = "("+(i)+"/"+AllEffect.Length+") "+AllEffect[i-1].name;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if ((i - 1) <= AllEffect.Length-2)
                i++;
            else
                i = 1;
            Destroy(MakedObject);
            MakedObject = Instantiate(AllEffect[i - 1], AllEffect[i - 1].transform.position, AllEffect[i-1].transform.rotation) as GameObject;
            Text.text = "(" + i + "/" + AllEffect.Length + ") " + AllEffect[i-1].name;

        }

        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if ((i - 1) > 0)
                i--;
            else
                i = AllEffect.Length;
            Destroy(MakedObject);
            MakedObject = Instantiate(AllEffect[i - 1], AllEffect[i - 1].transform.position, AllEffect[i - 1].transform.rotation) as GameObject;
            Text.text = "(" + i + "/" + AllEffect.Length + ") " + AllEffect[i-1].name;

        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            Destroy(MakedObject);
            MakedObject = Instantiate(AllEffect[i - 1], AllEffect[i - 1].transform.position, AllEffect[i - 1].transform.rotation) as GameObject;
            Text.text = "(" + i + "/" + AllEffect.Length + ") " + AllEffect[i-1].name;
            for(int a=0; a<5; a++){
                Vector3 pos = new Vector3(
                    AllEffect[i - 1].transform.position.x + UnityEngine.Random.Range(-5,5),
                    AllEffect[i - 1].transform.position.y,
                    AllEffect[i - 1].transform.position.z + UnityEngine.Random.Range(-5,5)
                );
                MakedObject = Instantiate(AllEffect[i - 1], pos, 
                AllEffect[i - 1].transform.rotation) 
                as GameObject;
                UnityEngine.Object.Destroy(MakedObject,1.5f);
            }
        }
    }

}