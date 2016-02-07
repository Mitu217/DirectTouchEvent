#!/bin/sh
echo ""
echo "Building DirectTouch..."
javac DirectTouch.java -bootclasspath ~/Library/Android/sdk/platforms/android-21/android.jar -d .
javah -classpath ~/Library/Android/sdk/platforms/android-21/android.jar: -d jni net.reinford.DirectTouch
mv jni/net_reinford_DirectTouch.h ./

echo ""
echo "Signature dump of DirectTouch..."

javap -s net.reinford.DirectTouch

echo "Creating DirectTouch.jar..."
jar cvfM ../../Assets/MobileTouchPlugin/Plugins/Android/DirectTouch.jar net

echo ""
echo "Compiling NativeBridge.cpp..."
~/Library/Android/ndk-r10e/ndk-build NDK_PROJECT_PATH=. NDK_APPLICATION_MK=Application.mk $*
mv libs/armeabi/libDirectTouchEvent.so ../../Assets/MobileTouchPlugin/Plugins/Android/

echo ""
echo "Cleaning up / removing build folders..."

rm -rf libs
rm -rf jni
rm -rf obj
rm -rf net

echo ""
echo "Done!"
