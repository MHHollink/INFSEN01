module Utils

let rec filter (b:'a->bool) (l:List<'a>) : List<'a> =
  match l with
  | [] -> []
  | x :: xs ->
    if b x then
      x :: filter b xs
    else
      filter b xs

let rec map (f:'a->'b) (l:List<'a>) : List<'b> =
  match l with
  | [] -> []
  | x :: y ->  
    f x :: map f y
      
type Difficulty = 
  | Retard
  | Easy
  | Medium
  | Hard
  | Master
  | Asian