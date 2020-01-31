[<RequireQualifiedAccess>]
module CopaFilmes.Frontend.HeaderComponent

open Fable.React
open Fable.React.Props

let ``default`` = FunctionComponent.Of (fun (props: {| PageTitle: string
                                                       PageDescription: string |}) ->
    div [ Class "header" ][
        
        div [ Class "app-title" ][
            str "Campeonato de Filmes"
        ]
        
        div [ Class "page-title" ][
            str props.PageTitle
        ]
        
        hr []
        
        div [ Class "page-description" ][
            str props.PageDescription
        ]
    ]
, memoizeWith = equalsButFunctions)
