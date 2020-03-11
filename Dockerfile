FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS buildnet

COPY ./Cheeseria.Data /src/Cheeseria.Data
WORKDIR /src/Cheeseria.Data
RUN dotnet restore
RUN dotnet build Cheeseria.Data.csproj

COPY ./Cheeseria.API /src/Cheeseria.API
WORKDIR /src/Cheeseria.API
RUN dotnet restore
RUN dotnet build Cheeseria.API.csproj
RUN dotnet publish Cheeseria.API.csproj -o /app/publish

FROM node:buster as buildangular
WORKDIR /src
COPY ./Cheeseria.Web/package*.json ./
RUN npm install
COPY ./Cheeseria.Web /src
RUN npm run build

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
WORKDIR /app
EXPOSE 80/tcp
EXPOSE 443/tcp
COPY --from=buildnet /app/publish .
COPY --from=buildangular /src/dist/CheeseriaWeb ./wwwroot
ENTRYPOINT ["dotnet", "Cheeseria.API.dll"]