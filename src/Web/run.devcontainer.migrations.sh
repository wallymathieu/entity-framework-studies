
#!/bin/bash
set -e
pushd $(dirname "${0}") > /dev/null
source ../../.env.devcontainer
export ASPNETCORE_ENVIRONMENT=Development
export ConnectionStrings__DefaultConnection=$Web__DefaultConnection
dotnet ef database update
