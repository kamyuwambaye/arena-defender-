using Microsoft.Xna.Framework;

namespace ArenaDefender.Utils;

public static class ColorPalette
{
    // Primary Palette (Neutral)
    public static Color Background = new Color(245, 245, 245);      // #F5F5F5
    public static Color DarkText = new Color(44, 44, 44);           // #2C2C2C
    public static Color MediumNeutral = new Color(160, 160, 160);   // #A0A0A0

    // Accent Color (Deep Burgundy/Rust)
    public static Color BurgundyAccent = new Color(139, 69, 69);    // #8B4545
    public static Color BurgundyLight = new Color(184, 88, 88);     // #B85858
    public static Color BurgundySoft = new Color(217, 196, 196);    // #D9C4C4

    // Functional Colors
    public static Color HealthLow = BurgundyAccent;
    public static Color HealthHigh = MediumNeutral;
    public static Color ScoreText = DarkText;
    public static Color UIText = DarkText;

    // Element Colors
    public static Color PlayerColor = BurgundySoft;
    public static Color PlayerAccent = BurgundyAccent;
    public static Color FastBugColor = MediumNeutral;
    public static Color StandardBugColor = BurgundyAccent;
    public static Color TankBugColor = DarkText;
    public static Color ProjectileColor = BurgundyAccent;
    public static Color ProjectileTrailColor = BurgundySoft;
    public static Color PowerUpGlowColor = BurgundyAccent;
    public static Color PowerUpBaseColor = MediumNeutral;

    // UI Colors
    public static Color HealthBarBackground = BurgundySoft;
    public static Color HealthBarFill = BurgundyAccent;
    public static Color HealthBarBorder = DarkText;
    public static Color MenuTitle = BurgundyAccent;
    public static Color MenuText = DarkText;
    public static Color HUDText = DarkText;
}
