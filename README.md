# Arena Defender - 2D Survival Game

A complete 2D survival game built with C# and MonoGame where players must survive waves of enemies while earning points and collecting power-ups.

## Project Structure

```
ArenaDefender/
├── Program.cs                 # Entry point
├── ArenaDefenderGame.cs        # Main game class
├── Entities/
│   ├── Player.cs             # Player character with movement and shooting
│   ├── Enemy.cs              # Enemy base class and three enemy types
│   ├── Projectile.cs         # Projectile system
│   └── PowerUp.cs            # Power-up system with three types
├── Systems/
│   ├── CollisionSystem.cs     # Collision detection
│   └── EnemySpawner.cs        # Enemy spawning and wave management
├── Utils/
│   └── MathUtils.cs          # Mathematical utilities demonstrating required concepts
└── Content/
    ├── Content.mgcb          # MonoGame content pipeline
    └── arial.spritefont      # Font asset
```

## Installation & Setup

### Prerequisites
- .NET 9.0 SDK or later
- Visual Studio Code
- MonoGame

### Installation Steps

1. **Install .NET 9.0 SDK:**
   ```bash
   # macOS (using Homebrew)
   brew install dotnet
   
   # Or download from: https://dotnet.microsoft.com/download/dotnet/9.0
   ```

2. **Install MonoGame Templates:**
   ```bash
   dotnet new install MonoGame.Templates.CSharp
   ```

3. **Build the Project:**
   ```bash
   cd ArenaDefender
   dotnet build
   ```

4. **Run the Game:**
   ```bash
   dotnet run
   ```

## Game Features

### Player System
- **Movement**: WASD or Arrow Keys
- **Aiming**: Mouse position determines aim direction
- **Shooting**: Left mouse button to fire projectiles
- **Health System**: Starts with 100 health, displayed in HUD
- **Score Tracking**: Accumulates points from defeating enemies
- **Speed Boost**: Temporary speed increase from power-ups

### Enemy System
Three distinct enemy types with different behaviors:

1. **Standard Enemy** (Red)
   - Moderate health (30 HP)
   - Direct movement toward player
   - Medium speed (150 units/sec)
   - 10 damage on contact
   - Worth 10 points

2. **Fast Enemy** (Orange)
   - Low health (15 HP)
   - Zigzag movement pattern for unpredictability
   - High speed (280 units/sec)
   - 5 damage on contact
   - Worth 15 points

3. **Strong Enemy** (Maroon/Dark Red)
   - High health (60 HP)
   - Charge attack pattern
   - Variable speed (100-250 units/sec)
   - 20 damage on contact
   - Worth 30 points
   - Changes color when charging

### Projectile System
- Projectiles fire from player position in aim direction
- Travel at 600 units/sec
- 20 damage per hit
- 10-second lifetime or screen boundary
- Yellow visual indicator

### Collision Detection
- **Player-Enemy**: Player takes damage, enemy dies
- **Projectile-Enemy**: Enemy takes damage, projectile disappears
- **Player-PowerUp**: Pickup range of 30 units
- Uses distance-based collision detection

### Power-Up System
Three power-up types that spawn randomly:

1. **Health Power-Up** (Lime Green)
   - Restores 25 health
   - Pulsing animation
   - 15-second lifetime

2. **Speed Boost** (Cyan)
   - 2x movement speed for 5 seconds
   - Rotating animation
   - 15-second lifetime

3. **Damage Boost** (Magenta)
   - Increases projectile speed by 100 units/sec
   - Pulsing size animation
   - 15-second lifetime

### Scoring System
- Defeating enemies: 10-30 points based on enemy type
- Surviving longer: Time bonus applied indirectly through score accumulation
- Score displayed in top-left HUD

### User Interface
- **Menu Screen**: Title, start instructions, controls
- **Gameplay HUD**: 
  - Score (top-left, yellow)
  - Health/MaxHealth (top-left, red)
  - Current Wave (top-left, cyan)
  - Time Survived (top-left, green)
  - Health bar (top-right with percentage)
- **Game Over Screen**: Final score, survival time, return to menu option

### Difficulty Scaling
- **Wave System**: Each wave increases enemy count
  - Wave 1: 3 enemies
  - Wave 2: 4 enemies
  - Wave N: 2 + N enemies
- **Spawn Rate**: Faster enemy spawning with progression
  - Initial: 1.5 seconds between spawns
  - Maximum: 0.3 seconds between spawns
- **Enemy Variety**: Random mix of all three enemy types per wave

## Mathematical Concepts Implemented

### 1. Distance Calculation
- **Enemy Detection**: Enemies check distance to player (DetectionRadius = 400)
- **Collision Detection**: Distance-based circular collision system
- **Power-up Pickup**: 30-unit pickup radius using distance
- **Implementation**: `Vector2.Distance()` in MathUtils and CollisionSystem

