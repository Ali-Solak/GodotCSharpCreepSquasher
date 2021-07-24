using Godot;
using System;

public class Mob : KinematicBody
{
    [Export]
    private float min_speed = 10.0f;
    [Export]
    private float max_speed = 18.0f;

    Vector3 velocity = Vector3.Zero;

    [Signal]
    public delegate void Squashed();

    public override void _PhysicsProcess(float _delta)
    {
        MoveAndSlide(velocity, Vector3.Up);
    }

    public void Initialize(Vector3 startPosition, Vector3 playerPosition)
    {
        Translation = startPosition;
        LookAt(playerPosition, Vector3.Up);

        double random = new Random().NextDouble() * (Math.PI / 4 - Math.PI / 4) + -Math.PI / 4;
        
        RotateY((float)random);

        float randomSpeed = (float)new Random().NextDouble() * max_speed - min_speed + min_speed;
        velocity = Vector3.Forward * randomSpeed;
        velocity = velocity.Rotated(Vector3.Up, Rotation.y);

        GetNode<AnimationPlayer>("AnimationPlayer").PlaybackSpeed = randomSpeed / min_speed;
    }
    public void _on_VisibilityNotifier_screen_exited()
    {
        QueueFree();
    }

    public void squash()
    {
        EmitSignal(nameof(Squashed));
        QueueFree();
    }

}
