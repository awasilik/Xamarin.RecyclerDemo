using Android.Views;
using Android.Widget;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace RecyclerDemo
{
    public class PlayerViewHolder : ViewHolder
    {
        public ImageView Image { get; set; }

        public TextView Name { get; set; }

        public TextView Age { get; set; }

        public TextView Nationality { get; set; }

        public TextView Club { get; set; }

        public PlayerViewHolder(View itemView)
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