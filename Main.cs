using Godot;
using System;

public class Main : Node
{

    [Export]
    private PackedScene mobScene = (PackedScene)ResourceLoader.Load("res://mob.tscn");
    [Export]
    private float timout = 0.7f;
    [Export]
    private float timoutDecrease = 0.006f;
    [Export]
    private float maxTimout = 0.05f;

    public override void _Ready()
    {
        GD.Randomize();

    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept") && GetNode<ColorRect>("UI/Retry").Visible)
        {
            GetTree().ReloadCurrentScene();
        }
    }
    public void _on_MobTimer_timeout()
    {
        if (timout > maxTimout)
        {
            GD.Print(timout);
            timout -= timoutDecrease;
            GetNode<Timer>("MobTimer").WaitTime = timout;
        }


        var mob = (Mob)mobScene.Instance();
        PathFollow mobSpawnLocation = GetNode<PathFollow>("SpawnPath/SpawnLocation");
        mobSpawnLocation.UnitOffset = (float)new Random().NextDouble();

        Vector3 playerPos = GetNode<KinematicBody>("Player").Transform.origin;

        AddChild(mob);
        mob.Initialize(mobSpawnLocation.Translation, playerPos);

        mob.Connect("Squashed", GetNode<Label>("UI/ScoreLabel"), "_on_Mob_squashed");

    }

    public void _on_Player_Hit()
    {
        GetNode<Timer>("MobTimer").Stop();
        GetNode<ColorRect>("UI/Retry").Show();
    }
}
