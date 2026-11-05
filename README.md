# 🎓 CampusNet - Simulación de Red Social Académica Dirigida (MVC en C#)

Este proyecto implementa la simulación de una red social académica dirigida llamada **CampusNet**, desarrollada en **C# (.NET Console App)** aplicando el patrón de arquitectura **MVC estricto** y modelando las relaciones de seguimiento entre usuarios mediante una **Lista de Adyacencia** (`Dictionary<string, List<string>>`).

---

## 👥 Integrantes del Equipo y Responsabilidades

| Nombre Completo | Rol / Responsabilidades en el Proyecto |
| :--- | :--- |
| **Jhonathan Martínez Gómez** | Implementación de la Capa Modelo (`Models/SocialGraph.cs`, `User.cs`, `Edge.cs`), algoritmos de recorridos **BFS / DFS**, detección de ciclos dirigidos y métricas de conectividad. |
| **Johnatan Sierra Alvarez** | Implementación de la Capa Controlador (`Controllers/SocialNetworkController.cs`), secuenciación de casos de uso, control de duplicados y operaciones CRUD. |
| **Brisney Nicolle Saldarriaga Aguirre** | Implementación de la Capa Vista (`Views/GraphConsoleView.cs`), formateo e impresión en consola y documentación del proyecto. |

---

## 🛠️ Arquitectura del Proyecto (MVC Estricto)

```text
CampusNet/
│
├── Models/
│   ├── UserRole.cs              # Enumeración para los roles (Estudiante, Profesor, Egresado)
│   ├── User.cs                  # Representación de un Vértice del grafo (Id, Nombre, Rol)
│   ├── Edge.cs                  # Representación de una Arista dirigida (Origen, Destino)
│   └── SocialGraph.cs           # Lógica de negocio del Grafo (Lista de adyacencia y algoritmos)
│
├── Views/
│   └── GraphConsoleView.cs      # Salida e impresión formateada exclusivamente en consola
│
├── Controllers/
│   └── SocialNetworkController.cs # Coordinación entre vista y modelo, secuencia de pruebas
│
├── Program.cs                   # Punto de entrada (Invoca únicamente al Controller)
└── README.md                    # Documentación formal de la actividad