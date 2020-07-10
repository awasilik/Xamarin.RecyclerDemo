namespace RecyclerDemo
{
    public interface IEditableAdapter
    {
        void MoveItem(int fromPosition, int toPosition);

        void RemoveItem(int position);

        void EditItem(int position);
    }
}