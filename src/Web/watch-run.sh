#!/bin/bash
pushd $(dirname "${0}") > /dev/null
export ASPNETCORE_ENVIRONMENT=Development
export ConnectionStrings__DefaultConnection=$Web__DefaultConnection
dotnet watch run b
