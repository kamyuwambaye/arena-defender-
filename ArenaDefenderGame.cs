using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ArenaDefender.Entities;
using ArenaDefender.Systems;
using ArenaDefender.Utils;
using System.Collections.Generic;
using System;

namespace ArenaDefender;

public class ArenaDefenderGame : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private Player player;
    private List<Enemy> enemies;
    private List<Projectile> projectiles;
    private List<PowerUp> powerUps;
    private CollisionSystem collisionSystem;
    private EnemySpawner spawner;
    private GameState gameState;
    private Texture2D pixelTexture;
    private SpriteFont font;
    private float gameTime;
    private float menuTimer = 2f;

    public ArenaDefenderGame()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        graphics.PreferredBackBufferWidth = 1200;
        graphics.PreferredBackBufferHeight = 800;
    }

    protected override void Initialize()
    {
        gameState = GameState.Menu;
        enemies = new List<Enemy>();
        projectiles = new List<Projectile>();
        powerUps = new List<PowerUp>();
        collisionSystem = new CollisionSystem();
        spawner = new EnemySpawner();
        gameTime = 0;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });

        try
        {
            font = Content.Load<SpriteFont>("arial");
        }
        catch
        {
            try
            {
                font = Content.Load<SpriteFont>("Fonts/arial");
            }
            catch
            {
                font = null;
            }
        }

        player = new Player(
            new Vector2(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height / 2f),
            pixelTexture,
            GraphicsDevice.Viewport.Width,
            GraphicsDevice.Viewport.Height
        );

        spawner.SetTexture(pixelTexture);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Decrease menu timer
        if (gameState == GameState.Menu)
            menuTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        switch (gameState)
        {
            case GameState.Menu:
                UpdateMenu();
                break;
            case GameState.Playing:
                UpdateGame(gameTime);
                break;
            case GameState.GameOver:
                UpdateGameOver();
                break;
        }

        base.Update(gameTime);
    }

    private void UpdateMenu()
    {
        // Auto-start after 2 seconds or on any key
        if (menuTimer <= 0 || Keyboard.GetState().GetPressedKeys().Length > 0 || Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            gameState = GameState.Playing;
            player.Reset();
            enemies.Clear();
            projectiles.Clear();
            powerUps.Clear();
            gameTime = 0;
            spawner.Reset();
            menuTimer = 2f;
        }
    }

    private void UpdateGame(GameTime gameTime)
    {
        this.gameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        player.Update((float)gameTime.ElapsedGameTime.TotalSeconds, Keyboard.GetState());

        spawner.Update((float)gameTime.ElapsedGameTime.TotalSeconds, enemies, this.gameTime);

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            enemies[i].Update((float)gameTime.ElapsedGameTime.TotalSeconds, player.Position);
            if (enemies[i].Health <= 0)
            {
                player.Score += enemies[i].ScoreValue;
                enemies.RemoveAt(i);
            }
        }

        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            projectiles[i].Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            if (!projectiles[i].IsActive)
                projectiles.RemoveAt(i);
        }

        for (int i = powerUps.Count - 1; i >= 0; i--)
        {
            powerUps[i].Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            if (!powerUps[i].IsActive)
                powerUps.RemoveAt(i);
        }

        collisionSystem.CheckCollisions(player, enemies, projectiles, powerUps);

        if (player.Health <= 0)
            gameState = GameState.GameOver;

        HandlePlayerAttack();
    }

    private void HandlePlayerAttack()
    {
        var newProjectiles = player.GetNewProjectiles();
        projectiles.AddRange(newProjectiles);
    }

    private void UpdateGameOver()
    {
        if (Keyboard.GetState().GetPressedKeys().Length > 0 || Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            gameState = GameState.Menu;
            menuTimer = 2f;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Utils.ColorPalette.Background);
        spriteBatch.Begin();

        switch (gameState)
        {
            case GameState.Menu:
                DrawMenu();
                break;
            case GameState.Playing:
                DrawGame();
                break;
            case GameState.GameOver:
                DrawGameOver();
                break;
        }

        spriteBatch.End();
        base.Draw(gameTime);
    }

    private void DrawMenu()
    {
        // Draw title area
        spriteBatch.Draw(pixelTexture,
            new Rectangle(200, 100, 800, 150),
            Utils.ColorPalette.BurgundyAccent);

        // Draw start button area
        spriteBatch.Draw(pixelTexture,
            new Rectangle(400, 350, 400, 80),
            Utils.ColorPalette.BurgundyAccent);

        // Draw instructions area
        spriteBatch.Draw(pixelTexture,
            new Rectangle(150, 500, 900, 100),
            Utils.ColorPalette.MediumNeutral);

        if (font != null)
        {
            var menuText = "ARENA DEFENDER";
            var instructionsText = "Press SPACE to Start";
            var controlsText = "WASD/Arrows: Move | Mouse: Aim | Left Click: Shoot";

            var menuSize = font.MeasureString(menuText);
            spriteBatch.DrawString(font, menuText,
                new Vector2(GraphicsDevice.Viewport.Width / 2 - menuSize.X / 2, 125),
                Utils.ColorPalette.UIText);

            var instructionsSize = font.MeasureString(instructionsText);
            spriteBatch.DrawString(font, instructionsText,
                new Vector2(GraphicsDevice.Viewport.Width / 2 - instructionsSize.X / 2, 370),
                Utils.ColorPalette.UIText);

            var controlsSize = font.MeasureString(controlsText);
            spriteBatch.DrawString(font, controlsText,
                new Vector2(GraphicsDevice.Viewport.Width / 2 - controlsSize.X / 2, 520),
                Utils.ColorPalette.UIText);
        }
    }

    private void DrawGame()
    {
        player.Draw(spriteBatch);

        foreach (var enemy in enemies)
            enemy.Draw(spriteBatch);

        foreach (var projectile in projectiles)
            projectile.Draw(spriteBatch);

        foreach (var powerUp in powerUps)
            powerUp.Draw(spriteBatch);

        DrawHUD();
    }

    private void DrawHUD()
    {
        // Health bar at top right
        int healthBarWidth = 240;
        int healthBarHeight = 35;
        float healthPercent = player.Health / (float)player.MaxHealth;
        int filledWidth = (int)(healthBarWidth * healthPercent);

        // Health bar background
        spriteBatch.Draw(pixelTexture,
            new Rectangle(GraphicsDevice.Viewport.Width - healthBarWidth - 20, 10, healthBarWidth, healthBarHeight),
            new Color(220, 220, 220, 255));

        // Health bar fill (burgundy)
        spriteBatch.Draw(pixelTexture,
            new Rectangle(GraphicsDevice.Viewport.Width - healthBarWidth - 20, 10, filledWidth, healthBarHeight),
            new Color(139, 69, 69, 255));

        // Health bar border
        DrawRectangleBorder(GraphicsDevice.Viewport.Width - healthBarWidth - 20, 10, healthBarWidth, healthBarHeight, 3, new Color(50, 50, 50, 255));

        // Health text
        DrawSimpleText($"HP{player.Health}/{player.MaxHealth}", GraphicsDevice.Viewport.Width - healthBarWidth - 15, 15, new Color(50, 50, 50, 255), 1);

        // Timer at bottom right
        int timerBarWidth = 240;
        int timerBarHeight = 35;
        int timerX = GraphicsDevice.Viewport.Width - timerBarWidth - 20;
        int timerY = GraphicsDevice.Viewport.Height - 50;

        // Timer bar background
        spriteBatch.Draw(pixelTexture,
            new Rectangle(timerX, timerY, timerBarWidth, timerBarHeight),
            new Color(220, 220, 220, 255));

        // Timer bar fill (shows time progression)
        float timeFillPercent = (gameTime % 60f) / 60f;
        int timeFillWidth = (int)(timerBarWidth * timeFillPercent);
        spriteBatch.Draw(pixelTexture,
            new Rectangle(timerX, timerY, timeFillWidth, timerBarHeight),
            new Color(139, 69, 69, 255));

        // Timer bar border
        DrawRectangleBorder(timerX, timerY, timerBarWidth, timerBarHeight, 3, new Color(50, 50, 50, 255));

        // Timer text
        DrawSimpleText($"T{(int)gameTime}s", timerX + 10, timerY + 8, new Color(50, 50, 50, 255), 1);

        // Score at top left
        spriteBatch.Draw(pixelTexture, new Rectangle(10, 10, 25, 35), new Color(139, 69, 69, 255));
        DrawSimpleText($"S{player.Score}", 40, 15, new Color(50, 50, 50, 255), 1);

        // Wave at bottom left
        spriteBatch.Draw(pixelTexture, new Rectangle(10, GraphicsDevice.Viewport.Height - 50, 25, 35), new Color(139, 69, 69, 255));
        DrawSimpleText($"W{spawner.CurrentWave}", 40, GraphicsDevice.Viewport.Height - 45, new Color(50, 50, 50, 255), 1);
    }

    private void DrawRectangleBorder(int x, int y, int width, int height, int thickness, Color color)
    {
        spriteBatch.Draw(pixelTexture, new Rectangle(x, y, width, thickness), color);
        spriteBatch.Draw(pixelTexture, new Rectangle(x, y + height - thickness, width, thickness), color);
        spriteBatch.Draw(pixelTexture, new Rectangle(x, y, thickness, height), color);
        spriteBatch.Draw(pixelTexture, new Rectangle(x + width - thickness, y, thickness, height), color);
    }

    private void DrawSimpleText(string text, int x, int y, Color color, int scale = 2)
    {
        if (font != null)
        {
            spriteBatch.DrawString(font, text, new Vector2(x, y), color);
        }
        else
        {
            // Simple pixel-based text rendering for numbers and basic chars
            int xPos = x;
            foreach (char c in text)
            {
                DrawCharacter(c, xPos, y, color, scale);
                xPos += 8 * scale;
            }
        }
    }

    private void DrawCharacter(char c, int x, int y, Color color, int scale)
    {
        switch (c)
        {
            case '0': DrawPixelChar0(x, y, color, scale); break;
            case '1': DrawPixelChar1(x, y, color, scale); break;
            case '2': DrawPixelChar2(x, y, color, scale); break;
            case '3': DrawPixelChar3(x, y, color, scale); break;
            case '4': DrawPixelChar4(x, y, color, scale); break;
            case '5': DrawPixelChar5(x, y, color, scale); break;
            case '6': DrawPixelChar6(x, y, color, scale); break;
            case '7': DrawPixelChar7(x, y, color, scale); break;
            case '8': DrawPixelChar8(x, y, color, scale); break;
            case '9': DrawPixelChar9(x, y, color, scale); break;
            case ':': DrawPixelCharColon(x, y, color, scale); break;
            case ' ': break;
            case 'H': DrawPixelCharH(x, y, color, scale); break;
            case 'P': DrawPixelCharP(x, y, color, scale); break;
            case '/': DrawPixelCharSlash(x, y, color, scale); break;
            case 'Y': DrawPixelCharY(x, y, color, scale); break;
            case 'O': DrawPixelCharO(x, y, color, scale); break;
            case 'U': DrawPixelCharU(x, y, color, scale); break;
            case 'L': DrawPixelCharL(x, y, color, scale); break;
            case 'S': DrawPixelCharS(x, y, color, scale); break;
            case 'T': DrawPixelCharT(x, y, color, scale); break;
            case 'R': DrawPixelCharR(x, y, color, scale); break;
            case 'E': DrawPixelCharE(x, y, color, scale); break;
            case 'A': DrawPixelCharA(x, y, color, scale); break;
            case 'W': DrawPixelCharW(x, y, color, scale); break;
        }
    }

    private void DrawPixelChar0(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "111,101,101,101,111");
    private void DrawPixelChar1(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "001,001,001,001,001");
    private void DrawPixelChar2(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "111,001,111,100,111");
    private void DrawPixelChar3(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "111,001,111,001,111");
    private void DrawPixelChar4(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "101,101,111,001,001");
    private void DrawPixelChar5(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "111,100,111,001,111");
    private void DrawPixelChar6(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "111,100,111,101,111");
    private void DrawPixelChar7(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "111,001,001,001,001");
    private void DrawPixelChar8(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "111,101,111,101,111");
    private void DrawPixelChar9(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "111,101,111,001,111");
    private void DrawPixelCharColon(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "000,010,000,010,000");
    private void DrawPixelCharH(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "101,101,111,101,101");
    private void DrawPixelCharP(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "111,101,111,100,100");
    private void DrawPixelCharSlash(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "001,001,010,100,100");
    private void DrawPixelCharY(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "101,101,111,001,001");
    private void DrawPixelCharO(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "111,101,101,101,111");
    private void DrawPixelCharU(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "101,101,101,101,111");
    private void DrawPixelCharL(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "100,100,100,100,111");
    private void DrawPixelCharS(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "111,100,111,001,111");
    private void DrawPixelCharT(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "111,010,010,010,010");
    private void DrawPixelCharR(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "111,101,111,110,101");
    private void DrawPixelCharE(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "111,100,110,100,111");
    private void DrawPixelCharA(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "111,101,111,101,101");
    private void DrawPixelCharW(int x, int y, Color color, int s) => DrawPixelGroup(x, y, color, s, "101,101,101,111,111");

    private void DrawPixelGroup(int x, int y, Color color, int s, string pattern)
    {
        string[] rows = pattern.Split(',');
        for (int row = 0; row < rows.Length; row++)
        {
            for (int col = 0; col < rows[row].Length; col++)
            {
                if (rows[row][col] == '1')
                    spriteBatch.Draw(pixelTexture, new Rectangle(x + col * s, y + row * s, s, s), color);
            }
        }
    }



    private void DrawGameOver()
    {
        // Draw semi-transparent overlay
        spriteBatch.Draw(pixelTexture,
            new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
            new Color(0, 0, 0, 150));

        // Draw YOU LOST title area (large burgundy box)
        spriteBatch.Draw(pixelTexture,
            new Rectangle(GraphicsDevice.Viewport.Width / 2 - 400, 40, 800, 120),
            new Color(139, 69, 69, 255));
        DrawRectangleBorder(GraphicsDevice.Viewport.Width / 2 - 400, 40, 800, 120, 4, new Color(50, 50, 50, 255));

        // Draw YOU LOST text
        if (font != null)
            spriteBatch.DrawString(font, "YOU LOST", new Vector2(GraphicsDevice.Viewport.Width / 2 - 120, 60), new Color(255, 255, 255, 255));
        else
            DrawSimpleText("YOU LOST", GraphicsDevice.Viewport.Width / 2 - 150, 55, new Color(255, 255, 255, 255), 3);

        // Draw stats area (light background)
        spriteBatch.Draw(pixelTexture,
            new Rectangle(GraphicsDevice.Viewport.Width / 2 - 350, 200, 700, 200),
            new Color(220, 220, 220, 255));
        DrawRectangleBorder(GraphicsDevice.Viewport.Width / 2 - 350, 200, 700, 200, 3, new Color(50, 50, 50, 255));

        // Draw final score
        if (font != null)
            spriteBatch.DrawString(font, $"Final Score: {player.Score}", new Vector2(GraphicsDevice.Viewport.Width / 2 - 150, 220), new Color(50, 50, 50, 255));
        else
            DrawSimpleText($"Score{player.Score}", GraphicsDevice.Viewport.Width / 2 - 120, 220, new Color(50, 50, 50, 255), 2);

        // Draw time survived
        if (font != null)
            spriteBatch.DrawString(font, $"Survived: {(int)gameTime} seconds", new Vector2(GraphicsDevice.Viewport.Width / 2 - 150, 260), new Color(50, 50, 50, 255));
        else
            DrawSimpleText($"Time{(int)gameTime}s", GraphicsDevice.Viewport.Width / 2 - 120, 260, new Color(50, 50, 50, 255), 2);

        // Draw wave reached
        if (font != null)
            spriteBatch.DrawString(font, $"Wave Reached: {spawner.CurrentWave}", new Vector2(GraphicsDevice.Viewport.Width / 2 - 150, 300), new Color(50, 50, 50, 255));
        else
            DrawSimpleText($"Wave{spawner.CurrentWave}", GraphicsDevice.Viewport.Width / 2 - 120, 300, new Color(50, 50, 50, 255), 2);

        // Draw RESTART button area (burgundy)
        int restartButtonX = GraphicsDevice.Viewport.Width / 2 - 150;
        int restartButtonY = 450;
        int buttonWidth = 300;
        int buttonHeight = 60;

        spriteBatch.Draw(pixelTexture,
            new Rectangle(restartButtonX, restartButtonY, buttonWidth, buttonHeight),
            new Color(139, 69, 69, 255));
        DrawRectangleBorder(restartButtonX, restartButtonY, buttonWidth, buttonHeight, 3, new Color(50, 50, 50, 255));

        if (font != null)
            spriteBatch.DrawString(font, "RESTART - Click or Press SPACE", new Vector2(restartButtonX + 20, restartButtonY + 15), new Color(255, 255, 255, 255));
        else
            DrawSimpleText("RESTART", restartButtonX + 50, restartButtonY + 15, new Color(255, 255, 255, 255), 2);
    }
}

public enum GameState
{
    Menu,
    Playing,
    GameOver
}
