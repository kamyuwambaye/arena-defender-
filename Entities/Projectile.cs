using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ArenaDefender.Entities;

public class Projectile
{
    public Vector2 Position { get; set; }
    public Vector2 Direction { get; set; }
    public float Speed { get; set; }
    public bool IsActive { get; set; }
    public int Damage { get; private set; } = 20;
    public float Lifetime { get; private set; } = 10f;

    private Texture2D texture;
    private int viewportWidth;
    private int viewportHeight;
    private float timeAlive;
    private List<Vector2> trailPositions;
    private const int MaxTrailLength = 15;

    public Projectile(Vector2 position, Vector2 direction, float speed, Texture2D texture, int viewportWidth, int viewportHeight)
    {
        Position = position;
        Direction = direction;
        Speed = speed;
        this.texture = texture;
        this.viewportWidth = viewportWidth;
        this.viewportHeight = viewportHeight;
        IsActive = true;
        timeAlive = 0;
        trailPositions = new List<Vector2>();
    }

    public void Update(float deltaTime)
    {
        timeAlive += deltaTime;

        trailPositions.Add(Position);
        if (trailPositions.Count > MaxTrailLength)
            trailPositions.RemoveAt(0);

        Position += Direction * Speed * deltaTime;

        if (timeAlive > Lifetime || IsOutOfBounds())
            IsActive = false;
    }

    private bool IsOutOfBounds()
    {
        return Position.X < -50 || Position.X > viewportWidth + 50 ||
               Position.Y < -50 || Position.Y > viewportHeight + 50;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Projectile with glow
        spriteBatch.Draw(texture, new Rectangle((int)Position.X - 6, (int)Position.Y - 6, 12, 12), new Color((byte)Utils.ColorPalette.BurgundyAccent.R, (byte)Utils.ColorPalette.BurgundyAccent.G, (byte)Utils.ColorPalette.BurgundyAccent.B, (byte)100));
        spriteBatch.Draw(texture, new Rectangle((int)Position.X - 4, (int)Position.Y - 4, 8, 8), Utils.ColorPalette.ProjectileColor);
    }
}
