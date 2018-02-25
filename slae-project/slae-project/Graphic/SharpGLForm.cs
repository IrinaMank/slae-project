using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using SharpGL;
namespace slae_project
{
    
    /// <summary>
    /// The main form class.
    /// </summary>
    public partial class SharpGLForm : Form
    {
        /// <summary>
        /// Вся логика лежит тут и находится в GraphicalLogic.cs
        /// Главное что это структура хранения объектов(числа,векторов,матриц в нашем формате)
        /// </summary>
        public GraphicData GD;
        public string setgsFileName = "settings.txt";
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpGLForm"/> class.
        /// </summary>
        public SharpGLForm(bool visibility)
        {
            InitializeComponent();
            //SharpGLWrappedThread ThreadController = new SharpGLWrappedThread();
            Visible = visibility;
            //Облегчим себе жизнь. Передадим в главную логическую сразу.
            GD = new GraphicData(openGLControl,this);

            //Manual Рендеринг, мы же не делаем игру, так что смысла в RealTime FPS нету.
            //Для повторной отрисовки вызовите функцию openGLControl.Refresh();
            openGLControl.RenderTrigger = RenderTrigger.Manual;
            openGLControl.DoRender();

            //ReadSettings();

            //установить границы скруллбаров и сбросить мышки-местоположение в лево-нижний угол
            Refresh_Window();
        }
        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RenderEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
        {
            GD.RealDraw();
        }
        /// <summary>
        /// Записать текущие настройки в файл settings.txt
        /// </summary>
        private void WriteSettings()
        {
            using (StreamWriter writer = new StreamWriter(setgsFileName, false, System.Text.Encoding.Default))
            {
                
                writer.WriteLine(trackBar_FontSize.Value);
                if(radioButton2_TargetPlus_Enabled.Checked == true)
                {
                    writer.WriteLine(1);
                }
                else
                {
                    writer.WriteLine(0);
                }
                if (radioButton1_Number_enabled.Checked == true)
                {
                    writer.WriteLine(1);
                }
                else
                {
                    writer.WriteLine(0);
                }

                if(radioButton1_General.Checked == true)
                {
                    writer.WriteLine(0);
                }
                else if(radioButton2_Double.Checked == true)
                {
                    writer.WriteLine(1);
                }
                else if(radioButton3_Exponential.Checked == true)
                {
                    writer.WriteLine(2);
                }
                writer.WriteLine(trackBar_QuantityAfterPoint.Value);


            }

        }

        private void errorFileMessage()
        {
            MessageBox.Show("Неправильный файл");
        }
        /// <summary>
        /// Прочитать настройки из файла settings.txt
        /// </summary>
        private void ReadSettings()
        {
            try
            {
                using (StreamReader reader = new StreamReader(setgsFileName, System.Text.Encoding.Default))
                {
                    int inputNumber;
                    bool error;

                    error = int.TryParse(reader.ReadLine(), out inputNumber);
                    if (error == false || inputNumber < 0) { errorFileMessage(); return; }
                    trackBar_FontSize.Value = inputNumber;

                    error = int.TryParse(reader.ReadLine(), out inputNumber);
                    if (error == false) { errorFileMessage(); return; }
                    switch (inputNumber)
                    {
                        case 0:
                            radioButton2_TargetPlus_Enabled.Checked = false;
                            radioButton1_TargetPlus_Disabled.Checked = true;
                            break;
                        case 1:
                            radioButton2_TargetPlus_Enabled.Checked = true;
                            radioButton1_TargetPlus_Disabled.Checked = false;
                            break;
                        default:
                            errorFileMessage(); return;
                    }

                    error = int.TryParse(reader.ReadLine(), out inputNumber);
                    if (error == false) { errorFileMessage(); return; }

                    switch (inputNumber)
                    {
                        case 0:
                            radioButton1_Number_enabled.Checked = false;
                            radioButton1_Number_disabled.Checked = true;
                            break;
                        case 1:
                            radioButton1_Number_enabled.Checked = true;
                            radioButton1_Number_disabled.Checked = false;
                            break;
                        default:
                            errorFileMessage(); return;
                    }


                    error = int.TryParse(reader.ReadLine(), out inputNumber);
                    if (error == false) { errorFileMessage(); return; }

                    switch (inputNumber)
                    {
                        case 0:
                            radioButton1_General.Checked = true;
                            break;
                        case 1:
                            radioButton2_Double.Checked = true;
                            break;
                        case 2:
                            radioButton3_Exponential.Checked = true;
                            break;
                        default:
                            errorFileMessage(); return;
                    }

                    error = int.TryParse(reader.ReadLine(), out inputNumber);
                    if (error == false || inputNumber < 0) { errorFileMessage(); return; }
                    trackBar_QuantityAfterPoint.Value = inputNumber;

                }
            }
            catch (Exception WhoNeedsError)
            {

            }
        }


        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            //  TODO: Initialise OpenGL here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the clear color.
            gl.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            Wrapped_Resized();
            //gl.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
        }

