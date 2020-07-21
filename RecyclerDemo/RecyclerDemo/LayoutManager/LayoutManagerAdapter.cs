using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using FFImageLoading;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace RecyclerDemo.LayoutManager
{
    public class LayoutManagerAdapter : RecyclerView.Adapter
    {
        private readonly string[] images;

        public LayoutManagerAdapter(string[] images)
        {
            this.images = images;
        }

        public override int ItemCount => images.Length;

        public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.list_item_image, parent, false);

            return new LayoutManagerViewHolder(itemView);
        }

        public override void OnBindViewHolder(ViewHolder holder, int position)
        {
            var imageView = holder.ItemView as ImageView;

            var imageUrl = images[position];

            ImageService.Instance
                .LoadUrl(imageUrl)
                .DownSampleInDip(320)
                .Into(imageView);
        }
    }
}