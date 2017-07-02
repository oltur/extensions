﻿using Signum.Engine;
using Signum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Signum.Engine.Basics;
using Signum.Utilities.Reflection;
using Signum.Utilities;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;
using Signum.React.Filters;
using Signum.Engine.DynamicQuery;
using Signum.React.ApiControllers;
using Signum.Entities.Tree;
using Signum.Engine.Tree;
using Signum.Entities.DynamicQuery;
using Signum.Entities.Basics;

namespace Signum.React.Tree
{
    public class TreeController : ApiController
    {
        [Route("api/tree/children/{typeName}/{id}"), HttpGet]
        public List<TreeNode> GetChildren(string typeName, string id) {
            Type type = TypeLogic.GetType(typeName);

            var lite = (Lite<TreeEntity>)Lite.ParsePrimaryKey(type, id);

            return giGetChildrenGeneric.GetInvoker(type)(lite)
                .OrderBy(a => a.route)
                .Select(a => new TreeNode
                {
                    lite = a.lite,
                    level = a.level,
                    disabled = a.disabled,
                    childrenCount = a.childrenCount,
                    loadedChildren = new List<TreeNode>()
                }).ToList();
        }

        [Route("api/tree/roots/{typeName}"), HttpGet]
        public List<TreeNode> GetRoots(string typeName)
        {
            Type type = TypeLogic.GetType(typeName);
            
            return ToTreeNodes(giGetChildrenGeneric.GetInvoker(type)(null));
        }

        static GenericInvoker<Func<Lite<TreeEntity>, List<TreeInfo>>> giGetChildrenGeneric =
            new GenericInvoker<Func<Lite<TreeEntity>, List<TreeInfo>>>(lite => GetChildrenGeneric<TreeEntity>(lite));
        static List<TreeInfo> GetChildrenGeneric<T>(Lite<T> lite) 
            where T: TreeEntity
        {
            var parentRoute = lite == null ? SqlHierarchyId.GetRoot() : lite.InDB(a => a.Route);
            //var parentRoute = lite == null ? SqlHierarchyId.GetRoot() : (lite.Id.ToString() == "6" ? SqlHierarchyId.Parse("/1/") : SqlHierarchyId.Parse("/1/1/"));
            var disabledMixin = MixinDeclarations.IsDeclared(typeof(T), typeof(DisabledMixin));
            return Database.Query<T>()
                .Where(t => (bool)(t.Route.GetAncestor(1) == parentRoute))
                .Select(t => new TreeInfo
                {
                    route = t.Route,
                    lite = t.ToLite(),
                    level = t.Level(),
                    disabled = disabledMixin && t.Mixin<DisabledMixin>().IsDisabled,
                    childrenCount = t.Children().Count(),
                })
                .ToList();
        }

        [Route("api/tree/findNodes/{typeName}"), HttpPost]
        public List<TreeNode> FindNodes(string typeName, FindNodesRequest request) {

            Type type = TypeLogic.GetType(typeName);

            var list = request.filters.Count == 0 ?
                giGetChildrenGeneric.GetInvoker(type)(null) : 
                giFindNodesGeneric.GetInvoker(type)(request.filters);

            var expanded = giFindExpandedGeneric.GetInvoker(type)(request.expandedNodes);

            return ToTreeNodes(list.Concat(expanded).ToList());
        }

        public class FindNodesRequest
        {
            public List<FilterTS> filters;
            public List<Lite<TreeEntity>> expandedNodes;
        }

        static GenericInvoker<Func<List<FilterTS>, List<TreeInfo>>> giFindNodesGeneric =
            new GenericInvoker<Func<List<FilterTS>, List<TreeInfo>>>(filters => FindNodesGeneric<TreeEntity>(filters));
        static List<TreeInfo> FindNodesGeneric<T>(List<FilterTS> filtersTs)
            where T : TreeEntity
        {
            var qd = DynamicQueryManager.Current.QueryDescription(typeof(T));
            var filters = filtersTs.Select(f => f.ToFilter(qd, false)).ToList();

            var disabledMixin = MixinDeclarations.IsDeclared(typeof(T), typeof(DisabledMixin));
            var list = DynamicQueryManager.Current.GetEntities(new QueryEntitiesRequest { QueryName = typeof(T), Filters = filters, Orders = new List<Order>() })
                            .Select(a => (T)a.Entity)
                            .SelectMany(t => t.Ascendants())
                            .Select(t => new TreeInfo
                            {
                                route = t.Route,
                                lite = t.ToLite(),
                                level = t.Level(),
                                disabled = disabledMixin && t.Mixin<DisabledMixin>().IsDisabled,
                                childrenCount = t.Children().Count(),
                            }).ToList();

            return list;
        }

        static GenericInvoker<Func<List<Lite<TreeEntity>>, List<TreeInfo>>> giFindExpandedGeneric =
             new GenericInvoker<Func<List<Lite<TreeEntity>>, List<TreeInfo>>>(expanded => FindExpandedGeneric<TreeEntity>(expanded));
        static List<TreeInfo> FindExpandedGeneric<T>(List<Lite<TreeEntity>> expanded)
            where T : TreeEntity
        {
            if (expanded == null || expanded.Count == 0)
                return new List<TreeInfo>();

            var disabledMixin = MixinDeclarations.IsDeclared(typeof(T), typeof(DisabledMixin));
            var list = Database.Query<T>()
                .Where(t => expanded.Contains(t.Parent().ToLite()))
                            .Select(t => new TreeInfo
                            {
                                route = t.Route,
                                lite = t.ToLite(),
                                level = t.Level(),
                                disabled = disabledMixin && t.Mixin<DisabledMixin>().IsDisabled,
                                childrenCount = t.Children().Count(),
                            }).ToList();

            return list;
        }

        static List<TreeNode> ToTreeNodes(List<TreeInfo> infos)
        {
            var dictionary = infos.Distinct(a => a.route).ToDictionary(a => a.route);

            var parentNodes = TreeHelper.ToTreeC(dictionary.Values, a => a.route.GetLevel() == 1 ? null :
                 dictionary.GetOrThrow(a.route.GetAncestor(1)));

            return parentNodes.OrderBy(a => a.Value.route).Select(n => new TreeNode(n)).ToList();
        }

    }

#pragma warning disable IDE1006 // Naming Styles
    class TreeInfo
    {
        public int childrenCount { get; set; }
        public Lite<TreeEntity> lite { get; set; }
        public bool disabled { get; set; }
        public SqlHierarchyId route { get; set; }
        public short level { get; set; }
    }

    public class TreeNode
    {
        public TreeNode() { }
        internal TreeNode(Node<TreeInfo> node)
        {
            this.lite = node.Value.lite;
            this.disabled = node.Value.disabled;
            this.childrenCount = node.Value.childrenCount;
            this.loadedChildren = node.Children.OrderBy(a => a.Value.route).Select(a => new TreeNode(a)).ToList();
            this.level = node.Value.level;
        }
        
        public Lite<TreeEntity> lite { set; get; }
        public bool disabled { get; set; }
        public int childrenCount { set; get; }
        public List<TreeNode> loadedChildren { set; get; }
        public short level { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles
}
