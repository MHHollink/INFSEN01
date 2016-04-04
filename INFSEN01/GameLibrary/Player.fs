module Player

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

type Player = 
  {
    Position : Vector2
    Velocity : Vector2
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
  } 