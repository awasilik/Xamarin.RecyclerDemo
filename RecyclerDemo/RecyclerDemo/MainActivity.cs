using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using CsvHelper;
using CsvHelper.Configuration;
using FFImageLoading;

namespace RecyclerDemo
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Init(savedInstanceState);

            PrepareEditableRecyclerView();
        }

        #region Init

        private void Init(Bundle savedInstanceState)
        {
            ImageService.Instance.Initialize();
            Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #endregion

        #region BasicRecycler

        private void PrepareRecyclerView()
        {
            var recycler = FindViewById<RecyclerView>(Resource.Id.players_recycler);

            var players = LoadData();
            var adapter = new PlayersAdapter(players);
            recycler.SetLayoutManager(new LinearLayoutManager(this));
            recycler.SetAdapter(adapter);
        }

        #endregion

        #region EditableRecycler

        private void PrepareEditableRecyclerView()
        {
            var recycler = FindViewById<RecyclerView>(Resource.Id.players_recycler);

            var players = LoadData();
            var adapter = new EditablePlayersAdapter(players);
            recycler.SetLayoutManager(new LinearLayoutManager(this));
            recycler.SetAdapter(adapter);
        }

        #endregion

        private IEnumerable<Player> LoadData()
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };

            using (var streamReader = new StreamReader(Assets.Open("players.csv")))
            using (var csvReader = new CsvReader(streamReader, csvConfig))
            {
                return csvReader.GetRecords<Player>().ToArray();
            }
        }
    }
}

