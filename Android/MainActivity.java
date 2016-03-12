package com.example.xiaof.myapplication;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.webkit.ValueCallback;
import android.webkit.WebChromeClient;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.EditText;
import android.widget.TextView;

public class MainActivity extends AppCompatActivity implements View.OnClickListener {

    private WebView mWebView;

    //{0} = selector, {1} = selector text
    String JQ = "function GetJQValue() { if (%1$s('%2$s').length == 1) {" +
            "return %1$s('%2$s')[0].outerHTML; }" +
            " else if (%1$s('%2$s').length > 1) {" +
            " var allhtml = '';" +
            " %1$s('%2$s').each(function() {allhtml=allhtml+%1$s(this)[0].outerHTML+'\\n';});" +
            " return allhtml;}" +
            " else return 'no item found.';} GetJQValue();";
    //N means Native JS, {0} = selector, {1} = selector text
    String JQN = "function GetJQValue() { if (%1$s('%2$s').length == 1) {" +
            "return %1$s('%2$s')[0].outerHTML; }" +
            " else if (%1$s('%2$s').length > 1) {" +
            " var allhtml = '';" +
            " for (var i = 0; i < %1$s('%2$s').length; i++) allhtml=allhtml+%1$s('%2$s')[i].outerHTML+'\\n';" +
            " return allhtml;}" +
            " else return 'no item found.';} GetJQValue();";

    //{0} = selector, {1} = selector text, {2} = attribute text
    String JQAttr = "function GetJQValue() { if (%1$s('%2$s').length == 1) {" +
            "return %1$s('%2$s').attr('%3$s'); }" +
            " else if (%1$s('%2$s').length > 1) {" +
            " var allhtml = '';" +
            " %1$s('%2$s').each(function() {allhtml=allhtml+%1$s(this).attr('%3$s')+'\\n';});" +
            " return allhtml;}" +
            " else return 'no item found.';} GetJQValue();";

    //N means Native JS, {0} = selector, {1} = selector text, {2} = attribute text
    String JQNAttr = "function GetJQValue() { if (%1$s('%2$s').length == 1) {" +
            "return %1$s('%2$s')[0].%3$s; }" +
            " else if (%1$s('%2$s').length > 1) {" +
            " var allhtml = '';" +
            " for (var i = 0; i < %1$s('%2$s').length; i++) allhtml=allhtml+%1$s('%2$s')[i].%3$s+'\\n';" +
            " return allhtml;}" +
            " else return 'no item found.';} GetJQValue();";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        mWebView = (WebView) findViewById(R.id.webView);
        mWebView.getSettings().setJavaScriptEnabled(true);
        mWebView.setWebChromeClient(new WebChromeClient());
        mWebView.setWebViewClient(new WebViewClient() {
            @Override
            public void onPageFinished(WebView view, String url) {
                GetSelector();
            }
        });
        refreshWebView();
        findViewById(R.id.button).setOnClickListener(this);
    }

    private void refreshWebView() {
        mWebView.loadUrl(((EditText) findViewById(R.id.txtURL)).getText().toString());
    }

    @Override
    public void onClick(View v) {
        refreshWebView();
    }

    String result = "document.querySelectorAll";
    void GetSelector()
    {
        mWebView.evaluateJavascript("(typeof jQuery != 'undefined').toString()", new ValueCallback<String>() {
            @Override
            public void onReceiveValue(String s) {
                if(s.contains("true"))
                {
                    result = "jQuery";
                }
            }
        });

        mWebView.evaluateJavascript("(typeof $ != 'undefined').toString()", new ValueCallback<String>() {
            @Override
            public void onReceiveValue(String s) {
                if(s.contains("true") && result != "jQuery")
                {
                    result = "$";
                }
                else
                {
                    String selector = ((EditText) findViewById(R.id.txtSelector)).getText().toString();
                    String attr = ((EditText) findViewById(R.id.txtAttr)).getText().toString();
                    String jsQuery = "";

                    if (result.length() > 10)
                    {
                        jsQuery = String.format((attr.equals("") ? JQN : JQNAttr), result, selector, attr);
                    }
                    else
                    {
                        jsQuery = String.format((attr.equals("") ? JQ : JQAttr), result, selector, attr);
                    }

                    mWebView.evaluateJavascript(jsQuery, new ValueCallback<String>() {
                        @Override
                        public void onReceiveValue(String s) {
                            ((TextView) findViewById(R.id.txtResult)).setText(s.replace("\\u003C", "<").replace("\\n", "\n"));
                        }
                    });
                }
            }
        });
    }
}
