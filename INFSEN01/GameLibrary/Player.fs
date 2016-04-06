module Player

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

open System
open Utils
open Drawable

// Player Information
type Player = 
  {
    Position : Vector2
    Velocity : Vector2
    Rotation : float
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
    player with Position = player.Position + player.Velocity * dt
                Velocity = player.Velocity * 0.9f
                Rotation = Math.Atan2((float) ms.Y - (float) player.Position.Y, (float) ms.X - (float) player.Position.X)
  }         

let drawPlayer (player:Player) : Drawable =
  {
    Drawable.Position = player.Position
    Drawable.Rotation = player.Rotation
    Drawable.Image    = "player"
  }

// Cursor information
type Cursor = 
  {
    Position : Vector2
    Rotation : float
  }

let newCursor (ms:MouseState) (cursor:Cursor): 
  Cursor =
    {
      cursor with Position = ms.Position.ToVector2()
    }

let drawCursor (cursor:Cursor) : Drawable =
  {
    Drawable.Position = cursor.Position
    Drawable.Rotation = cursor.Rotation
    Drawable.Image    = "cursor"
  }
  
// Gun information
type GunStatus = 
  | Cooldown of float32
  | Ready

// Bullet information
type Bullet =
  {
    Position : Vector2
    Rotation : float
  }

let updateBullet (dt:float32) (bullet:Bullet) : Bullet =
  let speed  = 850.0
  let deltaY = (float32) ( speed * ((Math.Sin ( bullet.Rotation )))) 
  let deltaX = (float32) ( speed * ((Math.Cos ( bullet.Rotation )))) 
  {
    bullet with Position = Vector2(bullet.Position.X + (deltaX * dt), bullet.Position.Y + (deltaY * dt))
  }

let updateBullets (newBullet:Unit->Bullet) (createNew:Unit->bool) (hasKilled:Bullet->bool) (dt:float32) (bullets:List<Bullet>) =
  let bullets = 
    if createNew() then
      newBullet() :: bullets
    else
      bullets
  let bullets = map (updateBullet dt) bullets
  let insideScreen (b:Bullet) : bool =
    b.Position.Y > -100.0f || b.Position.Y < 800.0f || b.Position.X > -100.0f || b.Position.X < 800.0f 
  let bullets = filter insideScreen bullets
  filter hasKilled bullets


let drawBullet (bullet:Bullet) : Drawable =
  {
    Drawable.Position = bullet.Position
    Drawable.Rotation = bullet.Rotation
    Drawable.Image    = "bullet"
  }