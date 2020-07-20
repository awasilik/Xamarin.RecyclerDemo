using AndroidX.RecyclerView.Widget;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace RecyclerDemo.Editable
{
    public class SimpleMotionController : ItemTouchHelper.Callback
    {
        private readonly IEditableAdapter adapter;

        public SimpleMotionController(IEditableAdapter adapter)
        {
            this.adapter = adapter;
        }

        public override int GetMovementFlags(RecyclerView recycler, ViewHolder viewHolder)
        {
            var dragFlags = ItemTouchHelper.Up | ItemTouchHelper.Down;
            var swipeFlags = ItemTouchHelper.Start | ItemTouchHelper.End;

            return MakeMovementFlags(dragFlags, swipeFlags);
        }

        public override bool OnMove(RecyclerView recycler, ViewHolder viewHolder, ViewHolder target)
        {
            adapter.MoveItem(viewHolder.AdapterPosition, target.AdapterPosition);

            return true;
        }
        public override void OnSwiped(ViewHolder viewHolder, int direction)
        {
            adapter.RemoveItem(viewHolder.AdapterPosition);
        }

        public override void OnSelectedChanged(ViewHolder viewHolder, int actionState)
        {
            base.OnSelectedChanged(viewHolder, actionState);

            if (viewHolder is IEditableViewHolder editorViewHolder)
            {
                if (actionState == ItemTouchHelper.ActionStateDrag)
                {
                    editorViewHolder.OnDragged();
                }
                else if (actionState == ItemTouchHelper.ActionStateSwipe)
                {
                    editorViewHolder.OnSwiped();
                }
            }
        }

        public override void ClearView(RecyclerView recycler, ViewHolder viewHolder)
        {
            base.ClearView(recycler, viewHolder);

            if (viewHolder is IEditableViewHolder editorViewHolder)
            {
                editorViewHolder.OnCleared();
            }
        }
    }
}