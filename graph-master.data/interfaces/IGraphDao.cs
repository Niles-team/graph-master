using System.Collections.Generic;
using System.Threading.Tasks;
using graph_master.models;

namespace graph_master.data.interfaces
{
    public interface IGraphDao
    {
        Task<Graph> CreateGraph(Graph graph);
        Task<Graph> UpdateGraph(Graph graph);
        Task<List<Graph>> GetUserGraphs(int userId);
        Task<List<Graph>> GetTeamGraphs(int teamId);
        Task RemoveGraph(int id);
    }
}