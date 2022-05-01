FROM mcr.microsoft.com/dotnet/aspnet:2.1-bionic-arm32v7
RUN apt-get -y update && apt-get -y install g++
WORKDIR /app
COPY published/ .
ENTRYPOINT ["dotnet", "PUTSWeb.dll"]
