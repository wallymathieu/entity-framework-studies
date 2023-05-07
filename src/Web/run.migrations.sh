
#!/bin/bash
pushd $(dirname "${0}") > /dev/null
source ../../.env
export ASPNETCORE_ENVIRONMENT=Development
export ConnectionStrings__DefaultConnection=$Web__DefaultConnection
dotnet ef database update
