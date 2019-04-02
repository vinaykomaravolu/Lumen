using System;
using System.Collections.Generic;
using UnityEngine;

public static class LeaderBoard{

    public const int maxSize = 10;
    private static List<Score> scores;
    private const string namePrefix = "top-name-";
    private const string scorePrefix = "top-score-";
    private const string timePrefix = "top-time-";

    // only need to call this once
    private static void load(){
        scores = new List<Score>();
        for (int i = 0; i < maxSize; i++){
            if (!PlayerPrefs.HasKey(namePrefix + i)) break;
            Score score = new Score{
                name = PlayerPrefs.GetString(namePrefix + i),
                score = PlayerPrefs.GetInt(scorePrefix + i),
                time = PlayerPrefs.GetFloat(timePrefix + i)
            };
            add(score);
        }
    }

    private static void save()
    {
        for (int i = 0; i < maxSize; i++)
        {
            if (i >= scores.Count) break;
            Score score = scores[i];
            PlayerPrefs.SetString(namePrefix + i, score.name);
            PlayerPrefs.SetInt(scorePrefix + i, score.score);
            PlayerPrefs.SetFloat(timePrefix + i, score.time);
        }
        PlayerPrefs.Save();
    }

    public static void add(Score score)
    {
        if (scores is null) load();
        scores.Add(score);
        scores.Sort();
        save();
    }

    public static List<Score> get()
    {
        load();
        return scores;
    }

    public static void reset()
    {
        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.DeleteKey(namePrefix + i);
            PlayerPrefs.DeleteKey(scorePrefix + i);
            PlayerPrefs.DeleteKey(timePrefix + i);
        }
        load();
    }
}