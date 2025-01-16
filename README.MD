# Structure du projet

## Solution (GestionPlongees)

- Core
    - Contient les entités, les interfaces et les règles métier.
- Application
  - Contient les cas d'utilisation et les DTO.
- -Infrastructure
    - Contient les implémentations des interfaces (DbContext, migrations,sauvegarde dans les fichiers JSON, etc.).
- WebAPI
    - Contient les contrôleurs et le point d'entrée de l'application (API Web ASP.NET Core).