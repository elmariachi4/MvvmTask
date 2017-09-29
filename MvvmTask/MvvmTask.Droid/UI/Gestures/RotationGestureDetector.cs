using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Customized.Layout;

namespace MvvmTask.Droid.UI.Gestures
{
    public class RotationGestureDetector : TwoFingerGestureDetector
        {
            public interface IOnRotationGestureListener
            {
                bool OnRotate(RotationGestureDetector detector);
                bool OnRotateBegin(RotationGestureDetector detector);
                void OnRotateEnd(RotationGestureDetector detector);
            }

            public class SimpleOnRotationGestureListener : IOnRotationGestureListener
            {
                private double _deltaDegrees;
                public virtual bool OnRotate(RotationGestureDetector detector)
                {
                    _deltaDegrees = detector.RotationDegreesDelta;
                   return false;
                }
                public virtual bool OnRotateBegin(RotationGestureDetector detector)
                {
                    return true;
                }

                public virtual void OnRotateEnd(RotationGestureDetector detector)
                {
                    // do nothing, overridden implementation may be used.
                }
            }

            private readonly IOnRotationGestureListener _listener;
            private bool _sloppyGesture;

            public RotationGestureDetector(Context context, IOnRotationGestureListener listener) : base(context)
            {
                _listener = listener;
            }
            protected override void HandleInProgressEvent(MotionEventActions actionCode, MotionEvent e)
            {
                switch (actionCode)
                {
                    case MotionEventActions.PointerDown:
                        // At least the second finger is on the screen now.
                        ResetState();
                        _previousEvent = MotionEvent.Obtain(e);
                        _timeDelta = 0;

                        UpdateStateByEvent(e);

                        // See if we have a sloppy gesture.
                        _sloppyGesture = IsSloppyGesture(e);
                        if (!_sloppyGesture)
                        {
                            // No, start gesture now.
                            _gestureInProgress = _listener.OnRotateBegin(this);
                        }
                        break;
                    case MotionEventActions.Move:
                        if (!_sloppyGesture)
                        {
                            break;
                        }

                        // See if we still have a sloppy gesture
                        _sloppyGesture = IsSloppyGesture(e);
                        if (!_sloppyGesture)
                        {
                            _gestureInProgress = _listener.OnRotateBegin(this);
                        }

                        break;
                    case MotionEventActions.PointerUp:
                        if (!_sloppyGesture)
                        {
                            break;
                        }

                        break;
                }
            }

            protected override void HandleStartProgressEvent(MotionEventActions actionCode, MotionEvent e)
            {
                switch (actionCode)
                {
                    case MotionEventActions.PointerUp:
                        // Gesture ended but
                        UpdateStateByEvent(e);

                        if (!_sloppyGesture)
                        {
                            _listener.OnRotateEnd(this);
                        }

                        ResetState();
                        break;
                    case MotionEventActions.Cancel:
                        if (!_sloppyGesture)
                        {
                            _listener.OnRotateEnd(this);
                        }
                        ResetState();
                        break;
                    case MotionEventActions.Move:
                        UpdateStateByEvent(e);

                        // Only accept the evt if our relative pressure is within
                        // a certain limit. This can help filter shaky data as a
                        // finger is lifted.
                        if (_currentPressure / _previousPressure > PressureThreshold)
                        {
                            bool updatePrevious = _listener.OnRotate(this);
                            if (updatePrevious)
                            {
                                if (_previousEvent != null)
                                {
                                    _previousEvent.Recycle();
                                }
                                _previousEvent = MotionEvent.Obtain(e);
                            }
                        }
                    break;
                }
            }

            protected override void ResetState()
            {
                base.ResetState();
                _sloppyGesture = false;
            }

            public float RotationDegreesDelta
            {
                get
                {
                    double diffRadians = System.Math.Atan2(_prevFingerDiffY, _prevFingerDiffX) - System.Math.Atan2(_currFingerDiffY, _currFingerDiffX);
                    return (float)(diffRadians * 180.0 / System.Math.PI);
                }
            }
        }
    

