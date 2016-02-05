#ifndef NativeBridge_iOS_h
#define NativeBridge_iOS_h

/**********************
 * Native C++ -> ObjC
 **********************/
class NativeBridge {
public:
    static void Initialization();
    static void EnableTouchView();
    static void DisableTouchView();
};

#endif /* NativeBridge_iOS_h */