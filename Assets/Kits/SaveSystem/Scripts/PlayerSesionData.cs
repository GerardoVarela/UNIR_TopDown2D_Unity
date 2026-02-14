using System;
using UnityEngine;

[Serializable]
public class PlayerSesionData
{
    public Level level;
    public Vector2 playerPosition;
    public int health;
    public int coins;
    // Add other relevant player data as needed

    public PlayerSesionData() // New Game
    {
        this.level = Level.MEADOW;
        this.health = 100;
        this.coins = 0;
    }

    public PlayerSesionData(Level level, int health, int coins) // Continue game without Player Position
    {
        this.level = level;
        this.health = health;
        this.coins = coins;
    }

    public PlayerSesionData(Level level, int health, int coins, Vector2 playerPosition) // Continue game with Player Position
    {
        this.level = level;
        this.health = health;
        this.coins = coins;
        this.playerPosition = playerPosition;
    }
}
