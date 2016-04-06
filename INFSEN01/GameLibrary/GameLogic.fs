module GameLogic

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

open Player
open Gamestate
open Drawable
open Zombie
open Utils

let update (ks:KeyboardState) (ms:MouseState) (dt:float32) (gamestate:Gamestate) = 
  let isShooting, newGun =
    match gamestate.Gun with 
    | Ready ->
      if ms.LeftButton.Equals(ButtonState.Pressed) then
        (fun () -> true), Cooldown 0.2f
      else 
        (fun () -> false), Ready
    | Cooldown t -> 
      if t > 0.0f then 
        (fun () -> false), Cooldown(t-dt)
      else 
        (fun () -> false), Ready
  let fireGun() =
    {
      Bullet.Position = gamestate.Player.Position
      Bullet.Rotation = gamestate.Player.Rotation
    }
  let rec isZombieHit (bullets:List<Bullet>) (z:Zombie) =
    match bullets with
    | [] -> true
    | p :: ps ->
      if Vector2.Distance(p.Position, z.Position) < 50.0f then
        false
      else
        isZombieHit ps z
  let rec hasKilled (zombies:List<Zombie>) (b:Bullet) =
    match zombies with
    | [] -> true
    | p :: ps ->
      if Vector2.Distance(p.Position, b.Position) < 40.0f then
        false
      else
        hasKilled ps b
  let zombieHitList = isZombieHit gamestate.Bullets
  let bulletHitList = hasKilled gamestate.Zombies
  {
    gamestate with Gamestate.Player  = updatePlayer ks ms dt gamestate.Player
                   Gamestate.Cursor  = newCursor ms gamestate.Cursor
                   Gamestate.Zombies = updateZombies zombieHitList dt gamestate.Zombies gamestate.Player.Position 
                   Gamestate.Bullets = updateBullets fireGun isShooting bulletHitList dt gamestate.Bullets
                   Gamestate.Gun     = newGun
  }

let draw (gamestate:Gamestate) : seq<Drawable> = 
  let bullets =
    map drawBullet gamestate.Bullets
  let zombies = 
    map drawZombie gamestate.Zombies
  [
      drawPlayer gamestate.Player
  ] @ 
  [
      drawCursor gamestate.Cursor
  ] @ bullets @ zombies
  |> Seq.ofList