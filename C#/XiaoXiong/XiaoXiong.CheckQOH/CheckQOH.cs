﻿using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaoXiong.CheckQOH.Model;

namespace XiaoXiong.CheckQOH
{
    public class CheckQOH
    {
        //public void oldXiaoXiong()
        //{
        //    Dictionary<string, int> letterIndex = new Dictionary<string, int>() { { "A", 1 }, { "B", 2 }, { "C", 3 }, { "D", 4 }, { "E", 5 }, { "F", 6 }, { "G", 7 }, { "H", 8 }, { "I", 9 }, { "J", 10 }, { "K", 11 }, { "L", 12 }, { "M", 13 }, { "N", 14 } };
        //    // SpreadsheetLight works on the idea of a currently selected worksheet.
        //    // If no worksheet name is provided on opening an existing spreadsheet,
        //    // the first available worksheet is selected.
        //    Console.WriteLine("Doesn't support the extension .xls");
        //    Console.WriteLine(@"Put your file at D:\Downloads and name it accountmoveline.xlsx");
        //    SLDocument sl = new SLDocument(@"D:\Open Orders Report.xlsm");
        //    int row = 969;
        //    int column = 10;

        //    //Get debit 如果重复 debit.value += 1
        //    Dictionary<string, int> debit = new Dictionary<string, int>();
        //    for (int i = 2; i < row; i++)
        //    {
        //        string currentDebit = sl.GetCellValueAsString(i, letterIndex["F"]);
        //        if (!string.IsNullOrEmpty(currentDebit))
        //        {
        //            if (debit.Keys.Contains(currentDebit))
        //            {
        //                debit[currentDebit] += 1;
        //            }
        //            else
        //            {
        //                debit.Add(currentDebit, 1);
        //            }
        //        }
        //    }

        //    //Get credit 如果重复 credit.value += 1
        //    Dictionary<string, int> credit = new Dictionary<string, int>();
        //    for (int i = 2; i < row; i++)
        //    {
        //        string currentCredit = sl.GetCellValueAsString(i, letterIndex["H"]);
        //        if (!string.IsNullOrEmpty(currentCredit))
        //        {
        //            if (credit.Keys.Contains(currentCredit))
        //            {
        //                credit[currentCredit] += 1;
        //            }
        //            else
        //            {
        //                credit.Add(currentCredit, 1);
        //            }
        //        }
        //    }

        //    // 可以删的debit数
        //    List<string> needToDelete = new List<string>();
        //    foreach (var item in credit)
        //    {
        //        //debit里有这个credit
        //        if (debit.ContainsKey(item.Key))
        //        {
        //            Console.WriteLine(item.Key);
        //            for (int i = 2; i < row; i++)
        //            {
        //                string currentDebit = sl.GetCellValueAsString(i, letterIndex["G"]);
        //                string currentCredit = sl.GetCellValueAsString(i, letterIndex["H"]);

