module GameLogic

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

open Player
open Gamestate
open Drawable

let update (ks:KeyboardState) (ms:MouseState) (dt:float32) (gamestate:Gamestate) = 
  {
    gamestate with Gamestate.Player = updatePlayer ks ms dt gamestate.Player
                   Gamestate.Cursor = newCursor ms gamestate.Cursor
  }

let draw (gamestate:Gamestate) : seq<Drawable> = 
    [
        {
            Drawable.Position = gamestate.Player.Position
            Drawable.Rotation = gamestate.Player.Rotation
            Drawable.Image    = "player"
        }
    ] @ 
    [
        {
            Drawable.Position = gamestate.Cursor.Position
            Drawable.Rotation = gamestate.Cursor.Rotation
            Drawable.Image    = "cursor" 
        }
    ]
    |> Seq.ofList