using System;
//using Windows.UI.Xaml.Input;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Calculator
{
    /// <summary>
    /// Interaction build calculator using 
    /// # Function
    /// #Enum
    /// #Tree
    /// # Recursive function
    /// </summary>
    public partial class MainWindow:Window
    {
        //private VisualCollection controls;
        //private TextBox textBox;
        public MainWindow()
        {
            InitializeComponent();
             result.Text = 0.ToString();
            //controls = new VisualCollection(this);
            //textBox = new TextBox();
            //controls.Add(textBox);

            //Loaded += ButtonEqual_Click;
        }
        
        

        #region buttons
        private void AddNumberToResult(double number)
        {
            if(result.Text.Length < 12)
                if(char.IsNumber(result.Text.Last()))
                {
                    if(result.Text.Length == 1 && result.Text =="0")
                    {
                        result.Text = string.Empty;
                    }
                    result.Text += number;
                } else
                {
                    if(number != 0 || result.Text.Contains('.'))
                        result.Text += number;
                }
        }
        
        private void PowerOff_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(7);
        }

        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(8);
        }

        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(9);
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(4);
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(5);
        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(6);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(1);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(2);
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(3);
        }

        private void Button0_Click(object sender, RoutedEventArgs e)
        {
            AddNumberToResult(0);
        }
       
        private void ButtonDot_Click(object sender, RoutedEventArgs e)
        {
            if(!result.Text.Contains('.'))
                result.Text += '.'.ToString();

            if(result.Text.Contains('+')  || result.Text.Contains('-') || result.Text.Contains('/') || result.Text.Contains('*'))
            {
                if(result.Text.Last() != '.')
                    result.Text += '.'.ToString();
            }
        }
        public string text;

        //Screen clear section
        private void ButtonC_Click(object sender, RoutedEventArgs e)
        {
            result.Text = 0.ToString();
            result2.Text= result3.Text = result4.Text = string.Empty;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(result.Text.Length > 1)
                result.Text = result.Text.Substring(0, result.Text.Length - 1);
            else
                result.Text = 0.ToString();            
        }
        #endregion button

        #region btnOparator 

        private void HistoryOfOpration()
        {
            result4.Text = result3.Text;
            result3.Text = result2.Text;
            result2.Text = result.Text;
        }
        private void BtnPlus_click(object sender, RoutedEventArgs e)
        {
            AddOperationToresult(Operation.PLUS);
        }

        private void BtnMultiplication_Click(object sender, RoutedEventArgs e)
        {
            AddOperationToresult(Operation.TIMES);
        }

        private void BtnDivide_Click(object sender, RoutedEventArgs e)
        {
            AddOperationToresult(Operation.DIV);
        }

        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            AddOperationToresult(Operation.MINUS);
        }
        private void ButtonEqual_Click(object sender, RoutedEventArgs e)
        {
                      
                if(string.IsNullOrEmpty(result.Text)) return;

                Operand tree = BuildTreeOperand();

                double value = Calc(tree);
                HistoryOfOpration();
                result.Text = value.ToString();
            
        }

      

        enum Operation { MINUS = 1, PLUS = 2, DIV = 3, TIMES = 4, NUMBER = 5}
        private void AddOperationToresult(Operation operation)
        {
            if(result.Text.Length == 1 && result.Text == "0") return;

            if(!char.IsNumber(result.Text.Last()))
                result.Text = result.Text.Substring(0, result.Text.Length-1); 

            switch(operation)
            {
                case Operation.MINUS: result.Text += "-";
                    break;
                case Operation.PLUS: result.Text += "+";
                    break;
                case Operation.DIV: result.Text += "/";
                    break;
                case Operation.TIMES: result.Text += "*";
                    break;

            }

        }        

        private void BtnSquare_Click(object sender, RoutedEventArgs e)
        {
            if(!result.Text.Contains("+") && !result.Text.Contains("-") && !result.Text.Contains("/") && !result.Text.Contains("*"))
            {
                HistoryOfOpration();
                result.Text = Math.Pow(double.Parse(result.Text), 2).ToString();
            }
        }

        private void BtnSquareRoot_Click(object sender, RoutedEventArgs e)
        {
            if(!result.Text.Contains("+") && !result.Text.Contains("-") && !result.Text.Contains("/") && !result.Text.Contains("*"))
            {
                HistoryOfOpration();
                result.Text = Math.Sqrt(double.Parse(result.Text)).ToString();
            }
        }
        #endregion btnOparator

        #region Equation
        private class Operand
        {
            public Operation operation = Operation.NUMBER;
            public double value = 0;

            public Operand left = null;
            public Operand right = null;
        }

        private Operand BuildTreeOperand()
        {
            Operand tree = null;

            string expression = result.Text;

            if(!char.IsNumber(result.Text.Last()))
                expression = result.Text.Substring(0, result.Text.Length-1);

            string numberStr = string.Empty;

            foreach(char c in expression.ToCharArray())
            {
                if(char.IsNumber(c) || c == '.' || numberStr == string.Empty && c == '-')
                    numberStr += c;
                else
                {
                    AddOperandToTree(ref tree, new Operand() { value = double.Parse(numberStr) });

                    numberStr = string.Empty;

                    Operation op = Operation.MINUS; // make is dufalt

                    switch(c)
                    {
                        case '*': op = Operation.TIMES;
                            break;
                        case '/': op = Operation.DIV;
                            break;
                        case '-': op = Operation.MINUS;
                            break;
                        case '+': op = Operation.PLUS;
                            break;
                    }
                    AddOperandToTree(ref tree, new Operand() { operation= op });
                }

            }
            AddOperandToTree(ref tree, new Operand() { value = double.Parse(numberStr) });

            return tree;
        }

        private void AddOperandToTree(ref Operand tree, Operand elem)
        {
            if(tree == null)
                tree = elem;
            else
            {
                if(elem.operation < tree.operation)
                {
                    Operand auxTree = tree;
                    tree = elem;
                    elem.left = auxTree;
                } else
                {
                    AddOperandToTree(ref tree.right, elem);
                }
            }
        }

        private double Calc(Operand tree)
        {
            if(tree.left == null && tree.right == null)
                return tree.value;
            else
            {
                double subResult = 0;
                switch(tree.operation)
                {
                    case Operation.TIMES: subResult =Calc(tree.left) * Calc(tree.right); // recursive calculation
                        break;
                    case Operation.DIV: subResult =Calc(tree.left) / Calc(tree.right); // recursive calculation
                        break;
                    case Operation.PLUS: subResult =Calc(tree.left) + Calc(tree.right); // recursive calculation
                        break;
                    case Operation.MINUS: subResult =Calc(tree.left) - Calc(tree.right); // recursive calculation
                        break;
                            
                }
                return subResult;
            }
        }

        #endregion

        private void BtnEqual_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                if(string.IsNullOrEmpty(result.Text)) return;

                Operand tree = BuildTreeOperand();

                double value = Calc(tree);
                HistoryOfOpration();
                result.Text = value.ToString();
            }
        }
    }
}
