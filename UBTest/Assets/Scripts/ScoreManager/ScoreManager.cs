using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public bool timed = false;

	public void AddPoints(int points)
    {
        SumScore.Add(points);
    }

    public void SubtractPoints(int points)
    {
        SumScore.Add(-points);
    }

    public void ToggleTimed()
    {
        timed = !timed;
    }

    public void ResetPoints()
    {
        SumScore.Reset();
    }

    public void CheckHighScore()
    {
        if (SumScore.Score > SumScore.HighScore)
            SumScore.SaveHighScore();
    }

    public void ClearHighScore()
    {
        SumScore.ClearHighScore();
    }

    void Update()
    {
        if (timed)
            // Use Time.deltaTime to create a steady addition of points.
            // This example would add 100 points per second
            SumScore.Add(Mathf.RoundToInt(Time.deltaTime * 1));


        //if (Input.GetKeyDown(KeyCode.P))
        //    AddPoints(1);
    }
}
