﻿// Copyright (c) 2013, Oracle and/or its affiliates. All rights reserved.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.Office.Core;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using Excel = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Provides extension methods and other static methods to leverage the work with Excel objects.
  /// </summary>
  public static class ExcelUtilities
  {
    #region Constants

    /// <summary>
    /// The default interior color for Excel cells committed to the MySQL server during an Edit Data operation.
    /// </summary>
    /// <remarks>Blue-ish.</remarks>
    public const string DEFAULT_COMMITED_CELLS_HTML_COLOR = "#7CC576";

    /// <summary>
    /// The default interior color for Excel cells that caused errors during a commit of an Edit Data operation.
    /// </summary>
    /// <remarks>Red-ish.</remarks>
    public const string DEFAULT_ERRORED_CELLS_HTML_COLOR = "#FF8282";

    /// <summary>
    /// The default interior color for Excel cells locked during an Edit Data operation (like the headers containing column names)..
    /// </summary>
    /// <remarks>Gray-ish</remarks>
    public const string DEFAULT_LOCKED_CELLS_HTML_COLOR = "#D7D7D7";

    /// <summary>
    /// The default name for the default MySQL style used for Excel tables.
    /// </summary>
    public const string DEFAULT_MYSQL_STYLE_NAME = "MySqlDefault";

    /// <summary>
    /// The default interior color for Excel cells accepting data from users to create a new row in the table during an Edit Data operation.
    /// </summary>
    /// <remarks>Yellow-ish.</remarks>
    public const string DEFAULT_NEW_ROW_CELLS_HTML_COLOR = "#FFFCC7";

    /// <summary>
    /// The default interior color for Excel cells containing values that have been changed by the user but not yet committed during an Edit Data operation.
    /// </summary>
    /// <remarks>Green-ish.</remarks>
    public const string DEFAULT_UNCOMMITTED_CELLS_HTML_COLOR = "#B8E5F7";

    /// <summary>
    /// The default interior color for Excel cells containing values that caused concurrency warnings during an Edit Data operation using optimistic updates.
    /// </summary>
    /// <remarks>Green-ish.</remarks>
    public const string DEFAULT_WARNING_CELLS_HTML_COLOR = "#FCC451";

    /// <summary>
    /// The interior color used to revert Excel cells to their original background color.
    /// </summary>
    public const int EMPTY_CELLS_OLE_COLOR = 0;

    #endregion Constants

    /// <summary>
    /// Initializes the <see cref="ExcelUtilities"/> class.
    /// </summary>
    static ExcelUtilities()
    {
      CommitedCellsHtmlColor = DEFAULT_COMMITED_CELLS_HTML_COLOR;
      ErroredCellsHtmlColor = DEFAULT_ERRORED_CELLS_HTML_COLOR;
      LockedCellsHtmlColor = DEFAULT_LOCKED_CELLS_HTML_COLOR;
      NewRowCellsHtmlColor = DEFAULT_NEW_ROW_CELLS_HTML_COLOR;
      UncommittedCellsHtmlColor = DEFAULT_UNCOMMITTED_CELLS_HTML_COLOR;
      WarningCellsHtmlColor = DEFAULT_WARNING_CELLS_HTML_COLOR;
    }

    #region Properties

    /// <summary>
    /// Gets the interior color for Excel cells committed to the MySQL server during an Edit Data operation.
    /// </summary>
    public static int CommitedCellsOleColor { get; private set; }

    /// <summary>
    /// Gets or sets the interior color for Excel cells committed to the MySQL server during an Edit Data operation.
    /// </summary>
    public static string CommitedCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(CommitedCellsOleColor));
      }

      set
      {
        CommitedCellsOleColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    /// <summary>
    /// Gets the interior color for Excel cells that caused errors during a commit of an Edit Data operation.
    /// </summary>
    public static int ErroredCellsOleColor { get; private set; }

    /// <summary>
    /// Gets or sets the interior color for Excel cells that caused errors during a commit of an Edit Data operation.
    /// </summary>
    public static string ErroredCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(ErroredCellsOleColor));
      }

      set
      {
        ErroredCellsOleColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    /// <summary>
    /// Gets the default interior color for Excel cells locked during an Edit Data operation (like the headers containing column names).
    /// </summary>
    public static int LockedCellsOleColor { get; private set; }

    /// <summary>
    /// Gets or sets the interior color for Excel cells locked during an Edit Data operation (like the headers containing column names).
    /// </summary>
    public static string LockedCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(LockedCellsOleColor));
      }

      set
      {
        LockedCellsOleColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    /// <summary>
    /// Gets the interior color for Excel cells accepting data from users to create a new row in the table during an Edit Data operation.
    /// </summary>
    public static int NewRowCellsOleColor { get; private set; }

    /// <summary>
    /// Gets or sets the interior color for Excel cells accepting data from users to create a new row in the table during an Edit Data operation.
    /// </summary>
    public static string NewRowCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(NewRowCellsOleColor));
      }

      set
      {
        NewRowCellsOleColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    /// <summary>
    /// Gets the interior color for Excel cells containing values that have been changed by the user but not yet committed during an Edit Data operation.
    /// </summary>
    public static int UncommittedCellsOleColor { get; private set; }

    /// <summary>
    /// Gets or sets the interior color for Excel cells containing values that have been changed by the user but not yet committed during an Edit Data operation.
    /// </summary>
    public static string UncommittedCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(UncommittedCellsOleColor));
      }

      set
      {
        UncommittedCellsOleColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    /// <summary>
    /// Gets the interior color for Excel cells containing values that caused concurrency warnings during an Edit Data operation using optimistic updates.
    /// </summary>
    public static int WarningCellsOleColor { get; private set; }

    /// <summary>
    /// Gets or sets the interior color for Excel cells containing values that caused concurrency warnings during an Edit Data operation using optimistic updates.
    /// </summary>
    public static string WarningCellsHtmlColor
    {
      get
      {
        return ColorTranslator.ToHtml(ColorTranslator.FromOle(WarningCellsOleColor));
      }

      set
      {
        WarningCellsOleColor = ColorTranslator.ToOle(ColorTranslator.FromHtml(value));
      }
    }

    #endregion Properties

    /// <summary>
    /// Adds a new row at the bottom of the given Excel range.
    /// </summary>
    /// <param name="range">The Excel range to add a new row to the end of it.</param>
    /// <param name="clearLastRowColoring">Flag indicating whether the previous row that was placeholder for new rows is cleared of its formatting.</param>
    /// <param name="newRowRange">An Excel range containing just the newly added row if <see cref="clearLastRowColoring"/> is <c>true</c>, or containing the row above the newly added one otherwise.</param>
    /// <returns>The original Excel range with the newly added row at the end of it.</returns>
    public static Excel.Range AddNewRow(this Excel.Range range, bool clearLastRowColoring, out Excel.Range newRowRange)
    {
      newRowRange = null;
      if (range == null)
      {
        return null;
      }

      range = range.Resize[range.Rows.Count + 1, range.Columns.Count];
      newRowRange = range.Rows[range.Rows.Count] as Excel.Range;
      if (newRowRange != null)
      {
        newRowRange.Interior.Color = NewRowCellsOleColor;
      }

      if (!clearLastRowColoring || range.Rows.Count <= 0)
      {
        return range;
      }

      newRowRange = range.Rows[range.Rows.Count - 1] as Excel.Range;
      if (newRowRange != null)
      {
        newRowRange.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
      }

      return range;
    }

    /// <summary>
    /// Checks if the given <see cref="Excel.Range"/> contains data in any of its cells.
    /// </summary>
    /// <param name="range">An excel range.</param>
    /// <returns><c>true</c> if the given range is not empty, <c>false</c> otherwise.</returns>
    public static bool ContainsAnyData(this Excel.Range range)
    {
      if (range == null || range.Count < 1)
      {
        return false;
      }

      bool hasData = false;
      if (range.Count == 1)
      {
        hasData = range.Value2 != null;
      }
      else if (range.Count > 1)
      {
        object[,] values = range.Value2;
        if (values == null)
        {
          return false;
        }

        foreach (object o in values)
        {
          if (o == null)
          {
            continue;
          }

          hasData = true;
          break;
        }
      }

      return hasData;
    }

    /// <summary>
    /// Creates a default <see cref="Excel.TableStyle"/> for MySQL imported data.
    /// </summary>
    /// <param name="workbook">The workbook where the new <see cref="Excel.Style"/> is added to.</param>
    /// <returns>A new <see cref="Excel.TableStyle"/> for MySQL imported data.</returns>
    public static Excel.TableStyle CreateMySqlTableStyle(this Excel.Workbook workbook)
    {
      if (workbook == null || workbook.TableStyles.Cast<Excel.TableStyle>().Any(style => style.Name == DEFAULT_MYSQL_STYLE_NAME))
      {
        return null;
      }

      Excel.TableStyle mySqlTableStyle = workbook.TableStyles.Add(DEFAULT_MYSQL_STYLE_NAME);
      mySqlTableStyle.ShowAsAvailableTableStyle = false;
      mySqlTableStyle.TableStyleElements[Excel.XlTableStyleElementType.xlWholeTable].SetAsMySqlStyle();
      mySqlTableStyle.TableStyleElements[Excel.XlTableStyleElementType.xlHeaderRow].SetAsMySqlStyle(LockedCellsOleColor, true);
      return mySqlTableStyle;
    }

    /// <summary>
    /// Gets a <see cref="Excel.Worksheet"/> with a given name existing in the given <see cref="Excel.Workbook"/> or creates a new one.
    /// </summary>
    /// <param name="workBook">The <see cref="Excel.Workbook"/> to look for a <see cref="Excel.Worksheet"/>.</param>
    /// <param name="workSheetName">The name of the new <see cref="Excel.Worksheet"/>.</param>
    /// <param name="selectTopLeftCell">Flag indicating whether the cell A1 receives focus.</param>
    /// <returns>The existing or new <see cref="Excel.Worksheet"/> object.</returns>
    public static Excel.Worksheet CreateWorksheet(this Excel.Workbook workBook, string workSheetName, bool selectTopLeftCell)
    {
      if (workBook == null)
      {
        return null;
      }

      Excel.Worksheet newWorksheet = null;
      try
      {
        newWorksheet = workBook.Worksheets.Add(Type.Missing, workBook.ActiveSheet, Type.Missing, Type.Missing);
        newWorksheet.Name = workBook.GetWorksheetNameAvoidingDuplicates(workSheetName);

        if (selectTopLeftCell)
        {
          newWorksheet.SelectTopLeftCell();
        }
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.WorksheetCreationErrorText, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }

      return newWorksheet;
    }

    /// <summary>
    /// Returns an Excel range with the first row cells corresponding to the column names.
    /// </summary>
    /// <param name="mysqlDataRange">If <c>null</c> the whole first row is returned, otherwise only the column cells within the editing range.</param>
    /// <returns>The Excel range with the first row cells corresponding to the column names</returns>
    public static Excel.Range GetColumnNamesRange(this Excel.Range mysqlDataRange)
    {
      return mysqlDataRange == null ? null : mysqlDataRange.Resize[1, mysqlDataRange.Columns.Count];
    }

    /// <summary>
    /// Analyzes an Excel range to detect if each column within contains any data.
    /// </summary>
    /// <param name="range">The Excel range whose columns are analyzed for data presence.</param>
    /// <param name="prependPkColumn">Flag indicating if a column representing a primary key is prepended to the list of columns from the Excel of data.</param>
    /// <returns>A list of boolean values for each of the columns in the Excel range representing if the column has data or not.</returns>
    public static List<bool> GetColumnsWithDataInfoList(this Excel.Range range, bool prependPkColumn = false)
    {
      if (range == null)
      {
        return null;
      }

      var excelData = range.ToBidimensionalArray();
      int numCols = excelData.GetUpperBound(1);
      int numRows = excelData.GetUpperBound(0);
      List<bool> columnsHaveAnyDataList = new List<bool>(numCols + 1);
      if (prependPkColumn)
      {
        columnsHaveAnyDataList.Add(true);
      }

      for (int colIdx = 1; colIdx <= numCols; colIdx++)
      {
        bool colHasAnyData = false;
        for (int rowIdx = 1; rowIdx <= numRows; rowIdx++)
        {
          if (excelData[rowIdx, colIdx] == null)
          {
            continue;
          }

          colHasAnyData = true;
          break;
        }

        columnsHaveAnyDataList.Add(colHasAnyData);
      }

      return columnsHaveAnyDataList;
    }

    /// <summary>
    /// Gets the active <see cref="Excel.Worksheet"/> existing in the given <see cref="Excel.Workbook"/> or creates a new one.
    /// </summary>
    /// <param name="workBook">The <see cref="Excel.Workbook"/> to look for a <see cref="Excel.Worksheet"/>.</param>
    /// <param name="worksheetName">The name of the new <see cref="Excel.Worksheet"/>.</param>
    /// <param name="selectTopLeftCell">Flag indicating whether the cell A1 receives focus.</param>
    /// <returns>The existing or new <see cref="Excel.Worksheet"/> object.</returns>
    public static Excel.Worksheet GetActiveOrCreateWorksheet(this Excel.Workbook workBook, string worksheetName, bool selectTopLeftCell)
    {
      Excel.Worksheet activeWorksheet;
      if (workBook != null)
      {
        activeWorksheet = workBook.ActiveSheet as Excel.Worksheet;
        if (activeWorksheet != null)
        {
          return activeWorksheet;
        }

        worksheetName = workBook.GetWorksheetNameAvoidingDuplicates(worksheetName);
      }
      else
      {
        workBook = Globals.ThisAddIn.Application.Workbooks.Add(Type.Missing);
      }

      activeWorksheet = workBook.CreateWorksheet(worksheetName, selectTopLeftCell);
      return activeWorksheet;
    }

    /// <summary>
    /// Gets a <see cref="Excel.Worksheet"/> with a given name existing in the given <see cref="Excel.Workbook"/> or creates a new one.
    /// </summary>
    /// <param name="workbook">The <see cref="Excel.Workbook"/> to look for a <see cref="Excel.Worksheet"/>.</param>
    /// <param name="workSheetName">The name of the new <see cref="Excel.Worksheet"/>.</param>
    /// <param name="selectTopLeftCell">Flag indicating whether the cell A1 receives focus.</param>
    /// <returns>The existing or new <see cref="Excel.Worksheet"/> object.</returns>
    public static Excel.Worksheet GetOrCreateWorksheet(this Excel.Workbook workbook, string workSheetName, bool selectTopLeftCell)
    {
      if (workbook == null)
      {
        return null;
      }

      Excel.Worksheet existingWorksheet = workbook.Worksheets.Cast<Excel.Worksheet>().FirstOrDefault(worksheet => string.Equals(worksheet.Name, workSheetName, StringComparison.InvariantCulture));
      if (existingWorksheet == null)
      {
        workbook.CreateWorksheet(workSheetName, selectTopLeftCell);
      }
      else if (selectTopLeftCell)
      {
        existingWorksheet.SelectTopLeftCell();
      }

      return existingWorksheet;
    }

    /// <summary>
    /// Gets the name of the parent <see cref="Excel.Workbook"/> of the given <see cref="Excel.Worksheet"/>.
    /// </summary>
    /// <param name="worksheet">An <see cref="Excel.Worksheet"/> object.</param>
    /// <returns>The name of the parent <see cref="Excel.Workbook"/>.</returns>
    public static string GetParentWorkbookName(this Excel.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return string.Empty;
      }

      Excel.Workbook parentWorkbook = worksheet.Parent as Excel.Workbook;
      return parentWorkbook != null ? parentWorkbook.Name : string.Empty;
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="Excel.ListObject"/> that avoids duplicates with existing ones in the current <see cref="Excel.Worksheet"/>.
    /// </summary>
    /// <param name="tableName">The proposed name for a <see cref="Excel.ListObject"/>.</param>
    /// <returns>A <see cref="Excel.ListObject"/> valid name.</returns>
    public static string GetTableNameAvoidingDuplicates(this string tableName)
    {
      return tableName.GetTableNameAvoidingDuplicates(1);
    }

    /// <summary>
    /// Gets the active workbook unique identifier if exists, if not, creates one and returns it.
    /// </summary>
    /// <returns>The guid string for the current workbook.</returns>
    public static string GetOrCreateId(this Excel.Workbook workbook)
    {
      if (workbook == null || workbook.CustomDocumentProperties == null)
      {
        return null;
      }

      DocumentProperty guid = ((DocumentProperties)workbook.CustomDocumentProperties).Cast<DocumentProperty>().FirstOrDefault(property => property.Name.Equals("WorkbookGuid"));
      if (guid != null)
      {
        return guid.Value.ToString();
      }

      string newGuid = Guid.NewGuid().ToString();
      DocumentProperties properties = workbook.CustomDocumentProperties;
      properties.Add("WorkbookGuid", false, MsoDocProperties.msoPropertyTypeString, newGuid);
      return newGuid;
    }

    /// <summary>
    /// Gets the a protection key for the provided worksheet if exists.
    /// </summary>
    /// <returns>The worksheet's protection key if the property exist, otherwise returns null.</returns>
    public static string GetProtectionKey(this Excel.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return null;
      }

      Excel.CustomProperties properties = worksheet.CustomProperties;
      if (properties == null)
      {
        return null;
      }

      Excel.CustomProperty guid = properties.Cast<Excel.CustomProperty>().FirstOrDefault(property => property.Name.Equals("WorksheetkGuid"));
      return guid == null ? null : guid.Value.ToString();
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="Excel.Worksheet"/> that avoids duplicates with existing ones in the given <see cref="Excel.Workbook"/>.
    /// </summary>
    /// <param name="workbook">A <see cref="Excel.Workbook"/>.</param>
    /// <param name="worksheetName">The proposed name for a <see cref="Excel.Worksheet"/>.</param>
    /// <returns>A <see cref="Excel.Worksheet"/> valid name.</returns>
    public static string GetWorksheetNameAvoidingDuplicates(this Excel.Workbook workbook, string worksheetName)
    {
      return workbook.GetWorksheetNameAvoidingDuplicates(worksheetName, 0);
    }

    /// <summary>
    /// Returns a Range object that represents the rectangular intersection of the given range with another range.
    /// </summary>
    /// <param name="range">The given Excel range.</param>
    /// <param name="otherRange">The intersecting Excel range.</param>
    /// <returns>The rectangular intersection of the given range with another range.</returns>
    public static Excel.Range IntersectWith(this Excel.Range range, Excel.Range otherRange)
    {
      return Globals.ThisAddIn.Application.Intersect(range, otherRange);
    }

    /// <summary>
    /// Returns a list of <see cref="Excel.TableStyle"/> names available to be used within the given <see cref="Excel.Workbook"/>.
    /// </summary>
    /// <param name="workbook">An <see cref="Excel.Workbook"/>.</param>
    /// <returns>A list of style names available in the given <see cref="Excel.Workbook"/>.</returns>
    public static List<string> ListTableStyles(this Excel.Workbook workbook)
    {
      return workbook == null ? null : (from Excel.TableStyle tableStyle in workbook.TableStyles orderby tableStyle.Name select tableStyle.Name).ToList();
    }

    /// <summary>
    /// Locks the given Excel range and sets its fill color accordingly.
    /// </summary>
    /// <param name="range">The Excel range to lock or unlock.</param>
    /// <param name="lockRange">Flag indicating whether the Excel range is locked or unlocked.</param>
    public static void LockRange(this Excel.Range range, bool lockRange)
    {
      if (lockRange)
      {
        range.Interior.Color = LockedCellsOleColor;
      }
      else
      {
        range.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
      }

      range.Locked = lockRange;
    }

    /// <summary>
    /// Unprotects the given Excel worksheet and stops listening for its Change event.
    /// </summary>
    /// <param name="worksheet">The Excel worksheet to unprotect.</param>
    /// <param name="changeEventHandlerDelegate">The change event handler delegate of the Excel worksheet.</param>
    /// <param name="protectionKey">The key used to unprotect the worksheet.</param>
    /// <param name="mysqlDataRange">The Excel range containing the MySQL data being edited.</param>
    public static void ProtectEditingWorksheet(this Excel.Worksheet worksheet, Excel.DocEvents_ChangeEventHandler changeEventHandlerDelegate, string protectionKey, Excel.Range mysqlDataRange)
    {
      if (worksheet == null)
      {
        return;
      }

      if (changeEventHandlerDelegate != null)
      {
        worksheet.Change += changeEventHandlerDelegate;
      }

      if (mysqlDataRange != null)
      {
        Excel.Range extendedRange = mysqlDataRange.Range["A2"];
        extendedRange = extendedRange.Resize[mysqlDataRange.Rows.Count - 1, worksheet.Columns.Count];
        extendedRange.Locked = false;

        // Column names range code
        Excel.Range headersRange = mysqlDataRange.GetColumnNamesRange();
        headersRange.LockRange(true);
      }

      worksheet.Protect(protectionKey,
                        false,
                        true,
                        true,
                        true,
                        true,
                        true,
                        false,
                        false,
                        false,
                        false,
                        false,
                        true,
                        false,
                        false,
                        false);
    }

    /// <summary>
    /// Removes the protectionKey property (if exists) for the current worksheet.
    /// </summary>    
    public static void RemoveRemoveProtectionKey(this Excel.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return;
      }

      var protectionKeyProperty = worksheet.CustomProperties.Cast<Excel.CustomProperty>().FirstOrDefault(property => property.Name.Equals("WorksheetkGuid"));
      if (protectionKeyProperty != null)
      {
        protectionKeyProperty.Delete();
      }
    }

    /// <summary>
    /// Places the A1 cell of the given <see cref="Excel.Worksheet"/> in focus.
    /// </summary>
    /// <param name="worksheet">A <see cref="Excel.Worksheet"/>.</param>
    public static void SelectTopLeftCell(this Excel.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return;
      }

      Globals.ThisAddIn.Application.Goto(worksheet.Range["A1", Type.Missing], false);
    }

    /// <summary>
    /// Sets the font and color properties of a <see cref="Excel.TableStyleElement"/> as a MySQL minimalistic style.
    /// </summary>
    /// <param name="styleElement">The <see cref="Excel.TableStyleElement"/> to modify.</param>
    /// <param name="interiorOleColor">The OLE color to paint the Excel cells interior with.</param>
    /// <param name="makeBold">Flag indicating whether the font is set to bold.</param>
    public static void SetAsMySqlStyle(this Excel.TableStyleElement styleElement, int interiorOleColor = EMPTY_CELLS_OLE_COLOR, bool makeBold = false)
    {
      styleElement.Font.Color = ColorTranslator.ToOle(Color.Black);
      if (interiorOleColor == EMPTY_CELLS_OLE_COLOR)
      {
        styleElement.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
      }
      else
      {
        styleElement.Interior.Color = interiorOleColor;
      }

      styleElement.Font.Bold = makeBold;
    }

    /// <summary>
    /// Sets the range cells interior color to the specified OLE color.
    /// </summary>
    /// <param name="range">Excel range to have their interior color changed.</param>
    /// <param name="oleColor">The new interior color for the Excel cells.</param>
    public static void SetInteriorColor(this Excel.Range range, int oleColor)
    {
      if (range == null)
      {
        return;
      }

      if (oleColor > 0)
      {
        range.Interior.Color = oleColor;
      }
      else
      {
        range.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
      }
    }

    /// <summary>
    /// Sets the interior color of all the Excel ranges within the given list to the specified color.
    /// </summary>
    /// <param name="rangesList">The list of Excel ranges to have their fill color changed.</param>
    /// <param name="oleColor">The new fill color for the Excel cells.</param>
    public static void SetInteriorColor(this IList<Excel.Range> rangesList, int oleColor)
    {
      if (rangesList == null)
      {
        return;
      }

      foreach (var range in rangesList)
      {
        range.SetInteriorColor(oleColor);
      }

      rangesList.Clear();
    }

    /// <summary>
    /// Sets the protection key for the worksheet.
    /// </summary>
    /// <returns>The new protection key provided for the worksheet.</returns>
    public static bool StoreProtectionKey(this Excel.Worksheet worksheet, string protectionKey)
    {
      if (worksheet == null || string.IsNullOrEmpty(protectionKey))
      {
        return false;
      }

      var protectionKeyProperty = worksheet.CustomProperties.Cast<Excel.CustomProperty>().FirstOrDefault(property => property.Name.Equals("WorksheetkGuid"));
      if (protectionKeyProperty == null)
      {
        Excel.CustomProperties properties = worksheet.CustomProperties;
        properties.Add("WorksheetkGuid", protectionKey);
        return true;
      }
      protectionKeyProperty.Value = protectionKey;
      return true;
    }

    /// <summary>
    /// Converts an Excel data range to a bidimensional array of data objects.
    /// </summary>
    /// <param name="range">An Excel range to convert.</param>
    /// <param name="isFormatted">Flag indicating whether the data from Excel is formatted for dates or not.</param>
    /// <returns>A bidimensional arraty of objects representing the data in the Excel range.</returns>
    public static object[,] ToBidimensionalArray(this Excel.Range range, bool isFormatted = true)
    {
      if (range == null)
      {
        return null;
      }

      object[,] excelData;
      if (range.Count == 1)
      {
        // Teat a single cell specially, it doesn't come in as an array but as a single value
        excelData = new object[2, 2];
        excelData[1, 1] = isFormatted ? range.Value : range.Value2;
      }
      else
      {
        excelData = isFormatted ? range.Value : range.Value2;
      }

      return excelData;
    }

    /// <summary>
    /// Unprotects the given Excel worksheet and stops listening for its Change event.
    /// </summary>
    /// <param name="worksheet">The Excel worksheet to unprotect.</param>
    /// <param name="changeEventHandlerDelegate">The change event handler delegate of the Excel worksheet.</param>
    /// <param name="protectionKey">The key used to unprotect the worksheet.</param>
    public static void UnprotectEditingWorksheet(this Excel.Worksheet worksheet, Excel.DocEvents_ChangeEventHandler changeEventHandlerDelegate, string protectionKey)
    {
      if (worksheet == null)
      {
        return;
      }

      if (changeEventHandlerDelegate != null)
      {
        worksheet.Change -= changeEventHandlerDelegate;
      }

      worksheet.Unprotect(worksheet.GetProtectionKey());
    }

    /// <summary>
    /// Checks if an Excel <see cref="Excel.Worksheet"/> with a given name exists in a <see cref="Excel.Workbook"/> with the given name.
    /// </summary>
    /// <param name="workBookName">Name of the <see cref="Excel.Workbook"/>.</param>
    /// <param name="workSheetName">Name of the <see cref="Excel.Worksheet"/>.</param>
    /// <returns><c>true</c> if the <see cref="Excel.Worksheet"/> exists, <c>false</c> otherwise.</returns>
    public static bool WorksheetExists(string workBookName, string workSheetName)
    {
      bool exists;
      if (workBookName.Length <= 0 || workSheetName.Length <= 0)
      {
        return false;
      }

      // Maybe the last deactivated sheet has been deleted?
      try
      {
        // Do NOT remove the following lines although the wSheet variable is not used in the method the casting of the
        // wBook.Worksheets[workSheetName] is needed to determine if the Worksheet is still valid and has not been disposed of.
        Excel.Workbook wBook = Globals.ThisAddIn.Application.Workbooks[workBookName];
        Excel.Worksheet wSheet = wBook.Worksheets[workSheetName] as Excel.Worksheet;
        exists = true;
      }
      catch
      {
        exists = false;
      }

      return exists;
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="Excel.ListObject"/> that avoids duplicates with existing ones in the current <see cref="Excel.Worksheet"/>.
    /// </summary>
    /// <param name="tableName">The proposed name for a <see cref="Excel.ListObject"/>.</param>
    /// <param name="copyIndex">Number of the copy of a <see cref="Excel.Worksheet"/> within its name.</param>
    /// <returns>A <see cref="Excel.ListObject"/> valid name.</returns>
    private static string GetTableNameAvoidingDuplicates(this string tableName, int copyIndex)
    {
      var activeWorkbook = Globals.ThisAddIn.Application.ActiveWorkbook;
      if (activeWorkbook == null)
      {
        return tableName;
      }

      string retName;
      do
      {
        retName = copyIndex > 1 ? string.Format("{0}.{1}", tableName, copyIndex) : tableName;
        copyIndex++;
      } while (activeWorkbook.Worksheets.Cast<Excel.Worksheet>().Any(ws => ws.ListObjects.Cast<Excel.ListObject>().Any(excelTable => excelTable.Name == retName)));

      return retName;
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="Excel.Worksheet"/> that avoids duplicates with existing ones in the given <see cref="Excel.Workbook"/>.
    /// </summary>
    /// <param name="workbook">A <see cref="Excel.Workbook"/>.</param>
    /// <param name="worksheetName">The proposed name for a <see cref="Excel.Worksheet"/>.</param>
    /// <param name="copyIndex">Number of the copy of a <see cref="Excel.Worksheet"/> within its name.</param>
    /// <returns>A <see cref="Excel.Worksheet"/> valid name.</returns>
    private static string GetWorksheetNameAvoidingDuplicates(this Excel.Workbook workbook, string worksheetName, int copyIndex)
    {
      if (workbook == null)
      {
        return worksheetName;
      }

      string retName;
      do
      {
        retName = copyIndex > 0 ? string.Format("Copy {0} of {1}", copyIndex, worksheetName) : worksheetName;
        copyIndex++;
      } while (workbook.Worksheets.Cast<Excel.Worksheet>().Any(ws => ws.Name == retName));

      return retName;
    }
  }
}