using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{

    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            label1.Text = "";
            textBox1.Enabled = false;


        }
        private string expression;
        private void ButtonPress(object btn)
        {
            Button btt = (Button)btn;
            char token = btt.Text[0];
            if (Char.IsDigit(token))
            {
                char exp = '0';
                if (!String.IsNullOrEmpty(expression)) exp = expression[expression.Length - 1];

                if (Char.IsDigit(exp) || exp == '.')
                    expression += token.ToString();
                else if (exp != ')')
                {
                    expression += ' ' + token.ToString();
                }
            }
           else if (token == '√')
            {
                char exp = '0';
                if (!String.IsNullOrEmpty(expression)) exp = expression[expression.Length - 1];

                if (!Char.IsDigit(exp) && exp != '.')
               
                {
                    expression += ' ' + token.ToString();
                }
            }
            else if (token == '.')
            {
                if (!String.IsNullOrEmpty(expression) && Char.IsDigit(expression[expression.Length - 1]))
                {
                    char br = expression[expression.Length - 1];
                    bool isFl = false;
                    for (int i = expression.Length - 1; i > 0 && br != ' '; i--)
                    {
                        br = expression[i];
                        if (br == '.')
                        {
                            isFl = true;
                            break;
                        }
                    }
                    if (!isFl) expression += '.';
                }
            }
            else if (token == '(')
            {
                char exp = '0';
                if (!String.IsNullOrEmpty(expression)) exp = expression[expression.Length - 1];
                if (!Char.IsDigit(exp) && !String.IsNullOrEmpty(expression) && exp != ')' && exp != '(')
                    expression += ' ' + token.ToString();
            }

            else if (token == ')')
            {
                char exp = '0';
                if (!String.IsNullOrEmpty(expression)) { exp = expression[expression.Length - 1];

                    if (Char.IsDigit(exp))
                        expression += ' ' + token.ToString();
                }
            }
            else
            {
                char exp = '0';
                if (!String.IsNullOrEmpty(expression))
                {
                    exp = expression[expression.Length - 1];
                    if (Char.IsDigit(exp) || exp == ')')
                        expression += ' ' + token.ToString();
                }
            }
            label1.Text = expression;
        }

        private bool IsOp(string x)
        {
            string op = "+-/*^";
            return op.Contains(x);

        }
        private bool IsFunc(string x)
        {
            string op = "√";
          
            return op==x;
        }
        private List<string> Shunting_Yard(string Expresion)
        {
            string[] tokens = Expresion.Split(' ');
            var map = new Dictionary<string, (int prioritate, bool asoc)>()
            {   {"√",(1,false)},
                {"+",(2,false) },
                {"-",(2,false) },
                {"*",(3,false) },
                {"/",(3,false) },
                {"^",(4,true) }

            };
            var output = new List<string>();
            var ops = new Stack<string>();
            foreach (string token in tokens)
            {
                if (float.TryParse(token, out _))
                {
                    output.Add(token);
                }
                else if (IsFunc(token))
                {
                    ops.Push(token);
                }
                else if (IsOp(token))
                {
                    if (ops.Count == 0) ops.Push(token);
                    else
                    {
                        
                        map.TryGetValue(token, out var op1);
                       
                        map.TryGetValue(ops.Peek(), out var op2);
                        while (ops.Count>0 && (IsFunc(ops.Peek()) || op2.prioritate > op1.prioritate || (op2.prioritate == op1.prioritate && op1.asoc == true) && (ops.Peek() != "(")))
                            output.Add(ops.Pop());
                        ops.Push(token);
                    }
                }
                else if (token == "(")
                {
                    ops.Push(token);
                }
                else if (token == ")") {
                    string top = "";
                    while (ops.Count>0 && (top=ops.Pop())!= "(")
                    {
                        output.Add(top);
                    }
                    
                   
                
                }
            }
            while (ops.Count > 0)
            {
                
                output.Add(ops.Pop());
                
            }

            return output;
               


        }
        private string Calc(string tok ,string x , string y)
        {
            if (tok == "^")
            {
                if (int.TryParse(x, out int opx) && int.TryParse(y, out int opy))
                {
                    return (Math.Pow(opx,opy).ToString());
                }
            }
            if (tok == "√")
            {
                if (double.TryParse(x, out double opx) )
                {
                   if(opx>0)
                    return (Math.Sqrt(opx)).ToString();
                }
            }
            if (float.TryParse(x, out float op1) && float.TryParse(y, out float op2))
            {
                
                if (tok == "+")
                {
                    return (op1 + op2).ToString();
                }
               
                if (tok == "-")
                {
                    return (op1 - op2).ToString();
                }
                if (tok == "*")
                {
                    
                    return (op1 * op2).ToString();
                }
                if (tok == "/")
                {
                    if (op2 == 0) return "Eroare , Numitorul nu poate fi 0";
                    return (op1 / op2).ToString();
                }
               
            }
           
          
            return "Eroare";
        }
        public string RPN(List<string> infix)
        {
            var stiva = new Stack<string>();
            foreach( string token in infix)
            {
                if (IsOp(token))
                {
                    var op2 = stiva.Pop();
                    var op1 = stiva.Pop();
                    var result = Calc(token, op1, op2);
                    stiva.Push(result);
                }
                else if (IsFunc(token))
                {
                    var op = stiva.Pop();
                    
                    var result = Calc(token, op,  "");
                    stiva.Push(result);
                    
                }
                else
                {
                    stiva.Push(token);
                }
            }



            return stiva.Pop();
        }
       
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
        }
        
        private void button15_Click(object sender, EventArgs e)
        {
            
           textBox1.Text= RPN(Shunting_Yard(expression));
            if (textBox1.Text.Contains('-'))
            {
                string x=textBox1.Text.Replace("-", String.Empty);
                textBox1.Text = x+'-';

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            expression = "";
            label1.Text = expression;
            textBox1.Text = "";

        }

        private void button16_Click(object sender, EventArgs e)
        {
            if(expression.Length >0)
           expression = expression.Remove(expression.Length - 1, 1);
            if (expression.Length > 0 && expression[expression.Length-1]==' ')
                expression = expression.Remove(expression.Length - 1, 1);
            label1.Text = expression;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            ButtonPress(sender);
        }
    }
}
