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
  let p = Vector2(float32(r.Next(0, 700)), 0.0f)
  let i = String.Concat("zombie_", r.Next 6)
  {
    Zombie.Position = p
    Zombie.Rotation = 0.0
    Zombie.Image    = i 
  }

let updateZombie (dt:float32) (pp:Vector2) (zombie:Zombie) : Zombie =
  {
    // TODO fix wandering / chasing
    zombie with Position = zombie.Position + Vector2.UnitY * dt * 10.0f
                Rotation = Math.Atan2((float) zombie.Position.Y - (float) pp.Y, (float) zombie.Position.X - (float) pp.X)
               
  }

let updateZombies (isZombieHit:Zombie->bool) (dt:float32) (zombies:List<Zombie>) (playerPosition:Vector2) =
  let spawnChange =
    if zombies.Length < 7 then
      100
    elif zombies.Length < 21 then
      21
    else
      7
  let zombies =
    if r.Next(100) < spawnChange then
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

