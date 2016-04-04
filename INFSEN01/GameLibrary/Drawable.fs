module Drawable

open Microsoft.Xna.Framework

type Drawable = 
  {
    Position : Vector2
    Rotation : float32
    Image    : string
  }