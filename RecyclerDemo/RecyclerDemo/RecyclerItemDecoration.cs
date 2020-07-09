using System;
using Android.Graphics;
using AndroidX.RecyclerView.Widget;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace RecyclerDemo
{
    public class RecyclerItemDecoration : ItemDecoration
    {
        private readonly Action<Canvas> onDrawAction;

        public RecyclerItemDecoration(Action<Canvas> onDrawAction)
        {
            this.onDrawAction = onDrawAction;
        }

        public override void OnDraw(Canvas c, RecyclerView parent, State state)
        {
            onDrawAction?.Invoke(c);
        }
    }
}