# Base image with SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

EXPOSE 80
EXPOSE 443

# Copy everything and restore
COPY . .
WORKDIR "/src/WebCrawlerApp.API"
RUN dotnet build "WebCrawlerApp.API.csproj" -c Release -o /app/build

# Install EF Core CLI
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Build the API project
WORKDIR "/src/WebCrawlerApp.API"
RUN dotnet build "WebCrawlerApp.API.csproj" -c Release -o /app/build

# Publish the API project
FROM build AS publish
RUN dotnet publish "WebCrawlerApp.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src /src
COPY entrypoint.sh /app/entrypoint.sh

# Copy the EF tool and set up the necessary environment for it
COPY --from=build /root/.dotnet/tools /root/.dotnet/tools
ENV PATH="${PATH}:/root/.dotnet/tools"

RUN dotnet dev-certs https --clean && dotnet dev-certs https --trust

RUN chmod +x /app/entrypoint.sh
ENV ConnectionStrings__DefaultConnection=""

ENTRYPOINT ["/app/entrypoint.sh"]
