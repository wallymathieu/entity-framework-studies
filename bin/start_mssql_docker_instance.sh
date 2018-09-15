#! /usr/bin/env sh
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=EF_TEST_PASSWORD' -v ef-mssql-volume:/var/opt/mssql -p 1433:1433 -d microsoft/mssql-server-linux 

