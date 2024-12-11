using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    private float score = 0f;

    void Update()
    {
        score += Time.deltaTime; 
        scoreText.text = Mathf.FloorToInt(score).ToString(); 
    }
}
