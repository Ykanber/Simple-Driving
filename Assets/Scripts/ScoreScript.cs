using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreScript : MonoBehaviour
{
    [SerializeField] TMP_Text _scoreText;

    private float score = 0f;
    private float scoreMultiplier = 1;

    public const string HighScoreKey = "HighScore";

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime * scoreMultiplier;

        _scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    private void OnDestroy()
    {
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, Mathf.FloorToInt(score));
        }
    }

}
