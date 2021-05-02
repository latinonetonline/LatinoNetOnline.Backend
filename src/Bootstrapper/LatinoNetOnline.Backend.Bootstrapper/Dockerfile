#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/Bootstrapper/LatinoNetOnline.Backend.Bootstrapper/LatinoNetOnline.Backend.Bootstrapper.csproj", "src/Bootstrapper/LatinoNetOnline.Backend.Bootstrapper/"]
COPY ["src/Worker/LatinoNetOnline.Backend.Worker.Host/LatinoNetOnline.Backend.Worker.Host.csproj", "src/Worker/LatinoNetOnline.Backend.Worker.Host/"]
COPY ["src/Worker/LatinoNetOnline.Backend.Worker.Application/LatinoNetOnline.Backend.Worker.Application.csproj", "src/Worker/LatinoNetOnline.Backend.Worker.Application/"]
COPY ["src/Shared/LatinoNetOnline.Backend.Shared.Abstractions/LatinoNetOnline.Backend.Shared.Abstractions.csproj", "src/Shared/LatinoNetOnline.Backend.Shared.Abstractions/"]
COPY ["src/Modules/CallForProposals/LatinoNetOnline.Backend.Modules.CallForProposals.Api/LatinoNetOnline.Backend.Modules.CallForProposals.Api.csproj", "src/Modules/CallForProposals/LatinoNetOnline.Backend.Modules.CallForProposals.Api/"]
COPY ["src/Shared/LatinoNetOnline.Backend.Shared.Infrastructure/LatinoNetOnline.Backend.Shared.Infrastructure.csproj", "src/Shared/LatinoNetOnline.Backend.Shared.Infrastructure/"]
COPY ["src/Modules/CallForProposals/LatinoNetOnline.Backend.Modules.CallForProposals.Core/LatinoNetOnline.Backend.Modules.CallForProposals.Core.csproj", "src/Modules/CallForProposals/LatinoNetOnline.Backend.Modules.CallForProposals.Core/"]
COPY ["src/Modules/Identities/LatinoNetOnline.Backend.Modules.Identities.Web/LatinoNetOnline.Backend.Modules.Identities.Web.csproj", "src/Modules/Identities/LatinoNetOnline.Backend.Modules.Identities.Web/"]
RUN dotnet restore "src/Bootstrapper/LatinoNetOnline.Backend.Bootstrapper/LatinoNetOnline.Backend.Bootstrapper.csproj"
COPY . .
WORKDIR "/src/src/Bootstrapper/LatinoNetOnline.Backend.Bootstrapper"
RUN dotnet build "LatinoNetOnline.Backend.Bootstrapper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LatinoNetOnline.Backend.Bootstrapper.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet LatinoNetOnline.Backend.Bootstrapper.dll 