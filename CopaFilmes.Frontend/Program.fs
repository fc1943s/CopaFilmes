module CopaFilmes.Frontend.Program

open CopaFilmes.Shared
open Elmish
open Elmish.React
open Fable.React
open Fable.Remoting.Client
open Fable.Core
open Fulma
open Browser

#if DEBUG
open Elmish.HMR
#endif


JsInterop.importAll "typeface-roboto"
JsInterop.importAll "typeface-arimo"
JsInterop.importAll "intersection-observer"
JsInterop.importAll "./node_modules/bulma/bulma.sass"
JsInterop.importAll "./public/index.scss"


type Message =
    | ConsultarFilmes
    | ConsultaErro of exn
    | FilmesLista of Model.Filme list
    
    | DisputarCampeonato of Model.Filme list
    | DisputaErro of exn
    | PodioLista of Model.Filme list option
    
    | PodioVoltar
    

type State =
    { Filmes: Model.Filme list
      Podio: Model.Filme list
      Error: string }
    static member inline Default =
        { Filmes = []
          Podio = []
          Error = "" }
    
type Props =
    { State: State
      Dispatch: Message -> unit }


let api =
    Remoting.createApi ()
    |> Remoting.buildProxy<Model.Api>
    

let init () =
    State.Default, Cmd.ofMsg ConsultarFilmes
    
    
let update (msg: Message) (state: State) =
    match msg with
    | ConsultarFilmes ->
        let cmd =
            Cmd.OfAsync.either 
                api.ConsultarFilmes ()
                FilmesLista
                ConsultaErro
        
        state, cmd
        
    | ConsultaErro ex ->
        { state with Error = "Erro ao consultar lista de filmes: " + ex.Message }, Cmd.none
        
    | FilmesLista filmes ->
        { state with Filmes = filmes }, Cmd.none
        
    | DisputarCampeonato filmes ->
        let cmd =
            Cmd.OfAsync.either
                api.DisputarCampeonato filmes
                PodioLista
                DisputaErro
            
        state, cmd
        
    | PodioLista filmes ->
        let state =
            match filmes with
            | Some filmes -> { state with Podio = filmes }
            | None -> { state with Error = "Lista de filmes inválida." }
                
        state, Cmd.none
        
    | DisputaErro ex ->
        { state with Error = "Erro ao realizar a disputa: " + ex.Message }, Cmd.none
        
    | PodioVoltar ->
        { state with Podio = [] }, Cmd.none
    
    
let lazyView props =
    
    let events = {|
        OnFilmesSelected = fun (filmes: Model.Filme list) ->
            props.Dispatch (DisputarCampeonato filmes)
            
        OnPodioVoltar = fun _ ->
            props.Dispatch PodioVoltar
            
        OnErrorClose = fun _ ->
            Dom.window.location.href <- Dom.window.location.href
    |}
    
    Container.container [ Container.IsFluid ][
       
        if props.State.Error <> "" then
            Notification.notification [ Notification.Color IsDanger ][
               
                Notification.delete [ Props [ Props.OnClick events.OnErrorClose ]][]
                str props.State.Error
            ] 
        
        match props.State.Filmes, props.State.Podio with
        | [], _ ->
            Progress.progress [][]

        | filmes, [] ->
            FilmesPageComponent.``default`` {| Filmes = filmes
                                               OnSelected = events.OnFilmesSelected |}
                                           
        | _, podio ->
            PodioPageComponent.``default`` {| Filmes = podio
                                              OnVoltar = events.OnPodioVoltar |}
            
    ]
    
    
let viewWrapper =
    fun (state: State) (dispatch: Message -> unit) ->
        FunctionComponent.Of lazyView
            { State = state
              Dispatch = dispatch }

Program.mkProgram init update viewWrapper
|> Program.withReactSynchronous "app"
|> Program.run

