using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace slae_project
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
            InitializeComponent();
            Show();

            WindowTypeChanger(SaveLoad.WindowType.Save);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox1 = (CheckBox)sender;
            if(checkBox1.Checked == true)
            {
                textBox2_NumberMatrix.Enabled = false;
            }
            else
            {
                textBox2_NumberMatrix.Enabled = true;
            }
        }

        private bool CheckNumberFromString(string str, ref int place)
        {
            if (int.TryParse(str, out place) == false)
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

        //Кнопка финального сохранения и загрузки
        private void button1_Click(object sender, EventArgs e)
        {
            int place = 0;
            switch (WinType)
            {
                case WindowType.Load:

                    
                    if (checkBox1.Checked == true)
                    {
                        place = Form1.sharpGL_limbo.List_Of_Objects.Count();
                    }
                    else
                    {
                        if(CheckNumberFromString(textBox2_NumberMatrix.Text,ref place) == false)
                        {
                            return;
                        }
                    }
                    Form1.sharpGL_limbo.ReadMatrix("GraphicData_" + textBox1_NameMatrix.Text + ".txt", place);
                    Form1.sharpGL_limbo.Refresh_Window();

                    MessageBox.Show("GraphicData_" + textBox1_NameMatrix.Text + ".txt" + " загружен.");
                    //Close();
                    break;
                case WindowType.Save:
                    if (CheckNumberFromString(textBox2_NumberMatrix.Text, ref place) == false)
                    {
                        return;
                    }
                    Form1.sharpGL_limbo.WriteMatrix("GraphicData_" + textBox1_NameMatrix.Text + ".txt", place);

                    MessageBox.Show("GraphicData_" + textBox1_NameMatrix.Text + ".txt" + " сохранен.");
                    //Close();
                    break;

            }
            Refresher();
        }


        private void radioButton1_Save_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1_Save.Checked)
            {
                radioButton2_Load.Checked = false;
                WindowTypeChanger(SaveLoad.WindowType.Save);
            }
        }

        private void radioButton2_Load_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2_Load.Checked)
            {
                radioButton1_Save.Checked = false;
                WindowTypeChanger(SaveLoad.WindowType.Load);
            }
        }

        private void Refresher()
        {
            countOfMatrices = (Form1.sharpGL_limbo.List_Of_Objects.Count - 1);
            groupBox2_NumberMatrix.Text = "Номер матрицы: (0-" + countOfMatrices.ToString() + ")";
        }
        private void WindowTypeChanger(WindowType type)
        {
            WinType = type;
            Refresher();
            switch (type)
            {
                case WindowType.Load:
                    checkBox1.Visible = true;
                    this.Name = "Загрузить";
                    this.Text = "Загрузить";
                    button1.Text = "Загрузить в файл";
                    break;
                case WindowType.Save:
                    checkBox1.Visible = false;
                    button1.Text = "Сохранить в файл";
                    this.Name = "Сохранить";
                    this.Text = "Сохранить";
                    break;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Refresher();
        }

        private void button2_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