### 2. Direction & Vectors
- **Player Movement**: Normalized velocity vectors for WASD/Arrow input
- **Enemy Pursuit**: Direction calculation toward player position
- **Projectile Motion**: Direction-based projectile movement
- **Aiming**: Mouse position determines aim direction vector
- **Implementation**: Vector normalization and calculations throughout

### 3. Algebra
- **Health Calculations**: Health -= Damage (clamped 0-MaxHealth)
- **Score Calculations**: Score += EnemyValue (non-negative)
- **Difficulty Scaling**: Enemies = 3 + (Wave - 1); SpawnRate = Max(0.3, 1.5 - (Wave - 1) * 0.1)
- **Power-up Effects**: Speed *= Multiplier, ProjectileSpeed += Increment
- **Implementation**: MathUtils.CalculateDamage(), difficulty scaling in EnemySpawner

### 4. Dot Product
- **Facing Detection**: Check if player is facing direction
  - `IsFacingDirection()`: Dot product > 0.5 means facing target
  - Used for potential enemy "field of view" mechanics
- **Implementation**: `Vector2.Dot()` in MathUtils, checking alignment between vectors

### 5. Cross Product
- **Perpendicular Direction**: FastEnemy uses cross product for zigzag pattern
  - Original direction is crossed to create perpendicular offset
  - Creates left/right wave pattern: `perpendicular = (-direction.Y, direction.X)`
- **Rotational Behavior**: Determines if target is left or right of facing direction
- **Implementation**: `CrossProduct()` function in MathUtils calculating (a.X * b.Y) - (a.Y * b.X)

### 6. Linear Interpolation (Lerp)
Used in three distinct situations:

1. **Health Bar Animation**: 
   - Smoothly animate health bar fill between current and target health
   - `Lerp(currentFill, targetFill, lerpAmount)`

2. **UI Color Transitions**:
   - Fade health bar color from green → orange → red based on health
   - `Color.Lerp(startColor, endColor, t)`

3. **Enemy Charge Animation**:
   - Smooth speed transition during charge attack
   - `Lerp(normalSpeed, chargeSpeed, transitionProgress)`

4. **Power-up Fade**:
   - Fade power-up opacity when near expiration
   - `Lerp(alpha, 0, fadeProgress)`

- **Implementation**: MathHelper.Lerp() and Vector2.Lerp() in MathUtils

## Controls

| Input | Action |
|-------|--------|
| WASD / Arrow Keys | Move player |
| Mouse | Aim |
| Left Click | Shoot projectile |
| Space | Start game / Return to menu |
| ESC | Exit game |

## Game States

1. **Menu**: Start screen with title and instructions
2. **Playing**: Active gameplay with enemies, scoring, and survival mechanics
3. **GameOver**: Final score display and option to return to menu

## Architecture

### Design Principles
- **Object-Oriented**: Clear entity hierarchy (Enemy base class, specific types)
- **SOLID Principles**: 
  - Single Responsibility: Each system handles one concern
  - Open/Closed: Enemy types extend base class without modifying core
  - Dependency Injection: CollisionSystem works with any entity list
- **Separation of Concerns**: Entities, Systems, and Utils are separated
- **Code Reusability**: MathUtils provides shared mathematical functions

### Key Classes
- `Player`: Manages player state, movement, and projectile firing
- `Enemy`: Abstract base with StandardEnemy, FastEnemy, StrongEnemy types
- `Projectile`: Manages projectile lifecycle and collision
- `PowerUp`: Abstract base with HealthPowerUp, SpeedBoostPowerUp, DamagePowerUp types
- `CollisionSystem`: Centralized collision detection logic
- `EnemySpawner`: Manages wave progression and enemy spawning
- `MathUtils`: Mathematical utility functions for game calculations

## Building for Deployment

### macOS
```bash
dotnet publish -c Release --runtime osx-arm64 --no-self-contained
```

### Windows
```bash
dotnet publish -c Release --runtime win-x64 --no-self-contained
```

### Linux
```bash
dotnet publish -c Release --runtime linux-x64 --no-self-contained
```

## Troubleshooting

### Content Build Issues
```bash
# Rebuild content
dotnet build --no-cache
```

### Font Not Loading
Ensure `arial.spritefont` exists in the Content folder and Content.mgcb references it.

### Graphics Issues on macOS
Make sure you're using MonoGame.Framework.DesktopGL package (included in .csproj).

## Learning Outcomes Met

✓ Developed complete game using C# and MonoGame
✓ Applied OOP and SOLID principles throughout architecture
✓ Implemented mathematical concepts (distance, vectors, algebra, dot/cross product, lerp)
✓ Built reusable components (entity systems, collision detection, spawning)
✓ Demonstrated effective code organization and documentation

## Future Enhancements

- Particle effects for explosions and power-ups
- Sound and music system
- Multiple weapon types
- Boss enemies
- Procedural difficulty scaling
- Leaderboard system
- Additional visual effects and polish

## License

Educational Project - Free to use and modify

---

**Author**: Arena Defender Development Team  
**Last Updated**: 2026-11-24  
**Version**: 1.0
