open System
open System
open System.Diagnostics

let errorMessage = "Invalid arguments."

type Arguments =
    { nValue: int
      cValue: int
      url: Uri }

let validateOptions (argv: string []): Result<(int * int), string> =
    let nOption = argv.[0]
    let nValue = argv.[1]
    let cOption = argv.[2]
    let cValue = argv.[3]

    let nOptionValid =
        match nOption with
        | "-n" -> true
        | _ -> false
    let cOptionValid =
        match cOption with
        | "-c" -> true
        | _ -> false
    
    if not nOptionValid || not cOptionValid then
        Error errorMessage
    else
        let nValueParseResult = Int32.TryParse nValue
        let cValueParseResult = Int32.TryParse cValue
        match (nValueParseResult, cValueParseResult) with
        | ((true, n), (true, c)) -> Ok (n, c)
        | _ -> Error errorMessage

let validateTargetUrl (url : string): Result<Uri, string> =
    let parsedUrl = Uri(url)
    let urlValid: bool =
        if parsedUrl.Host = "localhost" then true
        else false
    match urlValid with
    | true -> Ok parsedUrl
    | false -> Error "Invalid target url."

let validate (argv: string []): Result<Arguments, string> =
    match validateOptions argv with
    | Error message -> Error message
    | Ok (n, c) ->
        match validateTargetUrl argv.[4] with
        | Error message -> Error message
        | Ok url -> Ok { nValue = n
                         cValue = c
                         url = url }
let echoErrorMessage (message: string): int =
    printfn "%s" message
    1

let executeCommand (arguments: Arguments): int =
    let command = new ProcessStartInfo(FileName = "echo")
    command.Arguments <- sprintf "'%s %s %s'" (string arguments.nValue) (string arguments.cValue) (string arguments.url)
    command.UseShellExecute <- true
    
    Process.Start(command) |> ignore
    
    0

[<EntryPoint>]
let main argv =
    printfn "%s" "Using SafeAB now."
    
    match validate argv with
    | Error message -> echoErrorMessage message
    | Ok arguments -> executeCommand arguments
