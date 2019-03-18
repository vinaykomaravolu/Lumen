using System;

public class Score : IComparable<Score>{

    public int score;
    public string name;
    public float time;

    public int CompareTo(Score other){
        if (other.score < score) return 1;
        if (other.score > score) return -1;
        return 0;
    }

    public string getTimeString(){
        return (int)time / 60 + ":" + time % 60;
    }
}