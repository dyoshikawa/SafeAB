open System
open System.Diagnostics

type Arguments =
    { nValue: int
      cValue: int
      url: string }

let validate (argv: string []) : Result<Arguments, string> =
    let errorMessage = "Invalid arguments."
    
    let nOption = argv.[0]
    let nValue = argv.[1]
    let cOption = argv.[2]
    let cValue = argv.[3]
    let url = argv.[4]

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
        | ((true, n), (true, c)) -> Ok { nValue = n
                                         cValue = c
                                         url = url }
        | _ -> Error errorMessage

let echoErrorMessage (message: string): int =
    printfn "%s" message
    1

let executeCommand (arguments: Arguments): int =
    let app = new ProcessStartInfo(FileName = "echo")
    app.Arguments <- sprintf "'%s %s'" (string arguments.nValue) (string arguments.cValue)
    app.UseShellExecute <- true
    
    Process.Start(app) |> ignore
    
    0

[<EntryPoint>]
let main argv =
    printfn "%s" "Using SafeAB now."
    printfn "%A" argv
    
    match validate argv with
    | Ok arguments -> executeCommand arguments
    | Error message -> echoErrorMessage message
