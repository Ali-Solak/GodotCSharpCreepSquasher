using Godot;
using System;

public class ScoreLabel : Label
{

    private int score = 0;

    public void _on_Mob_squashed()
    {
        score++;
        Text = "Score: " + score;
    }
}
