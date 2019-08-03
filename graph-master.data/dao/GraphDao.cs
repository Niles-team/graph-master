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

        public async Task<Graph> CreateGraph(Graph graph)
        {
            return await Task.Run(() =>
            {
                graph.Id = QuerySingleOrDefault<int>(@"
                    insert into Graphs (Name, TeamId, UserId)
                    values (@name, @teamId, @userId)
                    returning Id
                ",
                 new
                 {
                     name = graph.Name,
                     teamId = graph.TeamId,
                     userId = graph.UserId,
                 });

                foreach (var link in graph.Links)
                {
                    link.Id = QuerySingleOrDefault<int>(@"
                        insert into Links (GraphId, Data)
                        values(@graphId, @data)
                        returning Id
                    ", new { graphId = graph.Id, data = link.Data });
                }
                foreach (var node in graph.Nodes)
                {
                    node.Id = QuerySingleOrDefault<int>(@"
                        insert into Nodes (GraphId, Data)
                        values(@graphId, @data)
                    ", new { graphId = graph.Id, data = node.Data });
                }

                return graph;
            });
        }

        public async Task<Graph> UpdateGraph(Graph graph)
        {
            return await Task.Run(() =>
            {
                Execute(@"
                    update Graphs 
                    set 
                        Name = @name,
                        TeamId = @teamId,
                        UserId = @userId
                    where Id = id
                    returning id
                ",
                new
                {
                    id = graph.Id,
                    name = graph.Name,
                    teamId = graph.TeamId,
                    userId = graph.UserId,
                });
                var links = getGraphLinks(graph.Id);
                var nodes = getGraphNodes(graph.Id);

                var linksToInsert = graph.Links.Where(o => !links.Any(l => l.Id == o.Id));
                var linksToUpdate = graph.Links.Where(o => links.Any(l => l.Id == o.Id));
                var linksToDelete = links.Where(o => !graph.Links.Any(l => l.Id == o.Id))
                                         .Select(o => o.Id).ToList();

                foreach (var link in linksToInsert)
                {
                    Execute(@"
                    insert into Links (GraphId, Data)
                    values(@graphId, @data)
                    ", new { graphId = graph.Id, data = link.Data });
                }
                foreach (var link in linksToUpdate)
                {
                    Execute(@"
                        update Links
                        set
                            Data = @data
                        where Id = @id
                    ", new { id = link.Id, data = link.Data });
                }
                Execute(@"
                    delete from Links
                    where Id in (@ids)
                ", new { ids = linksToDelete });

                var nodesToInsert = graph.Nodes.Where(o => !nodes.Any(n => n.Id == o.Id));
                var nodesToUpdate = graph.Nodes.Where(o => nodes.Any(n => n.Id == o.Id));
                var nodesToDelete = nodes.Where(o => !graph.Nodes.Any(n => n.Id == o.Id));

                foreach (var node in nodesToInsert)
                {
                    Execute(@"
                    insert into Nodes (GraphId, Data)
                    values(@graphId, @data)
                    ", new { graphId = graph.Id, data = node.Data });
                }
                foreach (var node in nodesToUpdate)
                {
                    Execute(@"
                        update Nodes
                        set
                            Data = @data
                        where Id = @id
                    ", new { id = node.Id, data = node.Data });
                }
                Execute(@"
                    delete from Nodes
                    where Id in (@ids)
                ", new { ids = nodesToDelete });

                return graph;
            });
        }

        public async Task<List<Graph>> GetTeamGraphs(int teamId)
        {
            return await Task.Run(() =>
            {
                var result = Query<Graph>(@"
                    select Id, Name, TeamId,
                    from graphs
                    where TeamId = @teamId
                    order by DateCreated desc
                ", new { teamId }).ToList();

                var nodes = getGraphNodes(result.Select(o => o.Id));
                var links = getGraphLinks(result.Select(o => o.Id));

                result.ForEach(o => o.Nodes = nodes.Where(n => n.GraphId == o.Id).ToList());
                result.ForEach(o => o.Links = links.Where(l => l.GraphId == o.Id).ToList());

                return result.ToList();
            });
        }

        public async Task<List<Graph>> GetUserGraphs(int userId)
        {
            return await Task.Run(() =>
            {
                var result = Query<Graph>(@"
                    select Id, Name, UserId,
                    from graphs
                    where UserId = @userId
                    order by DateCreated desc
                ", new { userId }).ToList();

                var nodes = getGraphNodes(result.Select(o => o.Id));
                var links = getGraphLinks(result.Select(o => o.Id));

                result.ForEach(o => o.Nodes = nodes.Where(n => n.GraphId == o.Id).ToList());
                result.ForEach(o => o.Links = links.Where(l => l.GraphId == o.Id).ToList());

                return result.ToList();
            });
        }

        public async Task RemoveGraph(int id)
        {
            await ExecuteAsync(@"
                delete from Graphs
                where Id = @id
            ", new { id });
        }

        private List<Link> getGraphLinks(int id)
        {
            return Query<Link>(@"
                select Id, Data
                from Links
                where GraphId = id
                order by Id
                ", new { id })
                .ToList();
        }

        private List<Node> getGraphNodes(int id)
        {
            return Query<Node>(@"
                select Id, Data
                from Nodes
                where GraphId = id
                order by Id
                ", new { id })
                .ToList();
        }

        private List<Link> getGraphLinks(IEnumerable<int> ids)
        {
            return Query<Link>(@"
                select Id, Data
                from Links
                where GraphId in (@ids)
                order by Id
                ", new { ids = ids.ToList() })
                .ToList();
        }

        private List<Node> getGraphNodes(IEnumerable<int> ids)
        {
            return Query<Node>(@"
                select Id, Data
                from Nodes
                where GraphId in (@ids)
                order by Id
                ", new { ids = ids.ToList() })
                .ToList();
        }
    }
}