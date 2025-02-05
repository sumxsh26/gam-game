using UnityEngine;
using UnityEngine.UI;

public class TimerManagerScript : MonoBehaviour
{
    public float timeToRestart;
    public float timer;
    public Text timerText;
    public Slider multipleSlider;

    // Start
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * multipleSlider.value;
        if (timer > timeToRestart)
        {
            timer = 0;
            Debug.Log("Timer has restarted");
        }

        timerText.text = Mathf.Floor(timer).ToString();
    }
}