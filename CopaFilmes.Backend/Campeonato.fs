[<RequireQualifiedAccess>]
module CopaFilmes.Backend.Campeonato


open CopaFilmes.Shared


let chavear filmes =
    let len = List.length filmes
    filmes
    |> List.take (len / 2)
    |> List.mapi (fun i x -> [ x; filmes.[len - i - 1] ])
    
let ordenar =
    List.sortBy (fun (x: Model.Filme) -> x.Titulo)
    
let disputarPartida =
    List.sortByDescending (fun (x: Model.Filme) -> x.Nota)

let disputarChaves =
    List.map disputarPartida
    >> List.map List.head
    
    
let disputarCampeonato filmes =
    if List.length filmes = 0 || List.length filmes % 2 <> 0 then
        None
    else
        let rec disputarCampeonato = function
            | [] -> []
            | [ partida ] -> // Partida Final
                partida
                |> disputarPartida
                
            | chaves ->
                chaves
                |> disputarChaves
                |> chavear 
                |> disputarCampeonato
            
        let podio =
            filmes
            |> ordenar
            |> chavear
            |> disputarCampeonato
        
        Some podio

