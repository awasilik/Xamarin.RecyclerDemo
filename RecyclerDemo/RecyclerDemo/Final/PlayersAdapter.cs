using System.Collections.ObjectModel;
using Android.Views;
using FFImageLoading;
using RecyclerDemo.Editable;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace RecyclerDemo.Final
{
    public class PlayersAdapter : Adapter, IEditableAdapter
    {
        private readonly ObservableCollection<Player> players;

        public PlayersAdapter(ObservableCollection<Player> players)
        {
            this.players = players;
        }

        public override int ItemCount => players.Count;

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

        public void AddPlayer(Player player)
        {
            players.Add(player);
            NotifyItemInserted(ItemCount - 1);
        }

        public void MoveItem(int fromPosition, int toPosition)
        {
            players.Move(fromPosition, toPosition);
            NotifyItemMoved(fromPosition, toPosition);
        }

        public void RemoveItem(int position)
        {
            players.RemoveAt(position);
            NotifyItemRemoved(position);
        }

        public void EditItem(int position)
        {
        }
    }
}