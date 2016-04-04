module Gamestate

open Microsoft.Xna.Framework

open Player;

type Gamestate =
  {
    Player : Player
  }

let init() = 
  {
    Player = 
      {
        Position = Vector2(0.0f, 0.0f)
        Velocity = Vector2.Zero
      }
  }
