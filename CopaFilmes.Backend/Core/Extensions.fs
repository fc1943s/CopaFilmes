module CopaFilmes.Backend.Core.Extensions


module Async =
    let wrap x =
        async { return x }
