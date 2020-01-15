module CopaFilmes.Backend.Tests.CampeonatoTests

open Xunit
open CopaFilmes.Backend
open CopaFilmes.Shared
open Xunit.Abstractions
open FsUnit.Xunit

let json = """
[{"id":"tt3606756","titulo":"Os Incríveis 2","ano":2018,"nota":8.5},
{"id":"tt4881806","titulo":"Jurassic World: Reino Ameaçado","ano":2018,"nota":6.7},
{"id":"tt5164214","titulo":"Oito Mulheres e um Segredo","ano":2018,"nota":6.3},
{"id":"tt7784604","titulo":"Hereditário","ano":2018,"nota":7.8},
{"id":"tt4154756","titulo":"Vingadores: Guerra Infinita","ano":2018,"nota":8.8},
{"id":"tt5463162","titulo":"Deadpool 2","ano":2018,"nota":8.1},
{"id":"tt3778644","titulo":"Han Solo: Uma História Star Wars","ano":2018,"nota":7.2},
{"id":"tt3501632","titulo":"Thor: Ragnarok","ano":2017,"nota":7.9},
{"id":"tt2854926","titulo":"Te Peguei!","ano":2018,"nota":7.1},
{"id":"tt0317705","titulo":"Os Incríveis","ano":2004,"nota":8.0},
{"id":"tt3799232","titulo":"A Barraca do Beijo","ano":2018,"nota":6.4},
{"id":"tt1365519","titulo":"Tomb Raider: A Origem","ano":2018,"nota":6.5},
{"id":"tt1825683","titulo":"Pantera Negra","ano":2018,"nota":7.5},
{"id":"tt5834262","titulo":"Hotel Artemis","ano":2018,"nota":6.3},
{"id":"tt7690670","titulo":"Superfly","ano":2018,"nota":5.1},
{"id":"tt6499752","titulo":"Upgrade","ano":2018,"nota":7.8}]
"""


type CampeonatoTests (output: ITestOutputHelper) =
    
    let findFromTitulo filmes titulo =
        filmes |> List.find (fun (x: Model.Filme) -> x.Titulo = titulo)
    
    [<Fact>]
    member _.Tests () =
        
        //
        // Casos fixos
        //
        
        let filmes =
            Filmes.FilmesJson.Parse json
            |> Filmes.converterJson
            
        let casos =
            [ [ "Os Incríveis 2"
                "Jurassic World: Reino Ameaçado"
                "Oito Mulheres e um Segredo"
                "Hereditário"
                "Vingadores: Guerra Infinita"
                "Deadpool 2"
                "Han Solo: Uma História Star Wars"
                "Thor: Ragnarok" ]
             
              [ "Deadpool 2"
                "Han Solo: Uma História Star Wars"
                "Hereditário"
                "Jurassic World: Reino Ameaçado"
                "Oito Mulheres e um Segredo"
                "Os Incríveis 2"
                "Thor: Ragnarok"
                "Vingadores: Guerra Infinita" ]
                 
              [ "Vingadores: Guerra Infinita"
                "Thor: Ragnarok"
                "Os Incríveis 2"
                "Jurassic World: Reino Ameaçado" ]
              
              [ "Vingadores: Guerra Infinita"
                "Os Incríveis 2" ]
              
              [ "Vingadores: Guerra Infinita" ] ]
            
        let rec innerLoop casos =
            match casos with
            | case :: expected :: tail ->
                let case =
                    case
                    |> List.map (findFromTitulo filmes)
                    
                let result =
                    if List.length case = List.length expected then
                        case
                        |> Campeonato.ordenar
                    else
                        case
                        |> Campeonato.chavear
                        |> Campeonato.disputarChaves
                    
                result
                |> List.map (fun x -> x.Titulo)
                |> should equal expected
                
                innerLoop (expected :: tail)
                
            | _ -> ()
            
        innerLoop casos
    
        //
        // Filmes com notas iguais
        //
        
        let a = filmes |> List.find (fun x -> x.Titulo = "Hotel Artemis")
        let b = filmes |> List.find (fun x -> x.Titulo = "Oito Mulheres e um Segredo")
        
        [ b; a ]
        |> Campeonato.disputarCampeonato
        |> Option.defaultValue []
        |> should equal [ a; b ]
        
        //
        // Testa o campeão de forma incremental
        // 
        
        for i in 0 .. filmes.Length do
            output.WriteLine (sprintf "\nQuantidade de filmes: %d" i)
            
            let task () =
                let competidores =
                    filmes
                    |> List.truncate i
                        
                let campeao =
                    competidores
                    |> Campeonato.disputarCampeonato
                    |> Option.defaultValue []
                    |> List.truncate 1
                    
                competidores
                |> List.sortByDescending (fun x -> x.Nota)
                |> List.truncate 1
                |> should equal campeao
                
                output.WriteLine (sprintf "Sucesso. Campeão: %A" (campeao |> List.map (fun x -> x.Titulo)))
                
            if i % 2 = 0 then
                task ()
            else
                task |> shouldFail
                output.WriteLine "Erro"
                
                

