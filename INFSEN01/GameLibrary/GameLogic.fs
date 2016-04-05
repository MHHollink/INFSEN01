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
  let rec isZombieHit (projectiles:List<Bullet>) (z:Zombie) =
    match projectiles with
    | [] -> false
    | p :: ps ->
      if Vector2.Distance(p.Position, z.Position) < 120.0f then
        true
      else
        isZombieHit ps z
        
  {
    gamestate with Gamestate.Player  = updatePlayer ks ms dt gamestate.Player
                   Gamestate.Cursor  = newCursor ms gamestate.Cursor
                   Gamestate.Bullets = updateBullets fireGun isShooting dt gamestate.Bullets
                   Gamestate.Gun     = newGun
  }

let draw (gamestate:Gamestate) : seq<Drawable> = 
  let bullets =
    map drawBullet gamestate.Bullets
  [
      drawPlayer gamestate.Player
  ] @ 
  [
      drawCursor gamestate.Cursor
  ] @ bullets
  |> Seq.ofList