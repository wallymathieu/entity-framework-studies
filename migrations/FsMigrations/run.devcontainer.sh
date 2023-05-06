#!/bin/bash
set -e
pushd $(dirname "${0}") > /dev/null
source ../../.env.devcontainer

dotnet run --connection "$WebFs__DefaultConnection"
