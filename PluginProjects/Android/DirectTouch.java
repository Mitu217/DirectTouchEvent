package net.reinford;

import android.app.Activity;
import android.graphics.Color;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.widget.FrameLayout;
import android.view.View.OnTouchListener;

import android.util.Log;

public class DirectTouch
{
  /*******************
   * inner params
   *******************/  
  private final int TAG_NUMBER = 26884;
  private Activity mActivity;
  
  /*******************
   * Loading Library 
   *******************/  
  static {
    System.loadLibrary("DirectTouchEvent");
  }

  /**********************
   * java -> c++ methods 
   **********************/ 
  public native boolean isDefaultTouch();
  public native void onTouchEvent(int[] touchID, double[] timestamp, 
    int[] phase, float[] posY, float[] posX, float[] radius, float[] pressure);


  /**********************
   * c++ -> java methods 
   **********************/ 
  public DirectTouch(Activity currentActivity) {
    mActivity = currentActivity;
  }
  
  public void Initialization() { 
    mActivity.runOnUiThread(new Runnable() {
      @Override
      public void run() {
        if (mActivity.findViewById(TAG_NUMBER) == null) {
          FrameLayout frameLayout = new FrameLayout(mActivity); 

          View view = new View(mActivity);
          view.setId(TAG_NUMBER);
          view.setBackgroundColor(Color.TRANSPARENT);
          view.setOnTouchListener( new DirectTouchEventListener() ); 
          
          mActivity.addContentView(frameLayout, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT));
          frameLayout.addView(view, new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT));
        }
      }
    });
  }

  public void EnableTouchView() {
    mActivity.findViewById(TAG_NUMBER).setVisibility(View.VISIBLE);
  }

  public void DisableTouchView() {
    mActivity.findViewById(TAG_NUMBER).setVisibility(View.INVISIBLE);
  }

  /********************
   * TouchEventLister
   ********************/ 
  private class DirectTouchEventListener implements OnTouchListener {
    int     i, h, action, id, length, hlength;
    double  eventTime, hEventTime;
    int[]   touchID, phase;
    float[] posY, posX, radius, pressure;
    double[] timestamp;

    @Override
    public boolean onTouch(View view, MotionEvent event) {
      // Get MotionEvent Status
      action = event.getAction();
      length = (event.getActionMasked() == MotionEvent.ACTION_MOVE || event.getActionMasked() == MotionEvent.ACTION_CANCEL) ? event.getPointerCount() : 1;
      hlength = event.getHistorySize();
      id = (action & MotionEvent.ACTION_POINTER_ID_MASK) >> MotionEvent.ACTION_POINTER_ID_SHIFT;

      // Initialize SendParams
      touchID = new int[length];
      timestamp = new double[length];
      phase = new int[length];
      posY = new float[length];
      posX = new float[length];
      radius = new float[length];
      pressure = new float[length];

      // Check Historical MotionEvent
      GetHistorialMotionTouchParams(event);

      // Check Current MotionEvent
      GetCurrentMotionTouchParams(event);

      // Fire Other View's TouchEvent and RootActivity's TouchEvent.
      ViewGroup parent = (ViewGroup)view.getParent();
      for(i=0; i<parent.getChildCount(); i++) {
        if(parent.getChildAt(i) == view) {
          continue;
        }else if(parent.getChildAt(i).dispatchTouchEvent(event)) {
          return true;
        }
      }

      if (isDefaultTouch()) {
        mActivity.onTouchEvent(event);
      }
      return true;
    }


    private void GetHistorialMotionTouchParams(MotionEvent event) {
      for(h=0; h<hlength; h++) {
        hEventTime = event.getHistoricalEventTime(h);
        switch(event.getActionMasked()) {
          case MotionEvent.ACTION_DOWN:
          case MotionEvent.ACTION_POINTER_DOWN:
            touchID[0] = event.getPointerId(id);
            timestamp[0] = hEventTime;
            posY[0] = event.getHistoricalY(0, h);
            posX[0] = event.getHistoricalX(0, h);
            radius[0] = event.getHistoricalSize(0, h);
            pressure[0] = event.getHistoricalPressure(0, h);
            phase[0] = 0;
            break;
              
          case MotionEvent.ACTION_MOVE:
            for(i=0; i<length; i++) {
              touchID[i] = event.getPointerId(i);
              timestamp[i] = hEventTime;
              posY[i] = event.getHistoricalY(i, h);
              posX[i] = event.getHistoricalX(i, h);
              radius[i] = event.getHistoricalSize(i, h);
              pressure[i] = event.getHistoricalPressure(i, h);
              phase[i] = 1;
            }
            break;

          case MotionEvent.ACTION_CANCEL:
            for(i=0; i<length; i++) {
              touchID[0] = event.getPointerId(i);
              timestamp[0] = hEventTime;
              posY[0] = event.getHistoricalY(i, h);
              posX[0] = event.getHistoricalX(i, h);
              radius[0] = event.getHistoricalSize(i, h);
              pressure[0] = event.getHistoricalPressure(i, h);
              phase[0] = 2;
            }
            break;

          case MotionEvent.ACTION_UP:
          case MotionEvent.ACTION_POINTER_UP:
            touchID[0] = event.getPointerId(id);
            timestamp[0] = hEventTime;
            posY[0] = event.getHistoricalY(0, h);
            posX[0] = event.getHistoricalX(0, h);
            radius[0] = event.getHistoricalSize(0, h);
            pressure[0] = event.getHistoricalPressure(0, h);
            phase[0] = 3;
            break;
        }
        
        // Send Historical TouchEvent to Unity.
        onTouchEvent(touchID, timestamp, phase, posY, posX, radius, pressure);
      }
    }


    private void GetCurrentMotionTouchParams(MotionEvent event) {
      eventTime = event.getEventTime();
      switch(event.getActionMasked()) {
        case MotionEvent.ACTION_DOWN:
        case MotionEvent.ACTION_POINTER_DOWN:
          touchID[0] = event.getPointerId(id);
          timestamp[0] = eventTime;
          posY[0] = event.getY(id);
          posX[0] = event.getX(id);
          radius[0] = event.getSize(id);
          pressure[0] = event.getPressure(id);
          phase[0] = 0;
          break;
            
        case MotionEvent.ACTION_MOVE:
          for(i=0; i<length; i++) {
            touchID[i] = event.getPointerId(i);
            timestamp[i] = eventTime;
            posY[i] = event.getY(i);
            posX[i] = event.getX(i);
            radius[i] = event.getSize(i);
            pressure[i] = event.getPressure(i);
            phase[i] = 1;
          }
          break;

        case MotionEvent.ACTION_CANCEL:
          for(i=0; i<length; i++) {
            touchID[i] = event.getPointerId(i);
            timestamp[i] = eventTime;
            posY[i] = event.getY(i);
            posX[i] = event.getX(i);
            radius[i] = event.getSize(i);
            pressure[i] = event.getPressure(i);
            phase[i] = 2;
          }
          break;

        case MotionEvent.ACTION_UP:
        case MotionEvent.ACTION_POINTER_UP:
          touchID[0] = event.getPointerId(id);
          timestamp[0] = eventTime;
          posY[0] = event.getY(id);
          posX[0] = event.getX(id);
          radius[0] = event.getSize(id);
          pressure[0] = event.getPressure(id);
          phase[0] = 3;
          break;
      }
      
      // Send TouchEvent to Unity.
      onTouchEvent(touchID, timestamp, phase, posY, posX, radius, pressure);
    }
  }
}

