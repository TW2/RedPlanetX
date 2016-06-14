using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedPlanetXv8.Composition.Ruby
{
    public partial class RubyEditorForm : Form
    {
        //"class rbScript\n  def doAction\n    #put something here\n  end\nend"
        /*
        List<string> keywords = new List<string>();
            keywords.Add("alias"); keywords.Add("and"); keywords.Add("begin");
            keywords.Add("break"); keywords.Add("case"); keywords.Add("class");
            keywords.Add("def"); keywords.Add("defined?"); keywords.Add("do");
            keywords.Add("else"); keywords.Add("elsif"); keywords.Add("end");
            keywords.Add("ensure"); keywords.Add("false"); keywords.Add("for");
            keywords.Add("if"); keywords.Add("in"); keywords.Add("module");
            keywords.Add("next"); keywords.Add("nil"); keywords.Add("not");
            keywords.Add("or"); keywords.Add("redo"); keywords.Add("rescue");
            keywords.Add("retry"); keywords.Add("return"); keywords.Add("self");
            keywords.Add("super"); keywords.Add("then"); keywords.Add("true");
            keywords.Add("undef"); keywords.Add("unless"); keywords.Add("until");
            keywords.Add("when"); keywords.Add("while"); keywords.Add("yield");
            */

        public RubyEditorForm()
        {
            InitializeComponent();

            Alsing.Windows.Forms.SyntaxBoxControl sbc = new Alsing.Windows.Forms.SyntaxBoxControl();
            sbc.Location = new Point(5, 5);
            sbc.Size = new Size(670, 580);
            this.Controls.Add(sbc);

            Alsing.SourceCode.SyntaxDocument sdoc = sbc.Document;
            sdoc.SyntaxFile = "C#.syn";
            sdoc.Text = "class rbScript\n  def doAction\n    #put something here\n  end\nend";
        }
    }
}
