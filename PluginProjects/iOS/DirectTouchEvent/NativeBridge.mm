#import <UIKit/UIKit.h>
#import "NativeBridge.h"
#import "DirectTouchEventView.h"


/**********************
 * Native C++ -> ObjC
 **********************/
void NativeBridge::Initialization()
{
    EnableTouchView();
}

void NativeBridge::EnableTouchView()
{
    UIViewController *rootViewController = (UIViewController*)[[[UIApplication sharedApplication] keyWindow] rootViewController];
    UIScreen* screen = [UIScreen mainScreen];
    
    // Create View of Caching TouchEvent
    if ([rootViewController.view viewWithTag: 26884] == nil){
        DirectTouchEventView *directTouchView = [[DirectTouchEventView alloc] initWithFrame:CGRectMake(0,0,screen.bounds.size.width,screen.bounds.size.height)];
        directTouchView.tag = 26884;
        directTouchView.backgroundColor = [UIColor colorWithWhite:0.0 alpha:0.0];
        [directTouchView setMultipleTouchEnabled:YES];
        [directTouchView setContentScaleFactor:screen.scale];
        
        [rootViewController.view addSubview:directTouchView];
        [rootViewController.view bringSubviewToFront:directTouchView];
    }
}


void NativeBridge::DisableTouchView()
{
    // Destroy View of Caching TouchEvent
    UIViewController *rootViewController = (UIViewController*)[[[UIApplication sharedApplication] keyWindow] rootViewController];
    [[rootViewController.view viewWithTag:26884] removeFromSuperview];
}
