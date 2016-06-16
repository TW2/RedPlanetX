using FastColoredTextBoxNS;
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

        FastColoredTextBox fctb = new FastColoredTextBox();
        string _filename = null;

        public RubyEditorForm()
        {
            InitializeComponent();

            this.Controls.Add(fctb);
            fctb.Location = new Point(5, 10);
            fctb.Size = new Size(670, 570);

            fctb.TextChanged += Fctb_TextChanged;

        }

        Style GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        Style BlueBoldStyle = new TextStyle(Brushes.Blue, null, FontStyle.Bold);

        private void Fctb_TextChanged(object sender, TextChangedEventArgs e)
        {
            //clear style of changed range
            e.ChangedRange.ClearStyle(GreenStyle);
            e.ChangedRange.ClearStyle(BlueBoldStyle);
            //comment highlighting
            e.ChangedRange.SetStyle(GreenStyle, @"#.*$", RegexOptions.Multiline);
            //keywords highlighting
            e.ChangedRange.SetStyle(BlueBoldStyle, @"\b(alias|and|begin|break|case|class|def|defined|do|else|elsif|end|ensure|false|for|if|in|module|next|nil|not|or|redo|rescue|retry|return|self|super|then|true|undef|unless|until|when|while|yield)\s+");

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if(fctb.Text.Length != 0)
            {
                if(_filename != null) { SaveImmediately(); } else { Save(); }
            }
            fctb.Text = "class rbScript\n  def doAction\n    #put something here\n  end\nend\n";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fctb.Text.Length != 0)
            {
                if (_filename != null) { SaveImmediately(); } else { Save(); }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (fctb.Text.Length != 0)
            {
                if (_filename != null) { SaveImmediately(); } else { Save(); }
            }
        }

        private void Save()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Ruby files|*.rb";
            sfd.AddExtension = true;
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                _filename = sfd.FileName;
                using (StreamWriter outputFile = new StreamWriter(_filename))
                {
                    outputFile.AutoFlush = true;
                    outputFile.Write(fctb.Text);
                }
            }            
        }

        private void SaveImmediately()
        {
            using (StreamWriter outputFile = new StreamWriter(_filename))
            {
                outputFile.AutoFlush = true;
                outputFile.Write(fctb.Text);
            }
        }
    }
}
