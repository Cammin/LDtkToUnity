using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Meaf75.Unity{

    public class TimeRecorderWindow : EditorWindow, IHasCustomMenu{

        private static DateTime nextRepaint;

        private static DateTime selectedDate;

        public static TimeRecorderWindow Instance;

        TimeRecorderInfo info;
		
		private static readonly Vector2 windowSize = new Vector2(563,560);

        [MenuItem("Tools/Time recorder/Time Calendar")]
        static void Init(){

            var runningStateLabel = TimeRecorder.isPaused ? "Paused" : "Running";

            // Get existing open window or if none, make a new one:
            var window = GetWindow<TimeRecorderWindow>();
            window.titleContent = new GUIContent($"Time recorder ({runningStateLabel})");
			
			window.minSize = windowSize;
            window.maxSize = windowSize;

            selectedDate = DateTime.Now;
        }

        private void OnEnable(){
            Instance = this;
            selectedDate = DateTime.Now;
            DrawWindow();
        }

        // This interface implementation is automatically called by Unity.
        void IHasCustomMenu.AddItemsToMenu(GenericMenu menu){
            GUIContent content = new GUIContent("Repaint");
            menu.AddItem(content, false, RepaintWindow);

            GUIContent content2 = new GUIContent("RepaintOnlyFew");
            menu.AddItem(content2, false, RepaintOnlyFew);
        }

        void RepaintOnlyFew(){
            VisualElement root = rootVisualElement;
            var totalDevLabel = root.Q<Label>("label-total-dev-time");
            totalDevLabel.text = "Test repaint";

            Debug.Log("seted");
        }

        private void DrawWindow(){

            VisualElement root = rootVisualElement;
            info = TimeRecorder.LoadTimeRecorderInfoFromRegistry();

            var timeRecorderTemplate = Resources.Load<VisualTreeAsset>("CalendarTemplate");
            var timeRecorderTemplateStyle = Resources.Load<StyleSheet>("CalendarTemplateStyle");
            root.styleSheets.Add(timeRecorderTemplateStyle);

            var dayElementTemplate = Resources.Load<VisualTreeAsset>("DayContainerTemplate");
            var dayElementTemplateStyle = Resources.Load<StyleSheet>("DayContainerTemplateStyle");
            root.styleSheets.Add(dayElementTemplateStyle);

            // Add tree to root element
            timeRecorderTemplate.CloneTree(root);

            // Update date label
            var dateLabel = root.Q<Label>("label-date");
            dateLabel.text = $"{selectedDate.Day}-{selectedDate.Month}-{selectedDate.Year}";

            // Fix buttons action
            var prevMonthBtn = root.Q<Button>("btn-prev-month");
            var nextMonthBtn = root.Q<Button>("btn-next-month");
            var timeRecorderPauseStateBtn = root.Q<Button>("time-recorder-state-btn");

            prevMonthBtn.clicked += () => ChangeMonthOffset(-1);
            nextMonthBtn.clicked += () => ChangeMonthOffset(1);
            timeRecorderPauseStateBtn.clicked += ChangeTimeRecorderPauseState;

            timeRecorderPauseStateBtn.text = TimeRecorderExtras.GetPauseButtonLabelForState(TimeRecorder.isPaused).ToUpperInvariant();

            // Set total label dev time
            var totalDevLabel = root.Q<Label>("label-total-dev-time");
            totalDevLabel.text = GetLabel(info?.totalRecordedTime ?? 0);

            // Generate days
            var daysContainers = new VisualElement[7];

            // Get reference to all containers before generate elements
            for(int i = 0; i < 7; i++){
                var dayElement = root.Q<VisualElement>("day-"+i);
                daysContainers[i] = dayElement;
            }

            int daysGenerated = 0;

            #region Step 1: Fill previous month
            var firstDay = new DateTime(selectedDate.Year,selectedDate.Month,1);

            if(firstDay.DayOfWeek != DayOfWeek.Monday){
                // Get previous month
                var previousMonthDate = selectedDate.AddMonths(-1);
                previousMonthDate = new DateTime(previousMonthDate.Year,previousMonthDate.Month,1);
                int end = DateTime.DaysInMonth(previousMonthDate.Year, previousMonthDate.Month);
                int reduceDays = firstDay.DayOfWeek == DayOfWeek.Sunday ? 6 : ((int) firstDay.DayOfWeek - 1) % 7;
                int start = end - reduceDays;

                // Generate Elements
                FillDateToDate(daysContainers,previousMonthDate,start,end,ref daysGenerated,true);
            }
            #endregion

            #region Step 2: Fill selected month
                int daysInSelectedMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);

                FillDateToDate(daysContainers,selectedDate,0,daysInSelectedMonth,ref daysGenerated);
            #endregion

            #region Step 3: Fill remaining month days
                var selectedMonthFinalDate = new DateTime(selectedDate.Year, selectedDate.Month, daysInSelectedMonth);

                if(selectedMonthFinalDate.DayOfWeek != DayOfWeek.Sunday){
                    // In this calendar the final day is "Sunday"
                    // so if selected month ends in "Sunday" there is no need to fill remaining days

                    var nextMonth = selectedDate.AddMonths(1);
                    int remainingDays = 7 - ((int) selectedMonthFinalDate.DayOfWeek);

                    FillDateToDate(daysContainers,nextMonth,0,remainingDays, ref daysGenerated,true);
                }
            #endregion
        }

        void FillDateToDate(VisualElement[] daysContainers,DateTime dateSelected, int start, int end, ref int daysGenerated, bool emptyMode = false){

            var dayElementTemplate = Resources.Load<VisualTreeAsset>("DayContainerTemplate");

            var displayingYear = info?.years?.Find(y => y.year == selectedDate.Year);
            var displayingMonth = displayingYear?.months.Find(m => m.month == selectedDate.Month);

            // Generate Date elements
            for(int i = start; i < end; i++){
                var date = new DateTime(dateSelected.Year, dateSelected.Month, i + 1);

                var dayContainer = daysContainers[(int) date.DayOfWeek];

                var dayElement = dayElementTemplate.CloneTree();
                string hoursTxt = "";

                // Fill seleted month with sotred data
                if(displayingMonth != null && !emptyMode){
                    // Get & set worked day info
                    var dayInfoIdx = displayingMonth.dates.FindIndex(d => d.date == i + 1);

                    if(dayInfoIdx != -1){
                        // Set worked time
                        var dayInfo = displayingMonth.dates[dayInfoIdx];

                        hoursTxt = GetLabel(dayInfo.timeInSeconds);
                    }
                }

                var dayLabel = dayElement.Q<Label>("label-day");
                dayLabel.text = emptyMode ? "" : $"{i+1}";

                var hoursLabel = dayElement.Q<Label>("label-hours");
                hoursLabel.text = hoursTxt;

                dayContainer.Add(dayElement);

                daysGenerated++;
            }
        }

        /// <summary> Change month  </summary>
        /// <param name="offset">-1 or 1</param>
        void ChangeMonthOffset(int offset){
            selectedDate = selectedDate.AddMonths(offset);
            RepaintWindow();
        }

        public void RepaintWindow(){
//            Debug.Log("Repainteando ");
            VisualElement root = rootVisualElement;
            root.Clear();
            DrawWindow();
        }

        string GetLabel(long timeInSeconds){

            var timespan = TimeSpan.FromSeconds(timeInSeconds);

            // Check if worked time is less than a second
            if(timespan.TotalSeconds < 60)
                return (int) timespan.TotalSeconds + " sec";

            // Check if worked time is less than an hour
            if(timespan.TotalMinutes < 60)
                return (int) timespan.TotalMinutes + " min";

            return $"{(int) timespan.TotalHours} h\n{timespan.Minutes} min";
        }


        private void ChangeTimeRecorderPauseState() {
            TimeRecorderTools.ChangeTimeRecorderPauseState(!TimeRecorder.isPaused);
        }

        /// <summary> Update visual elements from this window </summary>
        /// <param name="paused">is time recorder paused?</param>
        public void UpdatePausedState(bool paused) {
            TimeRecorder.isPaused = paused;

            var runningStateLabel = TimeRecorder.isPaused ? "Paused" : "Running";

            var timeRecorderPauseStateBtn = rootVisualElement.Q<Button>("time-recorder-state-btn");
            timeRecorderPauseStateBtn.text = TimeRecorderExtras.GetPauseButtonLabelForState(TimeRecorder.isPaused).ToUpperInvariant();

            GetWindow<TimeRecorderWindow>().titleContent = new GUIContent($"Time recorder ({runningStateLabel})");
        }
    }
}
