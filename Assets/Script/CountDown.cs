using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public static float CountDownTime;
    public Text TextCountDown;
    // Start is called before the first frame update
    void Start()
    {
        CountDownTime = 30.0f;
    }

    // Update is called once per frame
    void Update()
    {
        TextCountDown.text = string.Format("{0:00.0}", CountDownTime);
        CountDownTime -= Time.deltaTime;
    }
}
