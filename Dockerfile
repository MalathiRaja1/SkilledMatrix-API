FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY SkillMatrix.Api/SkillMatrix.Api.csproj SkillMatrix.Api/
RUN dotnet restore SkillMatrix.Api/SkillMatrix.Api.csproj

COPY . .
RUN dotnet publish SkillMatrix.Api/SkillMatrix.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "SkillMatrix.Api.dll"]
