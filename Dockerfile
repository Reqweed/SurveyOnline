FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app 
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SurveyOnline.Web/SurveyOnline.Web", "SurveyOnline.Web/"]
RUN dotnet restore "SurveyOnline.Web/SurveyOnline.Web.csproj"
COPY . . 
WORKDIR "/src/SurveyOnline.Web"
RUN dotnet build "./SurveyOnline.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SurveyOnline.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SurveyOnline.Web.dll"]