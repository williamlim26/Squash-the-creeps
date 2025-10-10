using Godot;
using System;

public partial class Mob : CharacterBody3D
{
    // Minimum speed of the mob in meters per second
    [Export]
    public int MinSpeed { get; set; } = 10;
    // Maximum speed of the mob in meters per second
    [Export]
    public int MaxSpeed { get; set; } = 10;

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    // Emitted when the player jumped on the mob.
    [Signal]
    public delegate void SquashedEventHandler();


    // This function will be called from the main scene
    public void Initialize(Vector3 startPosition, Vector3 playerPosition)
    {
        // We position the mob by placing it at the startPosition
        // and rotate it towards playerPosition, so it looks at the player
        LookAtFromPosition(startPosition, playerPosition, Vector3.Up);
        // Rotate this mob randomly within range of -45 and +45 degrees,
        // so that it doesn't move directly towards the player.
        RotateY((float)GD.RandRange(-Mathf.Pi / 4.0, Mathf.Pi / 4.0));

        // We calculate a random speed (integer)
        int randomSpeed = GD.RandRange(MinSpeed, MaxSpeed);
        // We calculate a forward velocity that represents the speed.
        Velocity = Vector3.Forward * randomSpeed;
        // We then rotate the velocity vector based on the mob's Y rotation
        // In order to move in the direction the mob is looking
        Velocity = Velocity.Rotated(Vector3.Up, Rotation.Y);
    }

    // We also specified this function name in PascalCase in the editor's connection window.
    private void OnVisibilityNotifierScreenExited()
    {
        QueueFree();
    }

    public void Squash()
    {
        EmitSignal(SignalName.Squashed);
        QueueFree();
    }
}
