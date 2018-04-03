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
    public partial class FAQ : Form
    {
        public FAQ()
        {
            InitializeComponent();
            Show();
            Wrapped_Changer();
            SetScroolBar();
            ScrollBarReaction();
        }
        int Shift = 10;
        private void FAQ_Resize(object sender, EventArgs e)
        {
            Wrapped_Changer();
            SetScroolBar();
            
        }
        public void SetScroolBar()
        {
            vScrollBar1.Maximum = Curs + Between - (this.Height - 80);
            Shift = this.Height - 50;
        }
        public void ScrollBarReaction()
        {
            if (vScrollBar1.Value > vScrollBar1.Maximum - 50)
            {
                button1.Visible = false;
            }
            else
            {
                button1.Visible = true;
            }

            if (vScrollBar1.Value > vScrollBar1.Minimum + 50)
            {
                button2.Visible = true;
            }
            else
            {
                button2.Visible = false;
            }
        }
        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            Wrapped_Changer();
            ScrollBarReaction();
        }
        int Curs = 10;
        int Between = 10;
        void Wrapped_Changer()
        {
            Curs = 10 - vScrollBar1.Value;
            int Begin = 10;
            int End = 30 + vScrollBar1.Size.Width;
            Between = 30;

            //Заголовок.
            label1.Location = new Point(this.Width/2 - label1.Size.Width/2, Curs + Between);
            label1.Text = "Справка";
            Curs += label1.Size.Height;

            label2.Location = new Point(Begin, Curs + Between);
            label2.Text = "Добро пожаловать в справочно-информационное бюро графического модуля.!\r\n" +
                "\r\n" +
                "Перемещайтесь пожалуйста вниз с помощью ползунка с правой части окна.\r\n" +
                "или с помощью кнопок-стрелок с левой части экрана для перемещения\r\n" +
                "скачками.";
            Curs += label2.Size.Height;

            //Картинка
            pictureBox1.Location = new Point(Begin, Curs + Between);
            pictureBox1.Size = new Size(this.Width - End, (int)((double)this.Width / pictureBox1.PreferredSize.Width * pictureBox1.PreferredSize.Height));
            Curs += pictureBox1.Size.Height;

            //Текст
            label3.Location = new Point(Begin, Curs + Between);
            label3.Text = "Краткая информация по элементам интерфейса \r\n" +
                "указанным красными числами на картинке выше\r\n" +
                "\r\n" +
                "1) 'Размер ячейки' регулирует размер шрифта текста, в следствии\r\n" +
                "которого изменяется размер ячейки. Начиная с некоего малого значения\r\n" +
                "В ячейке текст заменяется цветовым заполнением с градацией от синего-малого\r\n" +
                "к красному-большому значению\r\n" +
                "\r\n" +
                "2) Включить или выключить\r\n" +
                "Зеленный целеуказатель сопровождающий курсор мышки, при нажатии\r\n" +
                "перемещения через Телепортер(см. раздел Телепортер ниже), целеуказатель\r\n" +
                "укажет на запрашиваемую ячейку\r\n" +
                "\r\n" +
                "3) Включить или выключить цифровые надписи\r\n" + 
                "Возле зеленного целеукатаеля указаны I -столбец, J- строка, Х-значение ячейки\r\n" +
                "\r\n" +
                "4) Можно менять отображаемый формат чисел между основным, дробным и\r\n" +
                " экспоненциальным. Основной формат отличается тем что, когда может\r\n" +
                " отображает словно число целое, а когда не может, отобразит дробным\r\n" +
                ",и если совсем малое или большое число, то\r\n" +
                "отобразит экспоненциальным" +
                "\r\n" +
                "5) Количество отображаемых знаков у чисел на экране\r\n" +
                "\r\n" +
                "6) Телепортер, позволяет переместится к указанной ячейке, определенной строки\r\n" +
                "столбца и матрицы, номера матриц указаны слева от их названий\r\n" +
                "область допустимых строк и столбцов в данном окне указываются при выборе\r\n" +
                "определенной матрицы. При включенном целеуказатели, укажет на ячейку.\r\n" +
                "\r\n" +
                "7) Обновить и показать основную отображаемую информацию, а именно матрицу А\r\n" +
                "вектор результат Х, правый вектор B, и вектор Невязки. Данная кнопка служит\r\n" +
                " на случай, если в окне видна все еще старая информация, а уже необходимо\r\n" +
                " запросить новую на отображение, т.к. решение изменилось\r\n" +
                "\r\n" +
                "8) Сбросить настройки, служит для того, чтобы если вы сильно изменяли размеры\r\n" +
                " ячеек, форматы, настройки, количества чисел, или далеко убежали за границы\r\n" +
                " экрана. Данная кнопка вернет ваше местоположение и сбросит настройки на\r\n" +
                " по умолчанию\r\n" +
                "\r\n" +
                "9) Сохранение и загрузка позволяют сохранить с указанным именем любую матрицу\r\n" +
                "или вектор, и при этом позволяют так же повторно загрузить их в графический\r\n" +
                "модуль. Загруженные элементы пропадут после нажатия обновить и показать.\r\n" +
                "\r\n" +
                "10) Справка. Вы здесь\r\n" +
                "\r\n" +
                "11) Выход из окна графического модуля. Сама основная программа закрыта не будет.\r\n" +
                "\r\n" +
                "12) Отображаемый целеуказатель и номер строки, столбца и значение ячейки\r\n" +
                "\r\n" +
                "13) Начало матрицы А\r\n" +
                "\r\n" +
                "14) Начало вектора результата Х\r\n" +
                "\r\n" +
                "15) Начало правого вектора B\r\n" +
                "\r\n" +
                "16) Вектор невязки для иттерационых способов решений\r\n" +
                "отображается только если значений невязки более 1.\r\n" +
                "\r\n" +
                "17 и 18) Ползунки для перемещения по горизонтали и вертикали\r\n" +
                "Так же можно перемещаться зажав левую кнопку мыши\r\n" +
                "\r\n" +
                "Дополнительная информация:\r\n" +
                "На правую кнопку мыши нажав на ячейку, появится табличка указывающая\r\n" +
                "текущую строку, столбец и значение ячейки.\r\n";
            Curs += label3.Bounds.Height;

            //Заголовок.
            label4.Location = new Point(this.Width / 2 - label4.Size.Width / 2, Curs + Between);
            label4.Text = "Телепортер";
            Curs += label4.Size.Height;

            //Картинка
            pictureBox2.Location = new Point(Begin, Curs + Between);
            pictureBox2.Size = new Size(this.Width - End, (int)((double)this.Width / pictureBox2.PreferredSize.Width * pictureBox2.PreferredSize.Height));
            Curs += pictureBox2.Size.Height;

            //Текст
            label5.Location = new Point(Begin, Curs + Between);
            label5.Text = "Для более подробной информации смотрите пункт 6.";
            Curs += label5.Size.Height;

            //Заголовок.
            label6.Location = new Point(this.Width / 2 - label6.Size.Width / 2, Curs + Between);
            label6.Text = "Сохранение и загрузка";
            Curs += label6.Size.Height;

            //Картинка
            pictureBox3.Location = new Point(Begin, Curs + Between);
            pictureBox3.Size = new Size(this.Width - End, (int)((double)this.Width / pictureBox3.PreferredSize.Width * pictureBox3.PreferredSize.Height));
            Curs += pictureBox3.Size.Height;

            //Текст
            label7.Location = new Point(Begin, Curs + Between);
            label7.Text = "Для более подробной информации смотрите пункт 9.";
            Curs += label7.Size.Height;
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            if (vScrollBar1.Value + Shift < vScrollBar1.Maximum)
                vScrollBar1.Value += Shift;
            else vScrollBar1.Value = vScrollBar1.Maximum;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (vScrollBar1.Value - Shift > vScrollBar1.Minimum)
                vScrollBar1.Value -= Shift;
            else vScrollBar1.Value = vScrollBar1.Minimum;
        }
    }
}
