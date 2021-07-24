using Godot;
using System;
using System.Linq;

public class Player : KinematicBody
{

    [Export]
    private float speed = 14.0f;
    [Export]
    private float jumpImpulse = 20.0f;
    [Export]
    private float gravity = 75.0f;
    [Export]
    private Vector3 velocity = new Vector3();
    [Export]
    private float bounceImpulse = 16.0f;

    [Signal]
    public delegate void Hit();
    public override void _Ready()
    {

    }

    public override void _PhysicsProcess(float delta)
    {
        Vector3 direction = new Vector3();
        if (Input.IsActionPressed("move_right"))
        {
            direction.x += 1;
        }
        if (Input.IsActionPressed("move_left"))
        {
            direction.x -= 1;
        }
        if (Input.IsActionPressed("move_back"))
        {
            direction.z += 1;
        }
        if (Input.IsActionPressed("move_forward"))
        {
            direction.z -= 1;
        }
        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            GetNode<Spatial>("Pivot").LookAt(Translation + direction, Vector3.Up);
            GetNode<AnimationPlayer>("AnimationPlayer").PlaybackSpeed = 2.0f;
        }
        else
        {
            GetNode<AnimationPlayer>("AnimationPlayer").PlaybackSpeed = 0.9f;

        }
        velocity.x = direction.x * speed;
        velocity.z = direction.z * speed;

        if (IsOnFloor() && Input.IsActionPressed("jump"))
        {
            velocity.y += jumpImpulse;
        }

        velocity.y -= gravity * delta;
        velocity = MoveAndSlide(velocity, Vector3.Up);

        foreach (int index in Enumerable.Range(0, GetSlideCount()))
        {
            KinematicCollision collision = (KinematicCollision)GetSlideCollision(index);
            Node collidedObject = (Node)collision.Collider;
            if (collidedObject.IsInGroup("mob"))
            {
                var mob = (Mob)collision.Collider;
                if (Vector3.Up.Dot(collision.Normal) > 0.1)
                {
                    mob.squash();
                    velocity.y = bounceImpulse;
                }
            }
        }
         GetNode<Spatial>("Pivot").RotateX((float)(Math.PI / 6.0f * velocity.y / jumpImpulse));


    }
    public void _on_MobDetector_body_entered(Node body)
    {
        die();
    }

    public void die()
    {
        EmitSignal(nameof(Hit));
        QueueFree();
    }

}
