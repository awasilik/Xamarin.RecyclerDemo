using System.Collections.ObjectModel;
using Android.Views;
using FFImageLoading;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace RecyclerDemo.Basic
{
    public class BasicPlayersAdapter : Adapter
    {
        private readonly ObservableCollection<Player> players;

        public BasicPlayersAdapter(ObservableCollection<Player> players)
        {
            this.players = players;
        }

        public override int ItemCount => players.Count;

        public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.list_item_player, parent, false);

            return new BasicPlayerViewHolder(itemView);
        }

        public override void OnBindViewHolder(ViewHolder holder, int position)
        {
            var playerHolder = holder as BasicPlayerViewHolder;

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
    }
}