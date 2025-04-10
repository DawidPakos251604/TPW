using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    internal class ModelTable
    {
        private double _tableWidth;
        private double _tableHeight;

        public void SetCanvasSize(double tableWidth, double tableHeight)
        {
            _tableWidth = tableWidth;
            _tableHeight = tableHeight;
        }

        public double TableWidth => _tableWidth;
        public double TableHeight => _tableHeight;
    }
}
