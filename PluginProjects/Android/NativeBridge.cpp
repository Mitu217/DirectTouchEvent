#include "net_reinford_DirectTouch.h"
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


/***************************
 * innerparams
 ***************************/
extern "C" { 
  typedef void (*TouchEventCallback)(TouchInfo *touchInfo, int nVal);

  bool _isDefaultTouch;
  TouchEventCallback _callback;

  /**************
   * Initialize
   **************/
  JavaVM*   java_vm;
  jobject   JavaClass;
  jmethodID initialization, enableView, disableView;

  jint JNI_OnLoad(JavaVM* vm, void* reserved) {
    java_vm = vm;

    // attach our thread to the java vm; obviously it's already attached but this way we get the JNIEnv..
    JNIEnv* jni_env = 0;
    java_vm->AttachCurrentThread(&jni_env, 0);
  
    // first we try to find our main activity..
    jclass cls_Activity   = jni_env->FindClass("com/unity3d/player/UnityPlayer");
    jfieldID fid_Activity = jni_env->GetStaticFieldID(cls_Activity, "currentActivity", "Landroid/app/Activity;");
    jobject obj_Activity  = jni_env->GetStaticObjectField(cls_Activity, fid_Activity);
  
    // create a JavaClass object...
    jclass cls_JavaClass  = jni_env->FindClass("net/reinford/DirectTouch");
    jmethodID mid_JavaClass = jni_env->GetMethodID(cls_JavaClass, "<init>", "(Landroid/app/Activity;)V");
    jobject obj_JavaClass = jni_env->NewObject(cls_JavaClass, mid_JavaClass, obj_Activity);
  
    // create a global reference to the JavaClass object and fetch method id(s)..
    JavaClass     = jni_env->NewGlobalRef(obj_JavaClass);
    initialization = jni_env->GetMethodID(cls_JavaClass, "Initialization", "()V");
    enableView = jni_env->GetMethodID(cls_JavaClass, "EnableTouchView", "()V");
    disableView = jni_env->GetMethodID(cls_JavaClass, "DisableTouchView", "()V");

    return JNI_VERSION_1_6;   // minimum JNI version
  }


  /********************
   * C# -> Native C++
   ********************/
  int InitializationManager() {
    JNIEnv* jni_env = 0;
    java_vm->AttachCurrentThread(&jni_env, 0);
    jni_env->CallObjectMethod(JavaClass, initialization);

    _isDefaultTouch = true;
    return 0;
  }

  int RegisterTouchEventCallback(TouchEventCallback callback) {
    _callback = callback;
    return 0;
  }

  void EnableDefaultTouch() {
    _isDefaultTouch = true;
  }

  void DisableDefaultTouch() { 
    _isDefaultTouch = false;
  }

  void EnableNativePlugin() {
    JNIEnv* jni_env = 0;
    java_vm->AttachCurrentThread(&jni_env, 0);
    jni_env->CallObjectMethod(JavaClass, enableView);
  }

  void DisableNativePlugin() {
    JNIEnv* jni_env = 0;
    java_vm->AttachCurrentThread(&jni_env, 0);
    jni_env->CallObjectMethod(JavaClass, disableView);
  }

  /********************
   * Java -> Native C++
   ********************/
  JNIEXPORT jboolean JNICALL Java_net_reinford_DirectTouch_isDefaultTouch (JNIEnv *env, jobject obj) {
    return _isDefaultTouch;
  }

  JNIEXPORT void JNICALL Java_net_reinford_DirectTouch_onTouchEvent (JNIEnv *env, jobject obj, jintArray touchIDs, jdoubleArray timestamps, jintArray phases, jfloatArray possY, jfloatArray possX, jfloatArray radiuses, jfloatArray forces) {
    int length = (int)(env->GetArrayLength(touchIDs));

    jint *touchID = env->GetIntArrayElements(touchIDs, NULL);
    jdouble *timestamp = env->GetDoubleArrayElements(timestamps, NULL);
    jint *phase = env->GetIntArrayElements(phases, NULL);
    jfloat *posY = env->GetFloatArrayElements(possY, NULL);
    jfloat *posX = env->GetFloatArrayElements(possX, NULL);
    jfloat *radius = env->GetFloatArrayElements(radiuses, NULL);
    jfloat *force = env->GetFloatArrayElements(forces, NULL);

    TouchInfo *info = (TouchInfo *)malloc(sizeof(TouchInfo) * length);
    for(int i=0; i<length; i++) {

      info[i].touchID = *touchID++;
      info[i].timestamp = *timestamp++;
      info[i].phase = *phase++;
      info[i].posY = *posY++;
      info[i].posX = *posX++;
      info[i].radius = *radius++;
      info[i].pressure = *force++;
    }
    
    _callback(info, length);
    
    free(info);
  }

} // extern "C"



