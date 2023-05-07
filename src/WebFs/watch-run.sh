#!/bin/bash
set -e
pushd $(dirname "${0}") > /dev/null
source ../../.env
export ASPNETCORE_ENVIRONMENT=Development
export ConnectionStrings__DefaultConnection=$WebFs__DefaultConnection
dotnet watch run b
