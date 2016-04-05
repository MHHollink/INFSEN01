module Gamestate

open Microsoft.Xna.Framework

open Player
open Zombie

type Gamestate =
  {
    Player  : Player
    Gun     : GunStatus
    Cursor  : Cursor
    Zombies : List<Zombie> 
    Bullets : List<Bullet>
  }

let init() = 
  {
    Zombies   = []
    Bullets   = []
    Gun       = GunStatus.Ready
    Cursor    = 
      {
        Position = Vector2(0.0f, 0.0f)
        Rotation = 0.0
      }
    Player  = 
      {
        Position = Vector2(300.0f, 300.0f)
        Velocity = Vector2.Zero
        Rotation = 0.0
      }
  }
  