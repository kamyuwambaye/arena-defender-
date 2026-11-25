# Arena Defender - Custom Design Document

##  Design Overview

A geometric, modern minimalist game with a woman protagonist fighting off bugs in a clean, neutral aesthetic with burgundy accents.

---

##  Color Palette

### Primary Palette (Neutral)
- **Background**: `#F5F5F5` (Off-white/light gray)
- **Dark Text**: `#2C2C2C` (Near-black)
- **Medium Neutral**: `#A0A0A0` (Gray)

### Accent Color
- **Deep Burgundy/Rust**: `#8B4545` (Primary accent)
- **Lighter Burgundy**: `#B85858` (Hover/highlight)
- **Very Light Burgundy**: `#D9C4C4` (Soft highlight)

### Functional Colors
- **Health Bar (Low)**: `#8B4545` (Burgundy)
- **Health Bar (High)**: `#A0A0A0` (Neutral gray)
- **Score Text**: `#2C2C2C` (Dark)
- **UI Elements**: `#A0A0A0` with `#8B4545` accents

---

##  Player Character Design

### Shape & Form
- **Base**: Smooth curved circular/oval shape (head)
- **Body**: Curved trapezoid or smooth polygon underneath
- **Style**: Feminine, flowing, organic curves
- **Size**: 30x30 pixels (medium)

### Visual Details
- **Head**: Large smooth circle on top
- **Hair**: Curved lines or geometric patterns on top (organic swoosh)
- **Body**: Smooth rounded rectangle or pentagon
- **Color**: 
  - Primary: `#2C2C2C` (dark gray/black outline)
  - Fill: `#D9C4C4` (light burgundy/skin tone)
  - Accent: `#8B4545` (burgundy details - hair, clothing)

### Weapon/Aiming Indicator
- Small circle pointing in aim direction
- Color: `#8B4545` (burgundy)
- Position: Offset from player position

---

##  Enemy Designs (Bug Theme)

### Enemy Type 1: Fast Bug (4 Legs)
- **Shape**: Oval body with 4 small legs
- **Body**: Smooth ellipse
- **Legs**: 4 curved lines positioned around body
- **Size**: 20x20 pixels (small)
- **Color**: `#A0A0A0` (medium gray)
- **Speed**: 280 units/sec
- **Health**: 15 HP
- **Behavior**: Zigzag pattern

### Enemy Type 2: Standard Bug (6 Legs)
- **Shape**: Hexagon or oval body with 6 legs
- **Body**: Smooth hexagon or oval
- **Legs**: 6 thin lines arranged symmetrically
- **Size**: 24x24 pixels (medium)
- **Color**: `#8B4545` (burgundy - distinct from fast)
- **Speed**: 150 units/sec
- **Health**: 30 HP
- **Behavior**: Direct movement toward player

### Enemy Type 3: Tank Bug (8 Legs)
- **Shape**: Large oval with 8 thick legs (spider-like)
- **Body**: Larger smooth ellipse
- **Legs**: 8 more robust lines forming octopus/spider shape
- **Size**: 32x32 pixels (large)
- **Color**: `#2C2C2C` (dark - indicates dangerous)
- **Speed**: 100 units/sec normally, 250 when charging
- **Health**: 60 HP
- **Behavior**: Charge attacks (darkens when charging)

---

##  Projectiles

### Design
- **Shape**: Perfect circle
- **Size**: 8 pixels diameter
- **Color**: `#8B4545` (burgundy - matches player weapon)
- **Effect**: Trail of light burgundy dots behind trajectory
- **Lifetime**: 10 seconds or off-screen
- **Speed**: 600 units/sec

### Trail Effect
- Leave fading burgundy circles along path
- Fade opacity: `#D9C4C4` → transparent
- Creates smooth line effect showing projectile path

---

##  Power-Up Designs

### Health Power-Up
- **Shape**: Smooth glowing orb with plus symbol
- **Color**: `#A0A0A0` (gray base) with `#8B4545` glow
- **Glow Effect**: Pulsing burgundy aura
- **Size**: 20x20 pixels
- **Symbol**: Simple + (plus sign) in center
- **Animation**: Pulsing glow effect, rotating

