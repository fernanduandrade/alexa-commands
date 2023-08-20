module AlexaGoogleAgenda

open System.Threading
open Google.Apis.Calendar.v3
open Google.Apis.Calendar.v3.Data
open Google.Apis.Auth.OAuth2
open System
open Google.Apis.Services

let credentialsPath = "./credentials.json"
let scopes = [| CalendarService.Scope.CalendarReadonly |]

let credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
    GoogleClientSecrets.FromFile(credentialsPath).Secrets,
    scopes,
    "user",
    CancellationToken.None).Result

let service = new CalendarService(
    new BaseClientService.Initializer(
        HttpClientInitializer = credential,
        ApplicationName = "AlexaIntegration")
    )


let today = DateTime.UtcNow.Date
let startOfDay = Convert.ToDateTime(DateTime(today.Year, today.Month, today.Day, 8, 0, 0, DateTimeKind.Utc).ToString("yyyy-MM-ddTHH:mm:ssZ"))
let endOfDay = Convert.ToDateTime(DateTime(today.Year, today.Month, today.Day, 23, 0, 0, DateTimeKind.Utc).ToString("yyyy-MM-ddTHH:mm:ssZ"))


let request: EventsResource.ListRequest = service.Events.List("primary")
request.TimeMin <- startOfDay
request.TimeMax <- endOfDay
request.SingleEvents <- true
request.OrderBy <- EventsResource.ListRequest.OrderByEnum.StartTime


let response = request.Execute()

if response.Items |> Seq.isEmpty then
    printfn "Não há agendas para hoje"
else

    let result = response.Items
                |> Seq.map(fun agenda  -> 
                agenda.Summary, agenda.Description, agenda.Start.DateTime.Value.ToString("hh:mm"))
                |> Seq.map(fun (
                    title, description, timeStart) -> $"Nome da agenda: {title}, Pauta da agenda: {description}, horário de início {timeStart} \n")
                |> Seq.concat |> Seq.map string |> String.concat ""

    printfn "Você no total %A agendas para hoje, irei agora lista-las agora \n%s" response.Items.Count result