        //                //如果item.Key出现在debit或credit行
        //                if (item.Key == currentDebit || item.Key == currentCredit)
        //                {
        //                    //如果这个数只在debit和credit里出现一次 - 标记行为红色
        //                    if (debit[item.Key] == 1 && item.Value == 1)
        //                    {
        //                        SLStyle style = sl.CreateStyle();
        //                        style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.LightCoral, System.Drawing.Color.CornflowerBlue);
        //                        sl.SetRowStyle(i, style);
        //                        //sl.DeleteRow(i, 1);
        //                    }
        //                    else if (debit[item.Key] > 1 || item.Value > 1)
        //                    {
        //                        SLStyle style = sl.CreateStyle();
        //                        style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.LightSkyBlue, System.Drawing.Color.CornflowerBlue);
        //                        sl.SetRowStyle(i, style);
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    sl.SaveAs(@"D:\Downloads\accountmoveline Cleaned.xlsx");
        //}
        public static void ShipCheck()
        {
            Dictionary<string, int> letterIndex = new Dictionary<string, int>() { { "A", 1 }, { "B", 2 }, { "C", 3 }, { "D", 4 }, { "E", 5 }, { "F", 6 }, { "G", 7 }, { "H", 8 }, { "I", 9 }, { "J", 10 }, { "K", 11 }, { "L", 12 }, { "M", 13 }, { "N", 14 } };

            SLDocument sl = new SLDocument(@"F:\Github\storage\C#\XiaoXiong\SO billing based on Inventory Dates V1.xlsx", "Qty on Hand");
            List<QOH> qOHs = new List<QOH>();
            List<ComingPO> comingPOs = new List<ComingPO>();
            var sheetInfo = sl.GetWorksheetStatistics();
            
            for (int i = 2; i <= sheetInfo.EndRowIndex; i++)
            {
                QOH qoh = new QOH();
                qoh.Id = i;
                qoh.QOHInternalRef = sl.GetCellValueAsString($"A{i}");
                qoh.Qty = sl.GetCellValueAsInt32($"B{i}");
                qOHs.Add(qoh);
            }

            sl.SelectWorksheet("Coming POs");
            sheetInfo = sl.GetWorksheetStatistics();

            for (int i = 2; i <= sheetInfo.EndRowIndex; i++)
            {
                ComingPO comingPO = new ComingPO();
                comingPO.Id = i;
                comingPO.CPOInternalRef = sl.GetCellValueAsString($"D{i}");
                comingPO.Qty = sl.GetCellValueAsInt32($"G{i}");
                comingPO.ComingDate = sl.GetCellValueAsDateTime($"C{i}");
                comingPOs.Add(comingPO);
            }
            comingPOs = comingPOs.OrderBy(x => x.ComingDate).ToList();

            sl.SelectWorksheet("Detail Data for Bill Date");
            sheetInfo = sl.GetWorksheetStatistics();
            string interRef;
            int rToShip;
            DateTime shipDate;
            for (int i = 2; i < sheetInfo.EndRowIndex; i++)
            {
                shipDate = sl.GetCellValueAsDateTime($"E{i}");
                interRef = sl.GetCellValueAsString($"L{i}").Trim();
                rToShip = sl.GetCellValueAsInt32($"U{i}");
                foreach (var item in qOHs)
                {
                    if (interRef == item.QOHInternalRef.Trim())
                    {
                        int remain = item.Qty - rToShip;
                        //如果仓库不够或者没有
                        if (remain < 0)
                        {
                            int tempQty;
                            if (item.Qty > 0)
                            {
                                //如果仓库有货但不够
                                sl.SetCellValue($"AC{i}", shipDate, "MM/dd/yyyy");
                                sl.SetCellValue($"AD{i}", item.Qty);
                                item.QOHInternalRef = item.QOHInternalRef + " - chekced";
                                remain = remain * -1;
                            }
                            foreach (var cItem in comingPOs)
                            {
                                if (interRef == cItem.CPOInternalRef.Trim())
                                {
                                    remain = cItem.Qty - remain;
                                    char letterDate = 'C';
                                    char letterNumber = 'D';
                                    var a = sl.GetCellValueAsString($"A{letterDate}{i}");
                                    if (!string.IsNullOrWhiteSpace(sl.GetCellValueAsString($"A{letterDate}{i}")))
                                    {
                                        letterDate = 'E';
                                        letterNumber = 'F';
                                        if (!string.IsNullOrWhiteSpace(sl.GetCellValueAsString($"A{letterDate}{i}")))
                                        {
                                            letterDate = 'G';
                                            letterNumber = 'H';
                                        }
                                    }

                                    //如果coming不够或者没有
                                    if (remain < 0)
                                    {
                                        sl.SetCellValue($"A{letterDate}{i}", cItem.ComingDate, "yyyy/mm/dd");
                                        sl.SetCellValue($"A{letterNumber}{i}", cItem.Qty);
                                        item.QOHInternalRef = item.QOHInternalRef + " - chekced";
                                    }
                                    else if (remain == 0)
                                    {
                                        sl.SetCellValue($"A{letterDate}{i}", cItem.ComingDate, "yyyy/mm/dd");
                                        sl.SetCellValue($"A{letterNumber}{i}", cItem.Qty);
                                        item.QOHInternalRef = item.QOHInternalRef + " - chekced";
                                    }
                                    else if (remain > 0)
                                    {
                                        sl.SetCellValue($"A{letterDate}{i}", cItem.ComingDate, "yyyy/mm/dd");
                                        sl.SetCellValue($"A{letterNumber}{i}", remain);
                                        cItem.Qty = remain;
                                    }
                                }
                            }
                        }
                        else if (remain == 0)
                        {
                            sl.SetCellValue($"AC{i}", shipDate, "MM/dd/yyyy");
                            sl.SetCellValue($"AD{i}", rToShip);
                            item.QOHInternalRef = item.QOHInternalRef + " - chekced";
                        }
                        else if (remain > 0)
                        {
                            sl.SetCellValue($"AC{i}", shipDate, "MM/dd/yyyy");
                            sl.SetCellValue($"AD{i}", rToShip);
                            item.Qty = remain;
                        }
                        break;
                    }
                }
            }


            sl.SaveAs(@"F:\Github\storage\C#\XiaoXiong\aaa.xlsx");
            //qOH.SaveAs(@"D:\Open Orders Report1.xlsx");
            Console.WriteLine("Press ANY key");
        }
        
        public static void CheckComingOrder(int qty)
        {

        }
    }
}
