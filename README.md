# TaskManagerRemake
This is a work in progress. Currently this project has enough data and functionality to address most roadblocks and get a basic app strucuture.

## Main Tabs
- Processes: displays a list of all process in the simplest table possible. Currently processes are not grouped/categorized.
- Performance: this tab got the most work. It shows most of the information found in the original Task Manager, only for the CPU and Memory tabs.
- Users: empty at the moment.

## Performance Tabs
This tab is implemented using the master-detail pattern. The `PerformanceViewModel` is in charge of instantiating a list of all the `PerformanceItemDisplay`, one for each tab. This class is a standard class for all performance items, calling the constructor of each tab which implements `IPerformanceItem`.
This interface holds a method for each of the common elements found on the original task manager: tab title, tab specs, chart data, dynamic stats, static stats.

So far, with the implemented tabs (CPU & Memory) the data was found on WMI APIs or using Performance Counters.

### CPU
This tab requires a few calls to multiple WMI APIs, and using `Thread.Sleep` for some `PerformanceCounter` instances is challenging down the line with the timer running in the ViewModel to update the UI.
### Memory:
This tab requires a lot more `PerformanceCounter` than WMI calls. The most challenging part was understanding what each number meant and get the correct values.

## Remaining Items in No Particular Order:
- Error handling
- Add the rest of the performance tabs
- Remove tooltips from chart
- Add tiny charts on performance items thumbnails
- Add the rest of the main tabs
- Remove mouseover effect on ListViewItems/ template
- Add styles to Processes tab
- Categorize and group processes
- Add collapsible items to processes tab
- Add run task bar
- Add link to Resource monitor

## References
PerformanceCounter: http://zamov.online.fr/EXHTML/CSharp/CSharp_927308.html