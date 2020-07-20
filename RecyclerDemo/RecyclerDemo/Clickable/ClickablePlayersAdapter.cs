using System;
using System.Collections.ObjectModel;
using Android.Views;
using FFImageLoading;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace RecyclerDemo.Clickable
{
    public class ClickablePlayersAdapter : Adapter
    {
        private readonly ObservableCollection<Player> players;

        public event EventHandler<Player> OnItemClicked;

        public ClickablePlayersAdapter(ObservableCollection<Player> players)
        {
            this.players = players;
        }

        public override int ItemCount => players.Count;

        public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.list_item_player, parent, false);

            return new ClickablePlayerViewHolder(itemView);
        }

        public override void OnBindViewHolder(ViewHolder holder, int position)
        {
            var playerHolder = holder as ClickablePlayerViewHolder;

            var player = players[position];

            playerHolder.Name.Text = player.Name;
            playerHolder.Age.Text = $"{player.Age} y.o.";
            playerHolder.Nationality.Text = player.Nationality;
            playerHolder.Club.Text = player.Club;

            ImageService.Instance
                .LoadUrl(player.Photo)
                .DownSampleInDip(80)
                .Into(playerHolder.Image);
            
            playerHolder.OnClick(() => OnItemClicked?.Invoke(this, player));
        }
    }
}