[<RequireQualifiedAccess>]
module CopaFilmes.Frontend.PodioPageComponent

open Fable.React
open Fable.React.Props
open Fulma
open CopaFilmes.Shared


let ``default`` = FunctionComponent.Of (fun (props: {| Filmes: Model.Filme list
                                                       OnVoltar: unit -> unit |}) ->
    let events = {|
        OnVoltarClick = fun _ ->
            props.OnVoltar ()
    |}
    
    div [ Class "page-podio" ][
        
        HeaderComponent.``default`` {| PageTitle = "Resultado Final"
                                       PageDescription = "Veja o resultado final do Campeonato de Filmes de forma simples e rápida" |}
        
        div [ Class "ranking" ][
            
            props.Filmes
            |> List.mapi (fun i filme ->
                div [ Key filme.Id ][
                    div [][
                        div [][ str (sprintf "%dº" (i + 1)) ]
                    ]
                    div [][
                        div [][ str filme.Titulo ]
                    ]
                ]
            )
            |> ofList
        ]
            
        Button.button [ Button.Color IsDark
                        Button.OnClick events.OnVoltarClick ][
            str "< Voltar"
        ]
    ]
, memoizeWith = equalsButFunctions)
