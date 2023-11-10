using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskManagerRemake.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitProcessesTab();
        }

        public void InitProcessesTab()
        {
            Process[] allProcesses = Process.GetProcesses();

            // FlowDocument tableParent = (FlowDocument)this.FindName("TableParent");
            Table processesTable = new Table();
            this.TableParent.Blocks.Add(processesTable);

            processesTable.CellSpacing = 10;
            processesTable.Background = Brushes.White;

            // Add columns
            for (int i = 0; i < 4; i++)
            {
                processesTable.Columns.Add(new TableColumn());
            }
            
            // Add header row
            processesTable.RowGroups.Add(new TableRowGroup());
            processesTable.RowGroups[0].Rows.Add(new TableRow());
            
            TableRow headerRow = processesTable.RowGroups[0].Rows[0];
            headerRow.Background = Brushes.Silver;
            headerRow.FontSize = 20;

            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Name"))));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Id"))));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Paged Mem Size"))));

            // Add data rows to the table
            for (int i = 0; i < allProcesses.Length; i++)
            {
                Process process = allProcesses[i];

                processesTable.RowGroups[0].Rows.Add(new TableRow());
                TableRow currentRow = processesTable.RowGroups[0].Rows[i + 1]; // minus header row
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run($"{process.ProcessName}"))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run($"{process.Id}"))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run($"{process.PagedMemorySize64}"))));
            }
        }
    }
}
