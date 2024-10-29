FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY . ./

RUN dotnet publish -c Release -o out

#run the app on container start
CMD ["dotnet", "out/Trains.dll"]

