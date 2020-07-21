using System;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace RecyclerDemo.Editable
{
    public class PlayerViewHolder : ViewHolder, IEditableViewHolder
    {
        private EventHandler clickDelegate;

        private readonly View background;

        public ImageView Image { get; }

        public TextView Name { get; }

        public TextView Age { get; }

        public TextView Nationality { get; }

        public TextView Club { get; }

        public PlayerViewHolder(View itemView)
            : base(itemView)
        {
            Image = itemView.FindViewById<ImageView>(Resource.Id.player_image);
            Name = itemView.FindViewById<TextView>(Resource.Id.player_name);
            Age = itemView.FindViewById<TextView>(Resource.Id.player_age);
            Nationality = itemView.FindViewById<TextView>(Resource.Id.player_nationality);
            Club = itemView.FindViewById<TextView>(Resource.Id.player_club);

            background = itemView.FindViewById(Resource.Id.item_background);
            OnCleared();
        }

        public void OnClick(Action action)
        {
            ItemView.Click -= clickDelegate;

            clickDelegate = (s, e) => action?.Invoke();

            ItemView.Click += clickDelegate;
        }

        public void OnDragged()
        {
            background.SetBackgroundColor(Color.LightGray);
        }

        public void OnSwiped()
        {
            background.SetBackgroundColor(Color.IndianRed);
        }

        public void OnCleared()
        {
            background.SetBackgroundColor(Color.White);
        }
    }
}