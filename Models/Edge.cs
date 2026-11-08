namespace CampusNet.Models
{
    public class Edge
    {
        public string SourceId { get; }
        public string TargetId { get; }

        public Edge(string sourceId, string targetId)
        {
            SourceId = sourceId;
            TargetId = targetId;
        }
    }
}
