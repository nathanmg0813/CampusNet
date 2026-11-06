using System;
using System.Collections.Generic;
using CampusNet.Models;

namespace CampusNet.Views
{
    public class GraphConsoleView
    {
        public void ShowHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n==================================================");
            Console.WriteLine($"  {title.ToUpper()}");
            Console.WriteLine($"==================================================");
            Console.ResetColor();
        }

        public void ShowAdjacencyList(SocialGraph graph)
        {
            ShowHeader("Estructura del Grafo - Lista de Adyacencia");
            var adj = graph.GetAdjacencyList();

            foreach (var kvp in adj)
            {
                User? user = graph.GetUser(kvp.Key);
                if (user == null) continue;

                int inDeg = graph.GetInDegree(kvp.Key);
                int outDeg = graph.GetOutDegree(kvp.Key);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"[{user.Id}] {user.Name} ({user.Role}) ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"[Entrada: {inDeg} | Salida: {outDeg}]");
                Console.Write("   └── Sigue a: ");

                if (kvp.Value.Count == 0)
                {
                    Console.WriteLine("(Ninguno)");
                }
                else
                {
                    var followedNames = kvp.Value.ConvertAll(id => 
                    {
                        var u = graph.GetUser(id);
                        return u != null ? $"{u.Name} [{id}]" : id;
                    });
                    Console.WriteLine(string.Join(", ", followedNames));
                }
            }
        }

        public void ShowBFSResult(string startUserId, string startUserName, List<string> order, int count, SocialGraph graph)
        {
            Console.WriteLine($"\n--- Recorrido BFS desde: {startUserName} [{startUserId}] ---");
            Console.WriteLine($"• Vértices alcanzados ({count}):");
            var formattedOrder = order.ConvertAll(id => 
            {
                var u = graph.GetUser(id);
                return u != null ? $"{u.Name} [{id}]" : id;
            });
            Console.WriteLine($"  " + string.Join(" -> ", formattedOrder));
        }

        public void ShowDFSResult(List<string> order, bool hasCycles, List<string> cyclePaths, SocialGraph graph)
        {
            ShowHeader("Recorrido DFS Completo");
            Console.WriteLine("• Orden de descubrimiento:");
            var formattedOrder = order.ConvertAll(id => 
            {
                var u = graph.GetUser(id);
                return u != null ? $"{u.Name} [{id}]" : id;
            });
            Console.WriteLine("  " + string.Join(" -> ", formattedOrder));

            Console.WriteLine("\n• Análisis de Ciclos Dirigidos:");
            if (hasCycles)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  ¡Se detectó al menos un ciclo en el grafo!");
                Console.ResetColor();
                Console.WriteLine("  Conexiones de retorno:");
                foreach (var path in cyclePaths)
                    Console.WriteLine($"   - {path}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  No se detectaron ciclos (DAG).");
                Console.ResetColor();
            }
        }

        public void ShowSocialMetrics(List<User> zeroInDegree, List<User> mostInfluential, List<User> mostActive, SocialGraph graph)
        {
            ShowHeader("Consultas y Métricas Sociales");

            Console.WriteLine("1. Usuarios sin seguidores (Grado de entrada = 0):");
            foreach (var u in zeroInDegree)
                Console.WriteLine($"   - {u.Name} [{u.Id}] ({u.Role})");

            Console.WriteLine("\n2. Usuarios más influyentes (Mayor grado de entrada):");
            foreach (var u in mostInfluential)
                Console.WriteLine ($"   - {u.Name} [{u.Id}] ({u.Role}) con {graph.GetInDegree(u.Id)} seguidores.");

            Console.WriteLine("\n3. Usuarios más Activos (Mayor grado de salida):");
            foreach (var u in mostActive)
                Console.WriteLine($"   - {u.Name} [{u.Id}] ({u.Role}) siguiendo a {graph.GetOutDegree(u.Id)} usuarios.");
        }

        public void ShowReachability(string fromUser, string toUser, bool isReachable)
        {
            Console.Write($"• ¿Es alcanzable '{toUser}' desde '{fromUser}'?: ");
            if (isReachable)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("SÍ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("NO");
            }
            Console.ResetColor();
        }

        public void ShowMessage(string message, bool success = true)
        {
            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[ÉXITO] {message}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERROR] {message}");
            }
            Console.ResetColor();
        }
    }
}