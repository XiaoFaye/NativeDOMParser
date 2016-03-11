//
//  ViewController.m
//  JQuery
//
//  Created by James on 3/03/16.
//  Copyright © 2016 Macintosh. All rights reserved.
//

#import "ViewController.h"

@interface ViewController ()

@end

@implementation ViewController

- (NSString*)JQ
{
    //{0} = selector, {1} = selector text
    return @"function GetJQValue() { if ({0}('{1}').length == 1) {return {0}('{1}')[0].outerHTML; } else if ({0}('{1}').length > 1) {var allhtml = ''; {0}('{1}').each(function() {allhtml=allhtml+{0}(this)[0].outerHTML+'\\r\\n';}); return allhtml;} else return 'no item found.';} GetJQValue();";
}

- (NSString*)JQN
{
    //N means Native JS, {0} = selector, {1} = selector text
    return @"function GetJQValue() { if ({0}('{1}').length == 1) {return {0}('{1}')[0].outerHTML; } else if ({0}('{1}').length > 1) {var allhtml = ''; for (var i = 0; i < {0}('{1}').length; i++) allhtml=allhtml+{0}('{1}')[i].outerHTML+'\\r\\n'; return allhtml;} else return 'no item found.';} GetJQValue();";
}

- (NSString*)JQAttr
{
    //{0} = selector, {1} = selector text, {2} = attribute text
    return @"function GetJQValue() { if ({0}('{1}').length == 1) {return {0}('{1}').attr('{2}'); } else if ({0}('{1}').length > 1) {var allhtml = ''; {0}('{1}').each(function() {allhtml=allhtml+{0}(this).attr('{2}')+'\\r\\n';}); return allhtml;} else return 'no item found.';} GetJQValue();";
}

- (NSString*)JQNAttr
{
    //N means Native JS, {0} = selector, {1} = selector text, {2} = attribute text
    return @"function GetJQValue() { if ({0}('{1}').length == 1) {return {0}('{1}')[0].{2}; } else if ({0}('{1}').length > 1) { var allhtml = ''; for (var i = 0; i < {0}('{1}').length; i++) allhtml=allhtml+{0}('{1}')[i].{2}+'\\r\\n'; return allhtml;} else return 'no item found.';} GetJQValue();";
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (IBAction)LoadUrl{
    [super viewDidLoad];
    //NSString *fullURL = @"http://192.168.1.5";
    if (![_webView.request.URL.absoluteString isEqualToString:self.txtUrl.text]) {
        NSURL *url = [NSURL URLWithString:self.txtUrl.text];
        NSURLRequest *requestObj = [NSURLRequest requestWithURL:url];
        [_webView loadRequest:requestObj];
    }
    else
    {
        [self webViewDidFinishLoad: _webView];
    }
}

- (void)webViewDidFinishLoad:(UIWebView *)webView
{
    NSString *JQSelector = self.GetJQSelector;
    NSString *selector = self.txtSelector.text;
    NSString *attribute = self.txtAttr.text;
    NSString *query;
    NSString *result;
    
    if ([attribute isEqualToString:@""])
    {
        if (JQSelector.length > 10)
        {
            query = [self.JQN stringByReplacingOccurrencesOfString:@"{0}" withString:JQSelector];
            query = [query stringByReplacingOccurrencesOfString:@"{1}" withString:selector];
            result = [_webView stringByEvaluatingJavaScriptFromString:query];
        }
        else
        {
            query = [self.JQ stringByReplacingOccurrencesOfString:@"{0}" withString:JQSelector];
            query = [query stringByReplacingOccurrencesOfString:@"{1}" withString:selector];
            result = [_webView stringByEvaluatingJavaScriptFromString:query];
        }
    }
    else
    {
        if (JQSelector.length > 10)
        {
            query = [self.JQNAttr stringByReplacingOccurrencesOfString:@"{0}" withString:JQSelector];
            query = [query stringByReplacingOccurrencesOfString:@"{1}" withString:selector];
            query = [query stringByReplacingOccurrencesOfString:@"{2}" withString:attribute];
            result = [_webView stringByEvaluatingJavaScriptFromString:query];
        }
        else
        {
            query = [self.JQAttr stringByReplacingOccurrencesOfString:@"{0}" withString:JQSelector];
            query = [query stringByReplacingOccurrencesOfString:@"{1}" withString:selector];
            query = [query stringByReplacingOccurrencesOfString:@"{2}" withString:attribute];
            result = [_webView stringByEvaluatingJavaScriptFromString:query];
        }
    }
    
    self.lblResult.text = result;
}

- (NSString*)GetJQSelector
{
    NSString *result = [_webView stringByEvaluatingJavaScriptFromString:@"(typeof jQuery != 'undefined').toString()"];
    if ([result isEqualToString:@"true"]) {
        return @"jQuery";
    }
    
    result = [_webView stringByEvaluatingJavaScriptFromString:@"(typeof $ != 'undefined').toString()"];
    if ([result isEqualToString:@"true"]) {
        return @"$";
    }
    
    return @"document.querySelectorAll";
}

@end
