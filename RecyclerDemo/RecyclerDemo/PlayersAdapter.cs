using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.Views;
using FFImageLoading;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace RecyclerDemo
{
    class PlayersAdapter : Adapter
    {
        private readonly ObservableCollection<Player> players;

        public PlayersAdapter(IEnumerable<Player> players)
        {
            this.players = new ObservableCollection<Player>(players);
        }

        public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.list_item_player, parent, false);

            return new PlayerViewHolder(itemView);
        }

        public override void OnBindViewHolder(ViewHolder holder, int position)
        {
            var playerHolder = holder as PlayerViewHolder;

            var player = players[position];

            playerHolder.Name.Text = player.Name;
            playerHolder.Age.Text = $"{player.Age} y.o.";
            playerHolder.Nationality.Text = player.Nationality;
            playerHolder.Club.Text = player.Club;

            ImageService.Instance
                .LoadUrl(player.Photo)
                .DownSampleInDip(80)
                .Into(playerHolder.Image);
        }


        public override int ItemCount => players.Count;
    }
}