        /// <summary>
        /// Очевидно эта функция при изменении окна. В 2D инициализацию OpenGL
        /// </summary>
        void Wrapped_Resized()
        {
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            gl.LoadIdentity();

            //Мы двумерны.
            gl.Ortho2D(0, openGLControl.Width, 0, openGLControl.Height);
            gl.Viewport(0, 0, openGLControl.Width, openGLControl.Height);

            //  Set the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);

            
            if (GD != null) Refresh_Window(GD.RealDraw_Try_To_Initialize);
            else openGLControl.Refresh();
        }

        /// <summary>
        /// Выход из графического окна.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_exit_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        void setMouseData()
        {
            //Первые Х и Y дают GD.mouse.x и GD.mouse.y которые являются точной позицией
            //курсора для label элементов
            //А второй Х и Y = GD.mouse.true_x & GD.mouse.true_y для openGLcontrol окна мышка
            GD.mouse.setMouseData(MouseButtons.ToString(), Cursor.Position.X - Location.X - openGLControl.Location.X + 25, Cursor.Position.Y - Location.Y - openGLControl.Location.Y - 30, Cursor.Position.X - Location.X - openGLControl.Location.X - 8, -Cursor.Position.Y + Location.Y + Size.Height + openGLControl.Location.Y - 30);
        }

        /// <summary>
        /// Происходит при движении мышкой над окном OpenGL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            setMouseData();

            //Меняем курсор мышки на разные в зависимости нажата левая кнопка мышки или нет.
            if (MouseButtons.ToString() == "Left")
            {
                //Тут высчитывается насколько сместился курсор мышки нажатой

                Cursor.Current = Cursors.NoMove2D;
                GD.mouse.isPressed = true;

                //Местоположение экрана смещенное мышкой присваивает слайдерам(скруллбарам)
                try
                { hScrollBar1.Value = GD.mouse.ShiftedPosition.x; }
                catch (Exception error) { }
                try { vScrollBar1.Value = GD.mouse.ShiftedPosition.y; }
                catch (Exception error) { }
                //Обновили экран

            }
            else
            {
                Cursor.Current = Cursors.Hand;
            }
            openGLControl.Refresh();

