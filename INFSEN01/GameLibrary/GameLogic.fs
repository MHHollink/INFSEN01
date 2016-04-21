module GameLogic

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

open Player
open Gamestate
open Drawable
open Zombie
open Utils

let update (keyState:KeyboardState) (mouseState:MouseState) (delta:float32) (gamestate:Gamestate) = 
  
  let isShooting, newGun, bulletsFired = // Shooting and gun state
    match gamestate.Gun with 
    | Ready -> // If gun state ready
      if mouseState.LeftButton.Equals(ButtonState.Pressed) then
        true, Cooldown 0.2f, 1
      else 
        false, Ready, 0
    | Cooldown time -> // if cooldown for 'time' seconds
      if time > 0.0f then 
        if mouseState.LeftButton.Equals(ButtonState.Pressed) then
            false, Cooldown(time - (delta * 0.125f)), 0
        else
            false, Cooldown(time - (delta * 10.25f)), 0
      else 
        false, Ready, 0
  
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
  let rec zombieHasntKilledYou (zombies:List<Zombie>) (player:Player) = 
    match zombies with
    | [] -> true
    | p :: ps ->
      if Vector2.Distance(p.Position, player.Position) < 50.0f then
        false
      else 
        zombieHasntKilledYou ps player
  
  let newDifficulty = 
    if gamestate.Score   <  100 then Difficulty.Retard
    elif gamestate.Score <  250 then Difficulty.Easy
    elif gamestate.Score <  750 then Difficulty.Medium
    elif gamestate.Score < 2000 then Difficulty.Hard
    elif gamestate.Score < 5000 then Difficulty.Master
    else                             Difficulty.Asian

  let newListOfZombies = updateZombies (isZombieHit gamestate.Bullets) delta gamestate.Zombies gamestate.Player.Position gamestate.Difficulty
  let newListOfBullets = updateBullets fireGun isShooting (hasKilled gamestate.Zombies) delta gamestate.Bullets 
  let newAccuracy = 
    if (gamestate.BulletsFired + bulletsFired) = 0 then 0.0f
    else (float32)(gamestate.Kills + snd(newListOfZombies)) / (float32)(gamestate.BulletsFired + bulletsFired)
  let newHighscore = 
    if (gamestate.Score > gamestate.Highscore) then gamestate.Score
    else gamestate.Highscore
  if zombieHasntKilledYou gamestate.Zombies gamestate.Player then
  {
    gamestate with Gamestate.Player       = updatePlayer keyState mouseState delta gamestate.Player
                   Gamestate.Cursor       = newCursor mouseState gamestate.Cursor
                   Gamestate.Zombies      = fst(newListOfZombies)
                   Gamestate.Bullets      = newListOfBullets
                   Gamestate.Gun          = newGun
                   Gamestate.Score        = gamestate.Score - bulletsFired + (snd(newListOfZombies) * 4)
                   Gamestate.Difficulty   = newDifficulty
                   Gamestate.BulletsFired = gamestate.BulletsFired + bulletsFired
                   Gamestate.Kills        = gamestate.Kills + snd(newListOfZombies)
                   Gamestate.Accuracy     = newAccuracy
                   Gamestate.Highscore    = newHighscore
  }
  elif keyState.IsKeyDown(Keys.Enter) then
  {
    gamestate with Gamestate.Player       = Gamestate.newPlayer
                   Gamestate.Cursor       = newCursor mouseState gamestate.Cursor
                   Gamestate.Zombies      = []
                   Gamestate.Bullets      = []
                   Gamestate.Gun          = Ready
                   Gamestate.Score        = 0
                   Gamestate.Difficulty   = Difficulty.Retard
                   Gamestate.BulletsFired = 0
                   Gamestate.Kills        = 0
                   Gamestate.Accuracy     = 0.0f
  }
  else
  {
      gamestate with Gamestate.Cursor       = newCursor mouseState gamestate.Cursor
  }

let draw (gamestate:Gamestate) : seq<Drawable> = 
  let bullets =
    map drawBullet gamestate.Bullets
  let zombies = 
    map drawZombie gamestate.Zombies
  [ drawPlayer gamestate.Player ] @ zombies @ [ drawCursor gamestate.Cursor ] @ bullets |> Seq.ofList