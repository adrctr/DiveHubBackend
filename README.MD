# Structure du Projet : GestionPlongees

## Solution : GestionPlongees

### Core Layer
- **Rôle :** Définit les règles métier et les abstractions.
- **Contenu :**
  - Entités : Représentent les objets métier comme `User`, `Dive`, etc.
  - Interfaces :
    - `IUserService` : Interface pour les opérations liées aux utilisateurs.
    - `IDiveService` : Interface pour les opérations liées aux plongées.

---

### Application Layer
- **Rôle :** Orchestration des cas d'utilisation, sépare les détails de persistance et d'exposition.
- **Contenu :**
  - DTOs (Data Transfer Objects) :
    - Utilisés pour transférer les données entre les couches.
  - Services :
    - `UserService` : Implémente les cas d'utilisation pour la gestion des utilisateurs.
    - `DiveService` : Implémente les cas d'utilisation pour la gestion des plongées.

---

### Infrastructure Layer
- **Rôle :** Fournit les implémentations techniques, comme la persistance.
- **Contenu :**
  - **Entity Framework Core :**
    - Gère les interactions avec la base de données.
  - **Repository générique :**
    - Fournit des opérations CRUD réutilisables.

---

### Web API Layer
- **Rôle :** Fournit une interface pour accéder aux services via HTTP.
- **Contenu :**
  - Contrôleurs :
    - Interagissent avec les services pour réaliser les actions CRUD.
    - Manipulent uniquement les DTOs pour garantir l'isolation des couches.

---

### Résumé des couches
- **Core Layer :** Définition des entités et interfaces.
- **Application Layer :** Orchestration des cas d'utilisation.
- **Infrastructure Layer :** Gestion des détails techniques et persistance.
- **Web API Layer :** Interface HTTP pour l'application.

---

### Remarques

- **Avantages de garder IRepository<T> dans Core :**
  - **Flexibilité :** Permet de changer l'implémentation e la couche technique (par exemple, passer de EF à Dapper) sans modifier les couches supérieures.
  - **Cohérence :** Favorise l'indépendance des couches supérieures par rapport à l'implémentation technique.