﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Entities.DynamicQuery;
using Signum.Windows.Chart;
using Signum.Entities.Chart;
using System.Reflection;
using Signum.Entities.Reports;
using Signum.Entities.Authorization;
using Signum.Windows.Authorization;
using System.Windows;
using Signum.Utilities;
using Signum.Windows.Properties;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace Signum.Windows.Chart
{
    public static class ChartClient
    {
        public static readonly DependencyProperty UserChartProperty =
            DependencyProperty.RegisterAttached("UserChart", typeof(UserChartDN), typeof(ChartClient), new UIPropertyMetadata(null));
        public static UserChartDN GetUserChart(DependencyObject obj)
        {
            return (UserChartDN)obj.GetValue(UserChartProperty);
        }
        public static void SetUserChart(DependencyObject obj, UserChartDN value)
        {
            obj.SetValue(UserChartProperty, value);
        }


        public static void Start()
        {
            if (Navigator.Manager.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                QueryClient.Start();

                Navigator.AddSettings(new List<EntitySettings>()
                {
                    new EntitySettings<UserChartDN> { View = e => new UserChart() },
                    new EntitySettings<ChartScriptDN> { View = e => new ChartScript() },
                    new EmbeddedEntitySettings<ChartScriptParameterDN> { View = (e,p) => new ChartScriptParameter(p) }
                });

                SearchControl.GetMenuItems += SearchControl_GetCustomMenuItems;

                UserChartDN.SetConverters(query => QueryClient.GetQueryName(query.Key), queryname => QueryClient.GetQuery(queryname));

                string processName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

                var main = Registry.CurrentUser
                    .OpenSubKey("Software")
                    .OpenSubKey("Microsoft")
                    .OpenSubKey("Internet Explorer")
                    .OpenSubKey("Main", true)
                    .CreateSubKey("FeatureControl")
                    .CreateSubKey("FEATURE_BROWSER_EMULATION");

                main.SetValue(processName, 9999, RegistryValueKind.DWord);

                Constructor.Register<UserChartDN>(win =>
                {
                    MessageBox.Show(win,
                        Signum.Windows.Extensions.Properties.Resources._0CanOnlyBeCreatedFromTheChartWindow.Formato(typeof(UserChartDN).NicePluralName()),
                        Signum.Windows.Extensions.Properties.Resources.Create,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return null;
                }); 

                SearchControl.GetMenuItems += SearchControl_GetCustomMenuItems;

                ChartUtils.RemoveNotNullValidators();
            }
        }


        static MenuItem SearchControl_GetCustomMenuItems(SearchControl sc)
        {
            if (!ChartPermissions.ViewCharting.IsAuthorized())
                return null;

            var miResult = new MenuItem
            {

                Header = Signum.Windows.Extensions.Properties.Resources.Chart,
                Icon = ExtensionsImageLoader.GetImageSortName("chartIcon.png").ToSmallImage()
            };

            miResult.Click += delegate
            {
                ChartRequestWindow window = new ChartRequestWindow()
                {
                    FilterOptions = sc.FilterOptions,
                    DataContext = new ChartRequest(sc.QueryName)
                    {
                        Filters = sc.FilterOptions.Select(fo => fo.ToFilter()).ToList(),
                    }
                };

                window.Show();
            };

            return miResult;
        }
    }
}
