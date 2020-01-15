[<RequireQualifiedAccess>]
module CopaFilmes.Frontend.FilmesPageComponent

open CopaFilmes.Frontend.Core
open CopaFilmes.Shared
open Fable.React
open Fable.React.Props
open Fulma


let filmeItemComponent = FunctionComponent.Of (fun (props: {| Filme: Model.Filme
                                                              Checked: bool
                                                              OnCheck: Model.Filme -> bool -> unit |}) ->
    div [ Class "filme-item" ][
    
        CheckboxComponent.``default`` {| Checked = props.Checked
                                         OnCheck = props.OnCheck props.Filme |}
        
        div [][
            div [ Class "titulo" ][
                str props.Filme.Titulo
            ]
                
            div [ Class "ano" ][
                ofInt props.Filme.Ano
            ]
        ]
    ]
)


let ``default`` = FunctionComponent.Of (fun (props: {| Filmes: Model.Filme list
                                                       OnSelected: Model.Filme list -> unit |}) ->
    
    let state = Hooks.useState {| SelectedList = Map.empty<string, Model.Filme> |}
    
    let selected =
        state.current.SelectedList
        |> Seq.map (fun x -> x.Value)
        |> Seq.toList
        
    let selectedCount = Map.count state.current.SelectedList
        
    let events =
        {|
            OnFilmeCheck = fun (filme: Model.Filme) (checked': bool) ->
                let swap =
                    if checked'
                    then Map.add filme.Id filme
                    else Map.remove filme.Id
                
                state.update (fun state -> {| state with SelectedList = state.SelectedList |> swap |} )
                
            OnGerarCampeonatoClick = fun _ ->
                props.OnSelected selected
        |}
    
        
    
    div [ Class "page-filmes" ][
        
        HeaderComponent.``default`` {| PageTitle = "Fase de Seleção"
                                       PageDescription = "Selecione 8 filmes que você deseja que entrem na competição e
                                                          depois pressione o botão Gerar Meu Campeonato para prosseguir" |}
                                              
        ScrollObserverComponent.``default`` { Class = "header-scrolled" }
        
        div [ Class "toolbar" ][
            
            Button.button [ Button.Color IsDark
                            Button.Disabled (selectedCount <> 8)
                            Button.OnClick events.OnGerarCampeonatoClick ][
                str "Gerar Meu Campeonato"
            ]
            
            div [][
                str (sprintf "Selecionados %d de 8 filmes" selectedCount)
            ]
        ]
        
        div [ Class "filme-list" ][
            
            props.Filmes
            |> List.map (fun filme ->
                
                div [ Key filme.Id ][
                    
                    filmeItemComponent
                        {| Filme = filme
                           Checked = state.current.SelectedList.ContainsKey filme.Id
                           OnCheck = events.OnFilmeCheck |}
                ]
            )  
            |> (fun list ->
                    // Workaround para a recursividade da ultima linha funcionar (flexbox)
                    let maxColumns = 4
                    let ghostItems = [ for i in 0 .. maxColumns - 1 -> div [ Key (string (-1 - i)) ][] ]
                    list @ ghostItems)
            |> ofList
        ]
    ]
)
