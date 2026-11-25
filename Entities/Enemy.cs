using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ArenaDefender.Utils;
using System;

namespace ArenaDefender.Entities;

public abstract class Enemy
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public float Speed { get; set; }
    public int ScoreValue { get; set; }
    public int Damage { get; set; }
    public float DetectionRadius { get; set; }

    protected Texture2D texture;
    protected int viewportWidth;
    protected int viewportHeight;

    protected Enemy(Vector2 position, Texture2D texture, int viewportWidth, int viewportHeight)
    {
        Position = position;
        Velocity = Vector2.Zero;
        this.texture = texture;
        this.viewportWidth = viewportWidth;
        this.viewportHeight = viewportHeight;
        DetectionRadius = 400f;
    }

    public abstract void Update(float deltaTime, Vector2 playerPosition);

    protected void ClampPosition()
    {
        Vector2 pos = Position;
        pos.X = MathHelper.Clamp(pos.X, 20, viewportWidth - 20);
        pos.Y = MathHelper.Clamp(pos.Y, 20, viewportHeight - 20);
        Position = pos;
    }

    protected Vector2 CalculateDirection(Vector2 target)
    {
        Vector2 direction = target - Position;
        float distance = direction.Length();
        if (distance > 0)
            return direction / distance;
        return Vector2.Zero;
    }

    protected float CalculateDistance(Vector2 target)
    {
        return Vector2.Distance(Position, target);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }

    public abstract void Draw(SpriteBatch spriteBatch);
}

public class StandardEnemy : Enemy
{
    public StandardEnemy(Vector2 position, Texture2D texture, int viewportWidth, int viewportHeight)
        : base(position, texture, viewportWidth, viewportHeight)
    {
        MaxHealth = 30;
        Health = MaxHealth;
        Speed = 150f;
        ScoreValue = 10;
        Damage = 10;
    }

    public override void Update(float deltaTime, Vector2 playerPosition)
    {
        Vector2 direction = CalculateDirection(playerPosition);
        Position += direction * Speed * deltaTime;
        ClampPosition();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        int x = (int)Position.X;
        int y = (int)Position.Y;

        // 6-LEG BUG - BURGUNDY
        spriteBatch.Draw(texture, new Rectangle(x - 11, y - 11, 22, 22), Utils.ColorPalette.StandardBugColor);

        // Top legs
        spriteBatch.Draw(texture, new Rectangle(x - 16, y - 10, 5, 5), Utils.ColorPalette.StandardBugColor);
        spriteBatch.Draw(texture, new Rectangle(x + 11, y - 10, 5, 5), Utils.ColorPalette.StandardBugColor);

        // Middle legs
        spriteBatch.Draw(texture, new Rectangle(x - 18, y - 2, 5, 5), Utils.ColorPalette.StandardBugColor);
        spriteBatch.Draw(texture, new Rectangle(x + 13, y - 2, 5, 5), Utils.ColorPalette.StandardBugColor);

        // Bottom legs
        spriteBatch.Draw(texture, new Rectangle(x - 16, y + 8, 5, 5), Utils.ColorPalette.StandardBugColor);
        spriteBatch.Draw(texture, new Rectangle(x + 11, y + 8, 5, 5), Utils.ColorPalette.StandardBugColor);

        // Eyes
        spriteBatch.Draw(texture, new Rectangle(x - 5, y - 5, 2, 2), Utils.ColorPalette.DarkText);
        spriteBatch.Draw(texture, new Rectangle(x + 3, y - 5, 2, 2), Utils.ColorPalette.DarkText);
    }
}

public class FastEnemy : Enemy
{
    private float zigzagTimer;

    public FastEnemy(Vector2 position, Texture2D texture, int viewportWidth, int viewportHeight)
        : base(position, texture, viewportWidth, viewportHeight)
    {
        MaxHealth = 15;
        Health = MaxHealth;
        Speed = 280f;
        ScoreValue = 15;
        Damage = 5;
        zigzagTimer = 0;
    }

