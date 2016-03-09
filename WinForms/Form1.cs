using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JQuerySelector
{
    public partial class Form1 : Form
    {
        WebBrowser wb = new WebBrowser();
        string selector = string.Empty;
        bool errorCatch = false;
        
        public Form1()
        {
            InitializeComponent();

            wb.ScriptErrorsSuppressed = true;
            wb.DocumentCompleted += wb_DocumentCompleted;
        }

        //{0} = selector, {1} = selector text
        string JQ = "function GetJQValue() {{ if ({0}('{1}').length == 1) {{" +
                                                                                "return {0}('{1}')[0].outerHTML; }}" +
                                                                                " else if ({0}('{1}').length > 1) {{" +
                                                                                " var allhtml = '';" +
                                                                                " {0}('{1}').each(function() {{allhtml=allhtml+{0}(this)[0].outerHTML+'\\r\\n';}});" +
                                                                                " return allhtml;}}" +
                                                                                " else return 'no item found.';}} GetJQValue();";
        //N means Native JS, {0} = selector, {1} = selector text
        string JQN = "function GetJQValue() {{ if ({0}('{1}').length == 1) {{" +
                                                                                "return {0}('{1}')[0].outerHTML; }}" +
                                                                                " else if ({0}('{1}').length > 1) {{" +
                                                                                " var allhtml = '';" +
                                                                                " for (var i = 0; i < {0}('{1}').length; i++) allhtml=allhtml+{0}('{1}')[i].outerHTML+'\\r\\n';" + 
                                                                                " return allhtml;}}" +
                                                                                " else return 'no item found.';}} GetJQValue();";

        //{0} = selector, {1} = selector text, {2} = attribute text
        string JQAttr = "function GetJQValue() {{ if ({0}('{1}').length == 1) {{" +
                                                                                "return {0}('{1}').attr('{2}'); }}" +
                                                                                " else if ({0}('{1}').length > 1) {{" +
                                                                                " var allhtml = '';" +
                                                                                " {0}('{1}').each(function() {{allhtml=allhtml+{0}(this).attr('{2}')+'\\r\\n';}});" +
                                                                                " return allhtml;}}" +
                                                                                " else return 'no item found.';}} GetJQValue();";

        //N means Native JS, {0} = selector, {1} = selector text, {2} = attribute text
        string JQNAttr = "function GetJQValue() {{ if ({0}('{1}').length == 1) {{" +
                                                                                "return {0}('{1}')[0].{2}; }}" +
                                                                                " else if ({0}('{1}').length > 1) {{" +
                                                                                " var allhtml = '';" +
                                                                                " for (var i = 0; i < {0}('{1}').length; i++) allhtml=allhtml+{0}('{1}')[i].{2}+'\\r\\n';" + 
                                                                                " return allhtml;}}" +
                                                                                " else return 'no item found.';}} GetJQValue();";

        void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (txtSelector.Text.Trim() == string.Empty)
                    return;

                if (wb.Document != null)
                {
                    if (selector == string.Empty)
                        selector = GetSelector();

                    if (txtAttribute.Text.Trim() == string.Empty)
                        textBox2.Text = wb.Document.InvokeScript("eval", new string[] { string.Format(selector.Length > 10 ? JQN : JQ, selector, txtSelector.Text) }).ToString();
                    else
                        textBox2.Text = wb.Document.InvokeScript("eval", new string[] { string.Format(selector.Length > 10 ? JQNAttr : JQAttr, selector, txtSelector.Text, txtAttribute.Text) }).ToString();
                }
            }
            catch (Exception ex)
            {
                textBox2.Text = ex.Message; 
                errorCatch = true;
            }
            finally
            {
                btnGetContent.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnGetContent.Enabled = false;
            
            if (wb.Url == null || errorCatch == true || wb.Url.Host != (new Uri(txtUrl.Text.Trim()).Host))
            {
                wb.Url = new System.Uri(txtUrl.Text.Trim(), System.UriKind.Absolute);
                errorCatch = false;
            }
            else
                wb_DocumentCompleted(sender, new WebBrowserDocumentCompletedEventArgs(wb.Url));
        }

        string GetSelector()
        {
            string result = wb.Document.InvokeScript("eval", new string[] { "(typeof jQuery != 'undefined').toString()" }).ToString();
            if (result == "true")
                return "jQuery";

            result = wb.Document.InvokeScript("eval", new string[] { "(typeof $ != 'undefined').toString()" }).ToString();
            if (result == "true")
                return "$";

            return "document.querySelectorAll";
        }
    }
}
