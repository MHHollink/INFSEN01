module GameLogic

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

open Player
open Gamestate
open Drawable
open Zombie
open Utils

/// Update method called from the C# Main application. This method processes all the updates before returning the
/// new GameState. The parameters are :
///     Keyboard state     containing information about key presses
///     Mouse state        containing XY position of mouse as well as clickings
///     Delta Tie          the time passed since the last update loop
///     Gamestate          the old version of the gamestate AKA the one from the last update.
let update (keyState:KeyboardState) (mouseState:MouseState) (delta:float32) (gamestate:Gamestate) = 
  
  // A triple for the shooting machenic. it contains :
  //   bool  -> true if gun has been fired, false if not
  //   Gun   -> gun with new cooldown timer (or Ready)
  //   fired -> the amount of bullets fired by the gun (AKA the amount the score is reduced)
  let isShooting, newGun, bulletsFired = 
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
  
  // A function that returns a new bullet at player position
  let fireGun() =
    {
      Bullet.Position = gamestate.Player.Position
      Bullet.Rotation = gamestate.Player.Rotation
    }
  
  // recursive function to check which zombies are hit
  let rec isZombieHit (bullets:List<Bullet>) (z:Zombie) =
    match bullets with
    | [] -> true
    | p :: ps ->
      if Vector2.Distance(p.Position, z.Position) < 50.0f then
        false
      else
        isZombieHit ps z

  // recursive function to see which bullets have hit a target
  let rec hasKilled (zombies:List<Zombie>) (b:Bullet) =
    match zombies with
    | [] -> true
    | p :: ps ->
      if Vector2.Distance(p.Position, b.Position) < 40.0f then
        false
      else
        hasKilled ps b
  
  // recursive function to check if you are touching !!DEATH!! 
  let rec zombieHasntKilledYou (zombies:List<Zombie>) (player:Player) = 
    match zombies with
    | [] -> true
    | p :: ps ->
      if Vector2.Distance(p.Position, player.Position) < 50.0f then
        false
      else 
        zombieHasntKilledYou ps player
  
  // difficulty according to current score
  let newDifficulty = 
    if gamestate.Score   <  100 then Difficulty.Retard
    elif gamestate.Score <  250 then Difficulty.Easy
    elif gamestate.Score <  750 then Difficulty.Medium
    elif gamestate.Score < 2000 then Difficulty.Hard
    elif gamestate.Score < 5000 then Difficulty.Master
    else                             Difficulty.Asian

  // updated list of zombies (tupled with the amount of killed zombies) [ List<Zombie> * int ]
  let newListOfZombies = updateZombies (isZombieHit gamestate.Bullets) delta gamestate.Zombies gamestate.Player.Position gamestate.Difficulty
  
  // updated list of bullets
  let newListOfBullets = updateBullets fireGun isShooting (hasKilled gamestate.Zombies) delta gamestate.Bullets 
  
  // a float32 representation of how accurate your bullets are
  let newAccuracy = 
    if (gamestate.BulletsFired + bulletsFired) = 0 then 0.0f
    else (float32)(gamestate.Kills + snd(newListOfZombies)) / (float32)(gamestate.BulletsFired + bulletsFired)
  
  // a new highscore if score was bigger then old highscore
  let newHighscore = 
    if (gamestate.Score > gamestate.Highscore) then gamestate.Score
    else gamestate.Highscore

  let alive = zombieHasntKilledYou gamestate.Zombies gamestate.Player
  if alive then
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
                     Gamestate.Alive        = alive
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
                     Gamestate.Alive        = alive
    }
  else
    {
      gamestate with Gamestate.Cursor       = newCursor mouseState gamestate.Cursor
                     Gamestate.Alive        = alive
    }

// Draw method called by the C# application that turns all bullets, zombies, player, cursor into Drawable records
let draw (gamestate:Gamestate) : seq<Drawable> = 
  let bullets =
    map drawBullet gamestate.Bullets
  let zombies = 
    map drawZombie gamestate.Zombies
  [ drawPlayer gamestate.Player ] @ zombies @ [ drawCursor gamestate.Cursor ] @ bullets |> Seq.ofList