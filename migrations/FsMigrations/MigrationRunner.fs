module FsMigrations.MigrationRunner
open System
open FluentMigrator.Runner
open FluentMigrator.Runner.Processors
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging

let create connection processor=
    let serviceProvider = ServiceCollection()
                            .AddLogging(fun lb -> lb.AddDebug().AddFluentMigratorConsole() |> ignore)
                            .AddFluentMigratorCore()
                            .ConfigureRunner(
                                fun builder -> builder
                                                .AddSQLite()
                                                .AddSqlServer()
                                                .AddPostgres()
                                                .WithGlobalConnectionString(connection)
                                                .ScanIn(typeof<FsMigrations.Migrations.AddTables>.Assembly)
                                                    .For.Migrations() |> ignore)
                            .Configure(
                                fun (opt:SelectingProcessorAccessorOptions) -> opt.ProcessorId <- processor)
                            .BuildServiceProvider()

    // Instantiate the runner
    serviceProvider.GetRequiredService<IMigrationRunner>()
