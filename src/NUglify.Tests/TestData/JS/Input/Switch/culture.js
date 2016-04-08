// if we change the culture, it BETTER not change the decimal representation!
var foo = 3.1415927;

// replace all the culture tokens with current-culture values
define("dateFormat.tokens", {
    monthNames: %CurrentCulture.DateTimeFormat.MonthNames%,
    monthShortNames: %CurrentCulture.DateTimeFormat.AbbreviatedMonthNames%,
    dayNames: %CurrentCulture.DateTimeFormat.DayNames%,
    dayShortNames: %CurrentCulture.DateTimeFormat.AbbreviatedDayNames%, 
    ampm: ["%CurrentCulture.DateTimeFormat.AMDesignator%", "%CurrentCulture.DateTimeFormat.PMDesignator%"],
    timeSeparator: "%CurrentCulture.DateTimeFormat.TimeSeparator%",
    dateSeparator: "%CurrentCulture.DateTimeFormat.DateSeparator%",
    decimalPoint: "%CurrentCulture.NumberFormat.NumberDecimalSeparator%",
    percentSymbol: "%CurrentCulture.NumberFormat.PercentSymbol%",
});
