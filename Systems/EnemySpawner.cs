using ArenaDefender.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ArenaDefender.Systems;

public class EnemySpawner
{
    public int CurrentWave { get; private set; }
    private float spawnTimer;
    private float spawnRate;
    private int enemiesPerWave;
    private int enemiesSpawned;
    private Texture2D pixelTexture;
    private Random random;
    private int viewportWidth = 1200;
    private int viewportHeight = 800;

    public EnemySpawner()
    {
        CurrentWave = 1;
        spawnTimer = 0;
        spawnRate = 1.5f;
        enemiesPerWave = 3;
        enemiesSpawned = 0;
        random = new Random();
    }

    public void SetTexture(Texture2D texture)
    {
        pixelTexture = texture;
    }

    public void Reset()
    {
        CurrentWave = 1;
        spawnTimer = 0;
        spawnRate = 1.5f;
        enemiesPerWave = 3;
        enemiesSpawned = 0;
    }

    public void Update(float deltaTime, List<Enemy> enemies, float gameTime)
    {
        spawnTimer -= deltaTime;

        if (spawnTimer <= 0 && enemiesSpawned < enemiesPerWave)
        {
            SpawnEnemy(enemies);
            spawnTimer = spawnRate;
        }

        if (enemies.Count == 0 && enemiesSpawned >= enemiesPerWave)
        {
            NextWave();
        }
    }

    public void SpawnEnemy(List<Enemy> enemies, Texture2D texture)
    {
        Vector2 spawnPosition = GetRandomEdgePosition();

        Enemy newEnemy;
        int enemyType = random.Next(0, 3);

        switch (enemyType)
        {
            case 0:
                newEnemy = new StandardEnemy(spawnPosition, texture, viewportWidth, viewportHeight);
                break;
            case 1:
                newEnemy = new FastEnemy(spawnPosition, texture, viewportWidth, viewportHeight);
                break;
            case 2:
                newEnemy = new StrongEnemy(spawnPosition, texture, viewportWidth, viewportHeight);
                break;
            default:
                newEnemy = new StandardEnemy(spawnPosition, texture, viewportWidth, viewportHeight);
                break;
        }

        enemies.Add(newEnemy);
        enemiesSpawned++;
    }

    private void SpawnEnemy(List<Enemy> enemies)
    {
        Vector2 spawnPosition = GetRandomEdgePosition();

        Enemy newEnemy;
        int enemyType = random.Next(0, 3);

        switch (enemyType)
        {
            case 0:
                newEnemy = new StandardEnemy(spawnPosition, pixelTexture, viewportWidth, viewportHeight);
                break;
            case 1:
                newEnemy = new FastEnemy(spawnPosition, pixelTexture, viewportWidth, viewportHeight);
                break;
            case 2:
                newEnemy = new StrongEnemy(spawnPosition, pixelTexture, viewportWidth, viewportHeight);
                break;
            default:
                newEnemy = new StandardEnemy(spawnPosition, pixelTexture, viewportWidth, viewportHeight);
                break;
        }

        enemies.Add(newEnemy);
        enemiesSpawned++;
    }

    private Vector2 GetRandomEdgePosition()
    {
        int edge = random.Next(0, 4);
        Vector2 position = Vector2.Zero;

        switch (edge)
        {
            case 0:
                position = new Vector2(random.Next(0, viewportWidth), -20);
                break;
            case 1:
                position = new Vector2(random.Next(0, viewportWidth), viewportHeight + 20);
                break;
            case 2:
                position = new Vector2(-20, random.Next(0, viewportHeight));
                break;
            case 3:
                position = new Vector2(viewportWidth + 20, random.Next(0, viewportHeight));
                break;
        }

        return position;
    }

    private void NextWave()
    {
        CurrentWave++;
        enemiesSpawned = 0;

        enemiesPerWave = 3 + (CurrentWave - 1);
        spawnRate = MathF.Max(0.3f, 1.5f - (CurrentWave - 1) * 0.1f);
    }
}
