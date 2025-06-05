## Migration Entity Framework Core

Pour créer une migration initiale avec EF Core, utilisez la commande suivante dans le terminal à la racine de la solution :

```bash
dotnet ef migrations add initialcreate --startup-project DiveHub.WebApi --project DiveHub.Infrastructure
```

- `--startup-project DiveHub.WebApi` : Spécifie le projet de démarrage (API).
- `--project DiveHub.Infrastructure` : Spécifie le projet contenant le contexte de données et les entités.

Pour appliquer la migration à la base de données, exécutez :

```bash
dotnet ef database update --startup-project DiveHub.WebApi --project DiveHub.Infrastructure
```

Assurez-vous d’avoir installé les outils EF Core :

```bash
dotnet tool install --global dotnet-ef
```