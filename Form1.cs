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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DatasetExpertSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataTable dt;
        int columnIndex = 0;
        List<string> listFinalClasses; // Классы целевого столбца
        bool currentClassType; // Текущее значение аргументов строки(false) или числа(true)

        private void buttonReadDataset_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            columnIndex = 0;
            comboBoxVariable.Items.Clear();
            comboBoxVariable.SelectedIndex = -1;
            comboBoxVariable.SelectedItem = null;
            comboBoxVariable.Text = string.Empty;
            dataGridViewChance.Rows.Clear();
            labelMaxChanceAnswer.Text = "";

            // Получаем выбранный файл
            string filename = openFileDialog1.FileName;

            dt = new DataTable();

            // Creating the columns
            foreach (var headerLine in File.ReadLines(filename).Take(1))
            {
                foreach (var headerItem in headerLine.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    dt.Columns.Add(headerItem.Trim());
                }
            }

            // Добавляем строки
            foreach (var line in File.ReadLines(filename).Skip(1))
            {
                dt.Rows.Add(line.Split(','));
            }

            listFinalClasses = CreateListVariable(dt, dt.Columns.Count - 1);

            firstStartDataset();

            comboBoxVariable.Enabled = true;
            numericUpDownValue.Enabled = true;
            buttonAnswer.Enabled = true;
        }

        private List<string> CreateListVariable(DataTable dataTable, int col)
        {
            List<string> listVariable = new List<string>();
            foreach (DataRow row in dataTable.Rows)
            {
                if (!listVariable.Contains(row[col].ToString()))
                {
                    listVariable.Add(row[col].ToString());
                }
            }
            return listVariable;
        }

        private void firstStartDataset()
        {
            List<string> listVariableColumns = CreateListVariable(dt, columnIndex);

            if (numericValue(listVariableColumns[0]))
            {
                comboBoxVariable.Visible = false;
                numericUpDownValue.Visible = true;
                labelMin.Visible = true;
                labelMax.Visible = true;
                labelColumn.Text = dt.Columns[columnIndex].ToString();
                Dictionary<string, int> dictionary = searchAbsoluteChanceOneColumn(dt.Columns.Count - 1);

                foreach (var item in dictionary)
                {
                    dataGridViewChance.Rows.Add(item.Key, (float)item.Value / dt.Rows.Count);
                }
                
                currentClassType = true;
            }
            else
            {
                comboBoxVariable.Visible = true;
                numericUpDownValue.Visible = false;
                labelMin.Visible = false;
                labelMax.Visible = false;
                comboBoxVariable.Items.AddRange(listVariableColumns.ToArray());
                labelColumn.Text = dt.Columns[columnIndex].ToString();

                Dictionary<string, int> dictionary = searchAbsoluteChanceOneColumn(dt.Columns.Count - 1);

                foreach (var item in dictionary)
                {
                    dataGridViewChance.Rows.Add(item.Key, (float)item.Value / dt.Rows.Count);
                }
                
                currentClassType = false;
            }
        }

        private Dictionary<string, int> searchAbsoluteChanceOneColumn(int col)
        {
            List<string> listVariable = CreateListVariable(dt, col);

            // Создание словаря
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            foreach(string item in listVariable)
            {
                dictionary.Add(item, 0);
            }
            
            // Ищем количество элементов каждого уникального значения
            foreach(DataRow row in dt.Rows)
            {
                dictionary[row[col].ToString()]++;
            }
            
            return dictionary;
        }

        private Dictionary<string, float> searchChanceColumnWithFinal(int col, string parametr)
        {
            // Ключ - целевой класс, значение - вероятность принадлежности параметра к целевому классу
            Dictionary<string, float> dictionaryChance = new Dictionary<string, float>();
            
            // Перебираем уникальные значения целевого класса
            foreach (string finalClass in listFinalClasses)
            {
                // Общее кол-во элементов значения параметра
                int denominator = 0;
                // Кол-во элементов значения парамтра, соответствующего уникальному значению целевого класса
                int numerator = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (row[dt.Columns.Count - 1].ToString() == finalClass)
                    {
                        denominator++;

                        if (row[col].ToString() == parametr)
                        {
                            numerator++;
                        }
                    }
                }

                dictionaryChance.Add(finalClass, (float)numerator / denominator);
            }

            return dictionaryChance;
            
        }

        private void buttonAnswer_Click(object sender, EventArgs e)
        {
            // Ключ - целевой класс, значение - вероятность принадлежности параметра к целевому классу
            Dictionary<string, float> dictionaryChance = new Dictionary<string, float>();

            // Работаем с вероятностиями чисел или строк
            if(currentClassType)
            {
                float selectedValue = (float)numericUpDownValue.Value;

                foreach (DataGridViewRow rowFinal in dataGridViewChance.Rows)
                {
                    // Составляем список всех значений аргумента, которым соответствует текущий целевой класс
                    List<string> listVariableColumns = new List<string>();
                    foreach ( DataRow row in dt.Rows)
                    {
                        if(rowFinal.Cells[0].Value.ToString() == row[dt.Columns.Count - 1].ToString())
                        {
                            listVariableColumns.Add(row[columnIndex].ToString());
                        }
                    }

                    // Считаем априорную вероятность аргумента по списку значений
                    float chance = generateAnswerForFloat(listVariableColumns, selectedValue);

                    dictionaryChance.Add(rowFinal.Cells[0].Value.ToString(), chance);
                }
            }
            else
            {
                string parametr = comboBoxVariable.SelectedItem.ToString();

                dictionaryChance = searchChanceColumnWithFinal(columnIndex, parametr);
            }

            // Вероятность параметра
            float chanceParam = 0;
            foreach (DataGridViewRow row in dataGridViewChance.Rows)
            {
                chanceParam += (float)row.Cells[1].Value * dictionaryChance[row.Cells[0].Value.ToString()];
            }

            foreach (string key in dictionaryChance.Keys)
            {
                foreach (DataGridViewRow row in dataGridViewChance.Rows)
                {
                    if (row.Cells[0].Value?.ToString() == key)
                    {
                        // Вероятность целевого класса
                        float chanceFinal = (float)row.Cells[1].Value;

                        // Вероятность параметра при условии того, что он принадлежит целевому классу
                        float chanceParamWithFinal = dictionaryChance[key];

                        //float chanceParam = (float) searchAbsoluteChanceOneColumn(columnIndex)[parametr] / (dt.Rows.Count); = 

                        // Вероятность целевого класса, при условии что ему принадлежит параметр
                        float chanceFinalWithParam = chanceFinal * chanceParamWithFinal / chanceParam;

                        row.Cells[1].Value = chanceFinalWithParam;

                        break;
                    }
                }
            }

            //Проверка на конец колонок
            if (++columnIndex < dt.Columns.Count - 1)
            {
                List<string> listVariableColumns = CreateListVariable(dt, columnIndex);

                comboBoxVariable.Items.Clear();
                comboBoxVariable.Items.AddRange(listVariableColumns.ToArray());
                labelColumn.Text = dt.Columns[columnIndex].ToString();
                comboBoxVariable.SelectedIndex = -1;
                comboBoxVariable.SelectedItem = null;
                comboBoxVariable.Text = string.Empty;

                // Узнаем какие по типу аргументы в следующей колонке
                if (numericValue((string)dt.Rows[0][columnIndex]))
                {
                    comboBoxVariable.Visible = false;
                    numericUpDownValue.Visible = true;
                    labelColumn.Text = dt.Columns[columnIndex].ToString();
                    currentClassType = true;
                    labelMin.Visible = true;
                    labelMax.Visible = true;

                    List<float> listValue = new List<float>();
                    foreach (string str in CreateListVariable(dt, columnIndex))
                    {
                        listValue.Add(float.Parse(str, CultureInfo.InvariantCulture));
                    }

                    labelMin.Text = "От " + listValue.Min().ToString();
                    labelMax.Text = "До " + listValue.Max().ToString();
                    
                }
                else
                {
                    comboBoxVariable.Visible = true;
                    numericUpDownValue.Visible = false;
                    currentClassType = false;
                    labelMin.Visible = false;
                    labelMax.Visible = false;
                }

            }
            else
            {
                comboBoxVariable.Enabled = false;
                numericUpDownValue.Enabled = false;
                buttonAnswer.Enabled = false;
                float maxChance = -1;
                string classMaxChance = "";
                foreach (DataGridViewRow row in dataGridViewChance.Rows)
                {
                    if ((float)row.Cells[1].Value > maxChance)
                    {
                        maxChance = (float)row.Cells[1].Value;
                        classMaxChance = row.Cells[0].Value.ToString();
                    }
                }
                labelMaxChanceAnswer.Text = classMaxChance;
            }
        }

        private bool numericValue(string value)
        {
            return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float result);
        }

        private float generateAnswerForFloat(List<string> listVariableColumns, float selectedValue)
        {
            /*
            // Вычисляем значения сглаженной эмпирической функции распределения
            Assessment.SmoothedRandomVariable sdf;
            List<decimal> array = new List<decimal>();

            foreach(string value in listVariableColumns)
            {
                array.Add(Convert.ToDecimal(value, CultureInfo.InvariantCulture));
            }
            sdf = new Assessment.SmoothedRandomVariable(array, 0.1M);
            decimal answer = sdf.pdf((decimal)selectedValue);

            return (float)answer;

            */

            List<decimal> array = new List<decimal>();
            foreach (string value in listVariableColumns)
            {
                array.Add(Convert.ToDecimal(value, CultureInfo.InvariantCulture));
            }
            array.Sort();

            Assessment.Histogram hist = new Assessment.Histogram(array, (int)(1 + Math.Log(array.Count())));

            return (float)hist.Value((decimal)selectedValue);

        }

    }
}
