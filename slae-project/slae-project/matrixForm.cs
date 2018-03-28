﻿using System;
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
        static Double Eval(String expression)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            return Convert.ToDouble(table.Compute(expression, String.Empty));
        }

        private void matrixFormLoad(object sender, EventArgs e)
        {
            sizeRead();
            size = 2;
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

            matrixDataGrid.AllowUserToResizeRows = false; // запрещаем менять размер строчек
            matrixDataGrid.AllowUserToResizeColumns = false; // запрещаем менять размер столбцов
            matrixDataGrid.RowHeadersVisible = false; // делаем невидимыми заголовки строк
            matrixDataGrid.ColumnHeadersVisible = false; // делаем невидимыми заголовки столбцов
           
            vectorDataGrid.AllowUserToResizeRows = false; // запрещаем менять размер строчек
            vectorDataGrid.AllowUserToResizeColumns = false; // запрещаем менять размер столбцов
            vectorDataGrid.RowHeadersVisible = false; // делаем невидимыми заголовки строк
            vectorDataGrid.ColumnHeadersVisible = false; // делаем невидимыми заголовки столбцов
            
            vectorDataGrid.Width = 30; 
            groupBox2.Width = vectorDataGrid.Width + 60;
            sizeWrap();
        }

        /// <summary>
        /// Подгонка размеров элементов окна под текущую размерность матрицы. Вызывается каждый раз при измененении размеров матрицы.
        /// </summary>
        private void sizeWrap() {             
            matrixDataGrid.Height = 30 * size;
            matrixDataGrid.Width = 30 * size; 
            vectorDataGrid.Height = 30 * size;
            groupBox1.Height = matrixDataGrid.Height + 50; 
            groupBox1.Width = matrixDataGrid.Width + 30;
            groupBox2.Height = vectorDataGrid.Height + 50;  
        }

        public void textToOnlyNumbers(int col, int row)
        {
            try
            {
                matrixDataGrid[col, row].Value = Eval(matrixDataGrid[col, row].Value.ToString().Replace(",","."));
            }
            catch{
                matrixDataGrid[col, row].Value = null;
            }
        }

        private void doMatrixNull()
        {
            for (int i = 0; i < size - 1; i++)
            {
                for (int j = 0; j < size - 1; j++)
                    if (matrixDataGrid[j, i].Value == null)
                        matrixDataGrid[j, i].Value = 0;

                if (vectorDataGrid[0, i].Value == null)
                    vectorDataGrid[0, i].Value = 0;
            }
            
        }

        private void clearMatrix()
        {
            while (size > 2)
            {
                size--;
                matrixDataGrid.Rows.RemoveAt(size);
                matrixDataGrid.Columns.RemoveAt(size);
                vectorDataGrid.Rows.RemoveAt(size);              
            }

            for (int j = 0; j < size; j++)
            {
                for (int i = 0; i < size; i++)
                    matrixDataGrid[i, j].Value = null;
                vectorDataGrid[0, j].Value = null;
            }

            sizeWrap();
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
            clearMatrix();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            doMatrixNull();
            size--;
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
            FileLoadForm.filenames_format.Clear();
            FileLoadForm.filenames_format.Add(arrays[0].ToString(), name);

                FileLoadForm.F = new SimpleVector(size);
                string line1;
                for (int i = 0; i < size; i++)
                {
                    line1 = vectorDataGrid[0,i].Value.ToString();
                    FileLoadForm.F[i] = Convert.ToDouble(line1);                 
                }
            FileLoadForm.X0 = new SimpleVector(size);
            for (int i = 0; i < size; i++)
                {
                    FileLoadForm.X0[i] = 1.0;
                }
            this.Close();
        }

        private void matrixDataGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == size-1 || e.RowIndex == size-1)
            {
                for (int i=0; i<size;i++)
                    for (int j = 0; j < size; j++)
                    {
                        matrixDataGrid.Rows[j].Cells[i].Style.BackColor = Color.White;
                        vectorDataGrid.Rows[j].Cells[0].Style.BackColor = Color.White;
                    }
                if (size < 10)
                {
                    var column = new DataGridViewColumn();
                    column.Width = 30; //ширина колонки
                    column.ReadOnly = false; //значение в этой колонке нельзя править
                    column.Name = "column" + Convert.ToString(size); //текстовое имя колонки, его можно использовать вместо обращений по индексу
                    column.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
                    column.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки
                    column.DefaultCellStyle.BackColor = Color.Gray;

                    var row = new DataGridViewRow();
                    row.Height = 30; //ширина колонки
                    row.ReadOnly = false; //значение в этой колонке нельзя править
                    row.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
                    row.DefaultCellStyle.BackColor = Color.Gray;

                    var row1 = new DataGridViewRow();
                    row1.Height = 30; //ширина колонки
                    row1.ReadOnly = false; //значение в этой колонке нельзя править
                    row1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
                    row1.DefaultCellStyle.BackColor = Color.Gray;

                    size++;
                    matrixDataGrid.Columns.Add(column);
                    matrixDataGrid.Rows.Add(row);
                    vectorDataGrid.Rows.Add(row1);

                    sizeWrap();
                }
            }
        }

        private void matrixDataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (matrixDataGrid[e.ColumnIndex, e.RowIndex].Value != null)
            {                
                textToOnlyNumbers(e.ColumnIndex, e.RowIndex);
                if (property == 1)
                    matrixDataGrid[e.RowIndex, e.ColumnIndex].Value = matrixDataGrid[e.ColumnIndex, e.RowIndex].Value;
                matrixDataGrid.UpdateCellValue(e.RowIndex, e.ColumnIndex);
            }
            else
            {
                if (property == 1)
                    matrixDataGrid[e.RowIndex, e.ColumnIndex].Value = "";

                if (e.ColumnIndex == size - 2 || e.RowIndex == size - 2)
                {
                    bool notEmptyRow = false;
                    for (int i = 0; i < size; i++)
                    {
                        if (matrixDataGrid[e.ColumnIndex, i].Value != null || matrixDataGrid[i, e.RowIndex].Value != null)
                            notEmptyRow = true;
                    }
                    if (!notEmptyRow)
                    {
                        size--;
                        matrixDataGrid.Rows.RemoveAt(e.ColumnIndex);
                        matrixDataGrid.Columns.RemoveAt(e.ColumnIndex);
                        vectorDataGrid.Rows.RemoveAt(e.ColumnIndex);

                        sizeWrap();
                    }
                }
            }
        }
    }

    
}
