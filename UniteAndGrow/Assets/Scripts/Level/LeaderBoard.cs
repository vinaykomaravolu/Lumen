using System.Collections.Generic;
using UnityEngine;

public static class LeaderBoard{

    public const int maxSize = 10;
    private static List<Score> scores;
    private const string namePrefix = "top-name-";
    private const string scorePreFix = "top-score-";
    private const string timePreFix = "top-time-";

    // only need to call this once
    public static void load(){
        scores = new List<Score>();
        for (int i = 0; i < maxSize; i++){
            if (!PlayerPrefs.HasKey(namePrefix + i)) break;
            Score score = new Score{
                name = PlayerPrefs.GetString(namePrefix + i),
                score = PlayerPrefs.GetInt(scorePreFix + i),
                time = PlayerPrefs.GetFloat(timePreFix + i)
            };
            scores.Add(score);
        }
    }

    public static void add(Score score){
        scores.Add(score);
        scores.Sort();
        save();
    }

    public static List<Score> get(){
        return scores;
    }

    private static void save(){
        for (int i = 0; i < maxSize; i++){
            if (i >= scores.Count) break;
            Score score = scores[i];
            PlayerPrefs.SetString(namePrefix + i, score.name);
            PlayerPrefs.SetInt(scorePreFix + i, score.score);
            PlayerPrefs.SetFloat(timePreFix + i, score.time);
        }
        PlayerPrefs.Save();
    }

}