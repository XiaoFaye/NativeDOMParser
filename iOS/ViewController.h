//
//  ViewController.h
//  JQuery
//
//  Created by James on 3/03/16.
//  Copyright © 2016 Macintosh. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface ViewController : UIViewController <UIWebViewDelegate>
@property (weak, nonatomic) IBOutlet UITextField *txtSelector;
@property (weak, nonatomic) IBOutlet UITextField *txtAttr;
@property (weak, nonatomic) IBOutlet UITextField *txtUrl;
@property (weak, nonatomic) IBOutlet UIWebView *webView;
@property (weak, nonatomic) IBOutlet UITextView *lblResult;

- (void)webViewDidFinishLoad:(UIWebView *)webView;

- (IBAction)LoadUrl;

- (NSString*)GetJQSelector;

@property (readonly) NSString *JQ;
@property (readonly) NSString *JQN;
@property (readonly) NSString *JQAttr;
@property (readonly) NSString *JQNAttr;

@end
