using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ArenaDefender.Entities;

public abstract class PowerUp
{
    public Vector2 Position { get; set; }
    public bool IsActive { get; set; }
    public float PickupRadius { get; set; } = 30f;
    public float Lifetime { get; set; } = 15f;

    protected Texture2D texture;
    protected float timeAlive;
    protected int viewportWidth;
    protected int viewportHeight;

    protected PowerUp(Vector2 position, Texture2D texture, int viewportWidth, int viewportHeight)
    {
        Position = position;
        this.texture = texture;
        this.viewportWidth = viewportWidth;
        this.viewportHeight = viewportHeight;
        IsActive = true;
        timeAlive = 0;
    }

    public virtual void Update(float deltaTime)
    {
        timeAlive += deltaTime;
        if (timeAlive > Lifetime)
            IsActive = false;
    }

    protected void DrawGlowingOrb(SpriteBatch spriteBatch, Color baseColor)
    {
        spriteBatch.Draw(texture,
            new Rectangle((int)Position.X - 10, (int)Position.Y - 10, 20, 20),
            baseColor);
    }

    public abstract void ApplyEffect(Player player);
    public abstract void Draw(SpriteBatch spriteBatch);
}

public class HealthPowerUp : PowerUp
{
    private int healAmount = 25;

    public HealthPowerUp(Vector2 position, Texture2D texture, int viewportWidth, int viewportHeight)
        : base(position, texture, viewportWidth, viewportHeight) { }

    public override void ApplyEffect(Player player)
    {
        player.Heal(healAmount);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        int x = (int)Position.X;
        int y = (int)Position.Y;

        // Outer glow
        float glowSize = MathF.Sin(timeAlive * 3f) * 3f + 14f;
        spriteBatch.Draw(texture, new Rectangle(x - (int)glowSize / 2, y - (int)glowSize / 2, (int)glowSize, (int)glowSize), new Color((byte)Utils.ColorPalette.BurgundyAccent.R, (byte)Utils.ColorPalette.BurgundyAccent.G, (byte)Utils.ColorPalette.BurgundyAccent.B, (byte)80));

        // Middle glow
        spriteBatch.Draw(texture, new Rectangle(x - 10, y - 10, 20, 20), new Color((byte)Utils.ColorPalette.BurgundyAccent.R, (byte)Utils.ColorPalette.BurgundyAccent.G, (byte)Utils.ColorPalette.BurgundyAccent.B, (byte)120));

        // Core orb
        spriteBatch.Draw(texture, new Rectangle(x - 8, y - 8, 16, 16), Utils.ColorPalette.BurgundySoft);

        // Plus symbol
        spriteBatch.Draw(texture, new Rectangle(x - 1, y - 5, 2, 10), Utils.ColorPalette.BurgundyAccent);
        spriteBatch.Draw(texture, new Rectangle(x - 5, y - 1, 10, 2), Utils.ColorPalette.BurgundyAccent);
    }
}

public class SpeedBoostPowerUp : PowerUp
{
    private float speedMultiplier = 2f;
    private float boostDuration = 5f;

    public SpeedBoostPowerUp(Vector2 position, Texture2D texture, int viewportWidth, int viewportHeight)
        : base(position, texture, viewportWidth, viewportHeight) { }

    public override void ApplyEffect(Player player)
    {
        player.ApplySpeedBoost(speedMultiplier, boostDuration);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        int x = (int)Position.X;
        int y = (int)Position.Y;

        // Rotating outer glow
        float glowSize = 16f + MathF.Sin(timeAlive * 2.5f) * 2f;
        spriteBatch.Draw(texture, new Rectangle(x - (int)glowSize / 2, y - (int)glowSize / 2, (int)glowSize, (int)glowSize), new Color((byte)Utils.ColorPalette.BurgundyAccent.R, (byte)Utils.ColorPalette.BurgundyAccent.G, (byte)Utils.ColorPalette.BurgundyAccent.B, (byte)80));

        // Middle glow
        spriteBatch.Draw(texture, new Rectangle(x - 10, y - 10, 20, 20), new Color((byte)Utils.ColorPalette.BurgundyAccent.R, (byte)Utils.ColorPalette.BurgundyAccent.G, (byte)Utils.ColorPalette.BurgundyAccent.B, (byte)120));

        // Core orb
        spriteBatch.Draw(texture, new Rectangle(x - 8, y - 8, 16, 16), Utils.ColorPalette.PowerUpBaseColor);
    }
}

public class DamagePowerUp : PowerUp
{
    private float speedIncrease = 100f;

    public DamagePowerUp(Vector2 position, Texture2D texture, int viewportWidth, int viewportHeight)
        : base(position, texture, viewportWidth, viewportHeight) { }

    public override void ApplyEffect(Player player)
    {
        player.ProjectileSpeed += speedIncrease;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        int x = (int)Position.X;
        int y = (int)Position.Y;

        // Pulsing outer glow
        float glowSize = MathF.Sin(timeAlive * 2.5f) * 3f + 14f;
        spriteBatch.Draw(texture, new Rectangle(x - (int)glowSize / 2, y - (int)glowSize / 2, (int)glowSize, (int)glowSize), new Color((byte)Utils.ColorPalette.BurgundyLight.R, (byte)Utils.ColorPalette.BurgundyLight.G, (byte)Utils.ColorPalette.BurgundyLight.B, (byte)80));

        // Middle glow
        spriteBatch.Draw(texture, new Rectangle(x - 10, y - 10, 20, 20), new Color((byte)Utils.ColorPalette.BurgundyLight.R, (byte)Utils.ColorPalette.BurgundyLight.G, (byte)Utils.ColorPalette.BurgundyLight.B, (byte)120));

        // Core orb
        spriteBatch.Draw(texture, new Rectangle(x - 8, y - 8, 16, 16), Utils.ColorPalette.PowerUpBaseColor);

        // Star symbol
        spriteBatch.Draw(texture, new Rectangle(x - 1, y - 4, 2, 8), Utils.ColorPalette.BurgundyLight);
        spriteBatch.Draw(texture, new Rectangle(x - 4, y - 1, 8, 2), Utils.ColorPalette.BurgundyLight);
    }
}
