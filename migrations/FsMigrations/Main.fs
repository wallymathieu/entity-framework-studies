module FsMigrations.Main


open System
open FluentMigrator.Runner
open FluentMigrator.Runner.Processors
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
type CmdArgs = 
  { connection : string
    processor : string
    operation : string
  }

[<EntryPoint>]
let main argv =
    let defaultArgs = 
      { connection= "Server=localhost;Database=ef-core-studies-fsharp;MultipleActiveResultSets=true;User Id=sa;Password=EF_TEST_PASSWORD"
        processor = "SqlServer2016"
        operation = "migrate"
      }
    let printHelp () =
      printfn "Usage:"
      printfn "    --connection connection_string (Default: %s)" defaultArgs.connection
      printfn "    --processor processor_id (Default: %s)" defaultArgs.processor
      printfn "    --operation operation (Default: %s)" defaultArgs.operation
      exit 1
    let rec parseArgs b args = 
      match args with
      | [] -> b
      | "--connection" :: connection :: xs -> parseArgs { b with connection = connection } xs
      | "--processor" :: processor :: xs -> parseArgs { b with processor = processor } xs
      | "--operation" :: operation :: xs -> parseArgs { b with operation = operation } xs
      | invalidArgs ->
        printfn "error: invalid arguments %A" invalidArgs
        printHelp()

    let args = argv
              |> List.ofArray
              |> parseArgs defaultArgs
    
            // Initialize the services
    let serviceProvider = ServiceCollection()
                            .AddLogging(fun lb -> lb.AddDebug().AddFluentMigratorConsole() |> ignore)
                            .AddFluentMigratorCore()
                            .ConfigureRunner(
                                fun builder -> builder
                                                .AddSQLite()
                                                .AddSqlServer()
                                                .WithGlobalConnectionString(args.connection)
                                                .ScanIn(typeof<FsMigrations.Migrations.AddTables>.Assembly)
                                                    .For.Migrations() |> ignore)
                            .Configure<SelectingProcessorAccessorOptions>(
                                fun opt -> opt.ProcessorId <- args.processor)
                            .BuildServiceProvider()

    // Instantiate the runner
    let runner = serviceProvider.GetRequiredService<IMigrationRunner>()
    match args.operation with
    | "migrate" -> 
      runner.MigrateUp()
      0
    | _ ->
(* NOTE: To create db, run
CREATE DATABASE [ef-core-studies-fsharp]
*)
      printHelp()
      1 // return an integer exit code
