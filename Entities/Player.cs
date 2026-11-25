using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ArenaDefender.Utils;
using System.Collections.Generic;

namespace ArenaDefender.Entities;

public class Player
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; private set; } = 100;
    public int Score { get; set; }
    public float Speed { get; set; } = 300f;
    public float ProjectileSpeed { get; set; } = 600f;

    private Texture2D texture;
    private int viewportWidth;
    private int viewportHeight;
    private float shootCooldown;
    private float shootRate = 0.15f;
    private List<Projectile> newProjectiles;
    private Vector2 aimDirection;
    private float speedBoost;
    private float speedBoostTimer;

    public Player(Vector2 position, Texture2D texture, int viewportWidth, int viewportHeight)
    {
        Position = position;
        Velocity = Vector2.Zero;
        this.texture = texture;
        this.viewportWidth = viewportWidth;
        this.viewportHeight = viewportHeight;
        Health = MaxHealth;
        Score = 0;
        newProjectiles = new List<Projectile>();
        aimDirection = Vector2.UnitX;
        speedBoost = 1f;
        speedBoostTimer = 0f;
    }

    public void Reset()
    {
        Position = new Vector2(viewportWidth / 2f, viewportHeight / 2f);
        Velocity = Vector2.Zero;
        Health = MaxHealth;
        Score = 0;
        shootCooldown = 0;
        speedBoost = 1f;
        speedBoostTimer = 0f;
    }

    public void Update(float deltaTime, KeyboardState keyboardState)
    {
        HandleMovement(deltaTime, keyboardState);
        HandleAiming();
        HandleShooting(deltaTime);
        UpdateSpeedBoost(deltaTime);
        ClampPosition();
    }

    private void HandleMovement(float deltaTime, KeyboardState keyboardState)
    {
        Vector2 vel = Vector2.Zero;

        if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            vel.Y -= 1;
        if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            vel.Y += 1;
        if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            vel.X -= 1;
        if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            vel.X += 1;

        if (vel != Vector2.Zero)
        {
            vel = Vector2.Normalize(vel);
            Position += vel * Speed * speedBoost * deltaTime;
        }
    }

    private void HandleAiming()
    {
        var mouseState = Mouse.GetState();
        Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
        aimDirection = Vector2.Normalize(mousePosition - Position);
    }

    private void HandleShooting(float deltaTime)
    {
        shootCooldown -= deltaTime;

        if (Mouse.GetState().LeftButton == ButtonState.Pressed && shootCooldown <= 0)
        {
            var projectile = new Projectile(
                Position + aimDirection * 25,
                aimDirection,
                ProjectileSpeed,
                texture,
                viewportWidth,
                viewportHeight
            );
            newProjectiles.Add(projectile);
            shootCooldown = shootRate;
        }
    }

    private void UpdateSpeedBoost(float deltaTime)
    {
        if (speedBoostTimer > 0)
        {
            speedBoostTimer -= deltaTime;
            if (speedBoostTimer <= 0)
                speedBoost = 1f;
        }
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        speedBoost = multiplier;
        speedBoostTimer = duration;
    }

    private void ClampPosition()
    {
        Vector2 pos = Position;
        pos.X = MathHelper.Clamp(pos.X, 25, viewportWidth - 25);
        pos.Y = MathHelper.Clamp(pos.Y, 25, viewportHeight - 25);
        Position = pos;
    }

    public List<Projectile> GetNewProjectiles()
    {
        var result = new List<Projectile>(newProjectiles);
        newProjectiles.Clear();
        return result;
    }

    public void TakeDamage(int damage)
    {
        Health = MathHelper.Max(0, Health - damage);
    }

    public void Heal(int amount)
    {
        Health = MathHelper.Min(MaxHealth, Health + amount);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        int x = (int)Position.X;
        int y = (int)Position.Y;

        // HEAD
        spriteBatch.Draw(texture, new Rectangle(x - 10, y - 20, 20, 20), Utils.ColorPalette.PlayerColor);

        // HAIR/TOP
        spriteBatch.Draw(texture, new Rectangle(x - 10, y - 22, 20, 3), Utils.ColorPalette.PlayerAccent);

        // BODY
        spriteBatch.Draw(texture, new Rectangle(x - 12, y - 2, 24, 22), Utils.ColorPalette.PlayerColor);

        // CLOTHING STRIPE
        spriteBatch.Draw(texture, new Rectangle(x - 12, y + 10, 24, 5), Utils.ColorPalette.PlayerAccent);

        // WEAPON BASE
        spriteBatch.Draw(texture, new Rectangle(x + 10, y, 6, 6), Utils.ColorPalette.BurgundyAccent);

        // WEAPON AIM
        Vector2 gunTipPosition = Position + aimDirection * 25;
        spriteBatch.Draw(texture, new Rectangle((int)gunTipPosition.X - 5, (int)gunTipPosition.Y - 5, 10, 10), Utils.ColorPalette.BurgundyAccent);
    }
}
