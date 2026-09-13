using System;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Result from WerDateRange selection.
    /// StartDate is always today (current date).
    /// EndDate is the calculated past date based on the selected range.
    /// </summary>
    public class DateRangeResult
    {
        /// <summary>Current date (today).</summary>
        public DateTime StartDate { get; }

        /// <summary>Calculated past date based on selected range.</summary>
        public DateTime EndDate { get; }

        public DateRangeResult(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public override string ToString()
        {
            return EndDate.ToString("MMM. dd, yyyy") + " — " + StartDate.ToString("MMM. dd, yyyy");
        }
    }
}
