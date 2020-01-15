[<RequireQualifiedAccess>]
module CopaFilmes.Shared.Model

type Filme =
    { Id: string
      Titulo: string
      Ano: int
      Nota: decimal }

type Api =
    { ConsultarFilmes: unit -> Async<Filme list>
      DisputarCampeonato: Filme list -> Async<Filme list option> }

