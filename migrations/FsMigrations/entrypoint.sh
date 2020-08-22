#!/bin/bash

set -e
CONNECTION="Server=$SERVER;Database=$DATABASE;MultipleActiveResultSets=true;User Id=sa;Password=$PASSWORD"

dotnet FsMigrations.dll --connection "$CONNECTION"
