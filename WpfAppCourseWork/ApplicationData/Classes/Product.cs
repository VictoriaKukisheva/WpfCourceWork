using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppCourseWork.ApplicationData
{
    public partial class Product
    {
        public string CorrectImage
        {
            get
            {
                if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Resourses\\" + Image))
                {
                    return "\\Resourses\\" + Image;
                }
                else
                {
                    return "\\Resourses\\picture.jpg";
                }
            }
        }
    }
}