### Speed Boost Power-Up
- **Shape**: Smooth glowing orb with arrow symbol
- **Color**: `#A0A0A0` (gray base) with `#8B4545` glow
- **Glow Effect**: Rotating burgundy aura
- **Size**: 20x20 pixels
- **Symbol**: → (arrow) or spiral in center
- **Animation**: Rotating glow, orbiting particles

### Damage Boost Power-Up
- **Shape**: Smooth glowing orb with star symbol
- **Color**: `#A0A0A0` (gray base) with `#8B4545` glow
- **Glow Effect**: Pulsing burgundy aura
- **Size**: 20x20 pixels
- **Symbol**: ★ (star) in center
- **Animation**: Pulsing glow, slight rotation

---

##  UI Design

### Font
- **Font Family**: Modern sans-serif (Clean, minimalist)
- **Size**: 24px (default)
- **Color**: `#2C2C2C` (dark text on light background)
- **Weight**: Regular

### HUD Elements (During Gameplay)
- **Score**: Top-left, `#2C2C2C`
- **Health**: Top-left, `#2C2C2C` label + bar with `#8B4545` fill
- **Wave**: Top-left, `#2C2C2C`
- **Time**: Top-left, `#2C2C2C`
- **Health Bar**: Top-right
  - Background: `#D9C4C4` (light burgundy)
  - Fill: `#8B4545` (burgundy) → `#A0A0A0` when low
  - Border: `#2C2C2C` (1px dark outline)

### Menu Screens
- **Title**: Large, `#8B4545` (burgundy accent)
- **Buttons**: Text `#2C2C2C` on `#D9C4C4` background, hover highlights with `#8B4545`
- **Instructions**: `#A0A0A0` (medium gray)

### Game Over Screen
- **Title**: `#8B4545` (burgundy)
- **Score**: `#2C2C2C` (dark)
- **Stats**: `#A0A0A0` (medium gray)

---

##  Background & Environment

### Main Game Area
- **Background Color**: `#F5F5F5` (off-white/light gray)
- **Pattern**: Solid color (clean, minimal)
- **Optional**: Very subtle grid lines at `#E0E0E0` (almost invisible)

### Visual Hierarchy
- Solid background lets characters pop
- Neutral color keeps focus on gameplay
- Burgundy accents guide attention to important elements

---

##  Visual Effects

### Projectile Trails
- Small circles fade behind each projectile
- Color: `#D9C4C4` → transparent
- Creates visual feedback of shot direction
- Smooth motion blur effect

### Animations
- **Power-ups**: Pulsing glow effect (sine wave animation)
- **Health Bar**: Smooth lerp transition when health changes
- **Enemy Charge**: Tank bug darkens to pure `#2C2C2C` when charging
- **Hit Feedback**: Brief color brighten on collision

### No Effects (Intentionally Minimal)
- No particle explosions
- No screen shake
- No bloom/heavy glows
- Keeps design clean and focused

---

##  Visual Hierarchy Summary

1. **Player** (Highest Priority) - Light burgundy on neutral background
2. **Enemies** - Gray/burgundy/dark colored bugs
3. **Projectiles** - Burgundy circles with trails
4. **Power-ups** - Gray with burgundy glow
5. **UI** - Dark text on light background with burgundy accents
6. **Background** - Neutral, fades into nothing

---

##  Size Reference Chart

| Element | Width | Height | Purpose |
|---------|-------|--------|---------|
| Player | 30px | 30px | Primary character |
| Fast Bug | 20px | 20px | Small, quick |
| Standard Bug | 24px | 24px | Medium, balanced |
| Tank Bug | 32px | 32px | Large, threatening |
| Projectile | 8px | 8px | Small bullets |
| Power-up | 20px | 20px | Medium collectible |
| Health Bar | 200px | 20px | Status indicator |

---

##  Game Feel

- **Aesthetic**: Modern, clean, professional
- **Tone**: Geometric minimalism with organic touches
- **Mood**: Calm but focused gameplay
- **Accessibility**: High contrast, clear colors, readable fonts

---

## Implementation Notes

 Colors are CSS/Game friendly (hex values provided)
 All sizes are in pixels for easy implementation
 Shapes are described geometrically for easy drawing
 Effects are minimal to maintain clean look
 Design works well at game resolution (1200x800)

---

**Design Version**: 1.0  
**Date**: 2026-11-24  
**Status**: Ready for Implementation ✅
