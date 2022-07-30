using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreKeeper : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI shieldText;
    public TextMeshProUGUI hitRatioText;

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
        score =  (score <= 999999999) ? score : 999999999 ;
        scoreText.text = "Score: "+(score*25);
    }
    public void setLevel(int level){
        levelText.text = "Level: "+level;
    }
    public void setLives(int lives){
        lives = (lives < 0) ? 0 : lives;
        livesText.text = "Lives: "+ lives;
    }
    public void setShield(int shield){
        shield = (shield < 0) ? 0 : shield;
        shieldText.text = "Shield: "+ shield;
    }
    public void setHitRatio(float ratio){
        hitRatioText.text = ""; //"Hit Ratio: "+ ratio;
    }
}