            //Эту штуку приходится вызывать когда чтото с мышкой поделал.
            Application.DoEvents();
        }
        /// <summary>
        /// Кнопку мыши опустили вниз(по идеи левую)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseButtons.ToString() == "Left")
            {
                //Меняем курсор мышки на четырехстрелочковый.
                Cursor.Current = Cursors.NoMove2D;
                //Мышка нажата
                GD.mouse.isPressed = true;
            }

            Application.DoEvents();
        }

        /// <summary>
        /// Кнопку мышки левую отжали.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            //Мышка отжата.            
            GD.mouse.isPressed = false;
            GD.mouse.isPressedBefore = false;

            //Обновили экран
            openGLControl.Refresh();
        }

          /// <summary>
        /// Справа в менюшке есть кнопка "Обновить", это она.
        /// Просто обновляет изображение и ничего больше.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_test_Click(object sender, EventArgs e)
        {
            GD.List_Of_Objects.Clear();
            Refresh_Window();
            
            
            

            //AsyncTest.Start();

        }
        //Asynchronized AsyncTest = new Asynchronized();

        
        /// <summary>
        /// Сбрасывает все настройки по умолчанию в менюшке справа
        /// И заодно заново отрисовывает и сбрасывает ваше местоположение
        /// в лево нижний угол.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_reset_Click(object sender, EventArgs e)
        {
            trackBar_QuantityAfterPoint.Value = GD.FontQuanitityAfterPoint = 3;
            trackBar_FontSize.Value = 14; GD.FontSize = 14;

            GD.Grid.xCellSize = 80;
            GD.Grid.yCellSize = 35;

            radioButton1_General.Checked = true;
            radioButton2_Double.Checked = false;
            radioButton3_Exponential.Checked = false;
            GD.font_format = 0;

            radioButton1_Number_disabled.Checked = false;
            GD.TargetNumber = true;

            GD.TargetNumber = true;
            GD.TargetPlus = true;
            radioButton1_Number_enabled.Checked = true;
            radioButton1_Number_disabled.Checked = false;
            radioButton2_TargetPlus_Enabled.Checked = true;
            radioButton1_TargetPlus_Disabled.Checked = false;

            GD.BoolTextIsEnabledOtherwiseQuads = true;

            Refresh_Window(false);
        }

        /// <summary>
        /// Эту функцию я подарил юзерам, вызывать после добавления или удаления объектов
        /// Она обновляет изображение, настраивает максимумы скруллбаров(ибо оно зависит от границ матриц)
        /// И сбрасывает местоположение в лево-нижний угол
        /// </summary>
        public void Refresh_Window(bool TryInit = true)
        {
            if (TryInit) GD.RealDraw_Try_To_Initialize = true;
            openGLControl.Refresh();
            SetScrollBars();
            //GD.MoveToEndCursor();
            //openGLControl.Refresh();
        }

        

        /// <summary>
        /// Функция реагирующая на изменение размеров окна
        /// Своеобразно говоря дубль, ибо openGL....Resized тоже самое
        /// Хотя эта вроде бы чаще реагирует, еще даже пока ты не отпустил мышку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SharpGLForm_Resize(object sender, EventArgs e)
        {
            //Refresh_Window();
        }

        /// <summary>
        /// Высчитывает границы скруллбаров максимумов и 
        /// местоположение мышки-обзора в левый нижний угол возвращает
        /// </summary>
        public void SetScrollBars()
        {
            if (GD != null)
            {
                if (GD.mouse != null)
                {
                    GD.mouse.BorderEndRecalculate();
                    hScrollBar1.Minimum = GD.mouse.BorderBegin.x; hScrollBar1.Maximum = Math.Abs(GD.mouse.BorderEnd.x);
                    vScrollBar1.Minimum = GD.mouse.BorderBegin.y; vScrollBar1.Maximum = Math.Abs(GD.mouse.BorderEnd.y);

                    if (GD.mouse.BorderEnd.y >= GD.mouse.BorderBegin.y)
                    vScrollBar1.Value = Math.Abs(GD.mouse.BorderEnd.y);

                    if (GD.mouse.BorderBegin.x <= GD.mouse.BorderEnd.x)
                    hScrollBar1.Value = Math.Abs(GD.mouse.BorderBegin.x);
                }
            }
        }


        /// <summary>
        /// Функция реагирующая на изменение галочки на "Основной" формат записи чисел
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_General_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1_General.Checked == true)
            {
                GD.font_format = GraphicData.FontFormat.G;
                radioButton2_Double.Checked = false;
                radioButton3_Exponential.Checked = false;
            }
            setAutoCell();
            Refresh_Window(false);
        }

        /// <summary>
        /// Функция реагирующая на изменение галочки на "Дробный" формат записи чисел
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton2_Double_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2_Double.Checked == true)
            {
                GD.font_format = GraphicData.FontFormat.F;
                radioButton1_General.Checked = false;
                radioButton3_Exponential.Checked = false;
            }
            setAutoCell();
            Refresh_Window(false);
        }

        /// <summary>
        /// Функция реагирующая на изменение галочки на "Экспоненциальный" формат записи чисел
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton3_Exponential_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3_Exponential.Checked == true)
            {
                GD.font_format = GraphicData.FontFormat.E;
                radioButton2_Double.Checked = false;
                radioButton1_General.Checked = false;
            }
            setAutoCell();
            Refresh_Window(false);
        }

        /// <summary>
        /// Функция реагирующая на изменение ползунка скруллбара вертикального
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            GD.mouse.ShiftedPosition.y = vScrollBar1.Value;
            openGLControl.Refresh();
        }

        /// <summary>
        /// Функция реагирующая на изменение ползунка скруллбара горизонтального
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            GD.mouse.ShiftedPosition.x = hScrollBar1.Value;
            openGLControl.Refresh();
        }

        private int e_Delta_old = 0;
        private void openGLControl_MouseScroller(object sender, MouseEventArgs e)
        {
            try
            {
                
                vScrollBar1.Value -= (int)e.Delta/5;
                e_Delta_old = e.Delta;
            }
            catch (Exception error) { }


            //Обновили 
            Application.DoEvents();
        }

        private void radioButton1_Number_enabled_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1_Number_enabled.Checked)
            {
                radioButton1_Number_disabled.Checked = false;
                GD.TargetNumber = true;
                openGLControl.Refresh();
            }
        }

        private void radioButton1_Number_disabled_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1_Number_disabled.Checked)
            {
                radioButton1_Number_enabled.Checked = false;
                GD.TargetNumber = false;
                openGLControl.Refresh();
            }
        }

        private void radioButton2_TargetPlus_Enabled_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2_TargetPlus_Enabled.Checked)
            {
                radioButton1_TargetPlus_Disabled.Checked = false;
                GD.TargetPlus = true;
                openGLControl.Refresh();
            }
        }

        private void radioButton1_TargetPlus_Disabled_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1_TargetPlus_Disabled.Checked)
            {
                radioButton2_TargetPlus_Enabled.Checked = false;
                GD.TargetPlus = false;
                openGLControl.Refresh();
            }
            
        }
        private void label_tip(string tip, int shift_x = 0, int shift_y = 0)
        {
            setMouseData();
            label7_FAQ_move_phrase.Text = tip;
            label7_FAQ_move_phrase.Location = new Point(GD.mouse.x + shift_x, GD.mouse.y + shift_y);
            label7_FAQ_move_phrase.Visible = true;
        }
        private void label6_FAQ_MouseHover(object sender, EventArgs e)
        {
            label_tip("С помощью зажатой левой кнопкой мыши, а так же роликом\n" +
                "можно перемещаться по полю тоже.",0,-20);
        }
        private void label6_FAQ_MouseLeave(object sender, EventArgs e)
        {
            label7_FAQ_move_phrase.Visible = false;
        }

       

        //Сохрани текущие настройки рядовой!
        //Сэр, Есть Сэр!
        private void SharpGLForm_Deactivate(object sender, EventArgs e)
        {
            WriteSettings();
        }

        private void button1_SaveLoad_Click(object sender, EventArgs e)
        {
            if (!SaveLoadForm_is_opened())
            {
                SaveLoadForm = new SaveLoad(SaveLoad.WindowType.Save,this);
            }
            
        }
        SaveLoad SaveLoadForm = null;
        public bool SaveLoadForm_is_opened()
        {
            if (SaveLoadForm != null)
                if (!SaveLoadForm.IsDisposed)
                    return true;
            return false;
        }

        private string matrixRowToString(List<double> row)
        {
            string outputLine = "";
            foreach (double element in row)
            {
                outputLine += element.ToString(GD.font_format.ToString() + GD.FontQuanitityAfterPoint.ToString());
                outputLine += " ; ";
            }
            return outputLine;
        }

        private List<double> stringToMatrixRow(string strRow)
        {
            List<double> row = new List<double>();
            string[] numbers = strRow.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string element in numbers)
            {
                try
                {
                    row.Add(Convert.ToDouble(element));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Can't convert double in your file");
                }
            }
            return row;
        }

        /// <summary>
        /// Записать матрицу в файл
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="numObject">Номер матрицы в массиве объектов</param>
        public void WriteMatrix(string path, int numObject)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    writer.WriteLine(GD.List_Of_Objects[numObject].Name);
                    //writer.WriteLine(GD.List_Of_Objects[numObject].Name);
                    foreach (List<double> row in GD.List_Of_Objects[numObject].Matrix)
                    {
                        writer.WriteLine(matrixRowToString(row));
                    }

                    MessageBox.Show(path + " сохранен.");
                }

            }
            catch (Exception YouShouldGiveTheErrorToSomebodyElse)
            {
                MessageBox.Show(YouShouldGiveTheErrorToSomebodyElse.Message, "Трай Кетчуп ловко поймал ошибку!");
            }

        }

        /// <summary>
        /// Считать матрицу из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="numObject">Номер матрицы в массиве объектов</param>
        public void ReadMatrix(string path, int numObject)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path, System.Text.Encoding.Default))
                {
                    if (numObject > GD.List_Of_Objects.Count - 1)
                    {
                        GD.List_Of_Objects.Add(new GraphicData.GraphicObject(reader.ReadLine()));
                    }
                    else
                    {
                        GD.List_Of_Objects[numObject] = new GraphicData.GraphicObject(reader.ReadLine());
                    }
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        GD.List_Of_Objects[numObject].Matrix.Add(stringToMatrixRow(line));
                    }
                    MessageBox.Show(path + " загружен.");
                    Refresh_Window();
                }
            }
            catch (Exception IdontNeedErrors)
            {
                MessageBox.Show(IdontNeedErrors.Message, "Файл не обнаружен!");
            }

        }

        
        /// <summary>
        /// Функция реагирующая на изменение ползунка размера шрифта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar_FontSize_MouseCaptureChanged(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Функция реагирующая на изменения ползунка за колво знаков после запятой.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar_QuantityAfterPoint_MouseCaptureChanged(object sender, EventArgs e)
        {
            
        }

        private void SharpGLForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Clearer();
        }

        private void SharpGLForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Clearer();
        }
        public void Clearer()
        {
            GD.Grid.NetWorkValue.Clear();
            GD.Grid.NetWorkOS_X.Clear();
            GD.Grid.NetWorkOS_Y.Clear();
            GC.Collect(20000);
        }

        private void trackBar_FontSize_ValueChanged(object sender, EventArgs e)
        {
            if (trackBar_FontSize.Value >= 4)
            {
                GD.FontSize = trackBar_FontSize.Value;
                GD.BoolTextIsEnabledOtherwiseQuads = true;
                setAutoCell();
                GD.BoolLinesAreEnabled = true;
            }
            else
            {
                GD.BoolTextIsEnabledOtherwiseQuads = false;

                int size = 17 + trackBar_FontSize.Value;
                GD.Grid.xCellSize = size;
                GD.Grid.yCellSize = size;
                
                if (size < 6) GD.BoolLinesAreEnabled = false;
                else GD.BoolLinesAreEnabled = true;

                Refresh_Window(false);
            }
        }

        private void trackBar_QuantityAfterPoint_ValueChanged(object sender, EventArgs e)
        {
            GD.FontQuanitityAfterPoint = trackBar_QuantityAfterPoint.Value;
            setAutoCell();
            Refresh_Window(false);
        }
    }
}
