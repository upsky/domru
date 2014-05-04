//
//  GooglePlayLifecycleHelper.h
//  Unity-iPhone
//
//  Created by Mike Desaro on 3/27/14.
//
//

#import <Foundation/Foundation.h>
#import "AppDelegateListener.h"


@interface GooglePlayLifecycleHelper : NSObject<AppDelegateListener>

+ (GooglePlayLifecycleHelper*)sharedInstance;

@end
