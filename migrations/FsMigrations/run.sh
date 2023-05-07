#!/bin/bash
set -e
pushd $(dirname "${0}") > /dev/null
source ../../.env

dotnet run --connection "$WebFs__DefaultConnection"
