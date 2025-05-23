﻿namespace psdPH.Views.WeekView
{
    public class WeekCommand : CEDCommand
    {
        protected override bool IsEditableCommand(object parameter) => false;
        protected override void CreateExecuteCommand(object parameter)
        {
            var weekListData = (WeekListData)parameter;
            weekListData.NewWeek();
        }
        protected override void DeleteExecuteCommand(object parameter)
        {
            var weekData =(WeekData) parameter;
            weekData.WeekListData.Weeks.Remove(weekData);
        }
    }
}
