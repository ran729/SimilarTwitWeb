FROM ubuntu
FROM microsoft/dotnet:2.1-sdk


COPY . ./
RUN dotnet restore

WORKDIR SimilarTwitWeb.Api
RUN dotnet publish -c Release -o out
RUN cp sample.db out/sample.db
ENTRYPOINT ["dotnet", "out/SimilarTwitWeb.Api.dll"]
