using Android.Views;
using Android.Widget;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace RecyclerDemo.Basic
{
    public class BasicPlayerViewHolder : ViewHolder
    {
        public ImageView Image { get; }

        public TextView Name { get; }

        public TextView Age { get; }

        public TextView Nationality { get; }

        public TextView Club { get; }

        public BasicPlayerViewHolder(View itemView)
            : base(itemView)
        {
            Image = itemView.FindViewById<ImageView>(Resource.Id.player_image);
            Name = itemView.FindViewById<TextView>(Resource.Id.player_name);
            Age = itemView.FindViewById<TextView>(Resource.Id.player_age);
            Nationality = itemView.FindViewById<TextView>(Resource.Id.player_nationality);
            Club = itemView.FindViewById<TextView>(Resource.Id.player_club);
        }
    }
}