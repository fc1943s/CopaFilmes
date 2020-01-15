open CopaFilmes.Backend.Core.Extensions
open CopaFilmes.Shared
open CopaFilmes.Backend
open Saturn
open Fable.Remoting.Server
open Fable.Remoting.Giraffe

let api : Model.Api =
    { ConsultarFilmes = fun () -> Filmes.consultarAsync 
      DisputarCampeonato = Campeonato.disputarCampeonato >> Async.wrap }
    
let listen () =
    let webApp = 
        Remoting.createApi ()
        |> Remoting.fromValue api
        |> Remoting.buildHttpHandler 

    let app = application {
        url "http://0.0.0.0:8085/"
        use_router webApp
    }

    run app

[<EntryPoint>]
let main _ =
    listen ()
    0
