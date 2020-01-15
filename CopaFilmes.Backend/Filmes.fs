[<RequireQualifiedAccess>]
module CopaFilmes.Backend.Filmes


open CopaFilmes.Shared
open FSharp.Data      


[<Literal>]
let jsonUrl = "http://copafilmes.azurewebsites.net/api/filmes"


type FilmesJson = JsonProvider<""" [{"id":"tt3606756","titulo":"Os Incríveis 2","ano":2018,"nota":8.5}] """> 
      
      
let converterJson =
    Array.map (fun (filme: FilmesJson.Root) ->
        { Model.Id = filme.Id
          Model.Titulo = filme.Titulo
          Model.Ano = filme.Ano
          Model.Nota = filme.Nota }
    ) >> Array.toList
    
let consultarAsync = async {
    let! result =
        FilmesJson.AsyncLoad jsonUrl
        |> Async.Catch
        
    match result with
    | Choice1Of2 filmes ->
        return filmes
               |> converterJson
        
    | Choice2Of2 ex ->
        printfn "Erro ao consultar filmes: %A" ex.Message
        raise ex
        return []
}

