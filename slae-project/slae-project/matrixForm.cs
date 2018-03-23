using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using slae_project.Vector;

namespace slae_project
{
    public partial class matrixForm : Form
    {
        public int size;
        public int property;

        public matrixForm()
        {
            InitializeComponent();
        }

        
        private void matrixFormLoad(object sender, EventArgs e)
        {
            sizeRead();

            var column1 = new DataGridViewColumn();
            column1.Width = 30; //ширина колонки
            column1.ReadOnly = false; //значение в этой колонке нельзя править
            column1.Name = "column"; //текстовое имя колонки, его можно использовать вместо обращений по индексу
            column1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
            column1.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки
            vectorDataGrid.Columns.Add(column1);
            vectorDataGrid.BringToFront();

            for (int i = 0; i < size; i++) 
            {
                var column = new DataGridViewColumn();
                column.Width = 30; //ширина колонки
                column.ReadOnly = false; //значение в этой колонке нельзя править
                column.Name = "column" + Convert.ToString(i); //текстовое имя колонки, его можно использовать вместо обращений по индексу
                column.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
                column.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки

                var row = new DataGridViewRow();
                row.Height = 30; //ширина колонки
                row.ReadOnly = false; //значение в этой колонке нельзя править
                row.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте

                var row1 = new DataGridViewRow();
                row1.Height = 30; //ширина колонки
                row1.ReadOnly = false; //значение в этой колонке нельзя править
                row1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте


                matrixDataGrid.Columns.Add(column);
                matrixDataGrid.Rows.Add(row);
                vectorDataGrid.Rows.Add(row1);

            }

            

            doMatrixNull();

            if (property == 1)  // в случае симметричной матрицы
            {
                for(int i=0;i<size;i++)
                {
                    for (int j = i+1; j < size; j++) 
                    {
                        matrixDataGrid[j, i].ReadOnly = true;
                    }
                }

            }

            matrixDataGrid.Click += new System.EventHandler(dataGridChange);
            matrixDataGrid.AllowUserToResizeRows = false; // запрещаем менять размер строчек
            matrixDataGrid.AllowUserToResizeColumns = false; // запрещаем менять размер столбцов
            matrixDataGrid.RowHeadersVisible = false; // делаем невидимыми заголовки строк
            matrixDataGrid.ColumnHeadersVisible = false; // делаем невидимыми заголовки столбцов
            matrixDataGrid.AllowUserToAddRows = false; // запрещаем добавлять строки
            matrixDataGrid.Height = 30 * size; // высота рамки
            matrixDataGrid.Width = 30 * size; // ширина рамки

            vectorDataGrid.Click += new System.EventHandler(dataGridChange);
            vectorDataGrid.AllowUserToResizeRows = false; // запрещаем менять размер строчек
            vectorDataGrid.AllowUserToResizeColumns = false; // запрещаем менять размер столбцов
            vectorDataGrid.RowHeadersVisible = false; // делаем невидимыми заголовки строк
            vectorDataGrid.ColumnHeadersVisible = false; // делаем невидимыми заголовки столбцов
            vectorDataGrid.AllowUserToAddRows = false; // запрещаем добавлять строки
            vectorDataGrid.Height = 30 * size; // высота рамки
            vectorDataGrid.Width = 30; // ширина рамки


            groupBox1.Height = matrixDataGrid.Height + 50; // высота групбокса относительно размера матрицы
            groupBox1.Width = matrixDataGrid.Width + 30; // ширина групбокса относительно размера матрицы
            groupBox2.Height = vectorDataGrid.Height + 50; // высота групбокса относительно размера матрицы
            groupBox2.Width = vectorDataGrid.Width + 60; // ширина групбокса относительно размера матрицы
        }


        public void textToOnlyNumbers(ref string text) // Проверка, на цифру в поле
        {                                               //если чо-то левое то обнуление полей
            string buff = text;
            string result = "";
            int lettersCount = text.Count();
            bool dotNotWas = true;
            for (int i = 0; i < lettersCount; i++)
                if (buff[i] >= '0' && buff[i] <= '9' || buff[i] == '.' && dotNotWas)
                {
                    if (buff[i] == '.' && dotNotWas)
                        dotNotWas = false;
                    result += buff[i];
                }
            if (result == "")
                result = "0";
            text = result;
        }

        private void doMatrixNull()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrixDataGrid[j, i].Value = 0;

                }
                vectorDataGrid[0,i].Value = 0;
            }
        }

        private void dataGridChange(object sender, EventArgs e)
        {
            string Val;
            if (property == 1)
                for (int i = 0; i < size; i++)
                {
                    for (int j = i + 1; j < size; j++)
                    {
                        Val = Convert.ToString(matrixDataGrid[i, j].Value);
                        textToOnlyNumbers(ref Val);
                        matrixDataGrid[i, j].Value = Val;

                        matrixDataGrid[j, i].Value = matrixDataGrid[i, j].Value;
                        matrixDataGrid.UpdateCellValue(j, i);
                    }
                    Val = Convert.ToString(vectorDataGrid[0, i].Value);
                    textToOnlyNumbers(ref Val);
                    vectorDataGrid[0, i].Value = Val;
                }
            else for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        Val = Convert.ToString(matrixDataGrid[i, j].Value);
                        textToOnlyNumbers(ref Val);
                        matrixDataGrid[i, j].Value = Val;
                    }
                    Val = Convert.ToString(vectorDataGrid[0, i].Value);
                    textToOnlyNumbers(ref Val);
                    vectorDataGrid[0, i].Value = Val;
                }
           
        }

        private void sizeRead() // чтение размерности матрицы из файла
        {
            using (StreamReader reader = File.OpenText("mymatrixSet.txt"))
            {
                size = Convert.ToInt32(reader.ReadLine());
                property = Convert.ToInt32(reader.ReadLine());
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            doMatrixNull();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1.str_format_matrix = "Плотный";
            Factory.CreateMatrix(Form1.str_format_matrix);
            List<string> arrays = Factory.name_arr;
            int count_arr = arrays.Count();

            string name = "myMatrix.txt";
            using (StreamWriter writer = File.CreateText(name))
            {
                string line;
                writer.WriteLine(size.ToString());
                for (int j = 0; j < size; j++)
                {
                    for (int i = 0; i < size; i++)
                    {
                        line = matrixDataGrid[i,j].Value.ToString();
                        writer.Write(line + " ");
                    }
                    writer.Write("\r\n");
                }
            }
            Form2.filenames_format.Add(arrays[0].ToString(), name);

                Form2.F = new SimpleVector(size);
                string line1;
                for (int i = 0; i < size; i++)
                {
                    line1 = vectorDataGrid[0,i].Value.ToString();
                    Form2.F[i] = Convert.ToDouble(line1);
                 
                }
            Form2.X0 = new SimpleVector(size);
            for (int i = 0; i < size; i++)
                {
                    Form2.X0[i] = 1.0;
                }

            this.Close();

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }

    
}
