#!/bin/bash

api_name="DiveHub.ClientApi"
file_name="DiveHubAPI.json"

# Exécution de la génération de code avec Docker
docker run --rm -v "$(pwd)":/local openapitools/openapi-generator-cli generate \
    -i /local/$file_name \
    -g csharp \
    -o /local/$api_name \
    --additional-properties=packageName=$api_name,netCoreProjectFile=true,modelPropertyNaming=original,targetFramework=net8.0

# Pause de 1 seconde
sleep 1

# Attente que le dossier cible soit prêt pour la copie
ready=false
while [ "$ready" = false ]; do
    if cp -r "$api_name/src/$api_name"/* ../../DiveHubFrontend/$api_name/.; then
        ready=true
    else
        sleep 5
    fi
done

# Pause de 2 secondes
sleep 2

# Attente pour supprimer le dossier généré
ready=false
while [ "$ready" = false ]; do
    if rm -rf "$api_name"; then
        ready=true
    else
        sleep 1
    fi
done
