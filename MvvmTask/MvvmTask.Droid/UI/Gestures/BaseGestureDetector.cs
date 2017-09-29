
using Android.Content;
using Android.Views;

namespace MvvmTask.Droid.UI.Gestures
{
    public abstract class BaseGestureDetector
    {
        protected Context _context;
        protected bool _gestureInProgress;

        protected MotionEvent _previousEvent;
        protected MotionEvent _currentEvent;

        protected float _currentPressure;
        protected float _previousPressure;
        protected long _timeDelta;

        protected const float PressureThreshold = 0.67f;
        public BaseGestureDetector(Context context)
        {
            _context = context;
        }
        public bool OnTouchEvent(MotionEvent e)
        {
            MotionEventActions actionCode = e.Action & MotionEventActions.Mask;
            if (!_gestureInProgress)
            {
                HandleStartProgressEvent(actionCode, e);
            }
            else
            {
                HandleInProgressEvent(actionCode, e);
            }
            return true;
        }

        /// <summary>
        /// Called when the current event occurred when NO gesture is in progress
        /// yet. The handling in this implementation may set the gesture in progress
        /// or out of progress.
        /// </summary>
        /// <param name="actionCode">Action code.</param>
        /// <param name="e">The event.</param>
        protected abstract void HandleStartProgressEvent(MotionEventActions actionCode, MotionEvent e);

        /// <summary>
        /// Called when the current event occurred when a gesture IS in progress. The
        /// handling in this implementation may set the gesture out of progress.
        /// </summary>
        /// <param name="actionCode">Action code.</param>
        /// <param name="e">The event.</param></param>
        protected abstract void HandleInProgressEvent(MotionEventActions actionCode, MotionEvent e);

        /// <summary>
        /// Updates the current state with the given event.
        /// </summary>
        /// <param name="curr">The current event.</param>
        protected virtual void UpdateStateByEvent(MotionEvent curr)
        {
            MotionEvent prev = _previousEvent;

            // Reset _currentEvent.
            if (_currentEvent != null)
            {
                _currentEvent.Recycle();
                _currentEvent = null;
            }
            _currentEvent = MotionEvent.Obtain(curr);

            // Delta time.
            _timeDelta = 0;
            if (prev != null)
            {
                _timeDelta = curr.EventTime - prev.EventTime;

                // Pressure.
                _previousPressure = prev.GetPressure(prev.ActionIndex);
            }
            _currentPressure = curr.GetPressure(curr.ActionIndex);
        }

        /// <summary>
        /// Resets the state.
        /// </summary>
        protected virtual void ResetState()
        {
            if (_previousEvent != null)
            {
                _previousEvent.Recycle();
                _previousEvent = null;
            }
            if (_currentEvent != null)
            {
                _currentEvent.Recycle();
                _currentEvent = null;
            }
            _gestureInProgress = false;
        }

        /// <summary>
        /// Returns true if a gesture is in progress.
        /// </summary>
        /// <returns><c>true</c> if this instance is in progress; otherwise, <c>false</c>.</returns>
        public bool IsInProgress() => _gestureInProgress;

        /// <summary>
        /// Returns the time difference in milliseconds between the previous accepted
        /// GestureDetector event and the curent GestureDetector event.
        /// </summary>
        /// <returns>The time difference between the last move event in milliseconds.</returns>
        public long GetTimeTable() => _timeDelta;

        /// <summary>
        /// Returns the event time of the current GestureDetecture event being processed.
        /// </summary>
        /// <returns>Current GestureDetector event time in milliseconds.</returns>
        public long GetEventTime() => _currentEvent.EventTime;
    }
}