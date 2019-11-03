open System
open System.Diagnostics
open System.Text.RegularExpressions

let makeErrorMessage msg: string = sprintf "SafeAB: %s" msg

let allowedHosts(): string list = [ "localhost" ]

let isAllowedUrl (url: Uri): bool = allowedHosts() |> List.contains url.Host

let isUrl (target: string) = Regex.IsMatch(target, @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$")

let parseUrl (target: string): Result<Uri, string> =
    match isUrl target with
    | true -> Ok(Uri(target))
    | false -> Error "Invalid url."

let pickUrl (args: string []): Result<Uri, string> =
    let urls: string [] = Array.filter (fun (arg: string) -> isUrl arg) args
    match urls.Length with
    | 0 -> Error "Undefined target url."
    | 1 -> Ok(Uri(urls.[0]))
    | _ -> Error "Target urls are too much."

let validate (argv: string []): Result<string, string> =
    match pickUrl argv with
    | Error msg -> Error msg
    | Ok url ->
        match isAllowedUrl url with
        | false -> Error "Disallowed URL."
        | true -> Ok(String.concat " " argv)

let echoErrorMessage (message: string): int =
    printfn "%s" message
    1

let executeCommand (args: string): int =
    let command = new ProcessStartInfo(FileName = "ab")
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
