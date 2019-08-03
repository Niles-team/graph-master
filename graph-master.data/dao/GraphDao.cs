using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using graph_master.data.interfaces;
using graph_master.models;

namespace graph_master.data.dao
{
    public class GraphDao : BaseDao, IGraphDao
    {
        public GraphDao(string connectionString) : base(connectionString) { }

        public async Task<int> CreateGraph(Graph graph)
        {
            return await QueryFirstAsync<int>(@"
                insert into graphs (name, team_id, user_id)
                values (@name, @teamId, @userId)
                returning id
             ",
             new
             {
                 name = graph.name,
                 teamId = graph.teamId,
                 userId = graph.userId,
             });
        }

        public async Task<List<Graph>> GetTeamGraphs(int teamId)
        {
            var result = await QueryAsync<Graph>(@"
                select 
                    g.id as id,
                    g.name as name,
                    g.team_id as teamId,
                from graps g
                where g.team_id = @teamId
                order by g.date_created desc
            ", new { teamId });

            return result.ToList();
        }

        public async Task<List<Graph>> GetUserGraphs(int userId)
        {
            var result = await QueryAsync<Graph>(@"
                select 
                    g.id as id,
                    g.name as name,
                    g.user_id as userId,
                from graps g
                where g.user_id = @userId
                order by g.date_created desc
            ", new { userId });

            return result.ToList();
        }

        public Task RemoveGraph(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> UpdateGraph(Graph graph)
        {
            throw new System.NotImplementedException();
        }
    }
}