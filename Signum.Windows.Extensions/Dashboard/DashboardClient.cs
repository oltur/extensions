﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Entities.Dashboard;
using System.Reflection;
using Signum.Windows.Dashboard.Admin;
using System.Windows.Controls;
using Signum.Entities;
using System.Windows;
using Signum.Utilities;
using System.Linq.Expressions;
using Signum.Windows.Dashboard;
using Signum.Windows.Basics;
using Signum.Entities.UserQueries;
using Signum.Entities.Reflection;
using Signum.Services;
using Signum.Windows.Authorization;
using Signum.Entities.Chart;
using Signum.Windows.UserAssets;

namespace Signum.Windows.Dashboard
{
    public static class DashboardClient
    {
        public static Dictionary<Type, PartView> PartViews = new Dictionary<Type, PartView>();

        public static void Start()
        {
            if (Navigator.Manager.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                TypeClient.Start();

                UserAssetsClient.Start();
                UserAssetsClient.RegisterExportAssertLink<DashboardDN>();

                Navigator.AddSettings(new List<EntitySettings>()
                {
                    new EntitySettings<DashboardDN>() { View = e => new DashboardEdit(), Icon = ExtensionsImageLoader.GetImageSortName("controlPanel.png") },

                    new EntitySettings<CountSearchControlPartDN>() { View = e => new CountSearchControlPartEdit() },
                    new EntitySettings<LinkListPartDN>() { View = e => new LinkListPartEdit() },
                    new EntitySettings<UserQueryPartDN>() { View = e => new UserQueryPartEdit() },                
                    new EntitySettings<UserChartPartDN>() { View = e => new UserChartPartEdit() }
                });

                PartViews.Add(typeof(UserQueryPartDN), new PartView
                {
                    ViewControl = () => new UserQueryPartView(),
                    IsTitleEnabled = () => Navigator.IsNavigable(typeof(UserQueryDN), true),
                    OnTitleClick = part =>
                    {
                        Navigator.Navigate(((UserQueryPartDN)part).UserQuery);
                    }
                });

                PartViews.Add(typeof(UserChartPartDN), new PartView
                {
                    ViewControl = () => new UserChartPartView(),
                    IsTitleEnabled = ()=> Navigator.IsNavigable(typeof(UserChartDN), true),
                    OnTitleClick = part =>
                    {
                        Navigator.Navigate(((UserChartPartDN)part).UserChart);
                    }
                });

                PartViews.Add(typeof(CountSearchControlPartDN), new PartView
                {
                    ViewControl = () => new CountSearchControlPartView()
                });

                PartViews.Add(typeof(LinkListPartDN), new PartView
                {
                    ViewControl = () => new LinkListPartView()
                });

                LinksClient.RegisterEntityLinks<DashboardDN>((cp, ctrl) => new[]{  
                    !DashboardPermission.ViewDashboard.IsAuthorized() ? null:  
                    new QuickLinkAction(DashboardMessage.Preview, () => Navigate(cp, null))
                });

                LinksClient.RegisterEntityLinks<IdentifiableEntity>((entity, ctrl) =>
                {       
                    if(!DashboardPermission.ViewDashboard.IsAuthorized())
                        return null;

                    return Server.Return((IDashboardServer us) => us.GetDashboardEntity(entity.EntityType))
                        .Select(cp => new DashboardQuickLink(cp, entity)).ToArray();
                });
            }
        }

        class DashboardQuickLink : QuickLink
        {
            Lite<DashboardDN> controlPanel;
            Lite<IdentifiableEntity> entity;

            public DashboardQuickLink(Lite<DashboardDN> controlPanel, Lite<IdentifiableEntity> entity)
            {
                this.ToolTip = controlPanel.ToString(); 
                this.Label = controlPanel.ToString();
                this.controlPanel = controlPanel;
                this.entity = entity;
                this.IsVisible = true;
                this.Icon = ExtensionsImageLoader.GetImageSortName("controlPanel.png");
            }

            public override void Execute()
            {
                DashboardClient.Navigate(controlPanel, entity.Retrieve());
            }

            public override string Name
            {
                get { return controlPanel.Key(); }
            }
        }

        public static void Navigate(Lite<DashboardDN> controlPanel, IdentifiableEntity currentEntity)
        {
            Navigator.OpenIndependentWindow(() => new DashboardWindow
            {
                tbDashboard = { Text = NormalWindowMessage.Loading0.NiceToString().Formato(controlPanel.EntityType.NiceName()) }
            },
            afterShown: win =>
            {
                var cp = controlPanel.Retrieve();

                if (cp.EntityType != null)
                {
                    win.CurrentEntity = currentEntity;
                }

                win.DataContext = controlPanel.Retrieve();
            });
        }
    }

    public class PartView
    {
        public Expression<Func<FrameworkElement>> ViewControl;
        public Action<IPartDN> OnTitleClick;
        public Func<bool> IsTitleEnabled;
    }

    public class DashboardViewDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return null;

            PartView pv = DashboardClient.PartViews.TryGetC(item.GetType());

            if (pv == null)
                return null;

            return Fluent.GetDataTemplate(pv.ViewControl);
        }
    }
}
