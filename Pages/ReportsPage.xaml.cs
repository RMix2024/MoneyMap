namespace MoneyMap.Pages
    {
    public partial class ReportsPage : ContentPage
        {
        public ReportsPage()
            {
            InitializeComponent();
            TimeRangePicker.SelectedIndex = 0; // Default to "This Month"
            DrawCharts("This Month");
            }

        private void OnTimeRangeChanged(object sender, EventArgs e)
            {
            if (TimeRangePicker.SelectedIndex != -1)
                {
                string selectedRange = TimeRangePicker.SelectedItem.ToString();
                DrawCharts(selectedRange);
                }
            }

        private void DrawCharts(string timeRange)
            {
            // Simulated Data for Income vs Expenses
            List<float> incomeData = GetIncomeData(timeRange);
            List<float> expenseData = GetExpenseData(timeRange);

            // Draw Income vs Expenses
            IncomeExpenseChart.Drawable = new BarChartDrawable(incomeData, expenseData);

            // Draw Expense Breakdown
            List<float> categoryExpenses = GetCategoryExpenses(timeRange);
            ExpenseBreakdownChart.Drawable = new PieChartDrawable(categoryExpenses);

            // Draw Monthly Trends
            List<float> monthlyTrends = GetMonthlyTrends(timeRange);
            MonthlyTrendsChart.Drawable = new LineChartDrawable(monthlyTrends);
            }

        private List<float> GetIncomeData(string range) => new List<float> { 2000, 2500, 2700, 3000, 2800 };
        private List<float> GetExpenseData(string range) => new List<float> { 1800, 1900, 2200, 2400, 2300 };
        private List<float> GetCategoryExpenses(string range) => new List<float> { 500, 800, 400, 600 };
        private List<float> GetMonthlyTrends(string range) => new List<float> { 1500, 1600, 1800, 2000, 2100 };
        }

    // Custom Chart Drawables
    public class BarChartDrawable : IDrawable
        {
        private List<float> _income;
        private List<float> _expense;

        public BarChartDrawable(List<float> income, List<float> expense)
            {
            _income = income;
            _expense = expense;
            }

        public void Draw(ICanvas canvas, RectF dirtyRect)
            {
            canvas.FillColor = Colors.Green;
            for (int i = 0; i < _income.Count; i++)
                canvas.FillRectangle(i * 50 + 20, dirtyRect.Height - _income[i], 30, _income[i]);

            canvas.FillColor = Colors.Red;
            for (int i = 0; i < _expense.Count; i++)
                canvas.FillRectangle(i * 50 + 60, dirtyRect.Height - _expense[i], 30, _expense[i]);
            }
        }

    public class PieChartDrawable : IDrawable
        {
        private List<float> _values;

        public PieChartDrawable(List<float> values)
            {
            _values = values;
            }

        public void Draw(ICanvas canvas, RectF dirtyRect)
            {
            float total = 0;
            foreach (var v in _values) total += v;

            float startAngle = 0;
            float[] colors = { 0xFF5733, 0x33FF57, 0x3357FF, 0xFF33A1 };

            for (int i = 0; i < _values.Count; i++)
                {
                float sweepAngle = (_values[i] / total) * 360;
                canvas.FillColor = Color.FromUint((uint)colors[i % colors.Length]);
                canvas.FillArc(dirtyRect, startAngle, sweepAngle, true);
                startAngle += sweepAngle;
                }
            }
        }

    public class LineChartDrawable : IDrawable
        {
        private List<float> _values;

        public LineChartDrawable(List<float> values)
            {
            _values = values;
            }

        public void Draw(ICanvas canvas, RectF dirtyRect)
            {
            canvas.StrokeColor = Colors.Blue;
            canvas.StrokeSize = 3;
            for (int i = 0; i < _values.Count - 1; i++)
                {
                canvas.DrawLine(i * 50 + 20, dirtyRect.Height - _values[i],
                                (i + 1) * 50 + 20, dirtyRect.Height - _values[i + 1]);
                }
            }
        }
    }
