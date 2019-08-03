using System.Collections.Generic;
using System.Threading.Tasks;
using graph_master.models;

namespace graph_master.data.interfaces
{
    public interface IGraphDao
    {
        Task<int> CreateGraph(Graph graph);
        Task<int> UpdateGraph(Graph graph);
        Task<List<Graph>> GetUserGraphs(int userId);
        Task<List<Graph>> GetTeamGraphs(int teamId);
        Task RemoveGraph(int id);
    }
}