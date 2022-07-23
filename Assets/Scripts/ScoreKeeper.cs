using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreKeeper : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
      //setScore(1000);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setScore(int score){
        scoreText.text = ""+score;
    }
}
