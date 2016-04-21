module Zombie

open Microsoft.Xna.Framework

open System
open Drawable
open Utils

let r = new System.Random() // Random for spawning

// Zombie type with f
type Zombie =
  {
    Position : Vector2 // Position in X Y  format
    Rotation : float   // Rotation in radians ( -Math.PI to Math.PI )
    Image    : string  // String value of image path
  }

/// spawn creates a new Zombie on a position 100 pixels away from the border of the screen
/// returns a zombie with one of the 6 spites, XY coordinate and rotation set to 0
let spawn (playerPosition:Vector2) : Zombie =        
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

/// updateZombie is the update method for a single zombie, It Rotatas en moves the zombie to its next X and Y location
/// takes in a float32 represtatation of Delta Time. the Player position as a vector2 and the zombie that is updated.
/// Returns the zombie that was passed in as a new updated zombie
let updateZombie (dt:float32) (pp:Vector2) (zombie:Zombie) : Zombie =
  let speed  = 25.0;
  let deltaY = (float32)(speed * Math.Sin( zombie.Rotation ))
  let deltaX = (float32)(speed * Math.Cos( zombie.Rotation ))
  {
    zombie with Position = Vector2(zombie.Position.X - (deltaX * dt), zombie.Position.Y - (deltaY * dt))
                Rotation = Math.Atan2((float) zombie.Position.Y - (float) pp.Y, (float) zombie.Position.X - (float) pp.X)
  }

/// updateZombies is the update method that updates the entire list of zombies and might spawn a new one
/// spawning :
///     When there are les then 7 zombies, 100% change to spawn a new one;
///     When there are les then 21 zombies, 40% change to spawn a new one;
///     else there is only 1% change to spawn a new one;
/// updates the list that was passed a zombies parameter to a new list with all zombies that are currently in play
/// zombies that are hit accoording to isZombieHit are removed from this list
/// zombies taht are spawned by spawn are added to the list
/// all other zombies are rotated and moved by updateZombie
let updateZombies (isZombieHit:Zombie->bool) (dt:float32) (zombies:List<Zombie>) (playerPosition:Vector2) (dif:Difficulty) =
  let spawnChange =
    match dif with 
    | Retard ->
      if zombies.Length < 15 then 100
      else 0
    | Easy ->
      if zombies.Length < 30 then 100
      else 5
    | Medium ->
      if zombies.Length < 30 then 100
      else 10
    | Hard ->
      if zombies.Length < 50 then 100
      else 10
    | Master ->
      if zombies.Length < 50 then 100
      else 15
    | Asian ->
      if zombies.Length < 75 then 100
      else 20
  let zombies =
    if r.Next(100)+1 < spawnChange then spawn playerPosition :: zombies
    else zombies
  let amountOfZombiesToStartWith = zombies.Length;
  let zombies = filter isZombieHit ( map (updateZombie dt playerPosition) zombies )
  let amountOfZombiesRemaining = zombies.Length;
  zombies, amountOfZombiesToStartWith - amountOfZombiesRemaining
  

/// This method turns a zombie into a drawable that can be used in the Draw method in the GameLogic
let drawZombie (zombie:Zombie) : Drawable =
  {
    Drawable.Position = zombie.Position
    Drawable.Rotation = zombie.Rotation
    Drawable.Image    = zombie.Image
  }

