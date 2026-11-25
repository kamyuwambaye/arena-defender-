using Microsoft.Xna.Framework;
using System;

namespace ArenaDefender.Utils;

public static class MathUtils
{
    public static float CalculateDistance(Vector2 from, Vector2 to)
    {
        return Vector2.Distance(from, to);
    }

    public static Vector2 CalculateDirection(Vector2 from, Vector2 to)
    {
        Vector2 direction = to - from;
        float length = direction.Length();
        if (length > 0)
            return direction / length;
        return Vector2.Zero;
    }

    public static float DotProduct(Vector2 a, Vector2 b)
    {
        return Vector2.Dot(a, b);
    }

    public static float CrossProduct(Vector2 a, Vector2 b)
    {
        return (a.X * b.Y) - (a.Y * b.X);
    }

    public static Vector2 Lerp(Vector2 start, Vector2 end, float t)
    {
        t = MathHelper.Clamp(t, 0, 1);
        return Vector2.Lerp(start, end, t);
    }

    public static float Lerp(float start, float end, float t)
    {
        t = MathHelper.Clamp(t, 0, 1);
        return MathHelper.Lerp(start, end, t);
    }

    public static Color LerpColor(Color start, Color end, float t)
    {
        t = MathHelper.Clamp(t, 0, 1);
        return Color.Lerp(start, end, t);
    }

    public static bool IsInDetectionRadius(Vector2 position, Vector2 targetPosition, float radius)
    {
        return CalculateDistance(position, targetPosition) <= radius;
    }

    public static bool IsFacingDirection(Vector2 facing, Vector2 targetDirection)
    {
        float dot = DotProduct(Vector2.Normalize(facing), Vector2.Normalize(targetDirection));
        return dot > 0.5f;
    }

    public static Vector2 RotateVector(Vector2 vector, float angleRadians)
    {
        float cos = MathF.Cos(angleRadians);
        float sin = MathF.Sin(angleRadians);
        return new Vector2(
            vector.X * cos - vector.Y * sin,
            vector.X * sin + vector.Y * cos
        );
    }

    public static float CalculateDamage(int baseDamage, float multiplier, float reduction)
    {
        return baseDamage * multiplier - reduction;
    }

    public static float CalculateHealthRegeneration(float currentHealth, float maxHealth, float regenRate)
    {
        return MathHelper.Min(currentHealth + regenRate, maxHealth);
    }

    public static float CalculateDifficultyScaling(int wave)
    {
        return 1f + (wave - 1) * 0.15f;
    }
}
