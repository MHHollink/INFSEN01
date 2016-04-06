module Zombie

open Microsoft.Xna.Framework

open System
open Drawable
open Utils

let r = new System.Random()

type Zombie =
  {
    Position : Vector2
    Rotation : float
    Image    : string
  }

let spawn (pp:Vector2) =
  let c = r.Next(100)
  let p = 
    if c < 25 then 
      Vector2(1124.0f, float32(r.Next(0, 600))) // Right
    elif c < 50 then
      Vector2(float32(r.Next(0, 1024)), 700.0f) // Bottom
    elif c < 75 then 
      Vector2(-100.0f, float32(r.Next(0, 600)))  // Left
    else 
      Vector2(float32(r.Next(0, 1024)), -100.0f) // Top
  let i = String.Concat("zombie_", r.Next 6)
  {
    Zombie.Position = p
    Zombie.Rotation = 0.0
    Zombie.Image    = i 
  }

let updateZombie (dt:float32) (pp:Vector2) (zombie:Zombie) : Zombie =
  let speed  = 25.0;
  let deltaY = (float32)(speed * Math.Sin( zombie.Rotation ))
  let deltaX = (float32)(speed * Math.Cos( zombie.Rotation ))
  {
    // TODO fix wandering / chasing
    zombie with Position = Vector2(zombie.Position.X - (deltaX * dt), zombie.Position.Y - (deltaY * dt))
                Rotation = Math.Atan2((float) zombie.Position.Y - (float) pp.Y, (float) zombie.Position.X - (float) pp.X)
  }

let updateZombies (isZombieHit:Zombie->bool) (dt:float32) (zombies:List<Zombie>) (playerPosition:Vector2) =
  let spawnChange =
    if zombies.Length < 7 then
      100
    elif zombies.Length < 21 then
      40
    else
      1
  let zombies =
    if r.Next(100)+1 < spawnChange then
      spawn playerPosition :: zombies
    else
      zombies
  let zombies = map (updateZombie dt playerPosition) zombies 
  filter isZombieHit zombies

let drawZombie (zombie:Zombie) : Drawable =
  {
    Drawable.Position = zombie.Position
    Drawable.Rotation = zombie.Rotation
    Drawable.Image    = zombie.Image
  }

