//
//  GooglePlayLifecycleHelper.m
//  Unity-iPhone
//
//  Created by Mike Desaro on 3/27/14.
//
//

#import "GooglePlayLifecycleHelper.h"


@implementation GooglePlayLifecycleHelper

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - NSObject

+ (void)load
{
	UnityRegisterAppDelegateListener( [self sharedInstance] );
}


+ (GooglePlayLifecycleHelper*)sharedInstance
{
	static GooglePlayLifecycleHelper *sharedInstance;
	static dispatch_once_t onceToken;
	dispatch_once( &onceToken, ^{
		sharedInstance = [[self alloc] init];
	});
	
	return sharedInstance;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - NSNotification

- (void)didRegisterForRemoteNotificationsWithDeviceToken:(NSNotification*)notification
{
	NSLog( @"didRegisterForRemoteNotificationsWithDeviceToken: %@", notification.userInfo );
}


- (void)didFailToRegisterForRemoteNotificationsWithError:(NSNotification*)notification
{
	NSLog( @"didFailToRegisterForRemoteNotificationsWithError: %@", notification.userInfo );
}


- (void)didReceiveRemoteNotification:(NSNotification*)notification
{
	NSLog( @"didReceiveRemoteNotification" );
	
	Class klass = NSClassFromString( @"GPlayRTRoomDelegate" );
	if( [klass respondsToSelector:@selector(handleRemoteNotification:)] )
		[klass performSelector:@selector(handleRemoteNotification:) withObject:notification.userInfo];
}


- (void)onOpenURL:(NSNotification*)notification
{
	NSLog( @"application:openURL: %@", notification.userInfo );
	
	Class klass = NSClassFromString( @"GPlayManager" );
	if( [klass respondsToSelector:@selector(application:openURL:sourceApplication:annotation:)] )
	{
		UIApplication *application = [UIApplication sharedApplication];
		NSURL *url = notification.userInfo[@"url"];
		NSString *sourceApplication = notification.userInfo[@"sourceApplication"];
		id annotation = notification.userInfo[@"annotation"];
		
		[klass application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
	}
}


@end
