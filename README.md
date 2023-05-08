# Entity framework studies

This repository contains source for both c# and f#.

## C\#

- [web](./src/Web)
- [tests](./tests/Tests)

## F\#

- [web](./src/WebFs)
- [tests](./tests/FsTests)
- [migrations](./migrations/FsMigrations)

## Dev container

To ensure that the DB has the migrations applied
```bash
bash ./migrations/FsMigrations/run.sh && bash ./src/Web/run.migrations.sh
```

## Docker compose

Copy .devcontainer.env to .env:

```sh
cp .devcontainer.env .env
```

Start database

```sh
docker compose up -d db
```

Start and build web (as a deamon)

```sh
docker compose up -d web --build
```
