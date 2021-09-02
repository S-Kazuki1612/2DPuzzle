using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalScore : MonoBehaviour
{
    public Text ScoreText;
    float score;

    // Start is called before the first frame update
    void Start()
    {
        score = ScoreManager.getscore();

        ScoreText.text = string.Format("結果:{0}", score);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
