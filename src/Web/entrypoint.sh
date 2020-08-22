#!/bin/bash

set -e

until dotnet-ef database update; do
>&2 echo "SQL Server is starting up"
sleep 1
done

>&2 echo "SQL Server is up - executing command"
dotnet run
