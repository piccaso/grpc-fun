FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
RUN dotnet dev-certs https -q -ep /app/cert.pfx -p cert
WORKDIR /src
COPY ["*.sln", "./"]
COPY ["*/*.csproj", "./"]
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore 
COPY . .
RUN dotnet publish --no-restore "GrpcFun.Server/GrpcFun.Server.csproj" -c Release -o /app
RUN chmod -R a-w-x+X /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
ENV ASPNETCORE_URLS="https://+:5001" \
    ASPNETCORE_HTTPS_PORT="5001" \
    ASPNETCORE_Kestrel__Certificates__Default__Password="cert" \
    ASPNETCORE_Kestrel__Certificates__Default__Path="/app/cert.pfx"

EXPOSE 5001
WORKDIR /app
COPY --from=build /app .
USER nobody
ENTRYPOINT ["dotnet", "GrpcFun.Server.dll"]
