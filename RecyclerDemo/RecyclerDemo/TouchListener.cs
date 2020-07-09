using System;
using Android.Views;

namespace RecyclerDemo
{
    class TouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        private readonly Func<MotionEvent, bool> motionAction;

        public TouchListener(Func<MotionEvent, bool> motionAction)
        {
            this.motionAction = motionAction;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            return motionAction.Invoke(e);
        }
    }
}