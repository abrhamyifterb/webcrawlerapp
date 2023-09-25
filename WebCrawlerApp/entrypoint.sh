#!/bin/sh

set -e
set -x 

# Making sure the database is available before starting migrations
echo "Waiting for DB to be ready..."
sleep 10

# Run migrations
dotnet ef database update --project /src/WebCrawlerApp.Infrastructure --startup-project /src/WebCrawlerApp.API

# Start the app
exec dotnet /app/WebCrawlerApp.API.dll
