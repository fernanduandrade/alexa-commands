namespace He4rtTwitterSkill


open Amazon.Lambda.Core

open Alexa.NET.Request
open Alexa.NET
open Alexa.NET.Response


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[<assembly: LambdaSerializer(typeof<Amazon.Lambda.Serialization.Json.JsonSerializer>)>]
()


type Function() =
    /// <summary>
    /// Função que retorna diz apenas Oi 😉
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    member __.FunctionHandler (request: SkillRequest) (_: ILambdaContext) =
        match request with
        | _ -> ResponseBuilder.Tell("Oiii Fernando")
