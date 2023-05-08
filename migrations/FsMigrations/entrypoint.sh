#!/bin/bash

set -e
CONNECTION="Server=$SERVER;Database=$DATABASE;MultipleActiveResultSets=true;TrustServerCertificate=True;User Id=sa;Password=$PASSWORD"

dotnet FsMigrations.dll --connection "$CONNECTION"
