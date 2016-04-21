module Gamestate

open Microsoft.Xna.Framework

open Player
open Zombie
open Utils

type Gamestate =
  {
    Player       : Player
    Gun          : GunStatus
    Cursor       : Cursor
    Zombies      : List<Zombie> 
    Bullets      : List<Bullet>
    Difficulty   : Difficulty
    Score        : int
    Kills        : int
    BulletsFired : int
    Accuracy     : float32
    Highscore    : int
    Alive        : bool
  }

// spawnplace of a new player record
let newPlayer = 
      {
        Position    = Vector2(512.0f, 300.0f)
        Velocity    = Vector2.Zero
        Rotation    = 0.0
      }

// intial gamestate
let init() = 
  {
    Zombies      = []
    Bullets      = []
    Gun          = GunStatus.Ready
    Difficulty   = Difficulty.Retard
    Score        = 0
    Highscore    = 0
    Kills        = 0
    BulletsFired = 0
    Accuracy     = 1.0f
    Cursor       = 
      {
        Position    = Vector2(0.0f, 0.0f)
        Rotation    = 0.0
      }
    Player       = newPlayer
    Alive        = true
  }
  