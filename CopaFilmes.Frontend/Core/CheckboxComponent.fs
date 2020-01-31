[<RequireQualifiedAccess>]
module CopaFilmes.Frontend.Core.CheckboxComponent

open Fable.React
open Fable.React.Props
open Browser.Types

let ``default`` = FunctionComponent.Of (fun (props: {| Checked: bool
                                                       OnCheck: bool -> unit |}) ->
    let events = {|
        OnClick = fun _ ->
            props.OnCheck (not props.Checked)
            
        OnKeyUp = fun (e: KeyboardEvent) ->
           if [ "Enter"; " " ] |> List.contains e.key then
                props.OnCheck (not props.Checked)
    |}
    
    div [ Class "checkbox" ][
        div [ OnClick events.OnClick
              OnKeyUp events.OnKeyUp
              TabIndex 0 ][
            div [ Class (if props.Checked then "checked" else "") ][]
        ]
    ]
, memoizeWith = equalsButFunctions)
