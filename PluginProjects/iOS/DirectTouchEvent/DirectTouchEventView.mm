#import "DirectTouchEventView.h"

@implementation DirectTouchEventView

/****************
 * ObjC Methods
 ****************/
-  (id)initWithFrame:(CGRect)aRect
{
    rootView = [[[UIApplication sharedApplication] keyWindow] rootViewController].view;
    
    return [super initWithFrame:aRect];
}

- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
    [self sendTouchData:touches];
    
    if ([otherViews count] > 0) {
        [[otherViews lastObject] touchesBegan:touches withEvent:event];
    } else {
        if (DirectTouchManager::IsDefaultTouch()) {
            [rootView touchesBegan:touches withEvent:event];
        }
    }
}

- (void)touchesMoved:(NSSet *)touches withEvent:(UIEvent*)event
{
    [self sendTouchData:touches];
    
    if ([otherViews count] > 0) {
        [[otherViews lastObject] touchesMoved:touches withEvent:event];
    } else {
        if (DirectTouchManager::IsDefaultTouch()) {
            [rootView touchesMoved:touches withEvent:event];
        }
    }
}

- (void)touchesCancelled:(NSSet *)touches withEvent:(UIEvent*)event
{
    [self sendTouchData:touches];
    
    if ([otherViews count] > 0) {
        [[otherViews lastObject] touchesCancelled:touches withEvent:event];
    } else {
        if (DirectTouchManager::IsDefaultTouch()) {
            [rootView touchesCancelled:touches withEvent:event];
        }
    }
}

- (void)touchesEnded:(NSSet *)touches withEvent:(UIEvent *)event
{
    [self sendTouchData:touches];
    
    if ([otherViews count] > 0) {
        [[otherViews lastObject] touchesEnded:touches withEvent:event];
    } else {
        if (DirectTouchManager::IsDefaultTouch()) {
            [rootView touchesEnded:touches withEvent:event];
        }
    }
}

- (UIView *)hitTest:(CGPoint)point withEvent:(UIEvent *)event
{
    if (otherViews == nil) {
        otherViews = [NSMutableArray array];
    } else {
        [otherViews removeAllObjects];
    }
    
    for (UIView *subview in [rootView subviews]) {
        if (subview != self && [subview hitTest:point withEvent:event] != nil) {
            [otherViews addObject:subview];
        }
    }
    
    return self;
}


/**********************
 * ObjC -> Native C++
 **********************/
- (void)sendTouchData:(NSSet *)touches
{
    int nVal =  (int)[touches count];

    int *touchID = (int *)malloc(sizeof(int) * nVal);
    double *timestamp = (double *)malloc(sizeof(double) * nVal);
    int *phase = (int *)malloc(sizeof(int) * nVal);
    float *posY = (float *)malloc(sizeof(float) * nVal);
    float *posX = (float *)malloc(sizeof(float) * nVal);
    float *radius = (float *)malloc(sizeof(float) * nVal);
    float *pressure = (float *)malloc(sizeof(float) * nVal);
                                 
    CGPoint point;
    
    int count = 0;
    for (UITouch *touch in touches) {
        if ( [[UIDevice currentDevice].systemVersion floatValue] < 9.1) {
            point = [touch locationInView:self];
        } else {
            point = [touch preciseLocationInView:self];
        }
        
        touchID[count] = (int)[touch hash];
        timestamp[count] = [touch timestamp];
        phase[count] = [touch phase];
        posX[count] = point.x * touch.view.contentScaleFactor;
        posY[count] = point.y * touch.view.contentScaleFactor;
        radius[count] = [touch majorRadius];
        pressure[count] = [touch force];
        count++;
    }
    
    DirectTouchManager::OnTouchEvent(touchID, timestamp, phase, posY, posX, radius, pressure, nVal);
    
    free(touchID);
    free(timestamp);
    free(phase);
    free(posX);
    free(posY);
    free(radius);
    free(pressure);
}

@end