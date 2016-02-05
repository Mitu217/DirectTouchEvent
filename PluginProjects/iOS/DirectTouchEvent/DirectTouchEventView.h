#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "DirectTouchManager.h"

@interface DirectTouchEventView : UIView {
    UIView *rootView;
    NSMutableArray *otherViews;
    TouchEventCallback callback;
}

/****************
 * ObjC Methods
 ****************/

-  (id)initWithFrame:(CGRect)aRect;
- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event;
- (void)touchesEnded:(NSSet *)touches withEvent:(UIEvent *)event;
- (void)touchesCancelled:(NSSet*)touches withEvent:(UIEvent*)event;
- (void)touchesMoved:(NSSet*)touches withEvent:(UIEvent*)event;
- (UIView *)hitTest:(CGPoint)point withEvent:(UIEvent *)event;

/**********************
 * ObjC -> Native C++
 **********************/
- (void)sendTouchData:(NSSet *)touches;

@end