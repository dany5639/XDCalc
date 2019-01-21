using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XDCalc
{
    public partial class XDCalc : Form
    {
        private void button_clearTextBoxes(object sender, EventArgs e)
        {
            richTextBox_dec.Text = "";
            richTextBox_hex.Text = "";
        }

        public XDCalc()
        {
            InitializeComponent();
        }

        private void XDCalc_load(object sender, EventArgs e)
        {
            richTextBox_dec.KeyPress += new KeyPressEventHandler(richTextBox1_KeyPress);
            richTextBox_hex.KeyPress += new KeyPressEventHandler(richTextBox2_KeyPress);
        }

        void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            ReadTextBox(e, richTextBox_dec, Info.BoxType.Decimal);
        }

        void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            ReadTextBox(e, richTextBox_hex, Info.BoxType.Hexadecimal);
        }

        private void UpdateTextBoxes(RichTextBox richTextBox, string additional)
        {
            richTextBox.Text = $"{richTextBox.Text}[{DateTime.Now.ToString("hh:mm:ss")}] {additional}\n";
            richTextBox.SelectionStart = richTextBox.Text.Length;
            richTextBox.ScrollToCaret();
            richTextBox.ScrollToCaret();
        }

        private void ReadTextBox(KeyPressEventArgs e, RichTextBox richtextbox, Info.BoxType boxType)
        {
            var c = e.KeyChar;

            // Do a simple check if the line contains valid characters, 0 - 9, a - f, A - F, + - * /
            if (!Info.ValidChars_all.Contains((byte)c))
                e.Handled = true;

            var tempText_left = richTextBox_dec.Text;
            var tempText_right = richTextBox_hex.Text;

            if (e.KeyChar == 0x0D) // seems to be useless
            {
                e.Handled = true;

                richtextbox = ParseLines(richtextbox, boxType);
            }
        }

        private RichTextBox ParseLines(RichTextBox textfile, Info.BoxType boxType)
        {
            var lines = textfile.Text.Split("\n".ToCharArray());

            if (lines.Length == 0)
                return textfile;

            // lines contains every line in the text box. Only select the second to last. Last should be a new empty line.
            var lineItems = FindOpAndNumbers(lines[lines.Length - 2], boxType);

            if (lineItems.Count == 0)
                return textfile;

            CheckLineItems(lineItems);

            DoOperations(lineItems);

            return textfile;
        }

        private void CheckLineItems(List<Info.CalcItem> lineItems)
        {
            foreach (var a in lineItems)
            {
                var output = $"Line Item: " +
                    $"ID {a.ID}, " +
                    $"Type {a.Type}, " +
                    $"OP {a.Op}, " +
                    $"Val {a.Number:X16}, " +
                    $"";
            }
        }

        private List<Info.CalcItem> FindOpAndNumbers(string line, Info.BoxType boxType)
        {
            var chars = line.ToCharArray();

            foreach (var char_ in chars)
            {
                if (!Info.ValidChars_all.Contains((byte)char_))
                    return new List<Info.CalcItem>();
            }

            var items = new List<Info.CalcItem>();
            var charsQueue = new List<char>();
            var items_temp = new List<List<char>>();

            var d = "";
            ulong convertedVal = 0;

            for (int i = 0; i < chars.Length; i++)
            {
                // if it finds an operator
                if (Info.ValidChars_ops.Contains((byte)chars[i]))
                {
                    // items_temp list = list of all items on a line, such as numbers, operators

                    // incase there was a number before this operator, the char queue would be filled. return it as a new item, then add the operator as another item
                    items_temp.Add(charsQueue);

                    d = "";
                    foreach (var a in charsQueue)
                        d = $"{d}{a}";
                    convertedVal = 0;
                    if (boxType == Info.BoxType.Hexadecimal)
                        ulong.TryParse(d, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out convertedVal);
                    else
                        ulong.TryParse(d, NumberStyles.Integer, CultureInfo.InvariantCulture, out convertedVal);

                    // Log($"Parsed: {d}: 0x{convertedVal:X16}, {convertedVal:X16}");
                    items.Add(new Info.CalcItem
                    {
                        ID = items_temp.Count - 1,
                        Type = Info.ItemType.Integrals,
                        Number = convertedVal,
                        Op = Info.Operation.Nul,
                        OpByte = (char)0x0
                    });

                    // reset current chars list
                    charsQueue = new List<char>();

                    charsQueue.Add(chars[i]);
                    // add this operator to the items list
                    items_temp.Add(charsQueue);
                    // reset current chars list since operators should be only one char long
                    charsQueue = new List<char>();

                    var op = Info.Operation.Nul;
                    switch ((byte)chars[i])
                    {
                        case 0x2A: op = Info.Operation.Mul; break;
                        case 0x2B: op = Info.Operation.Add; break;
                        case 0x2F: op = Info.Operation.Div; break;
                        case 0x2D: op = Info.Operation.Sub; break;
                    }

                    items.Add(new Info.CalcItem
                    {
                        ID = items_temp.Count - 1,
                        Type = Info.ItemType.Operation,
                        Op = op,
                        OpByte = chars[i],
                        Number = 0,
                    });

                    continue;
                }

                if (Info.ValidChars_numbers.Contains((byte)chars[i]))
                {
                    // continue adding chars if it's a number
                    charsQueue.Add(chars[i]);
                    continue;
                }
            }

            // assuming no op was found, or there's a space, or end of line, add the current chars queue
            items_temp.Add(charsQueue);

            d = "";
            foreach (var a in charsQueue)
                d = $"{d}{a}";
            convertedVal = 0;
            if (boxType == Info.BoxType.Hexadecimal)
                ulong.TryParse(d, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out convertedVal);
            else
                ulong.TryParse(d, NumberStyles.Integer, CultureInfo.InvariantCulture, out convertedVal);

            // Log($"Parsed: {d}: 0x{convertedVal:X16}, {convertedVal:X16}");
            items.Add(new Info.CalcItem
            {
                ID = items_temp.Count - 1,
                Type = Info.ItemType.Integrals,
                Number = convertedVal,
                Op = Info.Operation.Nul,
                OpByte = (char)0x0
            });

            // reset current chars list
            charsQueue = new List<char>();

            var lineItems = new List<string>();

            // temp: draw current line items
            var k = -1;
            foreach (var a in items_temp)
            {
                k++;
                d = "";
                foreach (var b in a)
                    d = $"{d}{b}";

                lineItems.Add(d);
            }

            return items;
        }

        private void DoOperations_test(List<Info.CalcItem> lineItems)
        {
            // Hardcode to only detect addition between 2 numbers for now.
            ulong nr1 = 0;
            ulong nr2 = 0;
            var op1 = Info.Operation.Add;

            if (lineItems.Count > 0)
                nr1 = lineItems[0].Number;

            // maybe it shouldn't be like this
            ulong result = nr1;

            if (lineItems.Count == 3)
            {
                nr1 = lineItems[0].Number;
                op1 = lineItems[1].Op;
                nr2 = lineItems[2].Number;

                if (op1 == Info.Operation.Add)
                    result = nr1 + nr2;
            }

            UpdateTextBoxes(richTextBox_dec, $"{result:D16}");
            UpdateTextBoxes(richTextBox_hex, $"{result:X16}");
        }

        private void DoOperations(List<Info.CalcItem> lineItems)
        {
            // If input is only one or more numbers, assume the user wants to convert them to hex or dec
            if (lineItems.Find(x => x.Type == Info.ItemType.Operation) == null)
            {
                foreach (var a in lineItems)
                {
                    UpdateTextBoxes(richTextBox_dec, $"{a.Number:D16}");
                    UpdateTextBoxes(richTextBox_hex, $"{a.Number:X16}");
                }

                return;
            }

            // If any operand was detected, attempt to calculate mul and div first, then add and sub
            foreach (var a in lineItems)
            {
                // check if all operators have 2 operands
                if (a.Type == Info.ItemType.Operation)
                {
                    // check if operator isn't the first item in the list
                    if (a.ID == 0)
                        return;

                    // check if operator isn't the last item in the list
                    if (a.ID == lineItems.Count - 1)
                        return;

                    // Check if operator doesn't have another operator next to it.
                    if (lineItems[a.ID - 1].Type == Info.ItemType.Operation || lineItems[a.ID + 1].Type == Info.ItemType.Operation)
                        return;
                }
            }


            // keep looking for mul and div until no more are left
            CheckLineItems(lineItems);
            while (!(lineItems.Find(x => x.Op == Info.Operation.Mul) == null && lineItems.Find(x => x.Op == Info.Operation.Div) == null))
                lineItems = CalculateStuf_mul(lineItems);

            CheckLineItems(lineItems);
            while (!(lineItems.Find(x => x.Op == Info.Operation.Add) == null && lineItems.Find(x => x.Op == Info.Operation.Sub) == null))
                lineItems = CalculateStuf_add(lineItems);

            CheckLineItems(lineItems);

            if (lineItems.Count == 0)
                return;

            UpdateTextBoxes(richTextBox_dec, $"{lineItems.Last().Number:D16}");
            UpdateTextBoxes(richTextBox_hex, $"{lineItems.Last().Number:X16}");
        }

        private List<Info.CalcItem> CalculateStuf_mul(List<Info.CalcItem> lineItems)
        {
            var itemsToRemove = new List<int>();
            ulong result = 0;

            // if no more mul or div are found, return true;
            if (lineItems.Find(x => x.Op == Info.Operation.Mul) == null && 
                lineItems.Find(x => x.Op == Info.Operation.Div) == null)
                return lineItems;

            var firstOp = -1;
            foreach (var a in lineItems)
            {
                if (a.Type != Info.ItemType.Operation)
                    continue;

                firstOp = a.ID - 1;
                var n1 = lineItems[a.ID - 1];
                var n2 = lineItems[a.ID + 1];

                switch (a.Op)
                {
                    case Info.Operation.Mul:
                        result = n1.Number * n2.Number;
                        break;
                    case Info.Operation.Div:
                        if (n2.Number == 0)
                            return new List<Info.CalcItem>();
                        result = n1.Number / n2.Number;
                        break;
                    default:
                        continue;
                }

                // remove op, operands
                itemsToRemove.Add(a.ID);
                itemsToRemove.Add(a.ID - 1);
                itemsToRemove.Add(a.ID + 1);

                goto ending;
            }

        ending:

            // remove the operator and its operands that just got calculated
            var newList = new List<Info.CalcItem>();
            foreach (var a in lineItems)
                if (!itemsToRemove.Contains(a.ID))
                    newList.Add(a);

            // Add back to the line items list, the result of the last mul or div
            if (firstOp == -1)
                throw new Exception();

            newList.Add(new Info.CalcItem
            {
                ID = firstOp,
                Number = result,
                Type = Info.ItemType.Integrals,

            });

            // make sure all line items are ordered and sequential
            newList = newList.OrderBy(x => x.ID).ToList();
            for (int i = 0; i < newList.Count; i++)
                newList[i].ID = i;

            lineItems = newList;

            return lineItems;
        }

        private List<Info.CalcItem> CalculateStuf_add(List<Info.CalcItem> lineItems)
        {
            var itemsToRemove = new List<int>();
            ulong result = 0;

            // if no more mul or div are found, return true;
            if (lineItems.Find(x => x.Op == Info.Operation.Add) == null && 
                lineItems.Find(x => x.Op == Info.Operation.Sub) == null)
                return lineItems;

            var firstOp = -1;
            foreach (var a in lineItems)
            {
                if (a.Type != Info.ItemType.Operation)
                    continue;

                firstOp = a.ID - 1;
                var n1 = lineItems[a.ID - 1];
                var n2 = lineItems[a.ID + 1];

                switch (a.Op)
                {
                    case Info.Operation.Add:
                        result = n1.Number + n2.Number;
                        break;
                    case Info.Operation.Sub:
                        result = n1.Number - n2.Number;
                        break;
                    default:
                        continue;
                }

                // remove op, operands
                itemsToRemove.Add(a.ID);
                itemsToRemove.Add(a.ID - 1);
                itemsToRemove.Add(a.ID + 1);

                goto ending;
            }

        ending:

            // remove the operator and its operands that just got calculated
            var newList = new List<Info.CalcItem>();
            foreach (var a in lineItems)
                if (!itemsToRemove.Contains(a.ID))
                    newList.Add(a);

            // Add back to the line items list, the result of the last mul or div
            if (firstOp == -1)
                throw new Exception();
            
            newList.Add(new Info.CalcItem
            {
                ID = firstOp,
                Number = result,
                Type = Info.ItemType.Integrals,
            });

            // make sure all line items are ordered and sequential
            newList = newList.OrderBy(x => x.ID).ToList();
            for (int i = 0; i < newList.Count; i++)
                newList[i].ID = i;

            lineItems = newList;

            return lineItems;
        }

    }
}