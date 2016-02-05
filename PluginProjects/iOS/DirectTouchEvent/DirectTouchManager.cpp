#include "DirectTouchManager.h"
#include "NativeBridge.h"


bool isDefaultTouch;
TouchEventCallback callback;


/********************
 * C# -> Native C++
 ********************/
extern "C" {
    int InitializationManager() {
        NativeBridge::Initialization();
        isDefaultTouch = true;
        
        return 0;
    }
    
    int RegisterTouchEventCallback(TouchEventCallback cb) {
        callback = cb;
        
        return 0;
    }
    
    void EnableDefaultTouch() {
        isDefaultTouch = true;
    }

    void DisableDefaultTouch() {
        isDefaultTouch = false;
    }
    
    void EnableNativePlugin() {
        NativeBridge::EnableTouchView();
    }
    
    void DisableNativePlugin() {
        NativeBridge::DisableTouchView();
    }
}


/********************
 * ObjC -> Native C++
 ********************/
void DirectTouchManager::OnTouchEvent(int *touchIDs, double *timestamps, int *phases, float *possY, float *possX, float *radiuses, float *forces, int nVal) {
    TouchInfo *info = (TouchInfo *)malloc(sizeof(TouchInfo) * nVal);
    
    for (int i=0; i<nVal; i++) {
        info[i].touchID = touchIDs[i];
        info[i].timestamp = timestamps[i];
        info[i].phase = phases[i];
        info[i].posX = possX[i];
        info[i].posY = possY[i];
        info[i].radius = radiuses[i];
        info[i].pressure = forces[i];
    }
    callback(info, nVal);
    free(info);
}

bool DirectTouchManager::IsDefaultTouch() {
    return isDefaultTouch;
}
