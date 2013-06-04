using NextBusMetro.Converters;
using NextBusMetro.Items;
using NextBusParser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace NextBusMetro
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    
    public sealed partial class HomePage : NextBusMetro.Common.LayoutAwarePage
    {
        ObservableCollection<TileGroup> groups;
        Geolocator geo;
        public HomePage()
        {
            this.InitializeComponent();
            groups = new ObservableCollection<TileGroup>();
            geo = new Geolocator();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            progessBar.Visibility = Visibility.Visible;
            if (!API.isInitialized)
               await API.initializeRoutes();
            // TODO: Assign a collection of bindable groups to this.DefaultViewModel["Groups"]
            
            TileGroup nearbyGroup = new TileGroup();
            TileGroup favoritesGroup = new TileGroup();
            TileGroup actionsGroup = new TileGroup();
            TileGroup routesGroup = new TileGroup();

            routesGroup.Title = "Routes";
            nearbyGroup.Title = "Nearby Stops";
            favoritesGroup.Title = "Favorite Stops";
            actionsGroup.Title = "Actions";

            double lat = 999;
            double longt = 999;
            try
            {
                Geoposition pos = await geo.GetGeopositionAsync();
                lat = pos.Coordinate.Latitude;
                longt = pos.Coordinate.Longitude;
            }
            catch (UnauthorizedAccessException uaex)
            { }
            
            #region All Routes
            List<Route> routes = API.ds.Routes;
            foreach (Route r in routes)
            {
                TileItem item = new TileItem();
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = ColorConverter.getColor(r.Color);
                item.Items.Add(new TilePiece() { Title = r.Title, Details = r.Stops.Count+" Stops" });
                item.Color = brush;
                item.AssignedObject = r;
                routesGroup.Items.Add(item);

             

            }
            groups.Add(routesGroup);
#endregion

            #region Nearby Stops
            if (lat != 999)
            {
                foreach (Route r in routes)
                {
                    int counter = 0;
                    foreach (Direction d in r.Directions)
                    {
                        foreach (Stop s in d.Stops)
                        {
                            if (counter > 2)
                                break;

                            double distance = 0.6214 * calculateDistance(lat, longt, s.Latitude, s.Longitude);
                            if (distance < 0.5)
                            {
                                TileItem nearbyItem = new TileItem();
                                SolidColorBrush brush = new SolidColorBrush();
                                brush.Color = ColorConverter.getColor(r.Color);
                                nearbyItem.Color = brush;
                                nearbyItem.AssignedObject = s;

                                int[] prediction = await API.getPrediction(r.Tag, d.Tag, s.Tag);
                                if (prediction != null)
                                {
                                    string predictionStr = "";
                                    foreach (int i in prediction)
                                        predictionStr += i + " ";
                                    nearbyItem.Items.Add(new TilePiece() { Title = s.Title, Subtitle = predictionStr });
                                    nearbyGroup.Items.Add(nearbyItem);

                                    counter++;
                                }

                            }

                        }
                    }


                }
            }
            #endregion




            if(nearbyGroup.Items.Count!=0)
                groups.Add(nearbyGroup);

            if (nearbyGroup.Items.Count != 0)
            {
                groups.Add(favoritesGroup);
            }
            actionsGroup.Items.Add(new IItem());
            groups.Add(actionsGroup);
            
            
            this.DefaultViewModel["Groups"] = groups;
            progessBar.Visibility = Visibility.Collapsed;
        }

        private double calculateDistance(double lat, double longt, double lat2,  double longt2)
        {
            double dLat = Math.PI * (lat - lat2) / 180.0;
            double dLong = Math.PI * (longt - longt2) / 180.0;

            lat = Math.PI * lat / 180.0;
            lat2 = Math.PI * lat2 / 180.0;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLong / 2) * Math.Sin(dLong / 2) * Math.Cos(lat) * Math.Cos(lat2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return 6371 * c;
        }

        private void GridViewItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is TileItem)
            {
                TileItem clicked = e.ClickedItem as TileItem;
                object assignedObject = clicked.AssignedObject;
                if (assignedObject is Route)
                {
                    this.Frame.Navigate(typeof(RoutePage), assignedObject);
                }
            }
                
        }
    }
}
