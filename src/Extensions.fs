[<AutoOpen>]
module Extensions

open Elmish

module Async =
    let map f op =
        async {
            let! x = op
            let value = f x
            return value
        }

module Cmd =
    let fromAsync (operation: Async<'msg>): Cmd<'msg> =
        let delayedCmd (dispatch: 'msg -> unit): unit =
            let delayedDispatch =
                async {
                    let! msg = operation
                    dispatch msg }

            Async.StartImmediate delayedDispatch

        Cmd.ofSub delayedCmd

[<RequireQualifiedAccess>]
type Deferred<'t> =
    | HasNotStartedYet
    | InProgress
    | Resolved of 't

type AsyncOperationStatus<'t> =
    | Started
    | Finished of 't