    //---------------------
    public class RotateGestureDetector
    {
        private static int INVALID_POINTER_ID = -1;
        private PointF mFPoint = new PointF();
        private PointF mSPoint = new PointF();
        private int ptrID1, ptrID2;
        private float mAngle;
        private View view;
        private Context context;

        private OnRotateGestureListener mListener;

        public float GetAngle()
        {
            return mAngle;
        }

        public RotateGestureDetector(View v, OnRotateGestureListener listener)
        {
            view = v;
            mListener = listener;
            ptrID1 = INVALID_POINTER_ID;
            ptrID2 = INVALID_POINTER_ID;
        }

        public bool OnTouchEvent(MotionEvent evt)
        {
            switch (evt.ActionMasked)
            {
                case MotionEventActions.Outside:
                    break;
                case MotionEventActions.Down:
                    ptrID1 = evt.GetPointerId(evt.ActionIndex);
                    break;
                case MotionEventActions.PointerDown:
                    ptrID2 = evt.GetPointerId(evt.ActionIndex);
                    getRawPoint(evt, ptrID1, mSPoint);
                    getRawPoint(evt, ptrID2, mFPoint);
                    break;
                case MotionEventActions.Move:
                    if(ptrID1 != INVALID_POINTER_ID && ptrID2 != INVALID_POINTER_ID)
                    {
                        PointF nfPoint = new PointF();
                        PointF nsPoint = new PointF();
                        getRawPoint(evt, ptrID1, nsPoint);
                        getRawPoint(evt, ptrID2, nfPoint);
                        mAngle = angleBetweenLines(mFPoint, mSPoint, nfPoint, nsPoint);
                        if (mListener != null) 
                            mListener.OnRotate(this,(TouchImageView)view);
                    }
                    break;
                case MotionEventActions.Up:
                    ptrID1 = INVALID_POINTER_ID;
                    break;
                case MotionEventActions.PointerUp:
                    ptrID2 = INVALID_POINTER_ID;
                    break;
                case MotionEventActions.Cancel:
                    ptrID1 = INVALID_POINTER_ID;
                    ptrID2 = INVALID_POINTER_ID;
                    break;
            }
            return true;
        }

        void getRawPoint(MotionEvent ev, int index, PointF point)
        {
            int[] location = { 0, 0 };
            view.GetLocationOnScreen(location);

            float x = ev.GetX(index);
            float y = ev.GetY(index);

            double angle = Java.Lang.Math.ToDegrees(Math.Atan2(y, x));
            angle += view.Rotation;

             float length = PointF.Length(x, y);

            x = (float)(length * Math.Cos(Java.Lang.Math.ToRadians(angle))) + location[0];
            y = (float)(length * Math.Sin(Java.Lang.Math.ToRadians(angle))) + location[1];

            point.Set(x, y);
        }


        private float angleBetweenLines(PointF fPoint, PointF sPoint, PointF nFpoint, PointF nSpoint)
        {
            float angle1 = (float)Math.Atan2((fPoint.Y - sPoint.Y), (fPoint.X - sPoint.X));
            float angle2 = (float)Math.Atan2((nFpoint.Y - nSpoint.Y), (nFpoint.X - nSpoint.X));

            float angle = ((float)Java.Lang.Math.ToDegrees(angle1 - angle2)) % 360;
            if (angle < -180f) angle += 360.0f;
            if (angle > 180f) angle -= 360.0f;
            return -angle;
        }

  
        public interface OnRotateGestureListener
        {
            void OnRotate(RotateGestureDetector rotationDetector, TouchImageView v);
        }
        public class SimpleOnRotateGestureListener : OnRotateGestureListener
        {
            TouchImageView view;
            public SimpleOnRotateGestureListener(TouchImageView v)
            {
                view = v;
            }
            public virtual void OnRotate(RotateGestureDetector detector, TouchImageView v)
            {
                Matrix m = new Matrix();
                m.PostRotate(detector.GetAngle());
                view.ImageMatrix = m;
                
            }
        }
        //public class 
    }
}