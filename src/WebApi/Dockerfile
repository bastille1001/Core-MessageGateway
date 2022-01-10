FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS publish
ENV NUGET_CORE_READ_USERNAME nuget_write_user
ENV NUGET_CORE_READ_PASSWORD dchsjN2VHE1e6sH22iMm
WORKDIR /src
COPY ["src/WebApi/WebApi.csproj", "src/WebApi/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["../../NuGet.Config", "src/"]
RUN dotnet restore "src/WebApi/WebApi.csproj"
COPY . .
RUN dotnet publish "src/WebApi/WebApi.csproj" -c Release -o /app/publish

FROM base AS final
RUN mkdir -p /etc/config && mkdir -p /etc/secrets
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]