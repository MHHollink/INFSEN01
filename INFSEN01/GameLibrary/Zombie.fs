module Zombie

open Microsoft.Xna.Framework

let r = new System.Random()

type Zombie =
  {
    Position : Vector2
    Rotation : float32
    Image    : string
  }

let spawn() =
  {
    Zombie.Position = Vector2(float32(r.Next(0, 700)), 0.0f)
    Zombie.Rotation = 0.0f
    Zombie.Image    = "" 
  }

let update (dt:float32) (zombie:Zombie) : Zombie =
  {
    // TODO fix wandering / chasing
    zombie with Position = zombie.Position + Vector2.UnitY * dt * 10.0f
  }
