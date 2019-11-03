open System
open System.Diagnostics

let makeErrorMessage msg: string =
    sprintf "SafeAB: %s" msg

let isAllowedUrl (url: Uri): bool =
    url.Host = "localhost"

let pickUrl (args: string []): Result<Uri, string> =
    let urls: string [] = Array.filter (fun (arg: string) -> arg = "localhost") args
    let parsedUrl = Uri(urls.[0])
    if urls.Length = 1 then Ok parsedUrl
    else Error "Invalid target url count."

let validate (argv: string []): Result<string, string> =
    let url = pickUrl argv
    match url with
    | Error msg -> Error msg
    | Ok url ->
        match isAllowedUrl url with
        | false -> Error "Disallowed URL."
        | true -> Ok ""

let echoErrorMessage (message: string): int =
    printfn "%s" message
    1

let executeCommand (args: string): int =
    let command = new ProcessStartInfo(FileName = "echo")
    command.Arguments <- sprintf "'%s'" args
    command.UseShellExecute <- true

    Process.Start(command) |> ignore
    
    0

[<EntryPoint>]
let main argv =
    printfn "%s" "Using SafeAB now."
    
    match validate argv with
    | Error message -> echoErrorMessage message
    | Ok arguments -> executeCommand arguments
