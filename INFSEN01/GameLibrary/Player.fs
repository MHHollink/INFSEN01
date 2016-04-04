module Player

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

open System

// Player Information
type Player = 
  {
    Position : Vector2
    Velocity : Vector2
    Rotation : float32
  }

let updatePlayer (ks:KeyboardState) (ms:MouseState) (dt:float32) (player:Player) =
  
  let speed = 1000.0f;

  let player = 
    if ks.IsKeyDown(Keys.W) then 
      { player with Velocity = player.Velocity - Vector2.UnitY * speed * dt }
    else 
      player
  let player = 
    if ks.IsKeyDown(Keys.A) then 
      { player with Velocity = player.Velocity - Vector2.UnitX * speed * dt }
    else 
      player
  let player = 
    if ks.IsKeyDown(Keys.S) then 
      { player with Velocity = player.Velocity + Vector2.UnitY * speed * dt }
    else 
      player
  let player = 
    if ks.IsKeyDown(Keys.D) then 
      { player with Velocity = player.Velocity + Vector2.UnitX * speed * dt }
    else 
      player

  {
    player with Position = player.Position + player.Velocity * dt;
                Velocity = player.Velocity * 0.9f;
                Rotation = 0.0f//Math.Atan2(ms.Y - player.Position.Y, ms.X - player.Position.X)
  }         

// Cursor information
type Cursor = 
  {
    Position : Vector2
    Rotation : float32
  }

let newCursor (ms:MouseState) (cursor:Cursor): 
  Cursor =
    {
      cursor with Position = ms.Position.ToVector2();
    }
  



// Bullet information
type Bullet =
  {
    Position : Vector2
    Rotation : float32
  }