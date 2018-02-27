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
        public string Get_format()
        {
            return main_form.str_format_matrix;
        }
        //public List<string> Set_arrays()
        //{
        //    // return arr_format=get_empty_arrays(); передают вектор <string>
        //}
    }
}
