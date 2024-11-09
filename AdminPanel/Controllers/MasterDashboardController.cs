using Microsoft.AspNetCore.Mvc;

namespace  AdminPanel.Controllers
{
   
    public class MasterDashboardController : Controller
    {
        public IActionResult Index()
        {
            var monthlyData = new List<MonthlyData>
        {
            new MonthlyData { Month = "Jan", Import = 30.5, Export = 28.0 },
            new MonthlyData { Month = "Feb", Import = 32.0, Export = 30.5 },
            new MonthlyData { Month = "Mar", Import = 34.5, Export = 33.0 },
            new MonthlyData { Month = "Apr", Import = 31.0, Export = 29.5 },
            new MonthlyData { Month = "May", Import = 35.0, Export = 32.0 },
            new MonthlyData { Month = "Jun", Import = 33.5, Export = 31.0 },
            new MonthlyData { Month = "Jul", Import = 36.0, Export = 34.5 },
            new MonthlyData { Month = "Aug", Import = 34.5, Export = 32.5 },
            new MonthlyData { Month = "Sep", Import = 33.0, Export = 31.5 },
            new MonthlyData { Month = "Oct", Import = 32.5, Export = 30.0 },
            new MonthlyData { Month = "Nov", Import = 31.0, Export = 29.0 },
            new MonthlyData { Month = "Dec", Import = 30.0, Export = 28.5 }
        };

            ViewBag.MonthlyData = monthlyData;
            var pieData = new List<PieChartData>
        {
            new PieChartData { Product = "MBL", Percentage = 45 },
            new PieChartData { Product = "OBL", Percentage = 30 }

        };

            ViewBag.pieSource = pieData;

            // Optional: Define color palettes if you want custom colors
            var palettes = new[] { "#FF5733", "#33FF57", "#3357FF" };
            ViewBag.palettes = palettes;
            List<AxisLabelData> chartData = new List<AxisLabelData>
            {
                new AxisLabelData { x= "South Korea", y= 39.4 },
                new AxisLabelData { x= "India", y= 61.3 },
                new AxisLabelData { x= "Pakistan", y= 20.4 },
                new AxisLabelData { x= "Germany", y= 65.1 },
                new AxisLabelData { x= "Australia", y= 15.8 },
                new AxisLabelData { x= "Italy", y= 29.2 },
                new AxisLabelData { x= "United Kingdom", y= 44.6 },
                new AxisLabelData { x= "Saudi Arabia", y= 9.7 },
                new AxisLabelData { x= "Russia", y= 40.8 },
                new AxisLabelData { x= "Mexico", y= 31 },
                new AxisLabelData { x= "Brazil", y= 75.9 },
                new AxisLabelData { x= "China", y= 51.4 }
            };
            ViewBag.dataSource = chartData;

            List<PieDataPoints> PieChartPoints = new List<PieDataPoints>
            {
                new PieDataPoints { ExpenseCategory =  "Ocean Import BL", ExpensePercentage = 6.12, legendName="Ocean Import BL", DataLabelMappingName = "6.12%" },
                new PieDataPoints { ExpenseCategory =  "Ocean Export BL", ExpensePercentage = 57.28, legendName="Ocean Export BL", DataLabelMappingName = "57.28%" },
                new PieDataPoints { ExpenseCategory =  "Land Export BL", ExpensePercentage = 4.73, legendName="Land Export BL", DataLabelMappingName = "4.73%" },
                new PieDataPoints { ExpenseCategory =  "Air Export BL", ExpensePercentage = 5.96, legendName="Air Export BL", DataLabelMappingName = "5.96%" },
                new PieDataPoints { ExpenseCategory =  "Air Import BL", ExpensePercentage = 4.37, legendName="Air Import BL", DataLabelMappingName = "4.37%" },
                new PieDataPoints { ExpenseCategory =  "Land Import BL", ExpensePercentage = 7.48, legendName="Land Import BL", DataLabelMappingName = "7.48%" },

            };
            ViewBag.PieChartPoints = PieChartPoints;
            return View();
        }
        public class PieDataPoints
        {
            public string ExpenseCategory;
            public double ExpensePercentage;
            public string legendName;
            public string DataLabelMappingName;
        }
        public class AxisLabelData
        {
            public string x;
            public double y;
        }
        public class PieWithLegendChartData
        {
            public string X;
            public double Y;
            public string Text;
        }
        public class MonthlyData
        {
            public string Month { get; set; }  // Month name or abbreviation
            public double Import { get; set; } // Monthly import value
            public double Export { get; set; } // Monthly export value
        }

        public class PieChartData
        {
            public string Product { get; set; }
            public double Percentage { get; set; }
        }
    }
}
