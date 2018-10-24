module FsMigrations.Extensions

open FluentMigrator
open FluentMigrator.Builders.Create.Table
type ICreateTableWithColumnSyntax with
    member this.WithIdColumn()=
           this
                .WithColumn("Id")
                .AsInt32()
                .NotNullable()
                .PrimaryKey()
                .Identity();
    member this.WithVersionColumn()=
           this
                .WithColumn("Version")
                .AsInt32()
                .NotNullable();
