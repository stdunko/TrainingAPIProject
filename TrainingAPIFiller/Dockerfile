## Используем образ SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TrainingAPIFiller/TrainingAPIFiller.csproj", "TrainingAPIFiller/"]
RUN dotnet restore "TrainingAPIFiller/TrainingAPIFiller.csproj"
COPY . .
WORKDIR "/src/TrainingAPIFiller"
RUN dotnet build "TrainingAPIFiller.csproj" -c Release -o /app/build

# Публикация
FROM build AS publish
RUN dotnet publish "TrainingAPIFiller.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Итоговый образ для запуска
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrainingAPIFiller.dll"]
