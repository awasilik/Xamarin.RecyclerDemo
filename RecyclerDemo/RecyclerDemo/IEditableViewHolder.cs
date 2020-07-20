namespace RecyclerDemo
{
    public interface IEditableViewHolder
    {
        void OnDragged();

        void OnSwiped();

        void OnCleared();
    }
}