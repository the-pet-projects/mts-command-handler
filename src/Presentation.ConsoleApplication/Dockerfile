FROM microsoft/dotnet:2.0-runtime-jessie
ARG source
RUN echo "source: $source"

WORKDIR /app

COPY ${source:-/build} .

ENTRYPOINT ["dotnet", "PetProjects.Mts.CommandHandler.Presentation.ConsoleApplication.dll"]
