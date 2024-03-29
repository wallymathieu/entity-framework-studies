module FsTests.ContractTests
open System
open System.Net.Http
open System.Text
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.TestHost
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.Configuration
open Newtonsoft.Json.Linq
open WebFs
open Xunit
open FsMigrations

let db = IO.Path.Combine ( IO.Directory.GetCurrentDirectory(), "ApiFixture.db" )
let tryRemoveDbFile () =
    if IO.File.Exists(db) then
        try IO.File.Delete(db)
        with _ -> ()
let dbConn = "Data Source=" + db


type TestStartup (config) =
    inherit Startup (config)
    override self.ConfigureDbContext options =
        let migrationRunner = MigrationRunner.create dbConn "sqlite"
        migrationRunner.MigrateUp ()
        options.UseSqlite dbConn |> ignore
let createTestServer () =
    new TestServer(WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(IO.Directory.GetCurrentDirectory())
        .UseConfiguration(ConfigurationBuilder().Build())
        .UseStartup<TestStartup>())
type ApiFixture () =
    let server = createTestServer()
    member _.Server = server
    interface IDisposable with
        member _.Dispose () =
            server.Dispose()
            tryRemoveDbFile()
let appJson = "application/json"
///
let postJsonAsync (client: HttpClient) (url:string) (body:string) =
   task {
      let! response = client.PostAsync (url, new StringContent(body, Encoding.UTF8, appJson))
      response.EnsureSuccessStatusCode () |> ignore
      return! response.Content.ReadAsStringAsync () }
///
let putJsonAsync (client: HttpClient) (url:string) (body:string) =
   task {
      let! response = client.PutAsync (url, new StringContent(body, Encoding.UTF8, appJson))
      response.EnsureSuccessStatusCode () |> ignore
      return! response.Content.ReadAsStringAsync () }
///
let getJsonAsync (client: HttpClient) (url:string) =
    task {
      let! response = client.GetAsync url
      response.EnsureSuccessStatusCode () |> ignore
      return! response.Content.ReadAsStringAsync () }

type ContractTests (fixture:ApiFixture) =
    interface IClassFixture<ApiFixture>

    [<Fact>] member _.``Can save and get customer`` ()
        = task {
            use client = fixture.Server.CreateClient()
            let! createdCustomer = postJsonAsync client "/api/v1/customers" """{"firstname":"Test","lastname":"TRest"}"""
            let obj = JObject.Parse createdCustomer
            let id = obj.["id"].Value<string> ()
            Assert.NotNull id
            let! customerJson = getJsonAsync client ("/api/v1/customers/"+id)
            let customerId = (JObject.Parse customerJson).["id"].Value<string> ()
            Assert.Equal (id, customerId) }
    [<Fact>] member _.``Can save and get product`` ()
        = task {
            use client = fixture.Server.CreateClient()
            let! createdProductJson = postJsonAsync client "/api/v1/products" """{"name":"Test","cost":10}"""
            let obj = JObject.Parse createdProductJson
            let id = obj.["id"].Value<string> ()
            Assert.NotNull id
            let! productJson = getJsonAsync client ("/api/v1/products/"+id)

            let productId = (JObject.Parse productJson).["id"].Value<string> ()
            Assert.Equal (id, productId)

            let! _ = putJsonAsync client ("/api/v1/products/"+id) """{"name":"Test2","cost":11}"""
            let! productJson = getJsonAsync client ("/api/v1/products/"+id)
            let productJson' = JObject.Parse productJson
            let name = productJson'["name"].Value<string> ()

            Assert.Equal ("Test2", name) }
