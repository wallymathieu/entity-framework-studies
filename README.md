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
