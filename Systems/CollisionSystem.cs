using ArenaDefender.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ArenaDefender.Systems;

public class CollisionSystem
{
    public void CheckCollisions(Player player, List<Enemy> enemies, List<Projectile> projectiles, List<PowerUp> powerUps)
    {
        CheckPlayerEnemyCollisions(player, enemies);
        CheckProjectileEnemyCollisions(projectiles, enemies);
        CheckPlayerPowerUpCollisions(player, powerUps);
    }

    private void CheckPlayerEnemyCollisions(Player player, List<Enemy> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            float distance = Vector2.Distance(player.Position, enemies[i].Position);
            if (distance < 25 + 15)
            {
                player.TakeDamage(enemies[i].Damage);
                enemies[i].Health = 0;
            }
        }
    }

    private void CheckProjectileEnemyCollisions(List<Projectile> projectiles, List<Enemy> enemies)
    {
        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            for (int j = enemies.Count - 1; j >= 0; j--)
            {
                float distance = Vector2.Distance(projectiles[i].Position, enemies[j].Position);
                if (distance < 8 + 15)
                {
                    enemies[j].TakeDamage(projectiles[i].Damage);
                    projectiles[i].IsActive = false;
                    break;
                }
            }
        }
    }

    private void CheckPlayerPowerUpCollisions(Player player, List<PowerUp> powerUps)
    {
        for (int i = powerUps.Count - 1; i >= 0; i--)
        {
            float distance = Vector2.Distance(player.Position, powerUps[i].Position);
            if (distance < powerUps[i].PickupRadius + 15)
            {
                powerUps[i].ApplyEffect(player);
                powerUps[i].IsActive = false;
            }
        }
    }
}
