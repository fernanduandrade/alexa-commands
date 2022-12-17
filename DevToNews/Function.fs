namespace DevToNews


open Amazon.Lambda.Core

open System
open Alexa.NET
open FSharp.Data
open Alexa.NET.Request
open System.Text

[<assembly: LambdaSerializer(typeof<Amazon.Lambda.Serialization.Json.JsonSerializer>)>]
()


type Function() =

    member __.FunctionHandler (requet: SkillRequest) (_: ILambdaContext) =
        let devToLastestFsharpPostUrl = "https://dev.to/t/fsharp/latest"

        let document = HtmlDocument.Load devToLastestFsharpPostUrl

        let posts = document.CssSelect ".crayons-story"

        let postText selector (post: HtmlNode) =
            post.CssSelect(selector).[0].DirectInnerText().Trim()

        let getReadTime selector (readTime: HtmlNode) =
          readTime.CssSelect(selector).[0]

        let getLastestPost =
            posts
            |> Seq.map(fun post ->
                post |> postText ".crayons-story__hidden-navigation-link",
                post |> postText ".crayons-story__secondary",
                post |> getReadTime ".crayons-story__save" |> postText ".crayons-story__tertiary"
            )
            |> Seq.take 3
            |> Seq.map (fun (title, author, readTime) -> $"Título: {title}, Autor: {author}, Tempo de leitura {readTime}")
            |> Seq.concat |> Seq.map string |> String.concat ""

        ResponseBuilder.Tell(getLastestPost)
        
