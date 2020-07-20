using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using CsvHelper;
using CsvHelper.Configuration;
using FFImageLoading;
using RecyclerDemo.Basic;
using RecyclerDemo.Clickable;
using RecyclerDemo.Editable;
using RecyclerDemo.Final;

namespace RecyclerDemo
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private ObservableCollection<Player> players;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Init(savedInstanceState);
            players = new ObservableCollection<Player>(LoadData());

            //PrepareBasicRecycler();
            //PrepareClickableRecycler();
            //PrepareEditableRecycler();
            PrepareRecyclerView();
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

        private void PrepareBasicRecycler()
        {
            var recycler = FindViewById<RecyclerView>(Resource.Id.players_recycler);

            var adapter = new BasicPlayersAdapter(players);
            recycler.SetLayoutManager(new LinearLayoutManager(this));
            recycler.SetAdapter(adapter);
        }

        #endregion

        #region ClickableRecycler

        private void PrepareClickableRecycler()
        {
            var recycler = FindViewById<RecyclerView>(Resource.Id.players_recycler);

            var adapter = new ClickablePlayersAdapter(players);
            adapter.OnItemClicked += (s, e) => { Toast.MakeText(this, e.Name, ToastLength.Short).Show(); };

            recycler.SetLayoutManager(new LinearLayoutManager(this));
            recycler.SetAdapter(adapter);
        }

        #endregion

        #region EditableRecycler

        private void PrepareEditableRecycler()
        {
            var recycler = FindViewById<RecyclerView>(Resource.Id.players_recycler);

            var adapter = new EditablePlayersAdapter(players);

            recycler.SetLayoutManager(new LinearLayoutManager(this));
            recycler.SetAdapter(adapter);

            var motionController = new SimpleMotionController(adapter);
            var itemTouchHelper = new ItemTouchHelper(motionController);
            itemTouchHelper.AttachToRecyclerView(recycler);
        }

        #endregion

        #region FinalRecycler

        private void PrepareRecyclerView()
        {
            var recycler = FindViewById<RecyclerView>(Resource.Id.players_recycler);

            var adapter = new PlayersAdapter(players);

            recycler.SetLayoutManager(new LinearLayoutManager(this));
            recycler.SetAdapter(adapter);

            var motionController = new MotionController(adapter);
            var itemTouchHelper = new ItemTouchHelper(motionController);
            itemTouchHelper.AttachToRecyclerView(recycler);

            // we add it cause onChildDraw is not being called when scrolling recycler.
            // this causes the button to not disappear when user scrolls list
            recycler.AddItemDecoration(new RecyclerItemDecoration(motionController.OnDraw));

            var addButton = FindViewById(Resource.Id.add_player_button);
            addButton.Visibility = ViewStates.Visible;
            addButton.Click += (s, e) => adapter.AddPlayer(GetSamplePlayer());
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

        private static Player GetSamplePlayer()
        {
            var player = new Player();
            player.Name = "Random dude";
            player.Age = 40;
            player.Nationality = "Poland";
            player.Club = "Demant KS";
            player.Photo = "https://i.ibb.co/z2Nj7CG/New-Project.png";

            return player;
        }
    }
}

