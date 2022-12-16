namespace DevToNews


open Amazon.Lambda.Core

open System
open Alexa.NET
open FSharp.Data
open Alexa.NET.Request

[<assembly: LambdaSerializer(typeof<Amazon.Lambda.Serialization.Json.JsonSerializer>)>]
()


type Function() =

    member __.FunctionHandler (requet: SkillRequest) (_: ILambdaContext) =
        let devToLastestFsharpPostUrl = "https://dev.to/t/fsharp/latest"

        let document = HtmlDocument.Load devToLastestFsharpPostUrl

        let posts = document.CssSelect ".crayons-story__title"

        let postText selector (post: HtmlNode) =
            post.CssSelect(selector).[0].DirectInnerText().Trim()

        let getLastestPost =
            posts
            |> Seq.map(fun post ->
                post |> postText ".css-901oao .css-16my406 .r-poiln3 .r-bcqeeo .r-qvutc0"
            ) |> Seq.take 1

        for (title) in getLastestPost do
            printf "\"%s" title

        ResponseBuilder.Tell("Fernandinho es o melhor")
