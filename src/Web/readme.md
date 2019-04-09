# Web project

In order to start a similar project you can start by doing

```bash
dotnet new webapp
```

Note the following folders:

- [Controllers](./Controllers) The controllers follow the standard Web MVC pattern
- [Models](./Models) contains the data transfer objects or models used by views returned from the controllers
- [Views](./Views) The views usually render models to html
- [Data](./Data) contains the EF repository implementation here called [CoreDbContext](./Data/CoreDbContext.cs)
- [Migrations](./Migrations)
- [Commands](./Commands)
- [Entities](./Entities) contains objects persisted in the database with their own identity (id column). The ProductOrder entity is strictly not an entitiy since it is to work around [a limitation in the ORM](https://github.com/aspnet/EntityFrameworkCore/issues/1368).
- [ValueTypes](./ValueTypes) are objects that have no identity of their own. The equality of a value type is the same as the structural equality (that the content is the same), as you can read on [Relational operator](https://en.wikipedia.org/wiki/Relational_operator).
