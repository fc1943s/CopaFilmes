[<RequireQualifiedAccess>]
module CopaFilmes.Frontend.Core.ScrollObserverComponent

open System
open Browser
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props
open Fable.Core

type Props = {
    Class: string
}
    
type ScrollObserverComponent (initialProps) =
    inherit PureComponent<Props, unit>(initialProps)
    
    let id = sprintf "scroll-observer-%d" (Random().Next ())
    
    override this.componentDidMount () =
        let observer = JsInterop.createNew Dom.window?IntersectionObserver (fun entries ->
            if (Array.head entries)?boundingClientRect?y < 0 
            then Dom.document.body.classList.add this.props.Class
            else Dom.document.body.classList.remove this.props.Class
        )
        observer?observe (Dom.document.querySelector ("#" + id))
        
    override _.render () =
        div [ Id id
              Class "scroll-anchor" ][]
        
let ``default`` props =
    ofType<ScrollObserverComponent, _, _> props []
