using System;
using Android.Graphics;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace RecyclerDemo.Final
{
    public class MotionController : ItemTouchHelper.Callback
    {
        private readonly IEditableAdapter adapter;

        public MotionController(IEditableAdapter adapter)
        {
            this.adapter = adapter;
        }

        public override int GetMovementFlags(RecyclerView recycler, ViewHolder viewHolder)
        {
            var dragFlags = ItemTouchHelper.Up | ItemTouchHelper.Down;
            var swipeFlags = ItemTouchHelper.Start | ItemTouchHelper.End;

            return MakeMovementFlags(dragFlags, swipeFlags);
        }

        /**************** VERTICAL REORDER MOVE *******************/
        #region VERTICAL REORDER MOVE

        public override bool OnMove(RecyclerView recycler, ViewHolder viewHolder, ViewHolder target)
        {
            adapter.MoveItem(viewHolder.AdapterPosition, target.AdapterPosition);

            return true;
        }

        public override void OnSelectedChanged(ViewHolder viewHolder, int actionState)
        {
            base.OnSelectedChanged(viewHolder, actionState);

            if (actionState == ItemTouchHelper.ActionStateDrag && viewHolder is IEditableViewHolder editorViewHolder)
            {
                editorViewHolder.OnDragged();
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

        #endregion

        /**************** HORIZONTAL MODIFY MOVE *******************/
        #region HORIZONTAL SWIPE

        private enum ButtonsState
        {
            Gone,
            LeftVisible,
            RightVisible
        }

        private bool swipeBack;
        private bool shouldDrawButtons;

        private ButtonsState buttonsState = ButtonsState.Gone;
        private static float buttonSpace = 200f;

        private RectF buttonInstance = null;
        private ViewHolder currentViewHolder = null;

        public override void OnSwiped(ViewHolder viewHolder, int direction)
        {
        }

        public override int ConvertToAbsoluteDirection(int flags, int layoutDirection)
        {
            if (swipeBack)
            {
                swipeBack = buttonsState != ButtonsState.Gone;
                return 0;
            }

            return base.ConvertToAbsoluteDirection(flags, layoutDirection);
        }

        public override void OnChildDraw(Canvas canvas, RecyclerView recycler, ViewHolder viewHolder, float dX, float dY, int actionState,
            bool isCurrentlyActive)
        {
            if (actionState == ItemTouchHelper.ActionStateDrag)
            {
                shouldDrawButtons = false;
            }

            if (actionState == ItemTouchHelper.ActionStateSwipe)
            {
                shouldDrawButtons = true;

                if (buttonsState != ButtonsState.Gone)
                {
                    dX = buttonsState == ButtonsState.LeftVisible
                        ? Math.Max(dX, buttonSpace)
                        : Math.Min(dX, -buttonSpace);

                    base.OnChildDraw(canvas, recycler, viewHolder, dX, dY, actionState, isCurrentlyActive);
                }
                else
                {
                    SetTouchCancelListener(canvas, recycler, viewHolder, dX, dY, actionState, isCurrentlyActive);
                }
            }

            if (buttonsState == ButtonsState.Gone)
            {
                base.OnChildDraw(canvas, recycler, viewHolder, dX, dY, actionState, isCurrentlyActive);
            }

            currentViewHolder = viewHolder;
        }

        private void SetTouchCancelListener(Canvas canvas, RecyclerView recycler, ViewHolder viewHolder, float dX, float dY, int actionState, bool isCurrentlyActive)
        {
            recycler.SetOnTouchListener(new TouchListener(OnTouch));

            bool OnTouch(MotionEvent motionEvent)
            {
                swipeBack = motionEvent.Action == MotionEventActions.Up || motionEvent.Action == MotionEventActions.Cancel;

                if (swipeBack)
                {
                    if (dX < -buttonSpace) buttonsState = ButtonsState.RightVisible;
                    else if (dX > buttonSpace) buttonsState = ButtonsState.LeftVisible;

                    if (buttonsState != ButtonsState.Gone)
                    {
                        SetTouchDownListener(canvas, recycler, viewHolder, dX, dY, actionState, isCurrentlyActive);
                        SetItemsClickable(recycler, false);
                    }
                }

                return false;
            }
        }

        private void SetTouchDownListener(Canvas canvas, RecyclerView recycler, ViewHolder viewHolder, float dX, float dY, int actionState,
            bool isCurrentlyActive)
        {
            recycler.SetOnTouchListener(new TouchListener(OnTouch));

            bool OnTouch(MotionEvent me)
            {
                if (me.Action == MotionEventActions.Down)
                {
                    SetTouchUpListener(canvas, recycler, viewHolder, dX, dY, actionState, isCurrentlyActive);
                }

                return false;
            }
        }

        private void SetTouchUpListener(Canvas canvas, RecyclerView recycler, ViewHolder viewHolder, float dX, float dY, int actionState,
            bool isCurrentlyActive)
        {
            recycler.SetOnTouchListener(new TouchListener(OnTouch));

            bool OnTouch(MotionEvent motionEvent)
            {
                if (motionEvent.Action == MotionEventActions.Up)
                {
                    // clear view swipe state "0f"
                    base.OnChildDraw(canvas, recycler, viewHolder, 0f, dY, actionState, isCurrentlyActive);
                    // clear item touch listener
                    recycler.SetOnTouchListener(new TouchListener(null));
                    SetItemsClickable(recycler, true);
                    swipeBack = false;

                    AttachActions(viewHolder, motionEvent);

                    buttonsState = ButtonsState.Gone;
                    currentViewHolder = null;
                }

                return false;
            }
        }

        private static void SetItemsClickable(RecyclerView recycler, bool clickable)
        {
            for (var i = 0; i < recycler.ChildCount; ++i)
            {
                recycler.GetChildAt(i).Clickable = clickable;
            }
        }

        #endregion

        /**************** DRAWING THE BUTTONS *******************/
        #region DRAWING BUTTONS

        public void OnDraw(Canvas canvas)
        {
            if (currentViewHolder != null && shouldDrawButtons)
            {
                DrawButtons(canvas, currentViewHolder);
            }
        }

        private void DrawButtons(Canvas canvas, ViewHolder viewHolder)
        {
            var buttonWidth = buttonSpace - 20;
            var corners = 16f;

            var view = viewHolder.ItemView;
            var paint = new Paint();

            var leftButton = new RectF(view.Left, view.Top + 20, view.Left + buttonWidth, view.Bottom - 20);
            paint.Color = Color.LightSeaGreen;
            canvas.DrawRoundRect(leftButton, corners, corners, paint);
            DrawText("edit", canvas, leftButton, paint);

            var rightButton = new RectF(view.Right - buttonWidth, view.Top + 20, view.Right, view.Bottom - 20);
            paint.Color = Color.IndianRed;
            canvas.DrawRoundRect(rightButton, corners, corners, paint);
            DrawText("delete", canvas, rightButton, paint);

            buttonInstance = null;
            if (buttonsState == ButtonsState.LeftVisible) buttonInstance = leftButton;
            else if (buttonsState == ButtonsState.RightVisible) buttonInstance = rightButton;

            static void DrawText(string text, Canvas c, RectF button, Paint p)
            {
                p.Color = Color.White;
                p.AntiAlias = true;
                p.TextSize = 40;
                p.SetTypeface(Typeface.Create("ubuntu", TypefaceStyle.Normal));

                var textWidth = p.MeasureText(text);
                c.DrawText(text, button.CenterX() - textWidth / 2, button.CenterY() + 20, p);
            }
        }

        private void AttachActions(ViewHolder viewHolder, MotionEvent motionEvent)
        {
            if (buttonInstance != null && buttonInstance.Contains(motionEvent.GetX(), motionEvent.GetY()))
            {
                if (buttonsState == ButtonsState.LeftVisible) adapter.EditItem(viewHolder.AdapterPosition);
                else if (buttonsState == ButtonsState.RightVisible) adapter.RemoveItem(viewHolder.AdapterPosition);
            }
        }

        #endregion
    }
}