using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project
{
    class factory
    {
        List<string> arr_format;
        Form1 main_form;
        Form2 form_array;

        // Мы передвем Форматам матриц тип формата
        public string Get_format()
        {
            return main_form.str_format_matrix;
        }

        // Формат матриц передает нам имена массивов в вектор <string>
        //public List<string> Set_arrays()
        //{
        //    // return arr_format=get_empty_arrays(); передают вектор <string>
        //}

        // Мы передаем матрицам : название массива, путь к файлу
        public Dictionary<string,string> Get_arrays()
        {
            return form_array.filenames_format;
        }
    }
}
