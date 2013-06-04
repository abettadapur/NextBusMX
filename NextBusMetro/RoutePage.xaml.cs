using Bing.Maps;
using NextBusParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace NextBusMetro
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class RoutePage : NextBusMetro.Common.LayoutAwarePage
    {
        //a change for git
        const string MAPS_KEY = "An7YFWgFf8yiMsYq3JyjFvxNIaqYKWzD2dra2JgZO8dVqPuSamWUPJbsdkckX2Gr";
        Route currentRoute;
        Direction currentDirection;
        public RoutePage()
        {
            this.InitializeComponent();
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
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            Route route = navigationParameter as Route;
            switch (route.Title)
            {
                case "Red": RedButton.IsChecked = true; break;
                case "Blue": BlueButton.IsChecked = true; break;
                case "Green": GreenButton.IsChecked = true; break;
                case "Tech Trolley": TrolleyButton.IsChecked = true; break;
                case "Emory Shuttle": EmoryButton.IsChecked = true; break;
                case "Night": NightButton.IsChecked = true; break;
            }


        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
     
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            string title = (string)button.Content;
            Route r = API.ds.Routes.Where(d => d.Title == title).First();
            currentRoute = r;
            if (r.Directions.Count > 1)
            {
                directionPanel.Children.Clear();
                foreach (Direction d in r.Directions)
                {
                    RadioButton newButton = new RadioButton();
                    newButton.Checked+=directionButton_Checked;
                    newButton.Content = d.Title;
                    newButton.GroupName = "Direction";
                    newButton.Style = Application.Current.Resources["TabRadioButtonStyle"] as Style;
                    newButton.Margin = new Thickness(0, 0, 30, 0);
                    //newButton.IsChecked = true;
                    directionPanel.Children.Add(newButton);
                }
                (directionPanel.Children.First() as RadioButton).IsChecked = true;
            }
            else
            {
                directionPanel.Children.Clear();
                currentDirection = r.Directions.First();
                stopsList.ItemsSource = r.Stops;
            }




            //if ((string)button.Content == "Red")
            //    stopsList.ItemsSource = API.ds.Routes.Where(d => d.Title == "Red").First().Stops;
            //if ((string)button.Content == "Blue")
            //    stopsList.ItemsSource = API.ds.Routes.Where(d => d.Title == "Blue").First().Stops;
            //if ((string)button.Content == "Green")
            //    stopsList.ItemsSource = API.ds.Routes.Where(d => d.Title == "Green").First().Stops;
            //if ((string)button.Content == "Trolley")
            //    stopsList.ItemsSource = API.ds.Routes.Where(d => d.Title == "Tech Trolley").First().Stops;
            //if ((string)button.Content == "Night")
            //    stopsList.ItemsSource = API.ds.Routes.Where(d => d.Title == "Night").First().Stops;
            //if ((string)button.Content == "Emory")
            //    stopsList.ItemsSource = API.ds.Routes.Where(d => d.Title == "Emory Shuttle").First().Stops;
        }

        private void directionButton_Checked(object sender, RoutedEventArgs e)
        {
            var directions = currentRoute.Directions;
            string title = (sender as RadioButton).Content as string;
            Direction d = directions.Where(f => f.Title == title).First();
            stopsList.ItemsSource = d.Stops;
            currentDirection = d;
        }

        private async void stopsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (stopsList.SelectedItem == null)
            {
                PredictionBlock.Text = "";
                SubPredictionBlock.Text = "";
                StopMap.Children.Clear();
            }
            else
            {
                var predictions = await API.getPrediction(currentRoute.Tag, currentDirection.Tag, (stopsList.SelectedItem as Stop).Tag);
                if (predictions != null)
                {
                    SubPredictionBlock.Text = "";
                    PredictionBlock.Text = predictions[0] + " minutes";
                    for (int i = 1; i < predictions.Length; i++)
                        SubPredictionBlock.Text += predictions[i] + " minutes  ";
                }
                else
                    PredictionBlock.Text = "No predictions could be found";

                Pushpin p = new Pushpin();
                MapLayer.SetPosition(p, new Location((stopsList.SelectedItem as Stop).Latitude, (stopsList.SelectedItem as Stop).Longitude));
                StopMap.Children.Clear();
                StopMap.Children.Add(p);
                StopMap.ZoomLevel = 17;
                StopMap.ShowScaleBar = false;
                StopMap.ShowNavigationBar = false;
                StopMap.Credentials = MAPS_KEY;
                StopMap.SetView(new Location((stopsList.SelectedItem as Stop).Latitude, (stopsList.SelectedItem as Stop).Longitude));
                StopMap.Visibility = Visibility.Visible;

            }
                
        }
    }
}
