using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace slae_project.Graphic
{

    public partial class SaveLoad : Form
    {
        int countOfMatrices;
        public enum WindowType
        {
            Save = 0, Load = 1
        };
        WindowType WinType;
        public SaveLoad(WindowType type)
        {
            WinType = type;
            InitializeComponent();
            countOfMatrices = (Form1.sharpGL_limbo.Get_Form().GD.List_Of_Objects.Count - 1);
            switch (type)
            {
                case WindowType.Load:
                    this.Name = "Load";
                    this.Text = "Load";
                    button1.Text = "Load";
                    break;
                case WindowType.Save:
                    checkBox1.Visible = false;
                    button1.Text = "Save";
                    this.Name = "Save";
                    this.Text = "Save";
                    break;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox1 = (CheckBox)sender;
            if(checkBox1.Checked == true)
            {
                textBox2.Enabled = false;
            }
            else
            {
                textBox2.Enabled = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Visible(object sender, EventArgs e)
        {
            GroupBox groupBox1 = (GroupBox)sender;
            groupBox1.Text = "Номер матрицы: (0-" + countOfMatrices.ToString() + ")";
        }
        private bool CheckNumberFromString(string str, ref int place)
        {
            if (int.TryParse(textBox2.Text, out place) == false)
            {
                MessageBox.Show("Это не число");
                return false;
            }
            else
            {
                if (place < 0 || place > countOfMatrices)
                {
                    MessageBox.Show("Число не в диапазоне");
                    return false;
                }
            }
            return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int place = 0;
            switch (WinType)
            {
                case WindowType.Load:

                    
                    if (checkBox1.Checked == true)
                    {
                        place = Form1.sharpGL_limbo.Get_Form().GD.List_Of_Objects.Count();
                    }
                    else
                    {
                        if(CheckNumberFromString(textBox2.Text,ref place) == false)
                        {
                            return;
                        }
                    }
                    Form1.sharpGL_limbo.ReadMatrix(textBox1.Text + ".txt", place);
                    Close();
                    break;
                case WindowType.Save:
                    if (CheckNumberFromString(textBox2.Text, ref place) == false)
                    {
                        return;
                    }
                    Form1.sharpGL_limbo.WriteMatrix(textBox1.Text + ".txt", place);
                    Close();
                    break;

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
