using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{   /// <summary>
    /// klasa rozszerzajaca funkcjonalnosci okna Messenger 
    /// </summary>
    public static class FormExtentions
    {
        /// <summary>
        /// metoda likwidujaca konflikty miedzywatkowe przy obsludze przycisków
        /// </summary>
        /// <param name="button">dany przycisk</param>
        /// <param name="enabled">czy aktywny</param>
        public static void EnableButton(this Button button, bool enabled)
        {
            if (button.InvokeRequired)
                button.Invoke((Action)(() => EnableButton(button, enabled)));
            else
                button.Enabled = enabled;
        }
        /// <summary>
        ///  metoda likwidujaca konflikty miedzywatkowe przy obsludze pol tekstowych
        /// </summary>
        /// <param name="box">dane pole tekstowe</param>
        /// <param name="text">tekst</param>
        /// <param name="isMe">czy ten klient jest autorem tekstu</param>
        public static void ChangeText(this RichTextBox box, string text, bool isMe)
        {
            if (box.InvokeRequired)
                box.Invoke((Action)(() => ChangeText(box, text, isMe)));
            else
            {
                int beginning = box.Text.Length;
                box.AppendText(text + Environment.NewLine);
                if(!isMe)
                {
                    box.SelectionStart = beginning;
                    box.SelectionLength = text.Length;
                    box.SelectionColor = Color.BlueViolet;
                }
                box.SelectionStart = box.Text.Length;
                box.ScrollToCaret();
            }
        }
    }
}
