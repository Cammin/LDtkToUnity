using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Meaf75.Unity{
    [Serializable]
    public class DateInfo{
        public int date;
        public int timeInSeconds;
    }

    [Serializable]
    public class MonthInfo{
        public int month;
        public List<DateInfo> dates;
    }

    [Serializable]
    public class YearInfo{
        public int year;
        public List<MonthInfo> months;
    }

    [Serializable]
    public class TimeRecorderInfo {
        public List<YearInfo> years;
        /// <summary> Worked time in seconds </summary>
        public long totalRecordedTime;
    }

    [Serializable]
    public class TimeTrackerWindowData{
        /// <summary> This variable cannot be serialized by the JsonUtility </summary>
        public DateTime selectedDate;
        public long selectedDateTicks;

        public TimeTrackerWindowData(){
            selectedDate = DateTime.Now;
        }

        public string GetJson(){
            selectedDateTicks = selectedDate.Ticks;
            return JsonUtility.ToJson(this);
        }
    }

    public static class TimeRecorderExtras{
        public const string TIME_RECORDER_REGISTRY = "time_recorder_registry";
        public const string NEXT_SAVE_TIME_PREF = "next_save_time_recorder";

        public const string CORRUPTED_JSON_BACKUP = "corrupted_time_recorder_json_{0}.json";
        public const string TIME_RECORDER_WINDOW_P_PREF = "time_recorder_window_player_pref";

        public const string TIME_RECORDER_PAUSE_P_PREF = "time_recorder_pause";

        public static string GetPauseButtonLabelForState(bool paused) {
            return  paused ? "Resume ▶" : "Pause ▯▯";
        }
    }
}