    public override void Update(float deltaTime, Vector2 playerPosition)
    {
        zigzagTimer += deltaTime;

        Vector2 direction = CalculateDirection(playerPosition);

        Vector2 perpendicular = new Vector2(-direction.Y, direction.X);
        float zigzag = MathF.Sin(zigzagTimer * 3f) * 0.3f;

        Vector2 finalDirection = Vector2.Normalize(direction + perpendicular * zigzag);
        Position += finalDirection * Speed * deltaTime;

        ClampPosition();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        int x = (int)Position.X;
        int y = (int)Position.Y;

        // 4-LEG BUG - GRAY (SMALL & FAST)
        spriteBatch.Draw(texture, new Rectangle(x - 9, y - 9, 18, 18), Utils.ColorPalette.FastBugColor);

        // Top legs
        spriteBatch.Draw(texture, new Rectangle(x - 14, y - 8, 4, 4), Utils.ColorPalette.FastBugColor);
        spriteBatch.Draw(texture, new Rectangle(x + 10, y - 8, 4, 4), Utils.ColorPalette.FastBugColor);

        // Bottom legs
        spriteBatch.Draw(texture, new Rectangle(x - 14, y + 6, 4, 4), Utils.ColorPalette.FastBugColor);
        spriteBatch.Draw(texture, new Rectangle(x + 10, y + 6, 4, 4), Utils.ColorPalette.FastBugColor);

        // Eyes
        spriteBatch.Draw(texture, new Rectangle(x - 4, y - 4, 2, 2), Utils.ColorPalette.DarkText);
        spriteBatch.Draw(texture, new Rectangle(x + 2, y - 4, 2, 2), Utils.ColorPalette.DarkText);
    }
}

public class StrongEnemy : Enemy
{
    private float chargeTimer;
    private bool isCharging;

    public StrongEnemy(Vector2 position, Texture2D texture, int viewportWidth, int viewportHeight)
        : base(position, texture, viewportWidth, viewportHeight)
    {
        MaxHealth = 60;
        Health = MaxHealth;
        Speed = 100f;
        ScoreValue = 30;
        Damage = 20;
        chargeTimer = 2f;
        isCharging = false;
    }

    public override void Update(float deltaTime, Vector2 playerPosition)
    {
        chargeTimer -= deltaTime;

        Vector2 direction = CalculateDirection(playerPosition);

        if (chargeTimer <= 0)
        {
            isCharging = !isCharging;
            chargeTimer = isCharging ? 1.5f : 2f;
        }

        float currentSpeed = isCharging ? Speed * 2.5f : Speed;
        Position += direction * currentSpeed * deltaTime;

        ClampPosition();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        int x = (int)Position.X;
        int y = (int)Position.Y;

        Color bodyColor = isCharging ? Utils.ColorPalette.BurgundyAccent : Utils.ColorPalette.TankBugColor;

        // 8-LEG BUG - DARK/BURGUNDY WHEN CHARGING (LARGE & STRONG)
        spriteBatch.Draw(texture, new Rectangle(x - 13, y - 13, 26, 26), bodyColor);

        // Top legs
        spriteBatch.Draw(texture, new Rectangle(x - 20, y - 12, 6, 6), bodyColor);
        spriteBatch.Draw(texture, new Rectangle(x + 14, y - 12, 6, 6), bodyColor);

        // Upper middle legs
        spriteBatch.Draw(texture, new Rectangle(x - 22, y - 4, 6, 6), bodyColor);
        spriteBatch.Draw(texture, new Rectangle(x + 16, y - 4, 6, 6), bodyColor);

        // Lower middle legs
        spriteBatch.Draw(texture, new Rectangle(x - 22, y + 4, 6, 6), bodyColor);
        spriteBatch.Draw(texture, new Rectangle(x + 16, y + 4, 6, 6), bodyColor);

        // Bottom legs
        spriteBatch.Draw(texture, new Rectangle(x - 20, y + 10, 6, 6), bodyColor);
        spriteBatch.Draw(texture, new Rectangle(x + 14, y + 10, 6, 6), bodyColor);

        // Eyes - glow when charging
        if (isCharging)
        {
            spriteBatch.Draw(texture, new Rectangle(x - 6, y - 6, 4, 4), Utils.ColorPalette.BurgundyAccent);
            spriteBatch.Draw(texture, new Rectangle(x + 2, y - 6, 4, 4), Utils.ColorPalette.BurgundyAccent);
        }
        else
        {
            spriteBatch.Draw(texture, new Rectangle(x - 5, y - 5, 2, 2), Utils.ColorPalette.DarkText);
            spriteBatch.Draw(texture, new Rectangle(x + 3, y - 5, 2, 2), Utils.ColorPalette.DarkText);
        }
    }
}
