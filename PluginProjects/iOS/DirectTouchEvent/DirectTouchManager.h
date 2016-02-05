#include <stdio.h>
#include <stdlib.h>

/***************************
 * Structure for Shared C#
 ***************************/
typedef struct TouchInfo {
    int touchID;
    double timestamp;
    int phase;
    float posX;
    float posY;
    float radius;
    float pressure;
} TouchInfo;


/********************
 * C# -> Native C++
 ********************/
extern "C" {
    typedef void (*TouchEventCallback)(TouchInfo *touchInfo, int nVal);

    int InitializationManager();
    int RegisterTouchEventCallback(TouchEventCallback callback);
    void EnableDefaultTouch();
    void DisableDefaultTouch();
    void EnableNativePlugin();
    void DisableNativePlugin();
}


/********************
 * ObjC -> Native C++
 ********************/
class DirectTouchManager {
public:
    static void OnTouchEvent(int *touchIDs, double *timestamps, int *phases, float *possY, float *possX, float *radiuses, float *forces, int nVal);
    static bool IsDefaultTouch();
};
