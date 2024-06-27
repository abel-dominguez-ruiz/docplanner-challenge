FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
COPY ["./src/Api", "./Api/"]
COPY ["./src/DocPlanner.Client", "./DocPlanner.Client/"]
COPY ["./src/VerticalSlice", "./VerticalSlice/"]

RUN dotnet restore "./Api/Api.csproj" --disable-parallel
COPY . .
WORKDIR /src/Api
RUN dotnet build "Api.csproj" -c Release -o /src/build --no-restore
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM build AS final
EXPOSE 9091
WORKDIR /car-pooling-challenge
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS http://*:9091
ENTRYPOINT ["dotnet", "Api.dll"]

