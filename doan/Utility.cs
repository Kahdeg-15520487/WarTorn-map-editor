using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doan
{
    static class Utility
    {
        /// <summary>
        /// convert angle from degree to radian. use to ease process of drawing sprite
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float DegreeToRadian(float angle)
        {
            return (float)(Math.PI * angle / 180.0f);
        }
    }
}
