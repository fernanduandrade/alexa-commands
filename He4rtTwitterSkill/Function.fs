namespace He4rtTwitterSkill


open Amazon.Lambda.Core
open FSharp.Data
open Alexa.NET.Request
open Alexa.NET
open FunctionTypes
open FSharp.Data.HttpRequestHeaders
open System.Text.Json
open System


[<assembly: LambdaSerializer(typeof<Amazon.Lambda.Serialization.Json.JsonSerializer>)>]
()


type Function() =
    member __.FunctionHandler (request: SkillRequest) (_: ILambdaContext) =
        let token = Environment.GetEnvironmentVariable("BEARER")

        let result = 
            match Http.Request("https://api.twitter.com/2/tweets/search/recent?query=%40he4rtdevs&max_results=10&=", headers = [ Authorization $"Bearer {token}"]).Body with
                | Text text -> text
                | Binary binary -> failwithf "Expecting text, but got a binary response (%d bytes)" binary.Length
               

        let response = JsonSerializer.Deserialize<TweetResponse> result

        ResponseBuilder.Tell($"o ultimo tweet sobre a heart foi: {response.data.Head.text}")
        
