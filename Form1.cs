using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            buttonAnswer.Enabled = true;
        }

        private List<string> CreateListVariable(DataTable dataTable, int col)
        {
            List<string> listVariable = new List<string>();

            foreach(DataRow row in dataTable.Rows)
            {
                if (!listVariable.Contains( row[col].ToString() ))
                {
                    listVariable.Add(row[col].ToString());
                }
            }
            return listVariable;
        }

        private void firstStartDataset()
        {
            List<string> listVariableColumns = CreateListVariable(dt, columnIndex);

            comboBoxVariable.Items.AddRange(listVariableColumns.ToArray());
            labelColumn.Text = dt.Columns[columnIndex].ToString();

            Dictionary<string, int> dictionary = searchAbsoluteChanceOneColumn(dt.Columns.Count - 1);

            foreach (var item in dictionary)
            {
                dataGridViewChance.Rows.Add(item.Key, (float)item.Value / dt.Rows.Count);
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
            Dictionary<string, float> dictionaryChance;

            string parametr = comboBoxVariable.SelectedItem.ToString();

            dictionaryChance = searchChanceColumnWithFinal(columnIndex, parametr);
            
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

            if (++columnIndex < dt.Columns.Count - 1)
            {
                List<string> listVariableColumns = CreateListVariable(dt, columnIndex);

                comboBoxVariable.Items.Clear();
                comboBoxVariable.Items.AddRange(listVariableColumns.ToArray());
                labelColumn.Text = dt.Columns[columnIndex].ToString();
                comboBoxVariable.SelectedIndex = -1;
                comboBoxVariable.SelectedItem = null;
                comboBoxVariable.Text = string.Empty;

            }
            else
            {
                comboBoxVariable.Enabled = false;
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

    }
}
