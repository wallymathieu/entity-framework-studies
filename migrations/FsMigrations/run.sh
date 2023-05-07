#!/bin/bash
pushd $(dirname "${0}") > /dev/null
dotnet run --connection "$WebFs__DefaultConnection"
