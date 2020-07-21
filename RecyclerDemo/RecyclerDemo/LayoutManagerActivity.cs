using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using FFImageLoading;
using RecyclerDemo.LayoutManager;

namespace RecyclerDemo
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class LayoutManagerActivity : Activity
    {
        private int counter;

        private TextView title;
        private RecyclerView recycler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Init(savedInstanceState);

            var next = FindViewById(Resource.Id.next);
            next.Click += (s, e) => SwapLayoutManager();

            SwapLayoutManager();
        }

        private void SwapLayoutManager()
        {
            var c = counter % 3;
            counter++;

            if (c == 0)
            {
                title.Text = "LinearLayoutManager";
                recycler.SetLayoutManager(new LinearLayoutManager(this));
            }
            else if (c == 1)
            {
                title.Text = "GridLayoutManager";
                recycler.SetLayoutManager(new GridLayoutManager(this, 2));
            }
            else if (c == 2)
            {
                title.Text = "StaggeredGridLayoutManager";
                recycler.SetLayoutManager(new StaggeredGridLayoutManager(2, LinearLayoutManager.Vertical));
            }
        }

        #region Init

        private static readonly string[] Images = new[]
        {
            "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQeqmUeCZ8BLfJ0v97-f40WSOhFo90hhwU_cu480yQssr9Eyyd6&s",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/71/Calico_tabby_cat_-_Savannah.jpg/220px-Calico_tabby_cat_-_Savannah.jpg",
            "https://www.antyradio.pl/var/antyradio/storage/images/adrenalina/duperele/grumpy-cat-zdechl-przyczyna-smierci-31845/7334832-1-pol-PL/Grumpy-Cat-zdechl.-Co-bylo-przyczyna-smierci-najpopularniejszego-kota-internetu_articleSM.jpg",
            "https://i.pinimg.com/originals/ce/57/a0/ce57a061217efd6afd6e57fe22b5e31a.jpg",
            "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS1IvUWyJZtnTFn9FmzjyaPXqE28HHkSvzDd1W9eh0D22A9d-s&s",
            "https://i.pinimg.com/280x280_RS/ac/cd/e1/accde1bcd977e53ec5d29c934fd0b220.jpg",
            "https://media.wired.com/photos/5cdefb92b86e041493d389df/1:1/w_988,h_988,c_limit/Culture-Grumpy-Cat-487386121.jpg",
            "https://www.washingtonian.com/wp-content/uploads/2019/02/milada-vigerova-1295750-unsplash-2048x3072.jpg",
            "https://img.webmd.com/dtmcms/live/webmd/consumer_assets/site_images/article_thumbnails/other/cat_weight_other/1800x1200_cat_weight_other.jpg?resize=600px:*",
            "https://translax.eu/wp-content/uploads/2015/06/lion.jpg",
            "https://arc-anglerfish-washpost-prod-washpost.s3.amazonaws.com/public/WALN4MAIT4I6VLRIPUMJQAJIME.jpg",
            "https://cdn.vox-cdn.com/thumbor/hY2D3zvJkmJbkBtgnpMXYZUGl8E=/0x0:7952x5304/1200x675/filters:focal(3340x2016:4612x3288)/cdn.vox-cdn.com/uploads/chorus_image/image/66701094/GettyImages_1127317526.0.jpg",
            "https://www.petguide.com/wp-content/uploads/2015/09/facts-about-cats-main.jpg",
            "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcSBlutwrVwXYiJ4WIbDdGjxM16KJ8PGiRfTCg&usqp=CAU",
        };

        private void Init(Bundle savedInstanceState)
        {
            ImageService.Instance.Initialize();
            Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.lm_activity);

            title = FindViewById<TextView>(Resource.Id.title);
            recycler = FindViewById<RecyclerView>(Resource.Id.recycler);
            recycler.SetAdapter(new LayoutManagerAdapter(Images));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #endregion

    }
}