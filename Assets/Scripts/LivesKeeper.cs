using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LivesKeeper : MonoBehaviour
{
    public TextMeshProUGUI livesText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setLives(int lives){
        livesText.text = ""+lives;
    }
}
