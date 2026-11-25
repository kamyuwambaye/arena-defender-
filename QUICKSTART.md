# Arena Defender - Quick Start Guide

## 1. Prerequisites Check

Make sure you have:
- .NET 9.0 SDK or later
- Visual Studio Code

Check your .NET version:
```bash
dotnet --version
```

If you need to update, download from: https://dotnet.microsoft.com/download/dotnet/9.0

## 2. Install MonoGame Templates (First Time Only)

```bash
dotnet new install MonoGame.Templates.CSharp
```

## 3. Build and Run

```bash
# Navigate to the project directory
cd ArenaDefender

# Build the project
dotnet build

# Run the game
dotnet run
```

## 4. If Build Fails

### Content Build Issues
```bash
# Clean and rebuild
dotnet clean
dotnet build --no-cache
```

### macOS Specific Issues
Ensure you're using the DesktopGL version of MonoGame (should be in the .csproj).

## 5. Play the Game!

1. **Start Screen**: Press SPACE to start
2. **Controls**:
   - WASD / Arrow Keys = Move
   - Mouse = Aim
   - Left Click = Shoot
   - ESC = Exit

3. **Survive**: Stay alive as long as possible and rack up points!

## Project Layout

The project is organized into:
- **Entities**: Player, Enemy types, Projectiles, PowerUps
- **Systems**: Collision detection, Enemy spawning
- **Utils**: Mathematical utilities
- **Content**: Game assets (fonts)

All files follow C# naming conventions and are well-documented.

## Next Steps

Once running, you can:
1. Modify player speed in `Entities/Player.cs`
2. Add new enemy types in `Entities/Enemy.cs`
3. Create new power-ups in `Entities/PowerUp.cs`
4. Adjust wave difficulty in `Systems/EnemySpawner.cs`
5. Enhance visuals and add sounds

## Key Classes to Understand

- `ArenaDefenderGame.cs` - Main game loop and state management
- `Player.cs` - Player movement and shooting mechanics
- `Enemy.cs` - Three enemy types with different behaviors
- `CollisionSystem.cs` - All collision detection
- `EnemySpawner.cs` - Wave management and difficulty scaling

## Troubleshooting

**Q: Game won't build**
A: Make sure .NET 9.0 is installed and MonoGame templates are installed

**Q: No font appears**
A: Content pipeline error - try `dotnet clean && dotnet build`

**Q: Game runs but nothing appears**
A: Check GraphicsDevice initialization - ensure LoadContent completes

**Q: Can't move or shoot**
A: Check Input handling in Player.Update() method

---

Enjoy your Arena Defender game! 
