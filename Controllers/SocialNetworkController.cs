using System.Collections.Generic;
using CampusNet.Models;
using CampusNet.Views;


namespace CampusNet.Controllers
{
    // Controlar pirncipal para la aplicacion de red social.
    public class SocialNetworkController
    {
        private readonly SocialGraph _graph;
        private readonly GraphConsoleView _view;

        public SocialNetworkController()
        {
            _graph = new SocialGraph();
            _view = new GraphConsoleView();
        }

        public void Run()
        {
            BuildInitialGraph();
            _view.ShowAdjacencyList(_graph);

            ExecuteBFSTours();
            ExecuteDFSTour();
            ExecuteSocialQueries();
            ExecuteReachabilityTests();
            ExecuteCRUDOperations();

        }


            // Grafo inicial  con 12 usuarios y 19 relaciones
        private void BuildInitialGraph()
        {
            // 12 Usuarios (Cumple: >= 12 vértices)
            _graph.AddUser(new User("U01", "Ana Gómez", UserRole.Estudiante));
            _graph.AddUser(new User("U02", "Carlos Ruiz", UserRole.Profesor));
            _graph.AddUser(new User("U03", "Beatriz León", UserRole.Egresado));
            _graph.AddUser(new User("U04", "David Vera", UserRole.Estudiante));
            _graph.AddUser(new User("U05", "Elena Rostova", UserRole.Estudiante));
            _graph.AddUser(new User("U06", "Fernando Gil", UserRole.Profesor));
            _graph.AddUser(new User("U07", "Gloria Díaz", UserRole.Egresado));
            _graph.AddUser(new User("U08", "Hernán Silva", UserRole.Estudiante));
            _graph.AddUser(new User("U09", "Isabel Torres", UserRole.Estudiante));
            _graph.AddUser(new User("U10", "Juan Morales", UserRole.Profesor));
            _graph.AddUser(new User("U11", "Karla Peña", UserRole.Egresado));
            _graph.AddUser(new User("U12", "Luis Castro", UserRole.Estudiante));

            // 19 Relaciones (Cumple: >= 18 aristas)
            _graph.AddFollowRelation("U01", "U02");
            _graph.AddFollowRelation("U01", "U03");
            _graph.AddFollowRelation("U01", "U04");
            _graph.AddFollowRelation("U01", "U05");

            _graph.AddFollowRelation("U02", "U03");
            _graph.AddFollowRelation("U02", "U05");
            _graph.AddFollowRelation("U02", "U06");
            _graph.AddFollowRelation("U02", "U07");

            // Ciclo: U03 -> U04 -> U05 -> U03
            _graph.AddFollowRelation("U03", "U04");
            _graph.AddFollowRelation("U04", "U05");
            _graph.AddFollowRelation("U05", "U03");

            _graph.AddFollowRelation("U05", "U06");
            _graph.AddFollowRelation("U06", "U07");
            _graph.AddFollowRelation("U07", "U08");
            _graph.AddFollowRelation("U08", "U09");
            _graph.AddFollowRelation("U09", "U10");
            _graph.AddFollowRelation("U06", "U10");
            _graph.AddFollowRelation("U04", "U10");
            _graph.AddFollowRelation("U05", "U10");

            // Nodos aislados (Grado entrada 0)
            _graph.AddFollowRelation("U11", "U02");
            _graph.AddFollowRelation("U12", "U01");
        }

        private void ExecuteBFSTours()
        {
            _view.ShowHeader("Recorrido BFS (3 Nodos Distintos)");
            string[] startNodes = { "U01", "U03", "U11" };

            foreach (var nodeId in startNodes)
            {
                var user = _graph.GetUser(nodeId);
                if (user != null)
                {
                    var (order, count) = _graph.BFS(nodeId);
                    _view.ShowBFSResult(user.Id, user.Name, order, count, _graph);
                }
            }
        }

        private void ExecuteDFSTour()
        {
            var (order, hasCycles, cyclePaths) = _graph.DFSFull();
            _view.ShowDFSResult(order, hasCycles, cyclePaths, _graph);
        }

        private void ExecuteSocialQueries()
    
        {
            var zeroInDegree = _graph.GetUsersWithZeroInDegree();
            var mostInfluential = _graph.GetMostInfluentialUsers();
            var mostActive = _graph.GetMostActiveUsers();

            _view.ShowSocialMetrics(zeroInDegree, mostInfluential, mostActive, _graph);
        }

        private void ExecuteReachabilityTests()
        {
            _view.ShowHeader("Pruebas de Alcanzabilidad");
            _view.ShowReachability("Ana Gómez [U01]", "Juan Morales [U10]", _graph.IsReachableBFS("U01", "U10"));
            _view.ShowReachability("Juan Morales [U10]", "Ana Gómez [U01]", _graph.IsReachableBFS("U10", "U01"));
            _view.ShowReachability("Karla Peña [U11]", "Elena Rostova [U05]", _graph.IsReachableBFS("U11", "U05"));
        }

    // Implementación de operaciones CRUD
        private void ExecuteCRUDOperations()
        {
            _view.ShowHeader("Demostración de Operaciones CRUD");

            var newUser = new User("U13", "Mariana Ríos", UserRole.Estudiante);
            bool addedUser = _graph.AddUser(newUser);
            _view.ShowMessage(addedUser ? "Usuario U13 (Mariana Ríos) agregado correctamente." : "Error al agregar usuario.");

            bool addDuplicate = _graph.AddUser(newUser);
            _view.ShowMessage(addDuplicate ? "Duplicado aceptado." : "Validación: No se permiten duplicados.", addDuplicate);

            bool addedEdge = _graph.AddFollowRelation("U13", "U01");
            _view.ShowMessage(addedEdge ? "Relación U13 -> U01 agregada." : "Error al agregar relación.");

            bool updated = _graph.UpdateUser("U13", "Mariana Ríos M.", UserRole.Egresado);
            _view.ShowMessage(updated ? "Usuario U13 actualizado." : "Error al actualizar usuario.");

            bool removedEdge = _graph.RemoveFollowRelation("U01", "U02");
            _view.ShowMessage(removedEdge ? "Relación U01 -> U02 eliminada." : "Error al eliminar relación.");

            bool removedUser = _graph.RemoveUser("U04");
            _view.ShowMessage(removedUser ? "Usuario U04 (David Vera) eliminado del grafo." : "Error al eliminar usuario.");

            _view.ShowHeader("Estado Final del Grafo");
            _view.ShowAdjacencyList(_graph);
            
        }
    }
}